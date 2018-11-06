/*
 * The following code is Copyright 2018 Dr Warren Creemers (busyDuckman)
 * See LICENSE.md for more information.
 */
using System;
using System.IO;
using System.Runtime.CompilerServices;
using WDToolbox.AplicationFramework;

namespace WDToolbox.Data.DataStructures
{
    /// <summary>
    /// A boolean, that holds a reason why.
    ///
    /// An alternative to using exceptions or returning error message strings. 
    /// </summary>
    public class Why : ICloneable, IComparable<Why>, IEquatable<Why>
    {
        private static Random rand = new Random(); 
        public bool Status { get; private set; }
        public string Reason {get; private set;}

        public static readonly Why False = new Why(false);
        public static readonly Why True = new Why(true);

        public Why(bool status, string reason)
        {
            Reason = reason;
            Status = status;
        }

        public Why(bool status) : this (status, null) { }
        private Why(Why other) : this(other.Status, other.Reason) { }

        public static Why FalseBecause(string reason, bool logAndReportSecureError=false)
        {
            if (logAndReportSecureError)
            {
                string tag = rand.NextNextAlphaNumerics(6);
                WDAppLog.logError(ErrorLevel.Error, string.Format("Error logged with user obfuscation tag={0}: {1}", tag, reason));
                return new Why(false, string.Format("Internal system error, see logs for details (tag={0}).", tag));
            }
            else
            {
                return new Why(false, reason);
            }
        }

        public static Why FalseBecause(Exception ex, 
                         bool logAndReportSecureError = true,
                         [CallerLineNumber] int line = 0,
                         [CallerFilePath] string file = "",
                         [CallerMemberName] string member = "")
        {
            string location = makeLocationString(file, member, line);
            return Why.FalseBecause(describeException(ex, location), logAndReportSecureError);
        }

        public static Why TrueBecause(Exception ex,
                         [CallerLineNumber] int line = 0,
                         [CallerFilePath] string file = "",
                         [CallerMemberName] string member = "")
        {
            string location = makeLocationString(file, member, line);
            return new Why(true, describeException(ex, location));
        }


        public static Why TrueBecause(string reason)
        {
            return new Why(true, reason);
        }

        public static Why FalseBecause(string format, params object[] args)
        {
            return new Why(false, string.Format(format, args));
        }

        public static Why TrueBecause(string format, params object[] args)
        {
            return new Why(true, string.Format(format, args));
        }

        public override string ToString()
        {
            return (Reason != null) ? string.Format("{0}, ({1})", Status, Reason) : Status.ToString();
        }

        public string ToString(string statusTrue, string statusFalse)
        {
            string s = Status ? statusTrue : statusFalse;
            return (Reason != null) ? string.Format("{0}, ({1})", s, Reason) : s;
        }


        /// <summary>
        /// Short for ThrowOnFalse, used to turn make Why act like a try statement. 
        /// </summary>
        public void Try([CallerLineNumber] int line = 0,
                        [CallerFilePath] string file = "",
                        [CallerMemberName] string member = "")
        {
            ThrowOnFalse(line, file, member);
        }

        /// <summary>
        /// Throws and exception, if false
        /// </summary>
        public void ThrowOnFalse([CallerLineNumber] int line = 0,
                        [CallerFilePath] string file = "",
                        [CallerMemberName] string member = "")
        {
            if (!Status)
            {
                string message = string.IsNullOrWhiteSpace(Reason) ? "(no error message was provided)" : Reason.Trim();
                message = String.Format("{0}, {1}", makeLocationString(file, member, line), message);
                throw new Exception(message);
            }
        }

        /// <summary>
        /// Throws and exception, if true
        /// </summary>
        public void ThrowOnTrue([CallerLineNumber] int line = 0,
                        [CallerFilePath] string file = "",
                        [CallerMemberName] string member = "")
        {
            if (Status)
            {
                string message = string.IsNullOrWhiteSpace(Reason) ? "(no error message was provided)" : Reason.Trim();
                string fileName = Path.GetFileName(file ?? "");
                message = String.Format("[{0}:{1} at line {2}] {3}", fileName, member, line, message);
                throw new Exception(message);
            }
        }

        public static bool operator ==(Why a, Why b) 
        {
            //To prevent calling CompareTo from a null object.
            if(Object.ReferenceEquals(a, null))
            {
                return Object.ReferenceEquals(b, null);
            }

            //use the CompareTo operator, to ensure correctness.
            //CompareTo can handle b being null.
            return a.CompareTo(b) == 0;
        }

