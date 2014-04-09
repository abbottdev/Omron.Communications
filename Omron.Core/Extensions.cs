using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Omron.Core
{
   internal static class Extensions
    {

       public static bool Between(this int value, int lowerValue, int upperValue)
       {
           return value >= lowerValue && value <= upperValue;
       }

    }
}
