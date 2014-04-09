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
        
        IReadCommandExpression IReadCommandExpression.ForArea(string areaAddress)
        {
            this.AreaAddress = areaAddress;
            return this;
        }

        public string Area
        {
            get { return this.AreaAddress; }
        }
    } 

}
