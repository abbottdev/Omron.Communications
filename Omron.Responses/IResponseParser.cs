using Omron.Commands;
using Omron.Core.Frames;
using Omron.Responses.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Omron.Responses
{
    public interface IResponseParser
    {
        
        TResponse ParseResponse<TResponse, TCommand>(Frame frame)
            where TCommand : ICommand
            where TResponse : class, IResponse;

    }
}
