namespace MARM.Services;

public interface ITransmitterDeviceManager
{
    int BatteryLevel1 { get; }
    int BatteryLevel2 { get; }
    event Action<int, int> BatteryLevelChanged;
}
