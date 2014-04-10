using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Omron.Commands
{
    public class ReadCommand : IReadCommand
    {
        public int NumberOfItems { get; set; }

        public string Area { get; set; }

        public CommandTypes CommandType
        {
            get { return CommandTypes.Read; }
        }
    }
}
