﻿using Omron.Core.Frames;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Omron.Responses.Fins
{
    public class FinsResponseFrame : Frame
    {

        public FinsResponseFrame(byte[] buffer)
            : base(buffer)
        {
             
        }

        private void ValidateFrameAsResponseFrame()
        {

        }

    }
}
