using Omron.Responses.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Omron.Responses
{
    public interface IResponseForWriteCommand : IResponse<Commands.IWriteCommand>
    {
        string Response { get; }
    }

}
