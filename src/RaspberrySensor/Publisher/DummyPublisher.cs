using RaspberrySensor.Device;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberrySensor.Publisher
{
    public class DummyPublisher : IPublisher
    {
        private readonly ILogger _log;

        public DummyPublisher()
        {
            _log = Log.ForContext<DummyPublisher>();
        }

        public void Publish(IDataPoint dataPoint)
        {
            _log.Information(dataPoint.ToString());
        }
    }
}
