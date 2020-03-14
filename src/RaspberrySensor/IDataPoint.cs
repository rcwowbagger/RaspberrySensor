using System.Collections.Generic;

namespace ReaspberrySensor
{
    public interface IDataPoint
    {
        Dictionary<string, string> GetMetrics();
        Dictionary<string, object> GetMeasurements();
    }
}