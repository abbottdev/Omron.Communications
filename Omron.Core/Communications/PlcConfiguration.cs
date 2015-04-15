using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Omron.Core
{
    public class PlcConfiguration
    {
        public string Address { get; set; }

        public string Port { get; set; }

        public bool Serial { get; set; }

        /// <summary>
        /// When using an IP Address table on the PLC, the source node must match an entry for the PC which is communicating. When on the same subnet, the Node Number may match the IP address node directly with no configuration required.
        /// </summary>
        public byte SourceNode { get; set; }

        /// <summary>
        /// The Node that is configured on the physical PLC.
        /// </summary>
        public byte DestinationNode { get; set; }

        public IDictionary<string, string> Options { get; private set; }

        public Omron.Core.MemoryAreaParser.PlcMemoryModes PlcMemoryMode { get; set; }

        public PlcConfiguration()
        {
            this.Options = new Dictionary<string, string>();
        }
    }
}
