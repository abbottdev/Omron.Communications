using Omron.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Omron.Commands.Generators.Fins
{
    public class FinsFooterGenerator
    {

        public static Omron.Core.Frames.Frame BuildFrameFooter(IConnection provider, Omron.Core.Frames.Frame frame)
        {
            switch (provider.ProtocolType)
            {
                case ProtocolTypes.FinsTcpIp:
                    return BuildTcpIpFinsFooter(frame);
                case ProtocolTypes.FinsHostLink:
                    return BuildHostLinkFinsFooter(frame);
                default:
                    throw new NotImplementedException();
            }
        }

        private static Omron.Core.Frames.Frame BuildHostLinkFinsFooter(Omron.Core.Frames.Frame frame)
        {
            throw new NotImplementedException();
        }

        private static Omron.Core.Frames.Frame BuildTcpIpFinsFooter(Omron.Core.Frames.Frame frame)
        {
            return null;
        }
    }
}
