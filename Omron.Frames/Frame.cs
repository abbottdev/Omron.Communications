

namespace Omron.Frames
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Frame : IEnumerable<byte>, ICommandFrame
    {
        int originalLength;
        int _currentLength;

       // BitArray _bits;
        byte[] bytes;

        protected int Length
        {
            get
            {
                return bytes.Length;
            }
        }

        //private byte[] _buffer;

        public Frame(int frameLength)
        {
            ResetFrame(frameLength);
        }

        public Frame(byte[] buffer)
        {
            originalLength = buffer.Length; 
            _currentLength = buffer.Length;
            this.bytes = buffer;
        }

        public void ResetFrame(int frameLength)
        {
            this.bytes = new byte[frameLength];
            _currentLength = frameLength;
            originalLength = frameLength;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="byteIndex"></param>
        /// <param name="bitIndex">Remember bit 7 is the most significant bit (i.e. the right most bit), it's RTL not LTR</param>
        /// <param name="value"></param>
        protected void SetBit(int byteIndex, int bitIndex, bool value)
        {
            //byte newValue = Get(fieldIndex);
            var bits = new BitArray(new byte[] {bytes[byteIndex]});

            bits.Set(bitIndex, value);

            bits = new BitArray(bits.Cast<bool>().Reverse().ToArray());

            byte newByte = bits.ToByteArray().First();

            bytes[byteIndex] = newByte;
            //newValue = newValue.AddBit(bitIndex);

            //Set(fieldIndex, newValue);
        }

        protected void SetByteHex(int byteIndex, string hexValue)
        {
            byte value;

            if (hexValue.Length != 2)
            {
                throw new ArgumentException("Invalid hex value. It must be in the 2 character padded format, e.g. A3");
            }
            //Max length of hex value must be 2 chars as 2 hex (FF) = 255 max value of byte.
           
            value = Convert.ToByte(hexValue, 16);

            SetByte(byteIndex, value);
        }

        public byte GetByte(int byteIndex)
        {
            //if (byteIndex % 8 != 0)
            //    throw new ArgumentException("ByteIndex must be the index of the start of a byte");
            return bytes[byteIndex];

        }

        public bool GetBit(int byteIndex, int bit)
        {
            var bits = new BitArray(new byte[] {bytes[byteIndex]});

            return bits.Get(bit);
        }

        public void SetBytes(int startIndex, byte[] value)
        {
            if (startIndex + value.Length > bytes.Length)
            {
                Resize(bytes.Length + (bytes.Length - (startIndex + value.Length)));
            }

            value.CopyTo(bytes, startIndex);
        }

        public void SetByte(int byteIndex, byte value)
        {
            bytes[byteIndex] = value;
        }

        public void Resize(int size)
        {

            Array.Resize(ref bytes, size);

        }

        public byte[] GetRange(int startIndex, int endIndex)
        {
            byte[] result = new byte[endIndex - startIndex];

            //result = result
            //            .Skip(startIndex)
            //            .Take(endIndex - startIndex)
            //            .ToArray();

            for (var i = 0; i <= endIndex - startIndex; i++)
            {
                result[i] = bytes[startIndex + i];
            }

            return result;
        }


        IEnumerator<byte> IEnumerable<byte>.GetEnumerator()
        {
            return bytes.AsEnumerable().GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return bytes.GetEnumerator();//.AsEnumerable<byte>().GetEnumerator();
        }

        public virtual byte[] Parameter { get; set; }

        public byte[] BuildFrame()
        {
            return this.bytes;//.ToByteArray();
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            var bits = new BitArray(bytes);
            //var bytes = _bits.ToByteArray();

            sb.Append(String.Format("Bit count: {0}, byte count: {1}", bits.Length, bytes.Length));
            sb.AppendLine().Append("Byte Frame: ");

            for (int i = 0; i < bytes.Length; i++)
            {
                if (i > 0)
                    sb.Append(" ");

                sb.Append(bytes[i].ToString("X2"));
            }

            //sb.AppendLine();

            sb.AppendLine().Append(" Bits: ");
            for (int i = 0; i < bits.Length; i++)
            {
                sb.Append((bits.Get(i)) ? "1" : "0");
            }

            return sb.ToString();
            //return base.ToString();
        }
    }
}
