using Omron.Commands;
using Omron.Core.Frames;
using Omron.Responses.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Omron.Responses.Fins
{
    public interface IResponseForConnectionCommand : IResponse<IConnectionCommand>
    {
        int SourceUnitAddress { get; }
    }

    public class ConnectionResponse : IResponseForConnectionCommand
    {
        private Core.Frames.Frame frame;

        public ConnectionResponse(Frame frame)
        {
            this.frame = frame;
        }

        public int SourceUnitAddress
        {
            get { return frame.GetByte(38); }
        }
    }
}
