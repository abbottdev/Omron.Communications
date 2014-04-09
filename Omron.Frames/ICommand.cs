using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Omron.Frames
{
    public interface ICommandFrame
    {
        byte[] Parameter { get; set; }

        byte[] BuildFrame();
    }
      
}
