/*
 * The following code is Copyright 2018 Dr Warren Creemers (busyDuckman)
 * See LICENSE.md for more information.
 */
using System;
using System.Linq;

namespace WDToolbox//.DotNetExtension
{
    public static class RandomExtension
    {
        /// <summary>
        ///  Next boolean value.
        /// </summary>
        /// <param name="r"></param>
        /// <returns>true/false at 50% probability.</returns>
        public static bool nextBool(this Random r)
        {
            return r.Next(2) == 0;
        }
        
        /// <summary>
        /// Returns 'a'-'z'
        /// </summary>
        /// <param name="upperCase">Returns 'A'-'Z'</param>
        public static char NextLetter(this Random r, bool upperCase=false)
        {
            char c = (char)((int)'a' + r.Next(26));
            c = upperCase ? char.ToUpper(c) : c;
            return c;
        }

        /// <summary>
        /// Returns a string composing off: 'a'-'z'
        /// </summary>
        /// <param name="n">Length of string.</param>
        /// <param name="upperCase">Return uppercase string,</param>
        /// <returns></returns>
        public static string NextLetters(this Random r, int n, bool upperCase=false)
        {
            return new string(Enumerable.Repeat(1, n).Select(N => r.NextLetter(upperCase)).ToArray());
        }

        /// <summary>
        /// Returns 'a'-'z' - '0'-'9'.
        /// </summary>
        /// <param name="upperCase">Returns 'A'-'Z' - '0'-'9'.</param>
        public static char NextAlphaNumeric(this Random r, bool upperCase)
        {
            int i = r.Next(26 + 10);
            if (i >= 26)
            {
                char c = (char)((int)'a' + i);
                c = upperCase ? char.ToUpper(c) : c;
                return c;
            }
            else
            {
                return (char) ((int) '0' + (i-26));
            }
        }
        
        /// <summary>
        /// Returns a string composing off: 'a'-'z' - '0'-'9'.
        /// </summary>
        /// <param name="n">Length of string.</param>
        /// <param name="upperCase">Return uppercase string</param>
        /// <returns></returns>
        public static string NextNextAlphaNumerics(this Random r, int n, bool upperCase=false)
        {
            return new string(Enumerable.Repeat(1, n).Select(N => r.NextAlphaNumeric(upperCase)).ToArray());
        }
    }
}
