using Omron.Commands.Builder.Expressions;
using Omron.Commands.Builder.Generators;
using Omron.Commands.Frames.Fins;
using Omron.Communications;
using Omron.Core;
using Omron.Frames;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Omron.Commands.Expressions.Generators.Fins
{
    public class ReadCommandGenerator : ICommandGenerator<IReadCommandExpression, IReadCommand>
    {

        public Frame Generate(IReadCommandExpression expression, PlcConfiguration configuration, CommunicationProviderTypes providerType)
        {

            MemoryAreaParser addressParser = null;
            FinsReadCommandParameter parameter;
            Omron.Commands.Frames.Fins.FinsCommandFrame commandFrame;


            //Variable initialization.
            commandFrame = new Omron.Commands.Frames.Fins.FinsCommandFrame();

            addressParser = Core.MemoryAreaParser.Parse(configuration.PlcMemoryMode, expression.Area, true, false);


            commandFrame.Header.ResponseRequired = true;
            commandFrame.Header.ServiceId = ServiceManager.GetServiceId();

            commandFrame.Header.DestinationNodeAddress = Omron.Commands.Builder.Fins.IpAddressParser.ParseIpAddressNode(configuration.Address);

            commandFrame.Command = FinsCommandFrame.FinsCommands.MemoryAreaRead;

            parameter = new FinsReadCommandParameter(addressParser.MemoryAreaCode, addressParser.MemoryAddress, addressParser.Bit, expression.NumberOfItems);

            commandFrame.Parameter = parameter.GetBytes();


            //Append the header frame (Tcp or Udp, or Hostlink)
            var headerFrame = FinsHeaderGenerator.BuildFrameHeader(providerType, commandFrame);

           
            //Append the footer frame (Tcp or Udp, or Hostlink)
            var footerFrame = FinsFooterGenerator.BuildFrameFooter(providerType, commandFrame);
            
            var result = new Frame(new Frame[] { headerFrame, commandFrame as Frame, footerFrame});

            return result;

            //memoryAddressBytes = IntegerToByteArray(addressParser.MemoryAddress, 4, 2);

            //parameter[0] = addressParser.MemoryAreaCode;

            ////Copy the memory address bytes in to the parameter
            //Array.Copy(memoryAddressBytes, 0, parameter, 1, 2);

            ////Set which bit to read from - set to zero if it's a word read
            //parameter[5] = addressParser.Bit;

            ////Copy the no of items to read into the last 2 spots of the array.
            //Array.Copy(IntegerToByteArray(expression.NumberOfItems, 4, 2), 0, parameter, 6, 2);

            ////var addressHexString = addressParser.MemoryAddress.ToString("X2");

            ////addressHexString = addressHexString.PadLeft(4, Convert.ToChar("0"));

            ////Contract.Assert(addressHexString.Length == 4,
            ////                String.Format("The parser memory address returned an unxpected value of {0}, the max hex value that can be used is {1} ({2} in base 10)", addressHexString, "FFFF", 65535));

            ////var addressHexBytes = new byte[] { Convert.ToByte(addressHexString.Substring(0, 2), 16), Convert.ToByte(addressHexString.Substring(2, 2), 16) };

            //////TODO: Replace with integertobytearray
            ////Debug.Assert(addressHexBytes.Equals(IntegerToByteArray(parser.MemoryAddress, 4, 2)));


            ////var bytes = new byte[] { parser.MemoryAreaCode, addressHexBytes[0], addressHexBytes[1], parser.Bit };




            ////Array.Resize<byte>(ref bytes, bytes.Length + 2);

            ////Array.Copy(IntegerToByteArray(expression.NumberOfItems, 4, 2), bytes, bytes.Length - 2);

            //commandFrame.Parameter = parameter;

            //command.AreaAddress
        }

        private static int CalculateParameterLength(IReadCommandExpression expression)
        {
            const int COMMAND_CODE_BYTES = 2;
            const int MEMORY_AREA_CODE_BYTES = 1;

            int length = 0;

            length += COMMAND_CODE_BYTES;
            length += MEMORY_AREA_CODE_BYTES;

            //Address is 3 bytes long, 1st two bytes are the hex address, last byte is the binary address of the bit.
            length += 3;

            //Last 2 bytes are the length of the read command, i.e. how many times to we read consecutive bits, or bytes depending on the address.
            length += 2;

            return length;
        }

        private byte[] IntegerToByteArray(int value, int totalHexLength, int segmentSize)
        {
            var hex = value.ToString("X2");
            var hexOutputSplit = "";
            hex = hex.PadLeft(totalHexLength, Convert.ToChar("0"));

            for (int i = 0; i < totalHexLength; i += segmentSize)
            {
                hexOutputSplit += hex.Substring(i, segmentSize) + "|";
            }

            return hexOutputSplit
                        .Split(new char[] { Convert.ToChar("|") }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(s => Convert.ToByte(s))
                        .ToArray();


        }
    }
}

