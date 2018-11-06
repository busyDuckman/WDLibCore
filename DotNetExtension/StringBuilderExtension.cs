/*
 * The following code is Copyright 2018 Dr Warren Creemers (busyDuckman)
 * See LICENSE.md for more information.
 */

using System.Collections.Generic;
using System.Text;

namespace WDToolbox//.DotNetExtension
{
    public static class StringBuilderExtension
    {
        /// <summary>
        /// Appends all items to the builder
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="strings"></param>
        /// <returns></returns>
        public static void Append(this StringBuilder sb, IEnumerable<string> strings)
        {
            foreach(string s in strings)
            {
                sb.Append(s);
            }
        }
    }
}
