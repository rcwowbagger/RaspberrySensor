namespace RaspberrySensor.Device
{
    public interface IDevice
    {
        IDataPoint ReadData();
    }
}
