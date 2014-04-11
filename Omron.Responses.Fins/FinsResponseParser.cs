using Ninject;
using Omron.Commands;
using Omron.Core.Frames;
using Omron.Responses.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Omron.Responses.Fins
{
    public class FinsResponseParser : IResponseParser
    {
        IKernel kernel;

        public FinsResponseParser(IKernel kernel)
        {
            this.kernel = kernel;
        }

        public TResponse ParseResponse<TResponse, TCommand>(Frame receivedFame, Frame commandFrame, TCommand command)
            where TResponse : IResponse<TCommand>
            where TCommand : ICommand
        {
            //var response =  new Fins.ReadCommandResponse(frame) as TResponse;
            TResponse response;

            response = kernel.Get<TResponse>();

            if (response != null)
            {
                response.OriginalCommand = command;
            }

            response.Parse(commandFrame, receivedFame);

            //Find an instance of an interface that implements the above.
            return response;
        }
    }
}
