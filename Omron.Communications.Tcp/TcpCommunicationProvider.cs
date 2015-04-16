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
using System.Diagnostics;


namespace Omron.Transport.Windows.Tcp
{
    public class TcpCommunicationProvider : ITransport
    {
        private TcpClient client;
        private NetworkStream stream;

        /// <summary>
        /// Connects to a PLC
        /// </summary>
        /// <param name="device">The device configuration</param>
        /// <returns></returns>
        public async Task<bool> ConnectAsync(PlcConfiguration device)
        {
            client = new TcpClient();

            await client.ConnectAsync(device.Address, Convert.ToInt32(device.Port));

            if (client.Connected)
            {
                this.stream = client.GetStream();
            }

            return client.Connected;
        }

        /// <summary>
        /// Disconnects from the PLC
        /// </summary>
        public void Disconnect()
        {
            if (client != null && client.Connected)
                client.Close();

            if (stream != null)
                stream.Close();
        }

        /// <summary>
        /// Sends a Frame to the PLC
        /// </summary>
        /// <param name="frame">The <see cref="Omron.Core.Frames.Frame">Frame</see> to be sent to the PLC</param>
        /// <returns></returns>
        public async Task SendAsync(Frame frame)
        {
            byte[] bytes = frame.BuildFrame();

            Trace.WriteLine("Sending: " + Environment.NewLine +  frame.ToString());

            await stream.WriteAsync(bytes, 0, bytes.Length - 1);
        }

        public async Task<Frame> ReceiveAsync()
        {
            byte[] buffer;
            int read = 0;
            const int READ_SIZE = 512;

            buffer = new byte[client.ReceiveBufferSize];

            int chunk;

            while ((chunk = await stream.ReadAsync(buffer, read, buffer.Length - read)) > 0)
            {
                read += chunk;

                if (read == buffer.Length)
                {
                    int nextByte = stream.ReadByte();

                    if (nextByte == -1)
                    {
                        break;
                    }

                    byte[] newBuffer = new byte[buffer.Length * 2];
                    Array.Copy(buffer, newBuffer, buffer.Length);
                    newBuffer[read] = (byte)nextByte;
                    buffer = newBuffer;
                    read++;
                }
            }

            // Buffer is now too big. Shrink it.
            byte[] ret = new byte[read];
            Array.Copy(buffer, ret, read);

            var frame = new Frame(ret);

            Trace.WriteLine("Received: " + Environment.NewLine + frame.ToString());

            return frame;
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
