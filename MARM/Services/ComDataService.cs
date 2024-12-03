
using MARM.Enums;
using System.IO.Ports;
using System;

namespace MARM.Services
{
    public class ComDataService : IComDataService
    {
        public event Action<byte[]>? DataReceived;

        private SerialPort? serialPort;
        private bool isOpened = false;
        
        private byte[] buffer = new byte[255];


        public void Open(string comPort, int baudrate)
        {
            try
            {
                serialPort = new SerialPort(comPort, baudrate);
                serialPort.DataReceived += SerialPort_DataReceived;
                serialPort.Open();
                isOpened = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                isOpened = false;
            }
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            int byteReceivedLength = sp.Read(buffer,0,buffer.Length);
            if(byteReceivedLength > 0)
            {
                var dataReceiveds = new byte[byteReceivedLength];
                Array.Copy(buffer, dataReceiveds, byteReceivedLength);
                Console.WriteLine($"Data received: {BitConverter.ToString(dataReceiveds)}");
                OnDataReceived(dataReceiveds);
            }
        }

        protected virtual void OnDataReceived(byte[] data)
        {
            DataReceived?.Invoke(data);
        }

        public void Close()
        {
            try
            {
                if (serialPort != null)
                {
                    serialPort.DataReceived -= SerialPort_DataReceived;
                    serialPort.Close();
                    isOpened = false;
                }
                else { Console.WriteLine("Serial port is null"); }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        public async Task<byte[]> ReadData()
        {
            await Task.Delay(100);
            byte[] data = new byte[255];
            if(serialPort != null)
            {
                if (isOpened)
                {
                    int lengthAvailble = serialPort.Read(data, 0, data.Length);
                    if (lengthAvailble > 0)
                    {
                        var dataAvailble = new byte[lengthAvailble];
                        Array.Copy(data, dataAvailble, dataAvailble.Length);
                        return dataAvailble;
                    }
                }
            }
            return Array.Empty<byte>();
        }

        public bool IsConnected()
        {
            return serialPort!=null?serialPort.IsOpen:false;
        }

        public async Task SendByte(byte[] data)
        {
            await Task.Delay(100);
            if (serialPort != null)
            {
                if(isOpened)
                {
                    serialPort.Write(data, 0, data.Length);
                }
                
            }
        }
    }
}
