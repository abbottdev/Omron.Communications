using Omron.Commands;
using Omron.Core.Frames;
using Omron.Responses.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Omron.Responses.Fins
{
    public interface IResponseForConnectionCommand : IResponse<IPostConnectionCommand>
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

        public int SourceUnitAddress { get; private set; }

        public IPostConnectionCommand OriginalCommand { get; set; }

        public void Parse(Frame responseFrame)
        {
            //Don't need to do anything here really.
        }

        public object Response
        {
            get { return null; }
        }

        public void Parse(Frame commandFrame, Frame responseFrame)
        {
            this.SourceUnitAddress = responseFrame.GetByte(38);
        }
    }
}
