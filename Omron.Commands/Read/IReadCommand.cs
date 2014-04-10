using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
using Omron.Frames;
using System.Threading.Tasks;

namespace Omron.Commands
{
    public interface IReadCommand : ICommand
    {
        int NumberOfItems { get; set;  }
        string Area { get; set; }
    }
}
