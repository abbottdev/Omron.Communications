using Omron.Core;
using Omron.Frames;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omron.Communications
{
    public interface ICommuncationProvider
    {
        Task<bool> ConnectAsync(PlcConfiguration device);
        void Disconnect();
        Task SendAsync(Frame frame); 
        Task<Frame> ReceiveAsync();

        bool Connected { get; }
    }
}
