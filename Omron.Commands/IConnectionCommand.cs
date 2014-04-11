using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Omron.Commands
{
    public interface IConnectionCommand : ICommand
    {
        int SourceUnitAddress { get; set; }
    }

    public class ConnectionCommand : IConnectionCommand
    {

        public int SourceUnitAddress { get; set; }
    }

}
