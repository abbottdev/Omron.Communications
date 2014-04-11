using Omron.Core.Frames;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Omron.Commands.Expressions
{
    public interface IReadCommandExpression : ICommandExpression<ReadCommand>
    { 
        IReadCommandExpression ForArea(string areaAddress);
        IReadCommandExpression WithNumberOfItems(int numberOfReads);
    }

    public class ReadCommandExpression : IReadCommandExpression
    {
        private string _areaAddress;
        private int _noOfReads;

        public IReadCommandExpression ForArea(string areaAddress)
        {
            this._areaAddress = areaAddress;
            return this;
        }

        public IReadCommandExpression WithNumberOfItems(int numberOfReads)
        {
            this._noOfReads = numberOfReads;
            return this;
        }

        public ReadCommand GetCommand()
        {
            return new ReadCommand() { Area = _areaAddress, NumberOfItems = _noOfReads };
        }
    }
}
