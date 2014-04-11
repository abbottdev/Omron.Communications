using Omron.Core.Frames;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Omron.Responses.Fins
{
    public class ReadCommandResponse : Responses.IResponseForReadCommand
    { 
        private Commands.IReadCommand original;
        private object response;


        public Commands.IReadCommand OriginalCommand { get; set; }

        public void Parse(Frame commandFrame, Frame responseFrame)
        {
            byte[] bytes = commandFrame.GetRange(commandFrame.Length, responseFrame.Length - commandFrame.Length);

            //Bytes will be the bit or words returned.

            string value = Convert.ToString(bytes);

            response = Convert.ToInt32(value);

        }

        object IResponseForReadCommand.Response
        {
            get { return response; }
        }
    }
}
