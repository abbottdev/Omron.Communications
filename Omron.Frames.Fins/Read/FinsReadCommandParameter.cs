using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Omron.Core;

namespace Omron.Commands.Frames.Fins
{
    public sealed class FinsReadCommandParameter
    {
        public enum ReadCommandParameterIndex
        {
            MemoryArea = 0,
            ReadAddress_Hex1 = 1,
            ReadAddress_Hex2 = 2,
            BitAddress = 3,
            NumberOfReads = 4
        }
        public byte MemoryAreaCode { get; private set; }

        public int Address { get; private set; }

        public int NumberOfReads { get; private set; }

        public byte Bit { get; private set; }

        public byte[] GetBytes()
        {
            byte[] parameter, memoryAddressBytes;

            parameter = new byte[ParameterLength];

            memoryAddressBytes = IntegerToByteArray(Address, 4, 2);

            parameter[(int)ReadCommandParameterIndex.MemoryArea] = MemoryAreaCode;

            //Copy the memory address bytes in to the parameter
            Array.Copy(memoryAddressBytes, 0, parameter, (int)ReadCommandParameterIndex.ReadAddress_Hex1, 2);

            //Set which bit to read from - set to zero if it's a word read
            parameter[(int)ReadCommandParameterIndex.BitAddress] = Bit;

            //Copy the no of items to read into the last 2 spots of the array.
            Array.Copy(IntegerToByteArray(NumberOfReads, 4, 2), 0, parameter, (int)ReadCommandParameterIndex.NumberOfReads, 2);

            return parameter;

        }

        public FinsReadCommandParameter(byte memoryAreaCode, int address, byte bit, int NoOfReads)
        {
            this.MemoryAreaCode = memoryAreaCode;
            this.Address = address;
            this.Bit = bit;
            this.NumberOfReads = NoOfReads;

        }

        public override string ToString()
        {
            return GetBytes().ToStringWithFormat();
        }


        private int ParameterLength
        {
            get
            {
                const int COMMAND_CODE_BYTES = 2;
                const int MEMORY_AREA_CODE_BYTES = 1;

                int length = 0;

               // length += COMMAND_CODE_BYTES;
                length += MEMORY_AREA_CODE_BYTES;

                //Address is 3 bytes long, 1st two bytes are the hex address, last byte is the binary address of the bit.
                length += 3;

                //Last 2 bytes are the length of the read command, i.e. how many times to we read consecutive bits, or bytes depending on the address.
                length += 2;

                return length;
            }
        }

        private static byte[] IntegerToByteArray(int value, int totalHexLength, int segmentSize)
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
                        .Select(s => Convert.ToByte(s, 16))
                        .ToArray();


        }

    }
}
