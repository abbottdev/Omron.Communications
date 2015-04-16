using Omron.Commands.Frames.Fins;
using Omron.Core;
using Omron.Core.Frames;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Omron.Commands.Generators.Fins
{ 
    public class ConnectionFrameGenerator : IFrameGeneratorOf<IPostConnectionCommand>
    {

        public Frame Generate(IPostConnectionCommand command, PlcConfiguration configuration, ITransport provider)
        {
            //fixed frame to obtain an API No. node (EF in principle)
            const string API_NO_NODE_FRAME = "46494E530000000C000000000000000000000000";

            Frame frame;

            //Need to split this from content up into hex bytes of 2 hex characters.
            frame = new Frame(API_NO_NODE_FRAME.HexadecimalSplitToBytes());

            return frame;
        }
    }
}
