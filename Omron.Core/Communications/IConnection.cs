using Omron.Core;
using Omron.Core.Frames;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omron.Core
{
    public interface ITransport
    {
        Task<bool> ConnectAsync(PlcConfiguration device);
        void Disconnect();
        Task SendAsync(Frame frame); 
        Task<Frame> ReceiveAsync();

        ProtocolTypes ProtocolType { get; }

        bool Connected { get; }
    }
}
