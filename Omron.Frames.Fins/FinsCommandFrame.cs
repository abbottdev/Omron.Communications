using Omron.Frames;
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

        public FinsCommandFrame(byte[] parameterDataField)
            : base(12 + parameterDataField.Length)
        {
            _canResize = false;
            this.Header = new FinsCommandHeader(this);
        }

        internal enum FinsCommandCodes
        {
            MemoryAreaRead_MainRequestCode = 01,
            MemoryAreaRead_SubRequestCode = 01,
            MemoryAreaWrite_MainRequestCode = 01,
            MemoryAreaWrite_SubRequestCode = 02
        }




        public enum FinsCommands
        {
            Unknown = 0,
            MemoryAreaRead = 01,
            MemoryAreaWrite = 02
        }

        public FinsCommands Command
        {
            get
            {
                byte mrc, src;

                mrc = GetByte((int)FinsCommandFields.MRC);
                src = GetByte((int)FinsCommandFields.SRC);

                switch (mrc)
                {
                    case 1:
                        //Read, Write, Fill, Mutli read, transfer
                        switch (src)
                        {
                            case 1:
                                return FinsCommands.MemoryAreaRead;
                            case 2:
                                return FinsCommands.MemoryAreaWrite;
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
                        this.SetByte((int)FinsCommandFields.MRC, (byte)FinsCommandCodes.MemoryAreaRead_MainRequestCode);
                        this.SetByte((int)FinsCommandFields.SRC, (byte)FinsCommandCodes.MemoryAreaRead_SubRequestCode);
                        break;
                    case FinsCommands.MemoryAreaWrite:
                        this.SetByte((int)FinsCommandFields.MRC, (byte)FinsCommandCodes.MemoryAreaWrite_MainRequestCode);
                        this.SetByte((int)FinsCommandFields.SRC, (byte)FinsCommandCodes.MemoryAreaWrite_SubRequestCode);
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
            this.Header = new FinsCommandHeader(this);
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
