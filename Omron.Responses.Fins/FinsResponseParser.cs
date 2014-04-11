using Omron.Commands;
using Omron.Core.Frames;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Omron.Responses.Fins
{
    public class FinsResponseParser : IResponseParser
    {
  
        public TResponse ParseResponse<TResponse, TCommand>(Frame frame)
            where TResponse : class, Implementation.IResponse
            where TCommand : ICommand
        {
            return new Fins.ReadCommandResponse(frame) as TResponse;
        }
    }
}
