/*
 * The following code is Copyright 2018 Dr Warren Creemers (busyDuckman)
 * See LICENSE.md for more information.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WDToolbox//.DotNetExtension
{
    public static class EnumExtension
    {
        /// <summary>
        /// Handy extension, to replace Enum.GetName(e.GetType(), e) 
        /// </summary>
        public static string GetName(this Enum e) { return Enum.GetName(e.GetType(), e); }
    }
}
