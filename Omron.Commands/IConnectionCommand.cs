using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Omron.Commands
{
    public interface IPostConnectionCommand : ICommand
    {
        int SourceUnitAddress { get; set; }
    }

    public class ConnectionCommand : IPostConnectionCommand
    {

        public int SourceUnitAddress { get; set; }
    }

}
