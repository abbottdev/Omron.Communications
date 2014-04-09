using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Omron.Commands
{
    public enum CommandTypes
    {
        Read
    }

    public interface ICommand
    {

        CommandTypes CommandType { get; }
    }
}
