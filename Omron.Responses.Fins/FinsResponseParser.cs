using Omron.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Omron.Responses.Fins
{
    public class FinsResponseParser : IResponseParser
    {

        public Commands.CommandTypes ParseFrameType(Frames.Frame frame)
        { //We know the frame is a Fins frame so we can parse it.
            //  return new FinsResponseFrame(frame.BuildFrame()) as TFrame;
            return Commands.CommandTypes.Read;
        }



        //public IResponseForReadCommand ParseResponse(Frames.Frame frame)
        //{
        //    return new Fins.ReadCommandResponse(frame);
        //}


        //IResponseForWriteCommand IResponseParser.ParseResponse(Frames.Frame frame)
        //{
        //    throw new NotImplementedException();
        //}

        TResponse IResponseParser.ParseResponse<TResponse, TCommand>(Frames.Frame frame)
        {
            return new Fins.ReadCommandResponse(frame) as TResponse;
        }
    }
}
