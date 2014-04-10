using Omron.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Omron.Commands.Generators.Fins
{
    public class FinsFooterGenerator
    {

        public static Omron.Frames.Frame BuildFrameFooter(CommunicationProviderTypes providerType, Omron.Frames.Frame frame)
        {
            switch (providerType)
            {
                case CommunicationProviderTypes.TcpId:
                    return BuildTcpIpFinsFooter(frame);
                case CommunicationProviderTypes.Serial:
                    return BuildHostLinkFinsFooter(frame);
                default:
                    throw new NotImplementedException();
            }
        }

        private static Omron.Frames.Frame BuildHostLinkFinsFooter(Omron.Frames.Frame frame)
        {
            throw new NotImplementedException();
        }

        private static Omron.Frames.Frame BuildTcpIpFinsFooter(Omron.Frames.Frame frame)
        {
            return null;
        }
    }
}
