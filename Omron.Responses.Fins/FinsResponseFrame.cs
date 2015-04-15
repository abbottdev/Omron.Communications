using Omron.Core.Frames;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Omron.Responses.Fins
{
    public class FinsResponseFrame : Frame
    {

        public byte MainCommandCode
        {
            get
            {
                return this.GetByte(10);
            }
        }

        public byte SubCommandCode
        {
            get
            {
                return this.GetByte(11);
            }

        }

        public byte EndCodeCategory
        {
            get
            {
                return this.GetByte(12);
            }
        }

        public byte EndCodeDetails
        {
            get
            {
                return this.GetByte(13);
            }
        }

        public FinsResponseFrame(Frame original)
            : base(original.BuildFrame())
        {

        }
        public FinsResponseFrame(byte[] buffer)
            : base(buffer)
        {

        }


        private static readonly int FinsHeaderSize = 10;
        private static readonly int FinsCommandCodeSize = 2;
        private static readonly int FinsEndCodeSize = 2;

        public byte[] Data
        {
            get
            {
                return base.GetRange(FinsHeaderSize + FinsCommandCodeSize + FinsEndCodeSize, base.Length);
            }
        }
    }
}
