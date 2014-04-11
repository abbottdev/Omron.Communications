using Omron.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Omron.Commands.Generators
{
    public interface IFrameGeneratorOf<TCommand> where TCommand : ICommand
    {
        Core.Frames.Frame Generate(TCommand command, PlcConfiguration configuration, IConnection provider);
    }
}
