using System;
using System.Collections.Generic;

namespace RaspberrySensor.Device
{
    public interface IDataPoint
    {
        DateTime Timestamp { get;}
        Dictionary<string, string> GetMetrics();
        Dictionary<string, object> GetMeasurements();
    }
}