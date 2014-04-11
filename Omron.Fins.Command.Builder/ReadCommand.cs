using Omron.Commands.Expressions;
using Omron.Commands.Generators;
using Omron.Commands.Frames.Fins; 
using Omron.Core; 
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Omron.Commands.Generators.Fins;
using Omron.Core.Frames;

namespace Omron.Commands.Generators.Fins
{
    public class ReadCommandFrameGenerator : IFrameGeneratorOf<IReadCommand>
    {

        public Frame Generate(IReadCommand command, PlcConfiguration configuration, ITransport provider)
        {

            MemoryAreaParser addressParser = null;
            FinsReadCommandParameter parameter;
            Omron.Commands.Frames.Fins.FinsCommandFrame commandFrame;


            //Variable initialization.
            commandFrame = new Omron.Commands.Frames.Fins.FinsCommandFrame();

            addressParser = Core.MemoryAreaParser.Parse(configuration.PlcMemoryMode, command.Area, true, false);
             
            commandFrame.Header.ResponseRequired = true;
            commandFrame.Header.ServiceId = ServiceManager.GetServiceId();
            commandFrame.Header.DestinationNodeAddress = Omron.Core.IpAddressParser.ParseIpAddressNode(configuration.Address);

            commandFrame.Command = FinsCommandFrame.FinsCommands.MemoryAreaRead;

            parameter = new FinsReadCommandParameter(addressParser.MemoryAreaCode, addressParser.MemoryAddress, addressParser.Bit, command.NumberOfItems);

            commandFrame.Parameter = parameter.GetBytes();
             
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

