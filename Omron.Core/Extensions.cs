using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Omron.Core
{
    public static class Extensions
    {
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
