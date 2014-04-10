//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace Omron.Frames
//{
//    public abstract class CommandFrame
//    {
//        protected enum CommandFrameSegments
//        {
//            Header,
//            Content,
//            Footer
//        }

//        private Frame Header { get; set; }

//        protected Frame Content { get; private set; }

//        private Frame Footer { get; set; }

//        public void SetHeader(Frame header)
//        {
//            this.Header = header;
//        }

//        public void SetFooter(Frame footer)
//        {
//            this.Footer = footer;
//        }

//        public CommandFrame(int contentFrameLength)
//        {
//            this.Content = new Frame(contentFrameLength);
//        }

//        protected byte GetByte(CommandFrameSegments segment, int byteIndex)
//        {
//            return GetByte(FrameForSegment(segment), byteIndex);
//        }

//        protected void SetByte(CommandFrameSegments segment, int byteIndex, byte value)
//        {
//            SetByte(FrameForSegment(segment), byteIndex, value);
//        }

//        protected void SetBytes(CommandFrameSegments segment, int startIndex, byte[] value)
//        {
//            SetBytes(FrameForSegment(segment), startIndex, value);
//        }

//        protected void SetBit(CommandFrameSegments segment, int byteIndex, byte bitIndex, bool value)
//        {
//            SetBit(FrameForSegment(segment), byteIndex, bitIndex, value);
//        }

//        protected bool GetBit(CommandFrameSegments segment, int byteIndex, int bitIndex)
//        {
//            return GetBit(FrameForSegment(segment), byteIndex, bitIndex);
//        }

//        protected void SetByteHex(CommandFrameSegments segment, int byteIndex, string hexValue)
//        {
//            SetByteHex(FrameForSegment(segment), byteIndex, hexValue);
//        }

//        private IFrame FrameForSegment(CommandFrameSegments segment)
//        {
//            switch (segment)
//            {
//                case CommandFrameSegments.Content:
//                    return this.Content;
//                case CommandFrameSegments.Header:
//                    return this.Header;
//                case CommandFrameSegments.Footer:
//                    return this.Footer;
//                default:
//                    throw new InvalidOperationException("Invalid command frame segment");
//            }
//        }

//        private void SetByte(IFrame frame, int byteIndex, byte value)
//        {
//            frame.SetByte(byteIndex, value);
//        }

//        private void SetByteHex(IFrame frame, int byteIndex, string hexValue)
//        {
//            frame.SetByteHex(byteIndex, hexValue);
//        }

//        private void SetBytes(IFrame frame, int startIndex, byte[] value)
//        {
//            frame.SetBytes(startIndex, value);
//        }

//        private byte GetByte(IFrame frame, int byteIndex)
//        {
//            return frame.GetByte(byteIndex);
//        }

//        private void SetBit(IFrame frame, int byteIndex, int bitIndex, bool value)
//        {
//            frame.SetBit(byteIndex, bitIndex, value);
//        }


//        private bool GetBit(IFrame frame, int byteIndex, int bitIndex)
//        {
//            return frame.GetBit(byteIndex, bitIndex);
//        }

//        public Frame GetFrame()
//        {
//            var bytes = new byte[Header.Length - 1 + Content.Length - 1 + Footer.Length - 1];
//            return new Frame(bytes);
//        }

//    }
//}
