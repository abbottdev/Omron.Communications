using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Omron.Responses.Fins
{
    public class ReadCommandResponse : Responses.IResponseForReadCommand
    {
        private Frames.Frame frame;

        public ReadCommandResponse(Frames.Frame frame)
        {
            this.frame = frame;
        }
        public string Response
        {
            get
            {
                return Encoding.UTF8.GetString(frame.GetRange(5, 10), 0, 5);
            }
        }

        public Commands.CommandTypes CommandType
        {
            get { return Commands.CommandTypes.Read; }
        }
    }
}
