using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Omron.Core
{
    public static class Extensions
    {

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

        public static byte[] HexadecimalSplitToBytes(this string header)
        {
            byte[] bytes;
            int index = 0;

            bytes = new byte[(header.Length / 2)];

            for (int i = 0; i < bytes.Length; i++)
            {
                var temp = header.Substring(index, 2);

                bytes[i] = Convert.ToByte(temp, 16);

                index += 2;
            }

            return bytes;
        }

        public static string ToStringWithFormat(this byte[] bytes)
        {

            var sb = new StringBuilder();

            var bits = new BitArray(bytes);
            //var bytes = _bits.ToByteArray();

            sb.Append(String.Format("Bit count: {0}, byte count: {1}", bits.Length, bytes.Length));
            sb.AppendLine().Append("Byte Frame: ");

            for (int i = 0; i < bytes.Length; i++)
            {
                sb.Append(bytes[i].ToString("X2"));
            }

            //sb.AppendLine();

            sb.AppendLine().Append("Bits: ");
            for (int i = 0; i < bits.Length; i++)
            {
                sb.Append((bits.Get(i)) ? "1" : "0");
            }

            return sb.ToString();
            //return base.ToString();
        }

        public static bool Between(this int value, int lowerValue, int upperValue)
        {
            return value >= lowerValue && value <= upperValue;
        }

    }
}
