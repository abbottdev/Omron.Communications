using Omron.Core.Frames;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Omron.Responses.Fins
{
    public class FinsResponseFrame : Frame
    {
        
        public FinsResponseFrame(Frame original)
            : base(original.BuildFrame())
        {

        }
        public FinsResponseFrame(byte[] buffer)
            : base(buffer)
        {
             
        }



    }
}
