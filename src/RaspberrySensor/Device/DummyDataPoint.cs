using RaspberrySensor;
using System;
using System.Collections.Generic;

namespace RaspberrySensor.Device
{
    public class DummyDataPoint : IDataPoint
    {
        private static Random _random = null;

        public DummyDataPoint()
        {
            if (_random == null)
            {
                _random = new Random();
            }
        }

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public Dictionary<string, object> GetMeasurements()
        {
            return new Dictionary<string, object>()
            {
                {"TempCelcius", _random.Next(10,30)},
                {"Humidity",_random.Next(10,30) },
                {"HeatIndex",_random.Next(10,30) }
            };
        }

        public Dictionary<string, string> GetMetrics()
        {
            return new Dictionary<string, string>()
            {
                { "Device", "DummyDevice"},
            };
        }

        public override string ToString()
        {
            return $"[{Timestamp.ToString("HH:mm:ss.fff")}]\t[{this.GetMeasurements()["TempCelcius"]}C]\t[{this.GetMeasurements()["Humidity"]}%] \t[{this.GetMeasurements()["HeatIndex"]}HI]";

        }
    }
}
