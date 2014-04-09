using Omron.Commands;
using Omron.Responses.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Omron.Responses
{
    public interface IResponseParser
    {
        Commands.CommandTypes ParseFrameType(Frames.Frame frame);

        TResponse ParseResponse<TResponse, TCommand>(Omron.Frames.Frame frame)
            where TCommand : ICommand
            where TResponse : class, IResponse;

    }
}
