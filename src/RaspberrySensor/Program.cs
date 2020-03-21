using RaspberrySensor.Configuration;
using RaspberrySensor.Device;
using RaspberrySensor.Device.DHT;
using Serilog;
using System.Timers;
using Unosquare.RaspberryIO;
using Unosquare.WiringPi;

namespace RaspberrySensor
{
    class Program
    {
        private static Timer _sampleTimer;
        private static IDevice _dht;
        private static ILogger _logger;

        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
            _logger = Log.Logger.ForContext<Program>();
            ConfigurationHandler.Initialise();



            if (ConfigurationHandler.Get<bool>("SamplingEnamed"))
            {
                Pi.Init<BootstrapWiringPi>();
                _dht = new DHT(Pi.Gpio[4], DHTSensorTypes.DHT22);
            }
            else
            {
                _dht = new DummyDevice();
            }
            _logger.Information("Sensor setup...complete");
            SetTimer(ConfigurationHandler.Get<int>("SampleIntervalSec") * 1000);

            while(true)
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private static void SetTimer(int intervalMs)
        {
            _logger.Information("Set Timer");
            if (_sampleTimer != null)
            {
                _sampleTimer.Stop();
                _sampleTimer.Dispose();
            }

            _sampleTimer = new Timer()
            {
                AutoReset = true,
                Enabled = true,
                Interval = intervalMs
            };
            _sampleTimer.Elapsed += (a, b) =>
            {
                _logger.Debug("Timer Elapsed...");
                try
                {
                    var d = _dht.ReadData();

                    _logger.Information(d.ToString());
                }
                catch (DHTException)
                {
                }
            };
        }
    }
}
