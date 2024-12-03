using MARM.Enums;
namespace MARM.Services
{
    public class DataSendService : IDataSendService
    {
        private readonly ComDataService _comDataService;
        private int _indexSend;
        private bool _isSend = false;
        public bool IsSend {  get { return _isSend; } set { if(value!=_isSend) _isSend = value; } }

        public event Action<byte>? ButtonReceived;
        public event Action<byte[]>? RemoteStateReceived;
        public event Action<byte[]>? ShotStateReceived;
        public event Action<byte[]>? LeakStateReceived;


        public DataSendService(ComDataService comDataService)
        {
            _comDataService = comDataService;
            _comDataService.DataReceived += _comDataService_DataReceived;
        }

        private void _comDataService_DataReceived(byte[] buffer)
        { 
            if(buffer.Length<6) return;
            byte[] dataReceived = buffer;// ReceivedFrame(buffer);
            if (dataReceived.Length != 0 && dataReceived != null)
            {
                var commandType = dataReceived[3];
                switch (commandType)
                {
                    case (byte)CommandType.ButtonCallback:
                        OnButtonReceived(dataReceived[4]);
                        break;
                    case (byte)CommandType.RemoteStateCallback:
                        // Get data from frame
                        if (dataReceived.Length == 9)
                        {
                            byte[] dataState = { dataReceived[4], dataReceived[5], dataReceived[6] };
                            OnRemoteStateReceived(dataState);
                        }
                        break;
                    case (byte)CommandType.RemoteShotCallback:
                        // Get data from frame
                        byte[] dataShot = { dataReceived[4], dataReceived[5] };
                        OnShotStateReceived(dataShot);
                        break;
                    case (byte)CommandType.RemoteLeakCallback:
                        // Get data from frame
                        byte[] dataLeak = { dataReceived[4], dataReceived[5] };
                        OnLeakStateReceived(dataLeak);
                        break;
                    default:
                        break;
                }
                _isSend = false;
            }
        }

        protected virtual void OnButtonReceived(byte data)
        {
            ButtonReceived?.Invoke(data);
        }

        protected virtual void OnRemoteStateReceived(byte[] data)
        {
            RemoteStateReceived?.Invoke(data);
        }

        protected virtual void OnShotStateReceived(byte[] data)
        {
            ShotStateReceived?.Invoke(data);
        }

        protected virtual void OnLeakStateReceived(byte[] data)
        {
            LeakStateReceived?.Invoke(data);
        }

        public async Task LightControl(int lightNumber, bool state)
        {
            _isSend = true;
            if (!_comDataService.IsConnected()) return;
            byte[] frame = new byte[2];
            frame[0] = 0x05;

            byte byteValue = 0x00; // Khởi tạo byte với giá trị ban đầu là 0x00
            byteValue |= (byte)(lightNumber << 4); // Dịch số 3 vào 4 bit cao
            if (state == true) byteValue |= (byte)(1 << 2);  // Dịch số 1 vào 4 bit thấp
            else byteValue |= (byte)(0 << 2);

            byteValue >>= 1;

            frame[1] = byteValue;
            //Console.WriteLine("Light control: " + BitConverter.ToString(frame));
            await SendFrame(frame);
        }

        public async Task RemoteLightControl(int lightNumber, bool state)
        {
            _isSend = true;
            if (!_comDataService.IsConnected()) return;
            byte[] frame = new byte[3];
            frame[0] = 0x06;
            frame[1] = 0x00;

            byte byteValue = 0x00; // Khởi tạo byte với giá trị ban đầu là 0x00
            byteValue |= (byte)(lightNumber << 4); // Dịch số 3 vào 4 bit cao
            if (state == true) byteValue |= (byte)(1 << 2);  // Dịch số 1 vào 4 bit thấp
            else byteValue |= (byte)(0 << 2);

            byteValue >>= 1;

            frame[2] = byteValue;
            //Console.WriteLine("Remote light control: " + BitConverter.ToString(frame));
            await SendFrame(frame);
        }

        public async Task RemoteLightBlinkControl(int lightNumber, bool state)
        {
            _isSend = true;
            if (!_comDataService.IsConnected()) return;
            byte[] frame = new byte[3];
            frame[0] = 0x06;
            frame[1] = 0x00;

            byte byteValue = 0x00; // Khởi tạo byte với giá trị ban đầu là 0x00
            byteValue |= (byte)(lightNumber << 4); // Dịch số 3 vào 4 bit cao
            if (state == true) byteValue |= (byte)(1 << 3); // Dịch số 1 vào 4 bit thấp
            else byteValue |= (byte)(0 << 3);

            byteValue >>= 1;

            frame[2] = byteValue;
            //Console.WriteLine("Remote light control: " + BitConverter.ToString(frame));
            await SendFrame(frame);
        }

        public async Task RemoteUpdateStatus()
        {
            _isSend = true;
            if (!_comDataService.IsConnected()) return;
            byte[] frame = new byte[1];
            frame[0] = 0x01;
            await SendFrame(frame);
        }

        private async Task SendFrame(byte[] data)
        {
            byte[] dataSend = new byte[data.Length + 5];
            if (_indexSend == 255) _indexSend = 0;
            _indexSend++;
            dataSend[0] = 0x01;
            dataSend[1] = (byte)dataSend.Length;
            dataSend[2] = (byte)_indexSend;
            Array.Copy(data, 0, dataSend, 3, data.Length);
            dataSend[dataSend.Length - 1] = 0x03;
            dataSend[dataSend.Length - 2] = GetCRC(dataSend);
            Console.WriteLine("Frame Send: " + BitConverter.ToString(dataSend));

            await _comDataService.SendByte(dataSend);
        }

        private byte GetCRC(byte[] data)
        {
            byte CRC = 0;
            for (int i = 1; i < data.Length - 2; i++)
            {
                CRC ^= data[i];
            }
            return CRC;
        }

        private bool CheckCRC(byte[] buffer)
        {
            byte CRC = 0;
            for (int i = 1; i < buffer.Length - 2; i++)
            {
                CRC ^= buffer[i];
            }
            if (CRC == buffer[buffer.Length - 2]) return true;
            else return false;
        }

        private byte[] ReceivedFrame(byte[] buffer)
        {
            int startFrameIndex = Array.IndexOf(buffer, (byte)0x02);
            int endFrameIndex = Array.IndexOf(buffer, (byte)0x03);
            if (startFrameIndex >= 0 && endFrameIndex >= 0)
            {
                byte[] receivedFrame = new byte[endFrameIndex - startFrameIndex + 1];

                Array.Copy(buffer, startFrameIndex, receivedFrame, 0, endFrameIndex - startFrameIndex + 1);

                Console.WriteLine("Frame Received: " + BitConverter.ToString(receivedFrame));
                if (CheckCRC(receivedFrame))
                    return receivedFrame;
                else return Array.Empty<byte>();
            }
            else
            {
                Console.WriteLine("Frame in received data was not found");
                return Array.Empty<byte>();
            }
        }
    }
}
