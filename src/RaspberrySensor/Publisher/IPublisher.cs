using RaspberrySensor.Device;
using System;
using System.Threading.Tasks;

namespace RaspberrySensor.Publisher
{
    public interface IPublisher: IDisposable
    {
        void Publish(IDataPoint dataPoint);
        Task PublishAsync(IDataPoint dataPoint);
    }
}
