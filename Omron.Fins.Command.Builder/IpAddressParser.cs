using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Omron.Commands.Builder.Fins
{
    internal class IpAddressParser
    {
        //Required because we aren't targeting all .Net platforms, System.Net is only in standard windows, as TcpIp has different 
        //Implementations per platform.

        public static byte ParseIpAddressNode(string ipAddress)
        {
            Regex IPAddressPattern = new Regex("([0-9]{0,3})\\.*");
            MatchCollection matches;

            matches = IPAddressPattern.Matches(ipAddress);

            if (matches.Count > 3)
            {
                return byte.Parse(matches[3].Groups[1].Value);
            }
            else
            {
                throw new NotSupportedException("Only IPV4 Addresses are supported at this time.");
            }
            
        }

    }
}
