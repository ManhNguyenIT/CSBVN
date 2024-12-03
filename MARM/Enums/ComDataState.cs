namespace MARM.Enums
{
    public enum RemoteAddress : byte
    {
        Main = 0x00,
        Sub = 0x01,
        Main1 = 0x02,
        Sub1 = 0x03,
    }
    public enum CommandType : byte
    {
        
        RemoteStateCallback = 0x01,
        RemoteShotCallback = 0x02,
        RemoteLeakCallback = 0x03,
        ButtonCallback = 0x04,
        Output = 0x05,
        RemoteOutput = 0x06,

    }
    public enum ButtonMode : byte
    {
        ErrorConfirm = 0x00,
        Shot = 0x01,
        AddList = 0x02,
        StopShot = 0x03,
        BackShot = 0x04,

        PrevPage = 0x05,
        BackPage = 0x06,
        Home = 0x07,
        Setting = 0x0C,
        Data = 0x0D,
        ShotResult = 0x0E,
        PrevShot = 0x0F,
    }
}
