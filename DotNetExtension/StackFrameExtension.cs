/*
 * The following code is Copyright 2018 Dr Warren Creemers (busyDuckman)
 * See LICENSE.md for more information.
 */

using System.Diagnostics;
using System.Reflection;

namespace WDToolbox//.DotNetExtension
{
    public static class StackFrameExtension
    {
        /// <summary>
        /// Provides a explanation formatted for readability. 
        /// </summary>
        public static string GetADecentExplination(this StackFrame sf)
        {
            string expl = "";
            string file = sf.GetFileName();
            MethodBase func = sf.GetMethod();
            string funcName = func.Name;
            int line = sf.GetFileLineNumber();
            int col = sf.GetFileColumnNumber();

            if (!string.IsNullOrEmpty(file))
            {
                expl += "File: " + file;
            }

            if (!string.IsNullOrEmpty(funcName))
            {
                expl += "Method: " + funcName;
            }

            if (line >= 0)
            {
                expl += "Line: " + line;
                if (col >= 0)
                {
                    expl += "Col: " + line;
                }
            }



            return expl;
        }
    }
}
