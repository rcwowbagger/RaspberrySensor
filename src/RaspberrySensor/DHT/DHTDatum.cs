using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReaspberrySensor.DHT
{
    public class DHTDatum : IDataPoint
    {
        public DateTime Timestamp { get; set; }
        public float TempCelcius { get; set; }
        public float TempFahrenheit { get; set; }
        public float Humidity { get; set; }
        public double HeatIndex { get; set; }
        public string Device { get => _device; }

        string _device = Environment.MachineName;

        public Dictionary<string, object> GetMeasurements()
        {
            return new Dictionary<string, object>()
            {
                {"TempCelcius", TempCelcius},
                {"Humidity",Humidity },
                {"HeatIndex",HeatIndex }
            };
        }

        public Dictionary<string, string> GetMetrics()
        {
            return new Dictionary<string, string>()
            {
                { "Device", Device},
            };

        }

        public string ToJson() => JsonConvert.SerializeObject(this);

        public byte[] ToByteArray() => Encoding.UTF8.GetBytes(this.ToJson());

        public override string ToString()
        {
            return $"[{Timestamp.ToString("HH:mm:ss.fff")}]\t[{TempCelcius}C]\t[{Humidity}%] \t[{HeatIndex.ToString("N2")}HI]";

        }
    }
}
