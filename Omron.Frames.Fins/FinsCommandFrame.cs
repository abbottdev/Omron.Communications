using Omron.Core.Frames;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace Omron.Commands.Frames.Fins
{

    public partial class FinsCommandFrame : Frame
    {
        public const int BITS_PER_BYTE = 8;

        private bool _canResize;

        public enum FinsHeaderFields
        {
            ICF = 0,
            RSV,
            GCT,
            DNA,
            DA1,
            DA2,
            SNA,
            SA1,
            SA2,
            SID
        }

        public enum FinsCommandFields
        {
            MRC = FinsHeaderFields.SID + 1,
            SRC = MRC + 1
        }

        public FinsCommandHeader Header { get; private set; }

        public enum FinsMainRequestCodes
        {
            IO_MemoryAreaAccess = 01, 
            StatusReading = 06
        }

        public enum FinsSubRequestCodes
        {
            MemoryAreaRead = 01,
            MemoryAreaWrite = 02,
            CycleTimeRead = 20
        }



        public enum FinsCommands
        {
            Unknown = 0,
            MemoryAreaRead = 01,
            MemoryAreaWrite = 02,
            CycleTimeRead = 06
        }

        public FinsCommands Command
        {
            get
            {
                byte mrc, src;

                mrc = GetByte((int)FinsCommandFields.MRC);
                src = GetByte((int)FinsCommandFields.SRC);

                switch ((FinsMainRequestCodes)mrc)
                {
                    case FinsMainRequestCodes.IO_MemoryAreaAccess: 
                        //Read, Write, Fill, Mutli read, transfer
                        switch (src)
                        {
                            case (int)FinsSubRequestCodes.MemoryAreaRead:
                                return FinsCommands.MemoryAreaRead;
                            case (int)FinsSubRequestCodes.MemoryAreaWrite:
                                return FinsCommands.MemoryAreaWrite;
                            default:
                                break;
                        }

                        break;
                    case FinsMainRequestCodes.StatusReading:
                        switch (src)
                        {
                            case (int)FinsSubRequestCodes.CycleTimeRead:
                                return FinsCommands.CycleTimeRead;
                            default:
                                break;
                        }

                        break;
                    default:
                        break;
                }

                return FinsCommands.Unknown;
            }
            set
            {
                switch (value)
                {
                    case FinsCommands.MemoryAreaRead:
                        this.SetByte((int)FinsCommandFields.MRC, (byte)FinsMainRequestCodes.IO_MemoryAreaAccess);
                        this.SetByte((int)FinsCommandFields.SRC, (byte)FinsSubRequestCodes.MemoryAreaRead);
                        break;
                    case FinsCommands.MemoryAreaWrite:
                        this.SetByte((int)FinsCommandFields.MRC, (byte)FinsMainRequestCodes.IO_MemoryAreaAccess);
                        this.SetByte((int)FinsCommandFields.SRC, (byte)FinsSubRequestCodes.MemoryAreaWrite);
                        break;
                    case FinsCommands.CycleTimeRead:
                        this.SetByte((int)FinsCommandFields.MRC, (byte)FinsMainRequestCodes.StatusReading);
                        this.SetByte((int)FinsCommandFields.SRC, (byte)FinsSubRequestCodes.CycleTimeRead);
                        break;
                    default:
                        break;
                }
            }
        }

        public FinsCommandFrame()
            : base(12)
        {
            _canResize = true;
            this.Header = new FinsCommandHeader(this, true);
        }


        public FinsCommandFrame(Frame originalFrame) : base(originalFrame.BuildFrame())
        {
            _canResize = false;
            this.Header = new FinsCommandHeader(this, false);
        }

        private void SetParameterDataField(byte[] field)
        {
            Contract.Assert(_canResize, "Unable to set parameter data as this frame was initialised with a parameter already");
            base.Resize((int)FinsCommandFields.SRC + 1 + field.Length);
            base.SetBytes((int)FinsCommandFields.SRC + 1, field);
        }

        public byte[] Parameter
        {
            set
            {
                SetParameterDataField(value);
            }
            get
            {
                return GetRange((int)FinsCommandFields.SRC + 1, Length);
            }
        }



    }
}
