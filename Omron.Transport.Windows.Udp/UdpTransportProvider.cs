
using Ninject;
using Omron.Core;
using Omron.Core.Frames;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;
using System.Net;
using System.Net.Sockets;


namespace Omron.Transport.Windows.Udp
{
    public class UdpCommunicationProvider : ITransport
    {
        private UdpClient client;
        private IPEndPoint endpoint;


        public UdpCommunicationProvider()
        {
            client = new System.Net.Sockets.UdpClient(9600);
        }

        public async Task<bool> ConnectAsync(PlcConfiguration device)
        {
            this.endpoint = new IPEndPoint(IPAddress.Parse(device.Address), Convert.ToInt32(device.Port));

            await Task.Factory.StartNew(() =>
            {
                client.Connect(this.endpoint);
            });

            return client.Client.Connected;
        }

        public void Disconnect()
        {
            if (client != null && client.Client.Connected)
                client.Close();
        }

        public async Task SendAsync(Omron.Core.Frames.Frame frame)
        {
            byte[] bytes = frame.BuildFrame();

            await client.SendAsync(bytes, bytes.Length);
        }

        public async Task<Frame> ReceiveAsync()
        {
            var result = await client.ReceiveAsync();

            return new Frame(result.Data);
        }

        public bool Connected
        {
            get
            {

                if (client != null)
                {
                    return client.Client.Connected;
                }

                return false;
            }
        }


        public ProtocolTypes ProtocolType
        {
            get { return ProtocolTypes.FinsUdp; }
        }
    }
}

