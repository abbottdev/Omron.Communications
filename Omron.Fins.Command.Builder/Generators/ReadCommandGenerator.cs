using Omron.Commands.Builder.Expressions;
using Omron.Commands.Builder.Generators;
using Omron.Commands.Frames.Fins;
using Omron.Communications;
using Omron.Core;
using Omron.Frames;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Omron.Commands.Expressions.Generators.Fins
{
    public class ReadCommandGenerator : ICommandGenerator<IReadCommandExpression, IReadCommand>
    {
        public Frame Generate(IReadCommandExpression expression, PlcConfiguration configuration)
        {

            var commandFrame = new Omron.Commands.Frames.Fins.FinsCommandFrame();

            commandFrame.Header.ResponseRequired = true;
            commandFrame.Header.ServiceId = ServiceManager.GetServiceId();


            commandFrame.Header.DestinationNodeAddress = Omron.Commands.Builder.Fins.IpAddressParser.ParseIpAddressNode(configuration.Address);

            commandFrame.Command = FinsCommandFrame.FinsCommands.MemoryAreaRead;

            var parser = Core.MemoryAreaParser.Parse(configuration.PlcMemoryMode, expression.Area, true, false);

            //            parser.MemoryAddress
            var addressHexString = parser.MemoryAddress.ToString("X2");

            addressHexString = addressHexString.PadLeft(4, Convert.ToChar("0"));

            Contract.Assert(addressHexString.Length == 4,
                            String.Format("The parser memory address returned an unxpected value of {0}, the max hex value that can be used is {1} ({2} in base 10)", addressHexString, "FFFF", 65535));

            var addressHexBytes = new byte[] { Convert.ToByte(addressHexString.Substring(0, 2), 16), Convert.ToByte(addressHexString.Substring(2, 2), 16) };


            var bytes = new byte[] { parser.MemoryAreaCode, addressHexBytes[0], addressHexBytes[1], parser.Bit };


            commandFrame.Parameter = bytes;

            //command.AreaAddress
            return commandFrame;
        }
    }
}

