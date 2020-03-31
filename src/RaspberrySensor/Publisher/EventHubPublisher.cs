using Microsoft.Azure.EventHubs;
using Newtonsoft.Json;
using RaspberrySensor.Device;
using Serilog;
using System;
using System.Text;
using System.Threading.Tasks;

namespace RaspberrySensor.Publisher
{
    public class EventHubPublisher : IPublisher,IDisposable
    {
        private readonly string _connectionString;
        private readonly ILogger _log;
        private readonly EventHubClient _eventHubClient;
        public EventHubPublisher(string connectionString, string key)
        {
            _connectionString = connectionString;
            _log = Log.ForContext<EventHubPublisher>();
            var connectionStringBuilder = new EventHubsConnectionStringBuilder(_connectionString)
            {
                EntityPath = key
            };

            _eventHubClient = EventHubClient.CreateFromConnectionString(_connectionString);
        }

        public async void Dispose()
        {
            await _eventHubClient.CloseAsync();
        }

        public void Publish(IDataPoint dataPoint)
        {
            try
            {
                Task.Run(async () => await PublishAsync(dataPoint));
                _log.Debug($"Published {dataPoint.ToString()}");
            }
            catch(Exception ex)
            {
                _log.Error(ex, "Error publishing to eventhub");
            }
        }

        public async Task PublishAsync(IDataPoint dataPoint)
        {
            var message = JsonConvert.SerializeObject(dataPoint);
            await _eventHubClient.SendAsync(new EventData(Encoding.UTF8.GetBytes(message)));

            return;
        }
    }
}
