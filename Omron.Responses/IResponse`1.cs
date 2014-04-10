using Omron.Commands;
using Omron.Commands.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Omron.Responses.Implementation
{
    public interface IResponse<ForCommand> : IResponse where ForCommand : ICommand
    {
    }

    public interface IResponse
    {
        //Non-Generic base interface to verify types received
        Commands.CommandTypes CommandType { get; }
    }
}
