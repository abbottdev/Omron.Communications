using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Omron.Transport.Windows.Udp
{

    public class UdpDatagram
    {
        public readonly IPEndPoint IpEndPoint;
        public readonly byte[] Data;

        public UdpDatagram(IPEndPoint endPoint, byte[] data)
        {
            this.IpEndPoint = endPoint;
            this.Data = data;
        }

    }
    public static class UdpClientExtensions
    {

        public static async Task<UdpDatagram> ReceiveAsync(this UdpClient client)
        {
            byte[] bytes;
            IPEndPoint ipEndPoint = null;

            var task = Task.Factory
                .FromAsync<byte[]>(
                    (callback, state) => client.BeginReceive(callback, state),
                    (IAsyncResult asyncResult) => client.EndReceive(asyncResult, ref ipEndPoint), null);

            bytes = await task;

            return new UdpDatagram(ipEndPoint, bytes);
        }
    }
}
