/*
 * The following code is Copyright 2018 Dr Warren Creemers (busyDuckman)
 * See LICENSE.md for more information.
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using WDToolbox;

namespace WDToolbox//.AplicationFramework
{
    public static class ExceptionExtension
    {
        /// <summary>
        /// I find exception dumps too verbose at times.
        /// </summary>
        public static string GetADecentExplination(this Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            Exception current = ex;
            int traceDepth = 100;
            string indent = "";

            List<string> lines = (ex.StackTrace??"").GetLines(StringExtension.PruneOptions.EmptyOrWhiteSpaceLines, true);
            sb.Append(ex.Message.Trim());
            if (lines.Count > 0)
            {
                sb.AppendLine(" [call to " + getMethodName(lines[0]) + "]");
                lines.RemoveAt(0);
                foreach (string line in lines)
                {
                    string[]tokens = line.Split(new string[] {" in ", ":line"}, 3, StringSplitOptions.None);
                    if (tokens.Length == 3)
                    {
                        string method = getMethodName(tokens[0]);
                        string file = Path.GetFileName(tokens[1].Trim());
                        int lineNumber = tokens[2].ParseAllIntegers().Last();
                        sb.AppendLine(string.Format(": {0} in {1} line {2}", method, file, lineNumber));
                    }
                    else
                    {
                        sb.AppendLine(string.Format(": {0} in {1} line {2}", "?", "?", "?"));
                    }
                }
            }

            return sb.ToString();
        }

        // gets a method name, enclosed in brackets.
        private static string getMethodName(string s)
        {
            string end = s.Trim().Split(".".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).LastOrDefault();
            if (end != null)
            {
                string name =  s.TextAfterLast(".").TextBeforeFirst("(");
                if(!string.IsNullOrWhiteSpace(name))
                {
                    int commas = s.IndexOfAll(',').Length;
                    return name + "(" + (new string(',', commas)) + ")";
                }
            }
            return "(n/a)";
        }
    }
}
