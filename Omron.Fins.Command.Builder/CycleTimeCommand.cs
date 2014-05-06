using Omron.Commands.Frames.Fins;
using Omron.Core;
using Omron.Core.Frames;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Omron.Commands.Generators.Fins
{
    public class ReadCycleTimeFrameGenerator : IFrameGeneratorOf<IReadCycleTimeCommand>
    {




        public Frame Generate(IReadCycleTimeCommand command, PlcConfiguration configuration, ITransport provider)
        {
           // MemoryAreaParser addressParser = null;
            Omron.Commands.Frames.Fins.FinsCommandFrame commandFrame;

            //Variable initialization.
            commandFrame = new Omron.Commands.Frames.Fins.FinsCommandFrame();

            //addressParser = Core.MemoryAreaParser.Parse(configuration.PlcMemoryMode, command.Area, true, false);

            commandFrame.Header.ResponseRequired = true;
            commandFrame.Header.ServiceId = ServiceManager.GetServiceId();
            commandFrame.Header.DestinationNodeAddress = Omron.Core.IpAddressParser.ParseIpAddressNode(configuration.Address);

            commandFrame.Command = FinsCommandFrame.FinsCommands.CycleTimeRead;

            commandFrame.Parameter = new byte[] { Convert.ToByte("02", 16) };

            //Append the header frame (Tcp or Udp, or Hostlink)
            var headerFrame = FinsHeaderGenerator.BuildFrameHeader(provider, commandFrame);

            //Append the footer frame (Tcp or Udp, or Hostlink)
            var footerFrame = FinsFooterGenerator.BuildFrameFooter(provider, commandFrame);

            //Combine the content, header and footer frames together.
            var result = new Frame(new Frame[] { 
                headerFrame, 
                commandFrame as Frame, 
                footerFrame
            });

            return result;

        }
    }
}
