namespace RaspberrySensor.Device
{
    public class DummyDevice : IDevice
    {
        public IDataPoint ReadData()
        {
            return new DummyDataPoint();
        }
    }
}
