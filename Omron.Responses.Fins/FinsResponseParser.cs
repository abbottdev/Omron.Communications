using Ninject;
using Omron.Commands;
using Omron.Commands.Frames.Fins;
using Omron.Core.Frames;
using Omron.Responses.Implementation;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
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
            var MRC = (int)FinsCommandFrame.FinsCommandFields.MRC;
            var SRC = (int)FinsCommandFrame.FinsCommandFields.SRC;


            //var response =  new Fins.ReadCommandResponse(frame) as TResponse;
            TResponse response;

            response = kernel.Get<TResponse>();

            if (response != null)
            {
                response.OriginalCommand = command;
            }

            //Now ensure that the response frame bytes also match.
            Contract.Requires(commandFrame.GetByte(MRC) == receivedFame.GetByte(MRC), "The main request codes between the response frame and the command frame to not match");
            Contract.Requires(commandFrame.GetByte(SRC) == receivedFame.GetByte(SRC), "The sub request codes between the response frame and the command frame to not match");



            response.Parse(commandFrame, receivedFame);

            //Find an instance of an interface that implements the above.
            return response;
        }

    }
}
