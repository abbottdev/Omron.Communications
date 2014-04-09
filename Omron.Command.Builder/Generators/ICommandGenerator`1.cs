using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Omron.Commands.Builder.Generators
{
   public interface ICommandGenerator<TExpression, TCommand> where TCommand : ICommand where TExpression : Expressions.ICommandExpression<TCommand>
    {
        Frames.Frame Generate(TExpression expression);
    }
}
