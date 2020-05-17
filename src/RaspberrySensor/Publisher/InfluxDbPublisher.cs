using InfluxDB.LineProtocol.Client;
using InfluxDB.LineProtocol.Payload;
using RaspberrySensor.Device;
using System;
using System.Threading.Tasks;

namespace RaspberrySensor.Publisher
{
    public class InfluxDbPublisher : IPublisher
    {
        private readonly LineProtocolClient _client;
        private readonly string _uri;
        private readonly string _database;
        public InfluxDbPublisher(string uri, string database)
        {
            _client = new LineProtocolClient(new Uri(uri), database);
        }

        public void Dispose()
        {
        }

        public void Publish(IDataPoint dataPoint)
        {
            Task.Run(async () => await PublishAsync(dataPoint));
        }

        public async Task PublishAsync(IDataPoint dataPoint)
        {
            var point = new LineProtocolPoint(
                            "temperature",
                            dataPoint.GetMeasurements(),
                            dataPoint.GetMetrics(),
                            DateTime.UtcNow);

            var payload = new LineProtocolPayload();
            payload.Add(point);

            var influxResult = await _client.WriteAsync(payload);
            if (!influxResult.Success)
                Console.Error.WriteLine(influxResult.ErrorMessage);


            return;
        }
    }
}
