using RaspberrySensor.Device;

namespace RaspberrySensor.Publisher
{
    public interface IPublisher
    {
        void Publish(IDataPoint dataPoint);
    }
}
