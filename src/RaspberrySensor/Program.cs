using RaspberrySensor.Configuration;
using RaspberrySensor.Device;
using RaspberrySensor.Device.DHT;
using RaspberrySensor.Publisher;
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
        private static IPublisher _publisher;
        static void Main(string[] args)
        {
            ConfigurationHandler.Initialise();
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .MinimumLevel.Debug()
                .CreateLogger();
            _logger = Log.Logger.ForContext<Program>();


            if (ConfigurationHandler.Get<bool>("SamplingEnabled"))
            {
                Pi.Init<BootstrapWiringPi>();
                _device = new DHT(
                    Pi.Gpio[ConfigurationHandler.Get<int>("GpioPin")], 
                    ConfigurationHandler.Get<DHTSensorTypes>("DhtDevice"));
            }
            else
            {
                _device = new DummyDevice();
            }
            var publishKey = ConfigurationHandler.Get<string>("PublishKey");
            var connectionString = ConfigurationHandler.Get<string>("PublishAddress");

            if (ConfigurationHandler.Get<bool>("UseEventHub"))
            {
                _publisher = new EventHubPublisher(connectionString, publishKey);
            }
            else if (ConfigurationHandler.Get<bool>("UseInfluxDb"))
            {
                _publisher = new InfluxDbPublisher(connectionString, publishKey);
            }
            else
            {
                _publisher = new DummyPublisher();
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
            _logger.Information($"Timer Set [{intervalMs}]ms");
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
                _logger.Verbose("Timer Elapsed...");
                try
                {
                    var d = _device.ReadData();

                    _publisher.Publish(d);
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
