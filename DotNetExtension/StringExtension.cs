/*
 * The following code is Copyright 2018 Dr Warren Creemers (busyDuckman)
 * See LICENSE.md for more information.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WDToolbox.AplicationFramework;
using WDToolbox.Data.Text;
using WDToolbox.Maths.Range;

namespace WDToolbox//.DotNetExtension
{
    public enum CaseAndLayout { NoChange, CamelCase, 
        camelBack, lower_hyphen, 
        Camel_Hyphen, UPPER_HYPHEN,
        SentanceWithFullStop, SentanceWithoutFullStop
    }

    public enum PadAlignment { Left, Right, Centre };

    public static class StringExtension
    {
        public enum PruneOptions { NoPrune, EmptyLines, EmptyOrWhiteSpaceLines};
        
        /// <summary>
        /// Returns a list of lines in the string.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="pruneOptions">Specifies which lines can be skipped (ie blank)</param>
        /// <param name="trimEachLine"> If true each line is pruned.</param>
        /// <returns></returns>
        public static List<string> GetLines(this string s, PruneOptions pruneOptions=PruneOptions.NoPrune, bool trimEachLine=false)
        {
            if (s == null)
            {
                throw new NullReferenceException("Extension GetLines called on null object.");
            }

            List<string> res = new List<string>();
            string[] lines = s.Replace("\r\n", "\r")
                .Replace("\n", "\r")
                .Split("\r".ToCharArray());
            foreach (string line in lines)
            {
                bool add = true;
                switch (pruneOptions)
                {
                    case PruneOptions.EmptyLines:
                        add = !String.IsNullOrEmpty(line);
                        break;
                    case PruneOptions.EmptyOrWhiteSpaceLines:
                        add = !String.IsNullOrWhiteSpace(line);
                        break;
                    case PruneOptions.NoPrune: //nothing to do
                        break;
                }

                if (add)
                {
                    res.Add(trimEachLine ? line.Trim() : line);
                }
            }

            //some text ends with a new line, keeping the final blank is counter-intuative
            if (s.EndsWith("\n") || s.EndsWith("\r"))
            {
                if (res.Count > 1)
                {
                    if(string.IsNullOrEmpty(res.Last()))
                    {
                        res.RemoveAt(res.Count-1);
                    }
                }
            }
            return res;
        }

        /// <summary>
        /// Removes the first character from the start of a string and returns the shorter version.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="c">the first character, 0x0 if string is null or empty.</param>
        /// <returns></returns>
        public static string DequeueFirstChar(this string s, out char c)
        {
            if (String.IsNullOrEmpty(s))
            {
                c = (char)0;
                return "";
            }
            c = s[0];
            return (s.Length > 1) ? s.Substring(1) : "";
        }

        /// <summary>
        /// Changes text layout, eg camelCase
        /// </summary>
        /// <param name="s"></param>
        /// <param name="newLayout">The new layout.</param>
        /// <returns></returns>
        public static string ChangeCaseAndLayout(this string s, CaseAndLayout newLayout)
        {
            if (newLayout == CaseAndLayout.NoChange)
            {
                return s;
            }
            string norm = s.ToLower().Trim().Replace(" ", "_");
            norm = norm.Replace("__", "_");
            string[] tokens = norm.Split("_".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            StringBuilder sb = new StringBuilder();
            bool first =true;
            foreach (string origToken in tokens)
            {
                string token = origToken;
                switch (newLayout)
                {
                    case CaseAndLayout.CamelCase:
                        token = MakeFirstUpper(token);
                        break;

                    case CaseAndLayout.camelBack:
                        if (!first) { goto case CaseAndLayout.CamelCase; }
                        break;

                    case CaseAndLayout.lower_hyphen:
                        if (!first) { token = "_" + token; }
                        break;

                    case CaseAndLayout.Camel_Hyphen:
                        token = MakeFirstUpper(token);
                        if (!first) { token = "_" + token; }
                        break;

                    case CaseAndLayout.UPPER_HYPHEN:
                        token = token.ToUpper();
                        if (!first) { token = "_" + token; }
                        break;

                    case CaseAndLayout.SentanceWithFullStop:
                        if (first) { token = MakeFirstUpper(token); }
                        else { token = " " + token; }
                        break;
                    case CaseAndLayout.SentanceWithoutFullStop:
                        if (first) { token = MakeFirstUpper(token); }
                        else { token = " " + token; }
                        break;

                    default:
                        break;
                }
                first = false;

                sb.Append(token);
            }

            if (newLayout == CaseAndLayout.SentanceWithFullStop)
            {
                if (sb[sb.Length - 1] != '.')
                {
                    sb.Append(".");
                }
            }

            return sb.ToString();
        }

        private static string MakeFirstUpper(string token)
        {
            if (token.Length > 1)
            {
                token = (token.Length > 1) ?
                    token.ToUpper()[0] + token.Substring(1) :
                    token.ToUpper();
            }
            return token;
        }


        public static string Pad(this string str, int newLength, PadAlignment allignment = PadAlignment.Left)
        {
            return Pad(str, newLength, ' ', allignment);
        }

        public static string Pad(this string str, int newLength, char padCharacter = ' ', PadAlignment allignment = PadAlignment.Left)
        {
            int count = str.Length;
            if(count >= newLength) {
                return str.Clone().ToString();
            }
            
            switch (allignment)
	        {
                case PadAlignment.Right:
                    return (new string(padCharacter, newLength - str.Length)) +  str;
                case PadAlignment.Left:
                    return str + (new string(padCharacter, newLength - str.Length));
                 
                case PadAlignment.Centre:
                    int before = (newLength - str.Length) / 2;
                    int after = newLength - (str.Length + before);
                    return  (new string(padCharacter, before)) + str + (new string(padCharacter, after));
                default:
                    goto case PadAlignment.Left;
	        }
        }

        public static int[] ParseAllIntegers(this string str)
        {
            List<string> intStringList = new List<string>();
            string current = "";
            foreach (char c in str)
            {
                if (c == '-')
                {
                    if(current.Length == 0)
                    {
                        current = "-";
                        continue;
                    }
                    //no continue, theinteger is not continuing
                }
                else if ("0123456789".Contains(c))
                {
                    current += c;
                    continue;
                }

                //here only if the int is finished
                if ((current.Length > 0) && (current != "-"))
                {
                    intStringList.Add(current);
                }
                current = (c == '-') ? "-"  : "";
                
            }

            //done
            if ((current.Length > 0) && (current != "-"))
            {
                intStringList.Add(current);
            }

            List<int> nums = new List<int>();
            for (int i = 0; i < intStringList.Count; i++)
            {
                try
                {
                    int num = int.Parse(intStringList[i]);
                    nums.Add(num);
                }
                catch (Exception ex)
                {
                    WDAppLog.logException( ErrorLevel.Error, ex);
                }
            }

            return nums.ToArray();
        }

        public static string Repeat(this string s, int n)
        {
            StringBuilder sb = new StringBuilder();
            if(n > 0)
            {
                for(int i=0; i<n; i++)
                {
                    sb.Append(s);
                }
                return sb.ToString();
            }
            else
            {
                return "";
            }
        }

        public static List<string> ApplyFormatToLines(this IEnumerable<string> lines, LineFormat format)
        {
            List<string> res = new List<string>();
            int i=0;
            foreach (string s in lines)
            {
                res.Add(format.FormatLine(s, i));
                i++;
            }
            return res;
        }

        public static string ApplyFormatToLines(this string lines, LineFormat format)
        {
            List<string> lineList = lines.GetLines();
            StringBuilder sb = new StringBuilder();
            int i = 0;
            foreach (string s in lineList)
            {
                sb.AppendLine(format.FormatLine(s, i));
                i++;
            }
            return sb.ToString();
        }

        public static int[] IndexOfAll(this string s, char c)
        {
            List<int> all = new List<int>();
            int pos = s.IndexOf(c);
            while(pos >= 0) {
                all.Add(pos);
                pos = s.IndexOf(c, pos+1);
            }

            return all.ToArray();            
        }

        public static int[] IndexOfAll(this string s, string subString)
        {
            List<int> all = new List<int>();
            int pos = s.IndexOf(subString);
            while (pos > 0)
            {
                all.Add(pos);
                pos = s.IndexOf(subString, pos);
            }

            return all.ToArray();
        }

        /// <summary>
        /// As per substring, but going from the back of the string.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="len">number of character</param>
        /// <returns></returns>
        public static string SubStringFromEnd(this string s, int len)
        {
            return SubStringFromEnd(s, 0, len);
        }

        /// <summary>
        /// As per substring, but going from the back of the string.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="start">begin at s.Length - start - 1</param>
        /// <param name="len">number of character</param>
        /// <returns></returns>
        public static string SubStringFromEnd(this string s, int start, int len)
        {
            int actualStart = s.Length - start - len;
            return s.Substring(actualStart, len);
        }

        public static string ReplaceAt(this string s, int pos, char newChar)
        {
            return AlterCharAt(s, pos, C => newChar);
        }

        public static string AlterCharAt(this string s, int pos, Func<char, char> newChar)
        {
            if (s.Length == 0)
            {
                return s;
            }
            else if (s.Length == 1)
            {
                return (pos == 0) ? ("" + newChar(s[0])) : s;
            }
            else if (pos < 0)
            {
                return s;
            }
            else if (pos >= s.Length)
            {
                return s;
            }
            else if (pos == 0)
            {
                return newChar(s[0]) + s.Substring(1);
            }
            else if (pos == s.Length - 1)
            {
                return s.Substring(0, s.Length - 1) + newChar(s[s.Length - 1]);
            }
            else
            {
                return s.Substring(0, pos) + newChar(s[pos]) + s.Substring(pos+1);
            }
        }

        public static string EnsureEndsWith(this string s, string ending)
        {
            return s.EndsWith(ending) ? s : s + ending;
        }

         public static string EnsureEndsWith(this string s, string ending, StringComparison compType)
         {
            return s.EndsWith(ending, compType) ? s : s + ending;
         }

         public static string Wrap(this string s, int maxLineLen)
         {
             //make it sensible
             if(maxLineLen < 2)
             {
                 maxLineLen = 2;
             }

             List<string> lines = s.GetLines(PruneOptions.NoPrune);
             //List<string> newLines = new List<string>();
             StringBuilder sb = new StringBuilder();

             foreach (string line in lines)
             {
                 if(line.Length <= maxLineLen)
                 {
                     //newLines.Add(line);
                     sb.AppendLine(line);
                 }
                 else
                 {
                     string l = string.Copy(line);
                     while (l.Length > 0)
                     {
                         int pos = getLineBreakPoint(l, maxLineLen);
                         //newLines.Add(l.Substring(0, pos+1));
                         sb.AppendLine(l.Substring(0, pos + 1));
                         if (pos < l.Length - 1)
                         {
                             l = l.Substring(pos + 1);
                         }
                         else
                         {
                             break;
                         }
                     }
                 }
             }

             //return newLines;
             return sb.ToString();
         }

         private static int getLineBreakPoint(string l, int maxLineLen)
         {
             int pos = maxLineLen - 1;
             pos = Math.Min(pos, l.Length-1);

             if (pos == 0)
             {
                 return 0;
             }
             for (int i = pos; i >= 1; i--)
             {
                 if (char.IsWhiteSpace(l[i]))
                 {
                     return i;
                 }
             }
             return pos;
         }

         public static LowerCaseString GetLowerCaseString(this string s)
        {
            return new LowerCaseString(s);
        }

        public static bool EqualsIgnoreCase(this string s, string other)
        {
            return String.Compare(s, other, true) == 0;
        }
        
        public static bool EqualsIgnoreCaseTrimmed(this string s, string other)
        {
            return String.Compare(s.Trim(), other.Trim(), true) == 0;
        }

        public static string TextAfterFirst(this string s, string what)
        {
            if(what == null)
            {
                return s;
            }

            int pos = s.IndexOf(what);
            if (pos > 0)
            {
                pos += what.Length;
                if(pos < (s.Length-1))
                {
                    return s.Substring(pos);
                }
            }
            return "";
        }

        public static string TextAfterLast(this string s, string what)
        {
            if (what == null)
            {
                return s;
            }

            int pos = s.LastIndexOf(what);
            if (pos > 0)
            {
                pos += what.Length;
                if (pos < (s.Length - 1))
                {
                    return s.Substring(pos);
                }
            }
            return "";
        }

        public static string TextBeforeFirst(this string s, string what)
        {
            if (what == null)
            {
                return s;
            }

            int pos = s.IndexOf(what);
            if (pos > 0)
            {
                return s.Substring(0, pos);
            }
            return s;
        }

        public static string TextBeforeLast(this string s, string what)
        {
             if (what == null)
            {
                return s;
            }

            int pos = s.LastIndexOf(what);
            if (pos > 0)
            {
                return s.Substring(0, pos);
            }
            return s;
        }

        public static string ShortenTextWithEllipsis(this string s, int maxTotalLen)
        {
            if (s == null)
            {
                return null;
            }

            if(maxTotalLen <= 0)
            {
                return "";
            }
            else if(maxTotalLen <= 3)
            {
                //better ... than abc when length <= 3
                //as this way it is apparent a string was shortened
                return (s.Length <= maxTotalLen) ? s : new string('.', maxTotalLen);
            }

            if (s.Length > maxTotalLen)
            {
                return s.Substring(0, maxTotalLen - 3) + "...";
            }

            //here is s was fine.
            return s;
        }


        public static string ExpectAndRemoveInt(this string s, char[] seperators, out int value, bool trimToken=true)
        {
            /*
            return ExpectAndRemove<int>(s, 
                                        seperators, 
                                        out value, 
                                        delegate(string text, out int v) { return int.TryParse(text, out v); },
                                        trimToken);
            */
            return ExpectAndRemove<int>(s,
                                        seperators,
                                        out value,
                                        int.TryParse,
                                        trimToken);
        }

        public static string ExpectAndRemoveBool(this string s, char[] seperators, out bool value, bool trimToken = true)
        {
            return ExpectAndRemove<bool>(s,
                                        seperators,
                                        out value,
                                        bool.TryParse,
                                        trimToken);
        }

        public static string ExpectAndRemoveDouble(this string s, char[] seperators, out double value, bool trimToken = true)
        {
            return ExpectAndRemove<double>(s,
                                        seperators,
                                        out value,
                                        double.TryParse,
                                        trimToken);
        }

        public delegate bool ParseDelegate<T>(string text, out T value);

        public static string ExpectAndRemove<T>(this string s, 
            char[] seperators, 
            out T value,
            ParseDelegate<T> parse, 
            bool trimToken=true)

        {
            string valueString;
            string s2 = ExpectAndRemoveString(s, seperators, out valueString, trimToken);

            if ((valueString != null) && (parse != null))
            {
                try
                {
                    if (parse(valueString, out value))
                    {
                        return s2;
                    }
                }
                catch (Exception ex)
                {
                }
            }

            //error
            value = default(T);
            return null;
        }


        public static string ExpectAndRemoveString (this string s, char[] seperators, out string value, bool trimToken=true)
        {
            if(s == null)
            {
                value = null;
                return null;
            }
            if (seperators == null)
            {
                value = trimToken ? s.Trim() : s;
                return "";
            }

            string s2 = trimToken ? s.TrimStart() : s;
            int pos = s.IndexOfAny(seperators);
            if(pos >= 0)
            {
                value = trimToken? s2.Substring(0, pos).TrimEnd() : s2.Substring(0, pos);

                s2 = ((pos+1)<s2.Length) ? s2.Substring(pos+1) : "";
            }
            else
            {
                value = null;
                return null;
            }
            return s2;
        }

        public static string IndentLines(this string s, string indent="\t")
        {
            return s.GetLines().Select(L => indent + L + "\r\n").ToList().ListAll(null);
        }

        public static bool IsSomething(this string s)
        {
            return !string.IsNullOrWhiteSpace(s);
        }
        
        public static bool IsNothing(this string s)
        {
            return string.IsNullOrWhiteSpace(s);
        }

    }  //end class



    public class LineFormat
    {
        public static LineFormat CodeDump = new LineFormat(true, 1, "  ", "");

        public bool ShowLineNumbers {protected set; get;}
        public int FirstLineNum {protected set; get;}
        public string StringBefore {protected set; get;}
        public string StringAfter {protected set; get;}

        public LineFormat (
            bool showLineNumbers,
            int firstLineNum,
            string stringBefore,
            string stringAfter
            )
	    {
            ShowLineNumbers = showLineNumbers;
            FirstLineNum = firstLineNum;
            StringBefore = stringBefore??"";
            StringAfter = stringAfter??"";
	    }

        public LineFormat FromTabIndents(int tabs)
        {
            string indent = "\t".Repeat(tabs);
            return new LineFormat(false, 0, indent, "");
        }

        public LineFormat FromSpaceIndents(int spaces)
        {
            string indent = " ".Repeat(spaces);
            return new LineFormat(false, 0, indent, "");
        }

        internal string FormatLine(string line, int index, int lineNumPadSize=4)
        {
            string lineStr = ShowLineNumbers ? ("" + (index+FirstLineNum)).PadLeft(lineNumPadSize)
                                             : "";
            return lineStr + StringBefore + line + StringAfter;
        }
    }
} //end namespace
