using MARM.Enums;

namespace MARM.Services;

public class DeviceStateManager : ITargetConnectStateManager, ILightController, ITransmitterDeviceManager, IPageNavigationService
{
    public event Action<TargetConnectState>? MainTargetConnectStateChanged;
    public TargetConnectState MainTargetConnectState { get; private set; }

    public event Action<TargetConnectState>? SubTargetConnectStateChanged;
    public TargetConnectState SubTargetConnectState { get; private set; }

    public event Action<int, int>? BatteryLevelChanged;
    public int BatteryLevel1 { get; private set; }
    public int BatteryLevel2 { get; private set; }

    public event Action<int, bool>? LightStateChanged;

    private int _pageIndex;

    public event Action<int>? PageChanged;
    public int TotalPage { get; set; }

    //private Dictionary<int, int> _pageNames = new Dictionary<int, int> { { (int)ButtonMode.Home, 0 }, { (int)ButtonMode.Setting, 1 }, { (int)ButtonMode.Data, 2 }, { (int)ButtonMode.ShotResult, 3 } };
    private byte[] _pageNameIndexs = { (byte)ButtonMode.Home, (byte)ButtonMode.Setting, (byte)ButtonMode.Data , (byte)ButtonMode.ShotResult };
    
    public bool Light1 { get; private set; }
    public bool Light2 { get; private set; }
    public bool Light3 { get; private set; }
    public bool Light4 { get; private set; }

    private DataSendService _dataSendService;
    public DeviceStateManager(DataSendService dataSendService)
    {
        _dataSendService = dataSendService;
        _dataSendService.RemoteStateReceived += _dataSendService_RemoteStateReceived;
        _dataSendService.ButtonReceived += _dataSendService_ButtonReceived;
        MainTargetConnectState = TargetConnectState.Lost;
        SubTargetConnectState = TargetConnectState.Lost;
        BatteryLevel1 = 50;
        BatteryLevel2 = 50;
        _pageIndex = 0;
    }

    private void _dataSendService_ButtonReceived(byte buttonAddress)
    {
        if (buttonAddress == (byte)ButtonMode.Home) NavigateTo(Array.IndexOf(_pageNameIndexs, (byte)ButtonMode.Home));
        if (buttonAddress == (byte)ButtonMode.Setting) NavigateTo(Array.IndexOf(_pageNameIndexs, (byte)ButtonMode.Setting));
        if (buttonAddress == (byte)ButtonMode.Data) NavigateTo(Array.IndexOf(_pageNameIndexs, (byte)ButtonMode.Data));
        if (buttonAddress == (byte)ButtonMode.ShotResult) NavigateTo(Array.IndexOf(_pageNameIndexs, (byte)ButtonMode.ShotResult));
        if (buttonAddress == (byte)ButtonMode.PrevPage) NavigateForward();
        if (buttonAddress == (byte)ButtonMode.BackPage) NavigateBack();
    }

    private void _dataSendService_RemoteStateReceived(byte[] data)
    {
        if (data[0] == (byte)RemoteAddress.Main)
        {
            SetBattery((int)RemoteAddress.Main, (int)data[1]);
            SetMainTargetConnectState(TargetConnectState.Good);
        }
        if (data[0] == (byte)RemoteAddress.Sub)
        {
            SetBattery((int)RemoteAddress.Sub, (int)data[1]);
            SetSubTargetConnectState(TargetConnectState.Good);
        }
    }

    public void SetMainTargetConnectState(TargetConnectState targetConnectState)
    {
        if (MainTargetConnectState == targetConnectState) return;

        MainTargetConnectState = targetConnectState;
        MainTargetConnectStateChanged?.Invoke(targetConnectState);
    }

    public void SetSubTargetConnectState(TargetConnectState targetConnectState)
    {
        if (SubTargetConnectState == targetConnectState) return;

        SubTargetConnectState = targetConnectState;
        SubTargetConnectStateChanged?.Invoke(targetConnectState);
    }

