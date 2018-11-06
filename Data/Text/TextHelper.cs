/*
 * The following code is Copyright 2018 Dr Warren Creemers (busyDuckman)
 * See LICENSE.md for more information.
 */

using System;
using System.Collections;

namespace WDToolbox.Data.Text
{
    /// <summary>
    /// Helper functions for working with text
    /// </summary>
    public class TextHelper
    {
        /// <summary>
        /// Does the text contain non whitespace characters.
        /// </summary>
        /// <param name="text">Text in question.</param>
        /// <returns>True if the text contains non whitespace characters; otherwise false</returns>
        public static bool isNotBlank(string text)
        {
            if (text == null)
                return false;
            return (text.Trim() != "");
        }

        /// <summary>
        /// Convets a null sting to "".
        /// </summary>
        /// <param name="text">Text in question.</param>
        /// <returns>text if text not null; otherwise "".</returns>
        public static string nullAsEmpty(string text)
        {
            if (text == null)
                return "";
            else
                return text;
        }

        /// <summary>
        /// Index of the first occurance of a specified string in the array of strings.
        /// </summary>
        /// <param name="what">String to look for.</param>
        /// <param name="data">An array of strings.</param>
        /// <param name="caseSensitive">Whether to perform a case sensitive search.</param>
        /// <param name="trim">Wether the search is sensitive to trailing and leading whitespace.</param>
        /// <returns>Index of the first occurance of a specified string or -1 if the string was not found.</returns>
        public static int find(string what, string[] data, bool caseSensitive, bool trim)
        {
            int i;
            string w = what;
            string c;
            if (trim)
                w = w.Trim();
            if (!caseSensitive)
                w = w.ToLower();


            for (i = 0; i < data.Length; i++)
            {
                c = trim ? data[i].Trim() : data[i];
                c = caseSensitive ? c : c.ToLower();
                if (w == c)
                    return i;
            }

            return -1;
        }

        /// <summary>
        /// Quicly perform a ToString() operation on each object in an array.
        /// </summary>
        /// <param name="objects">Objects to be converted to strings.</param>
        /// <param name="seperator">Seperator to be used in the output string. (The Deliminator).</param>
        /// <returns>A deliminated string containing descriptions of all the objects in an array.</returns>
        public static string list(IList objects, string seperator)
        {
            string r = "";
            bool started = false;
            foreach (object o in objects)
            {
                try
                {
                    if (started)
                        r += seperator;

                    r += o.ToString();

                    started = true;
                }
                catch
                {
                }
            }

            return r;
        }
    }
}