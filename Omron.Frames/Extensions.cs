using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Omron.Frames
{

    public static class Extensions
    {
        

        public static bool HasBit(this byte value, int bit)
        {
            return ((int)value & bit) == bit;
        }

        public static byte AddBit(this byte value, int bit)
        {
            return (byte)((int)value | bit);
        }

        public static byte[] ToByteArray(this BitArray bits)
        {
            int numBytes = bits.Length / 8;
            if (bits.Length % 8 != 0) numBytes++;

            byte[] bytes = new byte[numBytes];
            int byteIndex = 0, bitIndex = 0;

            for (int i = 0; i < bits.Length; i++)
            {
                if (bits[i])
                    bytes[byteIndex] |= (byte)(1 << (7 - bitIndex));

                bitIndex++;
                if (bitIndex == 8)
                {
                    bitIndex = 0;
                    byteIndex++;
                }
            }

            return bytes;
        }
    }
}
