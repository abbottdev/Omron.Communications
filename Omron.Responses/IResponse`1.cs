using Omron.Commands;
using Omron.Commands.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Omron.Responses.Implementation
{
    public interface IResponse<ForCommand> : IResponse where ForCommand : ICommand
    {
        ForCommand OriginalCommand { set; get; }
        void Parse(Core.Frames.Frame commandFrame, Core.Frames.Frame responseFrame); 
    }

    public interface IResponse
    {  
    }
}
