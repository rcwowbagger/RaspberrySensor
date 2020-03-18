using ReaspberrySensor.DHT;
using Serilog;
using System;
using System.Threading;
using System.Timers;
using Unosquare.RaspberryIO;
using Unosquare.WiringPi;

namespace RaspberrySensor
{
    class Program
    {
        private static Timer _sampleTimer;
        private static DHT _dht;
        private static ILogger _logger;

        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
            _logger = Log.Logger.ForContext<Program>();

            Pi.Init<BootstrapWiringPi>();

            _dht = new DHT(Pi.Gpio[7], DHTSensorTypes.DHT22);
            _logger.Information("Sensor setup...complete");
            SetTimer(5000);

            while(true)
            {
                Thread.Sleep(1000);
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
                _logger.Information("Timer Elapsed...");
                try
                {
                    var d = _dht.ReadData();

                    _logger.Information(d.ToString());
                }
                catch (DHTException)
                {
                    _logger.Warning("Ex");
                }
            };
        }
    }
}
