/*
 * The following code is Copyright 2018 Dr Warren Creemers (busyDuckman)
 * See LICENSE.md for more information.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WDToolbox
{
    public static class IComparableExtension
    {
        /// <summary>
        /// Fluent programming version of min.
        /// eg: a = 1.Min(2); // a = 1
        /// </summary>
        public static T Min<T>(this T a, T b)
            where T : IComparable<T>
        {
            return (a.CompareTo(b) < 0) ? a : b;
        }

        /// <summary>
        /// Fluent programming version of max.
        /// eg: a = 1.Max(2);  // a = 2
        /// </summary>
        public static T Max<T>(this T a, T b)
            where T : IComparable<T>
        {
            return (b.CompareTo(a) > 0) ? b : a;
        }
    }
}
