using Omron.Commands;
using Omron.Responses.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Omron.Responses
{
    public interface IResponseForReadCycleTimeCommand : IResponse<IReadCycleTimeCommand> 
    {
        TimeSpan CycleTime { get; }
    }
}
