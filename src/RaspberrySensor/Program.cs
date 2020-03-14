using ReaspberrySensor.DHT;
using Serilog;
using System;
using System.Timers;
using Unosquare.RaspberryIO;

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


            _dht = new DHT(Pi.Gpio[7], DHTSensorTypes.DHT22);
            _logger.Information("Sensor setup...complete");
            SetTimer(5000);

            Console.Read();
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
