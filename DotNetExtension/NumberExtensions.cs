/*
 * The following code is Copyright 2018 Dr Warren Creemers (busyDuckman)
 * See LICENSE.md for more information.
 */

using System;

namespace WDToolbox
{
    /// <summary>
    /// A set of methods to convert a double to an integer.
    /// </summary>
    public enum IntegerConversionMode 
    {
        Floor, Ceiling, ToZero, AwayFromZero
    }
    public static class NumberExtensions
    {
        /// <summary>
        /// Fluent versions of Math.blah.
        /// Also use full for putting integer conversion mode options in a property grid etc.
        /// </summary>
        public static int ToInteger(this double d, IntegerConversionMode mode)
        {
            switch (mode)
	        {
		        case IntegerConversionMode.Floor:
                    return (int)Math.Floor(d);
                case IntegerConversionMode.Ceiling:
                    return (int)Math.Ceiling(d);
                case IntegerConversionMode.ToZero:
                    return (d > 0) ? (int)Math.Floor(d) : (int)Math.Ceiling(d);
                case IntegerConversionMode.AwayFromZero:
                    return (d > 0) ? (int)Math.Ceiling(d) : (int)Math.Floor(d);
	        }

            return (int)d;
        }
    }
}
