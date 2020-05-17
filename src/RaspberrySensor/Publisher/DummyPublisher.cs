using RaspberrySensor.Device;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RaspberrySensor.Publisher
{
    public class DummyPublisher : IPublisher
    {
        private readonly ILogger _log;

        public DummyPublisher()
        {
            _log = Log.ForContext<DummyPublisher>();
        }

        public void Dispose()
        {
        }

        public void Publish(IDataPoint dataPoint)
        {
            Task.Run(async () => await PublishAsync(dataPoint));
        }

        public Task PublishAsync(IDataPoint dataPoint)
        {
            _log.Information(dataPoint.ToString());
            return default;
        }
    }
}
