using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using Omron.Commands.Frames.Fins;

namespace Omron.Responses.Fins
{
    public class ReadCycleTimeResponse : Responses.IResponseForReadCycleTimeCommand
    {
        public TimeSpan CycleTime
        {
            get { throw new NotImplementedException(); }
        }

        public Commands.IReadCycleTimeCommand OriginalCommand { get; set; }


        public void Parse(Core.Frames.Frame commandFrame, Core.Frames.Frame responseFrame)
        {
            FinsResponseFrame response;
            FinsCommandFrame command;

            var MRC = (int)FinsCommandFrame.FinsCommandFields.MRC;
            var SRC = (int)FinsCommandFrame.FinsCommandFields.SRC;

            Contract.Requires(commandFrame.GetByte(MRC) == (byte)FinsCommandFrame.FinsMainRequestCodes.StatusReading, "The command frame has a Main Request code this is not valid for a Read cycle time command.");
            Contract.Requires(commandFrame.GetByte(SRC) == (byte)FinsCommandFrame.FinsSubRequestCodes.CycleTimeRead, "The command frame has a Sub Request code this is not valid for a Read cycle time command.");

            //TODO: Validate End code

            //Assuming we're here then the response must be valid.
            response = new FinsResponseFrame(responseFrame);
            command = new Omron.Commands.Frames.Fins.FinsCommandFrame(commandFrame);


        }
    }
}
