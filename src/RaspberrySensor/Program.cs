using RaspberrySensor.Configuration;
using RaspberrySensor.Device;
using RaspberrySensor.Device.DHT;
using Serilog;
using System;
using System.Timers;
using Unosquare.RaspberryIO;
using Unosquare.WiringPi;

namespace RaspberrySensor
{
    class Program
    {
        private static Timer _sampleTimer;
        private static IDevice _device;
        private static ILogger _logger;

        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
            _logger = Log.Logger.ForContext<Program>();
            ConfigurationHandler.Initialise();



            if (ConfigurationHandler.Get<bool>("SamplingEnabled"))
            {
                Pi.Init<BootstrapWiringPi>();
                _device = new DHT(Pi.Gpio[4], DHTSensorTypes.DHT22);
            }
            else
            {
                _device = new DummyDevice();
            }
            _logger.Information("Sensor setup complete");
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
                    var d = _device.ReadData();

                    _logger.Information(d.ToString());
                }
                catch (DHTException)
                {
                }
                catch(Exception ex)
                {
                    _logger.Error(ex, "Error reading from device");
                }
                _sampleTimer.Interval = ConfigurationHandler.Get<int>("SampleIntervalSec")*1000;
            };
        }
    }
}
