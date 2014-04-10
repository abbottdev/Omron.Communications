using Omron.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omron.Communications.Windows.Tcp
{
    public class TcpCommunicationProvider : ICommuncationProvider
    {
        private System.Net.Sockets.TcpClient client;

        public async System.Threading.Tasks.Task<bool> ConnectAsync(PlcConfiguration device)
        {
            client = new System.Net.Sockets.TcpClient();

            await client.ConnectAsync(device.Address, Convert.ToInt32(device.Port));

            //46494E530000000C000000000000000000000000
            //Need to send a fins command upon connection

            return client.Connected;
        }

        public void Disconnect()
        {
            if (client != null && client.Connected)
                client.Close();
        }

        public async System.Threading.Tasks.Task SendAsync(Omron.Frames.Frame frame)
        {
            byte[] bytes = frame.BuildFrame();

            await client.GetStream().WriteAsync(bytes, 0, bytes.Length - 1);
        }

        public async System.Threading.Tasks.Task<Omron.Frames.Frame> ReceiveAsync()
        {
            byte[] bytes;

            if (client.GetStream().DataAvailable)
            {
                bytes = new byte[client.Available - 1];
                await client.GetStream().ReadAsync(bytes, 0, client.Available - 1);
            }
            else
            {
                bytes = null;
            }
            return new Frames.Frame(bytes);
        }

        public bool Connected
        {
            get
            {

                if (client != null)
                {
                    return client.Connected;
                }

                return false;
            }
        }


        public CommunicationProviderTypes ProviderType
        {
            get { return CommunicationProviderTypes.TcpId; }
        }
    }
}
