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

        public IDictionary<string, string> Options { get; private set; }

        public Core.MemoryArea.PlcMemoryModes PlcMemoryMode { get; set; }

        public PlcConfiguration()
        {
            this.Options = new Dictionary<string, string>();
        }
    }
}
