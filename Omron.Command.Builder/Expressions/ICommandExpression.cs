using Omron.Frames;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Omron.Commands.Builder.Expressions
{
    public interface ICommandExpression<TCommand> where TCommand : ICommand
    {
    }
}
