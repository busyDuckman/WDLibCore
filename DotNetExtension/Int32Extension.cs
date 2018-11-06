/*
 * The following code is Copyright 2018 Dr Warren Creemers (busyDuckman)
 * See LICENSE.md for more information.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WDToolbox.Maths.Range;

namespace WDToolbox.DotNetExtension
{
    public static class Int32Extension
    {
        /// <summary>
        /// Clamps a value to a range. 
        /// </summary>
        public static Int32 Clamp(this Int32 value, Int32 minInclusive, Int32 maxInclusive)
        {
            if (value < minInclusive)
            {
                return minInclusive;
            }
            if (value > maxInclusive)
            {
                return maxInclusive;
            }
            return value;
        }

    }
}
