using Omron.Commands;
using Omron.Commands.Expressions;
using Omron.Responses.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Omron.Responses
{
    public interface IResponseForReadCommand : IResponse<IReadCommand>
    {
        string Response { get; }
    }

}
