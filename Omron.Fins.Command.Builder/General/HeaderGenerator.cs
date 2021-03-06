﻿using Omron.Core;
using Omron.Core.Frames;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Omron.Commands.Generators.Fins
{
    public sealed class FinsHeaderGenerator
    {

        public  static Frame BuildFrameHeader(ITransport provider, Frame frame)
        {
            switch (provider.ProtocolType)
            {
                case ProtocolTypes.FinsTcpIp:
                    return BuildTcpIpFinsHeader(frame);
                case ProtocolTypes.FinsHostLink:
                    return BuildHostLinkFinsHeader(frame);
                case ProtocolTypes.FinsUdp:
                    return BuildFinsUdpHeader(frame);
                default:
                    throw new NotImplementedException();
            }
        }

        private static Frame BuildFinsUdpHeader(Frame frame)
        {
            return null;
        }

        private static Frame BuildHostLinkFinsHeader(Frame frame)
        {
            throw new NotImplementedException();
        }

        private static Frame BuildTcpIpFinsHeader(Frame frame)
        {
            const string FINS_TCP_HEADER = "46494E53";

            string header = "";

            //trame = FINS_TCPHeader & Right$("00000000" & Hex$((Len(trame) + 16) / 2), 8) & "0000000200000000" & trame

            header += FINS_TCP_HEADER;
            header += (frame.Length + 16).ToString("X2").PadLeft(8, Convert.ToChar("0"));
            header += "00000002";//Not sure what the use of this is yet.
            header += "00000000"; //Says this is the 'command'

            //byte[] bytes;
            //int index = 0;

            //bytes = new byte[(header.Length / 2)];

            //for (int i = 0; i < bytes.Length; i++)
            //{
            //    var temp = header.Substring(index, 2);

            //    bytes[i] = Convert.ToByte(temp, 16);

            //    System.Diagnostics.Debug.WriteLine("Got hex {0} from string {1}, byte value is: {2}. Index: {3}, i: {4}", temp, header, bytes[i], index, i);

            //    index += 2;
            //}

            //var bytes =  header.Spl.Select(character => Convert.ToByte(character)).ToArray();

            return new Frame(header.HexadecimalSplitToBytes());
        }

    }
}