        public static bool operator !=(Why a, Why b) { return !(a == b); }
        public static bool operator ==(Why a, bool b) { return a.Status == b; }
        public static bool operator !=(Why a, bool b) { return a.Status != b; }
        public static bool operator ==(bool a, Why b) { return b == a; }
        public static bool operator !=(bool a, Why b) { return b != a; }
        public override bool Equals(object obj)
        {
            if (obj is Why)
            {
                return this == ((Why)obj);
            }
            else if ((obj is bool) || (obj is Boolean))
            {
                return this == ((bool)obj);
            }
            else
            {
                return base.Equals(obj);
            }
        }

        public bool Equals(Why other)
        {
            return this == other;
        }

        public int CompareTo(Why other)
        {
            //baisly this is (other != nuul), except it safe gards against a infinite recursion possible
            //when overloaded equalty operators that call CompareTo
            if(object.ReferenceEquals(other, null)) 
            {
                //By definition, any object compares greater than (or follows) Nothing, and two null references compare equal to each other.
                return 1;
            }

            int compare = 0;

            //compare Status
            compare = Status.CompareTo(other.Status);
            if (compare != 0)
            {
                return compare;
            }

            //compare Reason
            if (Reason == null)
            {
                return (other.Reason == null) ? 0 : -1;
            }

            compare = Reason.CompareTo(other.Reason);
            if (compare != 0)
            {
                return compare;
            }

            //must be equal
            return 0;
        }

        public override int GetHashCode()
        {
            return (Status ? 1 : 3) * Reason.GetHashCode();
            //return Status.GetHashCode();
        }

        public bool EqualsByStatusOnly(Why other)
        {
            return this.Status = other.Status;
        }

        public static implicit operator bool(Why a) { return a.Status; }
        public static implicit operator Why(bool a) { return a ? True : False; }

        public static bool operator true(Why a) { return a.Status; }
        public static bool operator false(Why a) { return !a.Status; }

        private static string mergeReasons(string a, string b)
        {
            if(a == null)
            {
                return b;
            }
            if(b == null)
            {
                return a;
            }
            return string.Format("{0} & {1}", a, b);
        }

        /// <summary>
        /// Performs a logical "or". (false only if A and B are false)
        /// In the event both operands are identical, and have reasons, the reasons are merged.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Why operator|(Why a, Why b)
        {
            if (a)
            {
                return b ? Why.TrueBecause(mergeReasons(a.Reason, b.Reason)) : a;
            }
            else
            {
                return b ? b : Why.FalseBecause(mergeReasons(a.Reason, b.Reason));
            }
        }

        /// <summary>
        /// Performs a logical "and". (true only if A and B are both true)
        /// In the event both operands are identical, and have reasons, the reasons are merged.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Why operator &(Why a, Why b)
        {
            if (a)
            {
                return b ? Why.TrueBecause(mergeReasons(a.Reason, b.Reason)) : b;
            }
            else
            {
                return b ? a : Why.FalseBecause(mergeReasons(a.Reason, b.Reason));
            }
        }

        /// <summary>
        /// Clones the instance, oncuding the reason
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return new Why(this);
        }

        private static string makeLocationString(string file, string member, int line)
        {
            if ((member == null) && (file == null))
            {
                return null;
            }

            string fileName = string.IsNullOrWhiteSpace(file) ? "" : Path.GetFileName(file);
            return String.Format("[{0}:{1} at line {2}]", fileName, member, line);
        }

        private static string describeException(Exception ex, string location = null)
        {
            if (!string.IsNullOrWhiteSpace(location))
            {
                return string.Format("{0} Exception ({2}) thrown: {1}",
                                                                location,
                                                                ex.Message ?? "(exception has no message)",
                                                                ex.GetType().Name);
            }
            else
            {
                return string.Format("Exception ({1}) thrown: {0}",
                                                                ex.Message ?? "(exception has no message)",
                                                                ex.GetType().Name);
            }
        }


        public static Why FromTry(Action action,
                                  bool logAndReportSecureError = true,
                                  [CallerLineNumber] int line = 0,
                                  [CallerFilePath] string file = "",
                                  [CallerMemberName] string member = "")
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                return Why.FalseBecause(ex, logAndReportSecureError, line, file, member);
            }

            return true;
        }
    }
}
