namespace MARM.Services
{
    public interface IDataSettingService
    {
        bool Transmit1 { get; set; }
        event Action<bool> Transmit1Changed;
        bool Transmit2 { get; set; }
        event Action<bool> Transmit2Changed;
        bool Transmit3 { get; set; }
        event Action<bool> Transmit3Changed;
        bool Transmit4 { get; set; }
        event Action<bool> Transmit4Changed;

        int TimerInterval { get; set; }
        event Action<int> TimerIntervalChanged;

        int Light1Mode { get; set; }
        event Action<int> Light1ModeChanged;
        int Light2Mode { get; set; }
        event Action<int> Light2ModeChanged;
        int Light3Mode { get; set; }
        event Action<int> Light3ModeChanged;
        int Light4Mode { get; set; }
        event Action<int> Light4ModeChanged;

        int Baudrate { get; set; }
        event Action<int> BaudrateChanged;
        string Port { get; set; }
        event Action<string> PortChanged;
    }
}
