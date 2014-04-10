using Omron.Commands.Builder.Expressions; 
using Omron.Frames;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Omron.Commands.Expressions.Fins
{
    public sealed class ReadCommandExpression : Omron.Commands.Builder.Expressions.IReadCommandExpression
    {
        internal string AreaAddress { get; set; }
        internal int NumberOfReads { get; set; }
        
        public string Area
        {
            get { return this.AreaAddress; }
        }

        public IReadCommandExpression ForArea(string areaAddress)
        {
            this.AreaAddress = areaAddress;
            return this;
        }

        public IReadCommandExpression WithNumberOfItems(int numberOfReads)
        {
            this.NumberOfReads = numberOfReads;
            return this;
        }

        public int NumberOfItems
        {
            get { return NumberOfReads; }
        }
    } 

}
