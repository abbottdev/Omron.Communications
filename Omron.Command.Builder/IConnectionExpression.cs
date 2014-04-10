using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Omron.Commands.Expressions
{
    public interface IConnectionExpression
    {
        void UseSourceNode(int node);

        int Node { get; }
    }
}
