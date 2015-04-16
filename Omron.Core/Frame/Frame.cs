

namespace Omron.Core.Frames
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Omron.Core;


    //public interface IFrame
    //{
    //    int Length { get; }

    //    void ResetFrame(int frameLength);
    //    void SetBit(int byteIndex, int bitIndex, bool value);
    //    bool GetBit(int byteIndex, int bit);
    //    void SetByteHex(int byteIndex, string hexValue);
    //    byte GetByte(int byteIndex);
    //    void SetBytes(int startIndex, byte[] value);
    //    void SetByte(int byteIndex, byte value);
    //    void Resize(int size);
    //    byte[] GetRange(int startIndex, int endIndex);
    //    void Insert(byte[] insert, int index);

    //}

    public class Frame : IEnumerable<byte>//, IFrame
    {
        int originalLength;
        int _currentLength;

        // BitArray _bits;
        byte[] bytes;

        public int Length
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
            var bits = new BitArray(new byte[] { bytes[byteIndex] });

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
            var bits = new BitArray(new byte[] { bytes[byteIndex] });

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
            for (var i = 0; i < result.Length; i++)
            {
                result[i] = this.bytes[startIndex + i];
            }

            return result;
        }

        public void Insert(byte[] insert, int index)
        {
            int originalLength = bytes.Length;

            if (insert == null)
                return;

            Array.Resize<byte>(ref bytes, bytes.Length + insert.Length);

            //Move all items in the array from start index back
            for (int i = bytes.Length - 1; i > insert.Length; i--)
            {
                bytes[i] = bytes[originalLength - (bytes.Length - i) - 1];
                bytes[originalLength + (bytes.Length - i) - 1] = default(byte);
            }

            //Now copy the items into the array
            insert.CopyTo(bytes, index);

        }

        IEnumerator<byte> IEnumerable<byte>.GetEnumerator()
        {
            return bytes.AsEnumerable().GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return bytes.GetEnumerator();//.AsEnumerable<byte>().GetEnumerator();
        }

        public Frame(Frame[] others)
        {
            int totalLength = 0;
            int offset = 0;

            totalLength = others.Select(f => (f == null) ? 0 : f.Length).Sum();

            bytes = new byte[totalLength];

            for (int i = 0; i < others.Length; i++)
            {
                if (others[i] != null)
                {
                    //System.Diagnostics.Debug.WriteLine("Copying {0} at offset {1}, i: {2}, offset: {3}", others[i].BuildFrame().ToStringWithFormat(), offset, i, offset);
                    others[i].BuildFrame().CopyTo(bytes, offset);
                    offset += others[i].Length;
                }
            }

        }

        public byte[] BuildFrame()
        {
            return this.bytes;//.ToByteArray();
        }

        public override string ToString()
        {
            return bytes.ToStringWithFormat();
        }
    }
}
