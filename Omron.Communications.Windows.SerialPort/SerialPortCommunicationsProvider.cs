using Omron.Core;
using Omron.Core.Frames;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omron.Communications.Windows.SerialPort
{
    public class SerialPortCommunicationProvider : IConnection
    {
        private System.IO.Ports.SerialPort port;

        public async System.Threading.Tasks.Task<bool> ConnectAsync(PlcConfiguration device)
        {
            port = new System.IO.Ports.SerialPort(device.Port);
            //TOOD: Configure the port based on settings 
            port = new System.IO.Ports.SerialPort(device.Port,
                Convert.ToInt32(device.Options["BaudRate"]),
                (Parity)Enum.Parse(typeof(Parity), device.Options["Parity"]),
                Convert.ToInt32(device.Options["DataBits"]), (StopBits)Enum.Parse(typeof(StopBits), device.Options["StopBits"]));

            port.Open();

            return true;
        }

        public void Disconnect()
        {
            port.Close();
        }

        public async System.Threading.Tasks.Task SendAsync(Omron.Core.Frames.Frame frame)
        {
            byte[] bytes = frame.BuildFrame();

            await Task.Factory.StartNew(() =>
            {
                port.Write(bytes, 0, bytes.Length - 1);
            });
        }

        public async System.Threading.Tasks.Task<Frame> ReceiveAsync()
        {
            return await Task<Frame>.Factory.StartNew(() =>
            {
                byte[] bytes;

                bytes = new byte[port.BytesToRead - 1];

                port.Read(bytes, 0, port.BytesToRead - 1);

                return new Frame(bytes);

            });
        }


        public bool Connected
        {
            get
            {
                if (port != null)
                    return port.IsOpen;

                return false;
            }
        }


        public ProtocolTypes ProtocolType
        {
            get { return ProtocolTypes.FinsHostLink; }
        }
    }
}
