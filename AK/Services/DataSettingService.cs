namespace AK.Services
{
    public class DataSettingService : IDataSettingService
    {
        private bool _Transmit1 { get; set; } = true;
        public event Action<bool>? Transmit1Changed;
        private bool _Transmit2 { get; set; }
        public event Action<bool>? Transmit2Changed;
        private bool _Transmit3 { get; set; }
        public event Action<bool>? Transmit3Changed;
        private bool _Transmit4 { get; set; }
        public event Action<bool>? Transmit4Changed;

        private int _TimerInterval { get; set; } = 500;
        public event Action<int>? TimerIntervalChanged;
        private int _Light1Mode { get; set; }
        public event Action<int>? Light1ModeChanged;
        private int _Light2Mode { get; set; }
        public event Action<int>? Light2ModeChanged;
        private int _Light3Mode { get; set; }
        public event Action<int>? Light3ModeChanged;
        private int _Light4Mode { get; set; }
        public event Action<int>? Light4ModeChanged;
        private int _Baudrate { get; set; } = 57600;
        public event Action<int>? BaudrateChanged;

        private string _Port { get; set; } = "COM16";
        public event Action<string>? PortChanged;


        public bool Transmit1 { get { return _Transmit1; } set { _Transmit1 = value; Transmit1Changed?.Invoke(_Transmit1); } }
        public bool Transmit2 { get { return _Transmit2; } set { _Transmit2 = value; Transmit2Changed?.Invoke(_Transmit1); } }
        public bool Transmit3 { get { return _Transmit3; } set { _Transmit3 = value; Transmit3Changed?.Invoke(_Transmit1); } }
        public bool Transmit4 { get { return _Transmit4; } set { _Transmit4 = value; Transmit4Changed?.Invoke(_Transmit1); } }

        public int TimerInterval { get { return _TimerInterval; } set { _TimerInterval = value; TimerIntervalChanged?.Invoke(_TimerInterval); } }
        public int Light1Mode { get { return _Light1Mode; } set { _Light1Mode = value; Light1ModeChanged?.Invoke(_Light1Mode); } }
        public int Light2Mode { get { return _Light2Mode; } set { _Light2Mode = value; Light2ModeChanged?.Invoke(_Light2Mode); } }
        public int Light3Mode { get { return _Light3Mode; } set { _Light3Mode = value; Light3ModeChanged?.Invoke(_Light3Mode); } }
        public int Light4Mode { get { return _Light4Mode; } set { _Light4Mode = value; Light4ModeChanged?.Invoke(_Light4Mode); } }
        public int Baudrate { get { return _Baudrate; } set { _Baudrate = value; BaudrateChanged?.Invoke(_Baudrate); } }

        public string Port { get { return _Port; } set { _Port = value; PortChanged?.Invoke(_Port); } }

    }
}
