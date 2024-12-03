namespace MARM.Services
{
    public interface IComDataService
    {
        event Action<byte[]>? DataReceived;
        void Open(string comPort, int baudrate);
        void Close();
        bool IsConnected();
        Task<byte[]> ReadData();
        Task SendByte(byte[] data);
    }
}
