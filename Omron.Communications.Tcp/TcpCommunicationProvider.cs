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
using System.Net.Sockets;


namespace Omron.Transport.Windows.Tcp
{
    public class TcpCommunicationProvider : ITransport
    {
        private TcpClient client;
        
        /// <summary>
        /// Connects to a PLC
        /// </summary>
        /// <param name="device">The device configuration</param>
        /// <returns></returns>
        public async Task<bool> ConnectAsync(PlcConfiguration device)
        {
            client = new TcpClient();

            await client.ConnectAsync(device.Address, Convert.ToInt32(device.Port));
            
            return client.Connected;
        }

        /// <summary>
        /// Disconnects from the PLC
        /// </summary>
        public void Disconnect()
        {
            if (client != null && client.Connected)
                client.Close();
        }

        /// <summary>
        /// Sends a Frame to the PLC
        /// </summary>
        /// <param name="frame">The <see cref="Omron.Core.Frames.Frame">Frame</see> to be sent to the PLC</param>
        /// <returns></returns>
        public async Task SendAsync(Frame frame)
        {
            byte[] bytes = frame.BuildFrame();

            await client.GetStream().WriteAsync(bytes, 0, bytes.Length - 1);
        }

        public async Task<Frame> ReceiveAsync()
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
            return new Frame(bytes);
        }

        /// <summary>
        /// Returns if the current connection should be treated as connected.
        /// </summary>
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

        /// <summary>
        /// Returns the Protocol type in use (Ethernet/UDP/TCP/Serial)
        /// </summary>
        public ProtocolTypes ProtocolType
        {
            get { return ProtocolTypes.FinsTcpIp; }
        }
    }
}