    private async Task TurnLight(int index, bool state)
    {
        await Task.Delay(100);
        switch (index)
        {
            case 0:
                if (Light1 != state)
                {
                    Light1 = state;
                    await _dataSendService.RemoteLightControl(0, state);
                    LightStateChanged?.Invoke(0, state);
                }
                break;
            case 1:
                if (Light2 != state)
                {
                    Light2 = state;
                    await _dataSendService.RemoteLightControl(1, state);
                    LightStateChanged?.Invoke(1, state);
                }
                break;
            case 2:
                if (Light3 != state)
                {
                    Light3 = state;
                    await _dataSendService.RemoteLightControl(2, state);
                    LightStateChanged?.Invoke(2, state);
                }
                break;
            case 3:
                if (Light4 != state)
                {
                    Light4 = state;
                    await _dataSendService.RemoteLightControl(3, state);
                    LightStateChanged?.Invoke(3, state);
                }
                break;
        }
    }

    public Task TurnOnLight(int index) => TurnLight(index, true);

    public Task TurnOffLight(int index) => TurnLight(index, false);

    public void SetBatteryLevel(int level1, int level2)
    {
        if (level1 != BatteryLevel1 || level2 != BatteryLevel2)
        {
            BatteryLevel1 = level1;
            BatteryLevel2 = level2;
            BatteryLevelChanged?.Invoke(BatteryLevel1, BatteryLevel2);
        }
    }

    public void SetBattery(int batteryNumber, int level)
    {
        if(batteryNumber == 0)
        {
            if(level != BatteryLevel1)
            {
                BatteryLevel1 = level;
                BatteryLevelChanged?.Invoke(BatteryLevel1, BatteryLevel2);
            }
        }
        if (batteryNumber == 1)
        {
            if (level != BatteryLevel2)
            {
                BatteryLevel2 = level;
                BatteryLevelChanged?.Invoke(BatteryLevel1, BatteryLevel2);
            }
        }
    }

    
    #region NextPage
    private List<int> pageLightCode = new List<int> { 4,3,2,1};
    public async void NavigateTo(int pageIndex)
    {
        await Task.Yield();

        if (pageIndex < 0 || pageIndex > TotalPage) _pageIndex = 0;
        else _pageIndex = pageIndex;
        PageChanged?.Invoke(_pageIndex);
        for (int i = 0; i < pageLightCode.Count; i++)
        {
            if (i == pageIndex)
                await _dataSendService.LightControl(pageLightCode[i], true);
            else
                await _dataSendService.LightControl(pageLightCode[i], false);
        }
        if (_pageIndex == 0) await _dataSendService.RemoteUpdateStatus();
    }

    public async void NavigateBack()
    {
        await Task.Yield();

        if (_pageIndex == 0)
        {
            _pageIndex = TotalPage - 1; 
        }
        else
        {
            _pageIndex--;
        }
        PageChanged?.Invoke(_pageIndex);
        for (int i = 0; i < pageLightCode.Count; i++)
        {
            if (i == _pageIndex)
                await _dataSendService.LightControl(pageLightCode[i], true);
            else
                await _dataSendService.LightControl(pageLightCode[i], false);
        }
        if (_pageIndex == 0) await _dataSendService.RemoteUpdateStatus();
    }

    public async void NavigateForward()
    {
        await Task.Yield();

        _pageIndex = (_pageIndex + 1) % TotalPage;
        PageChanged?.Invoke(_pageIndex);
        for (int i = 0; i < pageLightCode.Count; i++)
        {
            if (i == _pageIndex)
                await _dataSendService.LightControl(pageLightCode[i], true);
            else
                await _dataSendService.LightControl(pageLightCode[i], false);
        }
        if (_pageIndex == 0) await _dataSendService.RemoteUpdateStatus();
    }
    #endregion
}
