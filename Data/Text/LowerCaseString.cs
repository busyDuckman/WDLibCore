/*
 * The following code is Copyright 2018 Dr Warren Creemers (busyDuckman)
 * See LICENSE.md for more information.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace WDToolbox.Data.Text
{
    /// <summary>
    /// As similar as possible to System.String, but using only lower case letters.
    /// Does not support system references or normalising.
    /// </summary>
    public sealed class LowerCaseString : 
        IComparable, 
        ICloneable, 
        IComparable<LowerCaseString>, 
        IEnumerable<char>, 
        IEnumerable, 
        IEquatable<LowerCaseString>
    {
        /// <summary>
        /// Represents the empty string. This field is read-only.
        /// </summary>
        public static readonly string Empty = "";

        //--------------------------------------------------------------------------
        // LowerCaseString stuff
        //--------------------------------------------------------------------------
        private string s = null;

        private void setString(string value)
        {
            s = (value == null) ? null : value.ToLower();
        }

        public static IEnumerable<LowerCaseString> ToLower(IEnumerable<string> values)
        {
            List<LowerCaseString> valuesNoCase = new List<LowerCaseString>();
            foreach (string s in values)
            {
                valuesNoCase.Add(new LowerCaseString(s));
            }

            return valuesNoCase;
        }

        public static IEnumerable<string> ToString(IEnumerable<LowerCaseString> values)
        {
            List<string> valuesAsString = new List<string>();
            foreach (LowerCaseString s in values)
            {
                valuesAsString.Add(String.Copy(s.s));
            }

            return valuesAsString;
        }

        public static IEnumerable<LowerCaseString> ToLower<T>(IEnumerable<T> values)
        {
            List<LowerCaseString> valuesNoCase = new List<LowerCaseString>();
            foreach (T s in values)
            {
                valuesNoCase.Add(new LowerCaseString(s.ToString()));
            }

            return valuesNoCase;
        }

        public static IEnumerable<LowerCaseString> ToLower(IEnumerable<object> values)
        {
            List<LowerCaseString> valuesNoCase = new List<LowerCaseString>();
            foreach (object s in values)
            {
                valuesNoCase.Add(new LowerCaseString(s.ToString()));
            }

            return valuesNoCase;
        }

        private static char[] ToLower(char[] values)
        {
            if (values == null)
            {
                return null;
            }
            if (values.Length <= 0)
            {
                return new char[0];
            }

            char[] valuesNoCase = new char[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                valuesNoCase[i] = char.ToLower(values[i]);
            }
            return valuesNoCase;
        }

        private static bool nothingHere(char[] array)
        {
            if (array == null)
            {
                return true;
            }
            if (array.Length <= 0)
            {
                return true;
            }

            return false;
        }

        public static char[] ToLowerSet(char[] set)
        {
            if (set == null)
            {
                return null;
            }
            if (set.Length <= 0)
            {
                return new char[0];
            }

            HashSet<char> setLowerCase = new HashSet<char>();
            foreach (char c in set)
            {
                setLowerCase.Add(char.ToLower(c));
            }

            return setLowerCase.ToArray();
        }

#if UNSAFE
        [SecurityCritical]
        unsafe public LowerCaseString(char* value) { setString(new string(value)); }
                
        [SecurityCritical]
        unsafe public LowerCaseString(sbyte* value) { setString(new string(value)); }

        [SecurityCritical]
        unsafe public LowerCaseString(char* value, int startIndex, int length) { setString(new string(value, startIndex, length)); }
        
        [SecurityCritical]
        unsafe public LowerCaseString(sbyte* value, int startIndex, int length) { setString(new string(value, startIndex, length)); }
        
        [SecurityCritical]
        unsafe public LowerCaseString(sbyte* value, int startIndex, int length, Encoding enc) { setString(new string(value, startIndex, length)); }

#endif

        [SecuritySafeCritical]
        public LowerCaseString(char[] value) { setString(new string(value)); }

        [SecuritySafeCritical]
        public LowerCaseString(char c, int count) { setString(new string(c, count)); }

        [SecuritySafeCritical]
        public LowerCaseString(char[] value, int startIndex, int length) { setString(new string(value, startIndex, length)); }


        [SecurityCritical]
        public LowerCaseString(string value)
        {
            setString(value);
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="value"></param>
        [SecurityCritical]
        private LowerCaseString(LowerCaseString value)
        {
            s = String.Copy(value.s);
        }

        // Summary:
        //     Determines whether two specified strings have different values.
        //
        // Parameters:
        //   a:
        //     The first string to compare, or null.
        //
        //   b:
        //     The second string to compare, or null.
        //
        // Returns:
        //     true if the value of a is different from the value of b; otherwise, false.
        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public static bool operator !=(LowerCaseString a, LowerCaseString b) { return a.s != b.s; }
        //
        // Summary:
        //     Determines whether two specified strings have the same value.
        //
        // Parameters:
        //   a:
        //     The first string to compare, or null.
        //
        //   b:
        //     The second string to compare, or null.
        //
        // Returns:
        //     true if the value of a is the same as the value of b; otherwise, false.
        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public static bool operator ==(LowerCaseString a, LowerCaseString b) { return a.s == b.s; }

        // Summary:
        //     Gets the number of characters in the current System.String object.
        //
        // Returns:
        //     The number of characters in the current string.
        public int Length { get { return s.Length; } }

        // Summary:
        //     Gets the System.Char object at a specified position in the current System.String
        //     object.
        //
        // Parameters:
        //   index:
        //     A position in the current string.
        //
        // Returns:
        //     The object at position index.
        //
        // Exceptions:
        //   System.IndexOutOfRangeException:
        //     index is greater than or equal to the length of this object or less than
        //     zero.
        public char this[int index] { get { return s[index]; } }

        // Summary:
        //     Returns a reference to this instance of System.String.
        //
        // Returns:
        //     This instance of System.String.
        public object Clone() { return new LowerCaseString(this); }
        //
        // Summary:
        //     Compares two specified System.String objects and returns an integer that
        //     indicates their relative position in the sort order.
        //
        // Parameters:
        //   strA:
        //     The first string to compare.
        //
        //   strB:
        //     The second string to compare.
        //
        // Returns:
        //     A 32-bit signed integer that indicates the lexical relationship between the
        //     two comparands.Value Condition Less than zero strA is less than strB. Zero
        //     strA equals strB. Greater than zero strA is greater than strB.
        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public static int Compare(LowerCaseString strA, LowerCaseString strB) { return String.Compare(strA.s, strB.s); }

        //
        // Summary:
        //     Compares two specified System.String objects using the specified rules, and
        //     returns an integer that indicates their relative position in the sort order.
        //
        // Parameters:
        //   strA:
        //     The first string to compare.
        //
        //   strB:
        //     The second string to compare.
        //
        //   comparisonType:
        //     One of the enumeration values that specifies the rules to use in the comparison.
        //
        // Returns:
        //     A 32-bit signed integer that indicates the lexical relationship between the
        //     two comparands.Value Condition Less than zero strA is less than strB. Zero
        //     strA equals strB. Greater than zero strA is greater than strB.
        //
        // Exceptions:
        //   System.ArgumentException:
        //     comparisonType is not a System.StringComparison value.
        //
        //   System.NotSupportedException:
        //     System.StringComparison is not supported.
        [SecuritySafeCritical]
        public static int Compare(LowerCaseString strA, LowerCaseString strB, StringComparison comparisonType)
        { return String.Compare(strA.s, strB.s, comparisonType); }

        //
        // Summary:
        //     Compares two specified System.String objects, ignoring or honoring their
        //     case, and using culture-specific information to influence the comparison,
        //     and returns an integer that indicates their relative position in the sort
        //     order.
        //
        // Parameters:
        //   strA:
        //     The first string to compare.
        //
        //   strB:
        //     The second string to compare.
        //
        //   ignoreCase:
        //     true to ignore case during the comparison; otherwise, false.
        //
        //   culture:
        //     An object that supplies culture-specific comparison information.
        //
        // Returns:
        //     A 32-bit signed integer that indicates the lexical relationship between the
        //     two comparands.Value Condition Less than zero strA is less than strB. Zero
        //     strA equals strB. Greater than zero strA is greater than strB.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     culture is null.
        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public static int Compare(LowerCaseString strA, LowerCaseString strB, CultureInfo culture)
        { return String.Compare(strA.s, strB.s, true, culture); }
        //
        // Summary:
        //     Compares two specified System.String objects using the specified comparison
        //     options and culture-specific information to influence the comparison, and
        //     returns an integer that indicates the relationship of the two strings to
        //     each other in the sort order.
        //
        // Parameters:
        //   strA:
        //     The first string to compare.
        //
        //   strB:
        //     The second string to compare.
        //
        //   culture:
        //     The culture that supplies culture-specific comparison information.
        //
        //   options:
        //     Options to use when performing the comparison (such as ignoring case or symbols).
        //
        // Returns:
        //     A 32-bit signed integer that indicates the lexical relationship between strA
        //     and strB, as shown in the following tableValueConditionLess than zerostrA
        //     is less than strB.ZerostrA equals strB.Greater than zerostrA is greater than
        //     strB.
        //
        // Exceptions:
        //   System.ArgumentException:
        //     options is not a System.Globalization.CompareOptions value.
        //
        //   System.ArgumentNullException:
        //     culture is null.
        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public static int Compare(LowerCaseString strA, LowerCaseString strB, CultureInfo culture, CompareOptions options)
        { return String.Compare(strA.s, strB.s, culture, options); }
        //
        // Summary:
        //     Compares substrings of two specified System.String objects and returns an
        //     integer that indicates their relative position in the sort order.
        //
        // Parameters:
        //   strA:
        //     The first string to use in the comparison.
        //
        //   indexA:
        //     The position of the substring within strA.
        //
        //   strB:
        //     The second string to use in the comparison.
        //
        //   indexB:
        //     The position of the substring within strB.
        //
        //   length:
        //     The maximum number of characters in the substrings to compare.
        //
        // Returns:
        //     A 32-bit signed integer indicating the lexical relationship between the two
        //     comparands.Value Condition Less than zero The substring in strA is less than
        //     the substring in strB. Zero The substrings are equal, or length is zero.
        //     Greater than zero The substring in strA is greater than the substring in
        //     strB.
        //
        // Exceptions:
        //   System.ArgumentOutOfRangeException:
        //     indexA is greater than strA.System.String.Length.-or- indexB is greater than
        //     strB.System.String.Length.-or- indexA, indexB, or length is negative. -or-Either
        //     indexA or indexB is null, and length is greater than zero.
        public static int Compare(LowerCaseString strA, int indexA, LowerCaseString strB, int indexB, int length)
        { return String.Compare(strA.s, indexA, strB.s, indexB, length); }

        //
        // Summary:
        //     Compares substrings of two specified System.String objects, ignoring or honoring
        //     their case, and returns an integer that indicates their relative position
        //     in the sort order.
        //
        // Parameters:
        //   strA:
        //     The first string to use in the comparison.
        //
        //   indexA:
        //     The position of the substring within strA.
        //
        //   strB:
        //     The second string to use in the comparison.
        //
        //   indexB:
        //     The position of the substring within strB.
        //
        //   length:
        //     The maximum number of characters in the substrings to compare.
        //
        //   ignoreCase:
        //     true to ignore case during the comparison; otherwise, false.
        //
        // Returns:
        //     A 32-bit signed integer that indicates the lexical relationship between the
        //     two comparands.ValueCondition Less than zero The substring in strA is less
        //     than the substring in strB. Zero The substrings are equal, or length is zero.
        //     Greater than zero The substring in strA is greater than the substring in
        //     strB.
        //
        // Exceptions:
        //   System.ArgumentOutOfRangeException:
        //     indexA is greater than strA.System.String.Length.-or- indexB is greater than
        //     strB.System.String.Length.-or- indexA, indexB, or length is negative. -or-Either
        //     indexA or indexB is null, and length is greater than zero.
        public static int Compare(LowerCaseString strA, int indexA, LowerCaseString strB, int indexB, int length, bool ignoreCase)
        { return String.Compare(strA.s, indexA, strB.s, indexB, length); }
        //
        // Summary:
        //     Compares substrings of two specified System.String objects using the specified
        //     rules, and returns an integer that indicates their relative position in the
        //     sort order.
        //
        // Parameters:
        //   strA:
        //     The first string to use in the comparison.
        //
        //   indexA:
        //     The position of the substring within strA.
        //
        //   strB:
        //     The second string to use in the comparison.
        //
        //   indexB:
        //     The position of the substring within strB.
        //
        //   length:
        //     The maximum number of characters in the substrings to compare.
        //
        //   comparisonType:
        //     One of the enumeration values that specifies the rules to use in the comparison.
        //
        // Returns:
        //     A 32-bit signed integer that indicates the lexical relationship between the
        //     two comparands.Value Condition Less than zero The substring in the strA parameter
        //     is less than the substring in the strB parameter.Zero The substrings are
        //     equal, or the length parameter is zero. Greater than zero The substring in
        //     strA is greater than the substring in strB.
        //
        // Exceptions:
        //   System.ArgumentOutOfRangeException:
        //     indexA is greater than strA.System.String.Length.-or- indexB is greater than
        //     strB.System.String.Length.-or- indexA, indexB, or length is negative. -or-Either
        //     indexA or indexB is null, and length is greater than zero.
        //
        //   System.ArgumentException:
        //     comparisonType is not a System.StringComparison value.
        [SecuritySafeCritical]
        public static int Compare(LowerCaseString strA, int indexA, LowerCaseString strB, int indexB, int length, StringComparison comparisonType)
        { return String.Compare(strA.s, indexA, strB.s, indexB, length, comparisonType); }

        //
        // Summary:
        //     Compares substrings of two specified System.String objects, ignoring or honoring
        //     their case and using culture-specific information to influence the comparison,
        //     and returns an integer that indicates their relative position in the sort
        //     order.
        //
        // Parameters:
        //   strA:
        //     The first string to use in the comparison.
        //
        //   indexA:
        //     The position of the substring within strA.
        //
        //   strB:
        //     The second string to use in the comparison.
        //
        //   indexB:
        //     The position of the substring within strB.
        //
        //   length:
        //     The maximum number of characters in the substrings to compare.
        //
        //   ignoreCase:
        //     true to ignore case during the comparison; otherwise, false.
        //
        //   culture:
        //     An object that supplies culture-specific comparison information.
        //
        // Returns:
        //     An integer that indicates the lexical relationship between the two comparands.Value
        //     Condition Less than zero The substring in strA is less than the substring
        //     in strB. Zero The substrings are equal, or length is zero. Greater than zero
        //     The substring in strA is greater than the substring in strB.
        //
        // Exceptions:
        //   System.ArgumentOutOfRangeException:
        //     indexA is greater than strA.System.String.Length.-or- indexB is greater than
        //     strB.System.String.Length.-or- indexA, indexB, or length is negative. -or-Either
        //     strA or strB is null, and length is greater than zero.
        //
        //   System.ArgumentNullException:
        //     culture is null.
        public static int Compare(LowerCaseString strA, int indexA, LowerCaseString strB, int indexB, int length, CultureInfo culture)
        { return String.Compare(strA.s, indexA, strB.s, indexB, length, true, culture); }
        //
        // Summary:
        //     Compares substrings of two specified System.String objects using the specified
        //     comparison options and culture-specific information to influence the comparison,
        //     and returns an integer that indicates the relationship of the two substrings
        //     to each other in the sort order.
        //
        // Parameters:
        //   strA:
        //     The first string to use in the comparison.
        //
        //   indexA:
        //     The starting position of the substring within strA.
        //
        //   strB:
        //     The second string to use in the comparison.
        //
        //   indexB:
        //     The starting position of the substring within strB.
        //
        //   length:
        //     The maximum number of characters in the substrings to compare.
        //
        //   culture:
        //     An object that supplies culture-specific comparison information.
        //
        //   options:
        //     Options to use when performing the comparison (such as ignoring case or symbols).
        //
        // Returns:
        //     An integer that indicates the lexical relationship between the two substrings,
        //     as shown in the following table.ValueConditionLess than zeroThe substring
        //     in strA is less than the substring in strB.ZeroThe substrings are equal or
        //     length is zero.Greater than zeroThe substring in strA is greater than the
        //     substring in strB.
        //
        // Exceptions:
        //   System.ArgumentException:
        //     options is not a System.Globalization.CompareOptions value.
        //
        //   System.ArgumentOutOfRangeException:
        //     indexA is greater than strA.Length.-or-indexB is greater than strB.Length.-or-indexA,
        //     indexB, or length is negative.-or-Either strA or strB is null, and length
        //     is greater than zero.
        //
        //   System.ArgumentNullException:
        //     culture is null.
        public static int Compare(LowerCaseString strA, int indexA, LowerCaseString strB, int indexB, int length, CultureInfo culture, CompareOptions options)
        { return String.Compare(strA.s, indexA, strB.s, indexB, length, culture, options); }
        //
        // Summary:
        //     Compares two specified System.String objects by evaluating the numeric values
        //     of the corresponding System.Char objects in each string.
        //
        // Parameters:
        //   strA:
        //     The first string to compare.
        //
        //   strB:
        //     The second string to compare.
        //
        // Returns:
        //     An integer that indicates the lexical relationship between the two comparands.ValueCondition
        //     Less than zero strA is less than strB. Zero strA and strB are equal. Greater
        //     than zero strA is greater than strB.
        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public static int CompareOrdinal(LowerCaseString strA, LowerCaseString strB)
        { return String.CompareOrdinal(strA.s, strB.s); }
        //
        // Summary:
        //     Compares substrings of two specified System.String objects by evaluating
        //     the numeric values of the corresponding System.Char objects in each substring.
        //
        // Parameters:
        //   strA:
        //     The first string to use in the comparison.
        //
        //   indexA:
        //     The starting index of the substring in strA.
        //
        //   strB:
        //     The second string to use in the comparison.
        //
        //   indexB:
        //     The starting index of the substring in strB.
        //
        //   length:
        //     The maximum number of characters in the substrings to compare.
        //
        // Returns:
        //     A 32-bit signed integer that indicates the lexical relationship between the
        //     two comparands.ValueCondition Less than zero The substring in strA is less
        //     than the substring in strB. Zero The substrings are equal, or length is zero.
        //     Greater than zero The substring in strA is greater than the substring in
        //     strB.
        //
        // Exceptions:
        //   System.ArgumentOutOfRangeException:
        //     strA is not null and indexA is greater than strA.System.String.Length.-or-
        //     strB is not null andindexB is greater than strB.System.String.Length.-or-
        //     indexA, indexB, or length is negative.
        [SecuritySafeCritical]
        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public static int CompareOrdinal(LowerCaseString strA, int indexA, LowerCaseString strB, int indexB, int length)
        { return String.CompareOrdinal(strA.s, indexA, strB.s, indexB, length); }

        //
        // Summary:
        //     Compares this instance with a specified System.Object and indicates whether
        //     this instance precedes, follows, or appears in the same position in the sort
        //     order as the specified System.Object.
        //
        // Parameters:
        //   value:
        //     An object that evaluates to a System.String.
        //
        // Returns:
        //     A 32-bit signed integer that indicates whether this instance precedes, follows,
        //     or appears in the same position in the sort order as the value parameter.Value
        //     Condition Less than zero This instance precedes value. Zero This instance
        //     has the same position in the sort order as value. Greater than zero This
        //     instance follows value.-or- value is null.
        //
        // Exceptions:
        //   System.ArgumentException:
        //     value is not a System.String.
        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public int CompareTo(object value)
        {
            if ((value != null) && (value is LowerCaseString))
            {
                return s.CompareTo(((LowerCaseString)value).s);
            }
            return s.CompareTo(value);
        }
        //
        // Summary:
        //     Compares this instance with a specified System.String object and indicates
        //     whether this instance precedes, follows, or appears in the same position
        //     in the sort order as the specified System.String.
        //
        // Parameters:
        //   strB:
        //     The string to compare with this instance.
        //
        // Returns:
        //     A 32-bit signed integer that indicates whether this instance precedes, follows,
        //     or appears in the same position in the sort order as the value parameter.Value
        //     Condition Less than zero This instance precedes strB. Zero This instance
        //     has the same position in the sort order as strB. Greater than zero This instance
        //     follows strB.-or- strB is null.
        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public int CompareTo(LowerCaseString strB) { return s.CompareTo(strB.s); }
        //
        // Summary:
        //     Concatenates the members of a constructed System.Collections.Generic.IEnumerable<T>
        //     collection of type System.String.
        //
        // Parameters:
        //   values:
        //     A collection object that implements System.Collections.Generic.IEnumerable<T>
        //     and whose generic type argument is System.String.
        //
        // Returns:
        //     The concatenated strings in values.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     values is null.
        [ComVisible(false)]
        public static LowerCaseString Concat(IEnumerable<LowerCaseString> values)
        {
            StringBuilder sb = new StringBuilder();
            foreach (LowerCaseString lcs in values)
            {
                sb.Append(lcs.s);
            }
            return new LowerCaseString(sb.ToString());
        }

        //
        // Summary:
        //     Concatenates the members of an System.Collections.Generic.IEnumerable<T>
        //     implementation.
        //
        // Parameters:
        //   values:
        //     A collection object that implements the System.Collections.Generic.IEnumerable<T>
        //     interface.
        //
        // Type parameters:
        //   T:
        //     The type of the members of values.
        //
        // Returns:
        //     The concatenated members in values.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     values is null.
        [ComVisible(false)]
        public static LowerCaseString Concat<T>(IEnumerable<T> values)
        {
            StringBuilder sb = new StringBuilder();
            foreach (T item in values)
            {
                sb.Append(item.ToString());
            }
            return new LowerCaseString(sb.ToString());
        }

        //
        // Summary:
        //     Creates the string representation of a specified object.
        //
        // Parameters:
        //   arg0:
        //     The object to represent, or null.
        //
        // Returns:
        //     The string representation of the value of arg0, or System.String.Empty if
        //     arg0 is null.
        public static LowerCaseString Concat(object arg0)
        { return new LowerCaseString(String.Concat(arg0)); }

        //
        // Summary:
        //     Concatenates the string representations of the elements in a specified System.Object
        //     array.
        //
        // Parameters:
        //   args:
        //     An object array that contains the elements to concatenate.
        //
        // Returns:
        //     The concatenated string representations of the values of the elements in
        //     args.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     args is null.
        //
        //   System.OutOfMemoryException:
        //     Out of memory.
        public static LowerCaseString Concat(params object[] args)
        { return new LowerCaseString(String.Concat(args)); }
        //
        // Summary:
        //     Concatenates the elements of a specified System.String array.
        //
        // Parameters:
        //   values:
        //     An array of string instances.
        //
        // Returns:
        //     The concatenated elements of values.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     values is null.
        //
        //   System.OutOfMemoryException:
        //     Out of memory.
        public static LowerCaseString Concat(params string[] values)
        { return new LowerCaseString(String.Concat(values)); }

        //
        // Summary:
        //     Concatenates the string representations of two specified objects.
        //
        // Parameters:
        //   arg0:
        //     The first object to concatenate.
        //
        //   arg1:
        //     The second object to concatenate.
        //
        // Returns:
        //     The concatenated string representations of the values of arg0 and arg1.
        public static LowerCaseString Concat(object arg0, object arg1)
        { return new LowerCaseString(String.Concat(arg0, arg1)); }

        //
        // Summary:
        //     Concatenates two specified instances of System.String.
        //
        // Parameters:
        //   str0:
        //     The first string to concatenate.
        //
        //   str1:
        //     The second string to concatenate.
        //
        // Returns:
        //     The concatenation of str0 and str1.
        [SecuritySafeCritical]
        public static LowerCaseString Concat(string str0, string str1)
        { return new LowerCaseString(String.Concat(str0, str1)); }


        //
        // Summary:
        //     Returns a value indicating whether the specified System.String object occurs
        //     within this string.
        //
        // Parameters:
        //   value:
        //     The string to seek.
        //
        // Returns:
        //     true if the value parameter occurs within this string, or if value is the
        //     empty string (""); otherwise, false.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     value is null.
        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public bool Contains(LowerCaseString value) { return s.Contains(value.s); }

        //
        // Summary:
        //     Creates a new instance of System.String with the same value as a specified
        //     System.String.
        //
        // Parameters:
        //   str:
        //     The string to copy.
        //
        // Returns:
        //     A new string with the same value as str.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     str is null.
        [SecuritySafeCritical]
        public static LowerCaseString Copy(LowerCaseString str) { return new LowerCaseString(str); }
        //
        // Summary:
        //     Copies a specified number of characters from a specified position in this
        //     instance to a specified position in an array of Unicode characters.
        //
        // Parameters:
        //   sourceIndex:
        //     The index of the first character in this instance to copy.
        //
        //   destination:
        //     An array of Unicode characters to which characters in this instance are copied.
        //
        //   destinationIndex:
        //     The index in destination at which the copy operation begins.
        //
        //   count:
        //     The number of characters in this instance to copy to destination.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     destination is null.
        //
        //   System.ArgumentOutOfRangeException:
        //     sourceIndex, destinationIndex, or count is negative -or- count is greater
        //     than the length of the substring from startIndex to the end of this instance
        //     -or- count is greater than the length of the subarray from destinationIndex
        //     to the end of destination
        [SecuritySafeCritical]
        public void CopyTo(int sourceIndex, char[] destination, int destinationIndex, int count)
        { s.CopyTo(sourceIndex, destination, destinationIndex, count); }
        //
        // Summary:
        //     Determines whether the end of this string instance matches the specified
        //     string.
        //
        // Parameters:
        //   value:
        //     The string to compare to the substring at the end of this instance.
        //
        // Returns:
        //     true if value matches the end of this instance; otherwise, false.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     value is null.
        public bool EndsWith(LowerCaseString value) { return s.EndsWith(value.s); }

        //
        // Summary:
        //     Determines whether the end of this string instance matches the specified
        //     string when compared using the specified comparison option.
        //
        // Parameters:
        //   value:
        //     The string to compare to the substring at the end of this instance.
        //
        //   comparisonType:
        //     One of the enumeration values that determines how this string and value are
        //     compared.
        //
        // Returns:
        //     true if the value parameter matches the end of this string; otherwise, false.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     value is null.
        //
        //   System.ArgumentException:
        //     comparisonType is not a System.StringComparison value.
        [ComVisible(false)]
        [SecuritySafeCritical]
        public bool EndsWith(LowerCaseString value, StringComparison comparisonType) { return s.EndsWith(value.s, comparisonType); }
        //
        // Summary:
        //     Determines whether the end of this string instance matches the specified
        //     string when compared using the specified culture.
        //
        // Parameters:
        //   value:
        //     The string to compare to the substring at the end of this instance.
        //
        //   ignoreCase:
        //     true to ignore case during the comparison; otherwise, false.
        //
        //   culture:
        //     Cultural information that determines how this instance and value are compared.
        //     If culture is null, the current culture is used.
        //
        // Returns:
        //     true if the value parameter matches the end of this string; otherwise, false.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     value is null.
        public bool EndsWith(LowerCaseString value, CultureInfo culture) { return s.EndsWith(value.s, true, culture); }
        //
        // Summary:
        //     Determines whether this instance and a specified object, which must also
        //     be a System.String object, have the same value.
        //
        // Parameters:
        //   obj:
        //     The string to compare to this instance.
        //
        // Returns:
        //     true if obj is a System.String and its value is the same as this instance;
        //     otherwise, false.
        //RE_INSERT WHEN THE COMPILER GROWS UP [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        public override bool Equals(object obj)
        {
            if ((obj != null) && (obj is LowerCaseString))
            {
                return s.Equals(((LowerCaseString)obj).s);
            }
            return s.Equals(obj);
        }
        //
        // Summary:
        //     Determines whether this instance and another specified System.String object
        //     have the same value.
        //
        // Parameters:
        //   value:
        //     The string to compare to this instance.
        //
        // Returns:
        //     true if the value of the value parameter is the same as this instance; otherwise,
        //     false.
        //RE_INSERT WHEN THE COMPILER GROWS UP [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        //RE_INSERT WHEN THE COMPILER GROWS UP [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public bool Equals(LowerCaseString value) { return s.Equals(value.s); }
        //
        // Summary:
        //     Determines whether two specified System.String objects have the same value.
        //
        // Parameters:
        //   a:
        //     The first string to compare, or null.
        //
        //   b:
        //     The second string to compare, or null.
        //
        // Returns:
        //     true if the value of a is the same as the value of b; otherwise, false. If
        //     both a and b are null, the method returns true.
        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public static bool Equals(LowerCaseString a, LowerCaseString b) { return string.Equals(a.s, b.s); }
        //
        // Summary:
        //     Determines whether this string and a specified System.String object have
        //     the same value. A parameter specifies the culture, case, and sort rules used
        //     in the comparison.
        //
        // Parameters:
        //   value:
        //     The string to compare to this instance.
        //
        //   comparisonType:
        //     One of the enumeration values that specifies how the strings will be compared.
        //
        // Returns:
        //     true if the value of the value parameter is the same as this string; otherwise,
        //     false.
        //
        // Exceptions:
        //   System.ArgumentException:
        //     comparisonType is not a System.StringComparison value.
        [SecuritySafeCritical]
        public bool Equals(LowerCaseString value, StringComparison comparisonType) { return s.Equals(value.s, comparisonType); }
        //
        // Summary:
        //     Determines whether two specified System.String objects have the same value.
        //     A parameter specifies the culture, case, and sort rules used in the comparison.
        //
        // Parameters:
        //   a:
        //     The first string to compare, or null.
        //
        //   b:
        //     The second string to compare, or null.
        //
        //   comparisonType:
        //     One of the enumeration values that specifies the rules for the comparison.
        //
        // Returns:
        //     true if the value of the a parameter is equal to the value of the b parameter;
        //     otherwise, false.
        //
        // Exceptions:
        //   System.ArgumentException:
        //     comparisonType is not a System.StringComparison value.
        [SecuritySafeCritical]
        public static bool Equals(LowerCaseString a, LowerCaseString b, StringComparison comparisonType)
        { return string.Equals(a.s, b.s, comparisonType); }
        //
        // Summary:
        //     Replaces one or more format items in a specified string with the string representation
        //     of a specified object.
        //
        // Parameters:
        //   format:
        //     A composite format string (see Remarks).
        //
        //   arg0:
        //     The object to format.
        //
        // Returns:
        //     A copy of format in which any format items are replaced by the string representation
        //     of arg0.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     format is null.
        //
        //   System.FormatException:
        //     The format item in format is invalid.-or- The index of a format item is not
        //     zero.
        public static LowerCaseString Format(string format, object arg0)
        { return new LowerCaseString(string.Format(format, arg0)); }

        //
        // Summary:
        //     Replaces the format item in a specified string with the string representation
        //     of a corresponding object in a specified array.
        //
        // Parameters:
        //   format:
        //     A composite format string (see Remarks).
        //
        //   args:
        //     An object array that contains zero or more objects to format.
        //
        // Returns:
        //     A copy of format in which the format items have been replaced by the string
        //     representation of the corresponding objects in args.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     format or args is null.
        //
        //   System.FormatException:
        //     format is invalid.-or- The index of a format item is less than zero, or greater
        //     than or equal to the length of the args array.
        public static LowerCaseString Format(string format, params object[] args)
        { return new LowerCaseString(string.Format(format, args)); }
        //
        // Summary:
        //     Replaces the format item in a specified string with the string representation
        //     of a corresponding object in a specified array. A specified parameter supplies
        //     culture-specific formatting information.
        //
        // Parameters:
        //   provider:
        //     An object that supplies culture-specific formatting information.
        //
        //   format:
        //     A composite format string (see Remarks).
        //
        //   args:
        //     An object array that contains zero or more objects to format.
        //
        // Returns:
        //     A copy of format in which the format items have been replaced by the string
        //     representation of the corresponding objects in args.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     format or args is null.
        //
        //   System.FormatException:
        //     format is invalid.-or- The index of a format item is less than zero, or greater
        //     than or equal to the length of the args array.
        public static LowerCaseString Format(IFormatProvider provider, string format, params object[] args)
        { return new LowerCaseString(string.Format(format, args)); }

        //
        // Summary:
        //     Retrieves an object that can iterate through the individual characters in
        //     this string.
        //
        // Returns:
        //     An enumerator object.
        public CharEnumerator GetEnumerator() { return s.GetEnumerator(); }

        //
        // Summary:
        //     Returns the hash code for this string.
        //
        // Returns:
        //     A 32-bit signed integer hash code.
        //RE_INSERT WHEN THE COMPILER GROWS UP [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        [SecuritySafeCritical]
        public override int GetHashCode() { return s.GetHashCode(); }

        //
        // Summary:
        //     Reports the zero-based index of the first occurrence of the specified Unicode
        //     character in this string.
        //
        // Parameters:
        //   value:
        //     A Unicode character to seek (as lower case). 
        //
        // Returns:
        //     The zero-based index position of value if that character is found, or -1
        //     if it is not.
        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public int IndexOf(char valueCaseIgnored) { return s.IndexOf(Char.ToLower(valueCaseIgnored)); }

        //
        // Summary:
        //     Reports the zero-based index of the first occurrence of the specified string
        //     in this instance.
        //
        // Parameters:
        //   value:
        //     The string to seek.
        //
        // Returns:
        //     The zero-based index position of value if that string is found, or -1 if
        //     it is not. If value is System.String.Empty, the return value is 0.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     value is null.
        public int IndexOf(LowerCaseString value) { return s.IndexOf(value.s); }
        //
        // Summary:
        //     Reports the zero-based index of the first occurrence of the specified Unicode
        //     character in this string. The search starts at a specified character position.
        //
        // Parameters:
        //   value:
        //     A Unicode character to seek (as lower case).
        //
        //   startIndex:
        //     The search starting position.
        //
        // Returns:
        //     The zero-based index position of value if that character is found, or -1
        //     if it is not.
        //
        // Exceptions:
        //   System.ArgumentOutOfRangeException:
        //     startIndex is less than 0 (zero) or greater than the length of the string.
        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public int IndexOf(char valueCaseIgnored, int startIndex) { return s.IndexOf(Char.ToLower(valueCaseIgnored), startIndex); }
        //
        // Summary:
        //     Reports the zero-based index of the first occurrence of the specified string
        //     in this instance. The search starts at a specified character position.
        //
        // Parameters:
        //   value:
        //     The string to seek.
        //
        //   startIndex:
        //     The search starting position.
        //
        // Returns:
        //     The zero-based index position of value if that string is found, or -1 if
        //     it is not. If value is System.String.Empty, the return value is startIndex.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     value is null.
        //
        //   System.ArgumentOutOfRangeException:
        //     startIndex is less than 0 (zero) or greater than the length of this string.
        public int IndexOf(LowerCaseString value, int startIndex) { return s.IndexOf(value.s, startIndex); }
        //
        // Summary:
        //     Reports the zero-based index of the first occurrence of the specified string
        //     in the current System.String object. A parameter specifies the type of search
        //     to use for the specified string.
        //
        // Parameters:
        //   value:
        //     The string to seek.
        //
        //   comparisonType:
        //     One of the enumeration values that specifies the rules for the search.
        //
        // Returns:
        //     The index position of the value parameter if that string is found, or -1
        //     if it is not. If value is System.String.Empty, the return value is 0.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     value is null.
        //
        //   System.ArgumentException:
        //     comparisonType is not a valid System.StringComparison value.
        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public int IndexOf(LowerCaseString value, StringComparison comparisonType)
        { return s.IndexOf(value.s, comparisonType); }
        //
        // Summary:
        //     Reports the zero-based index of the first occurrence of the specified character
        //     in this instance. The search starts at a specified character position and
        //     examines a specified number of character positions.
        //
        // Parameters:
        //   value:
        //     A Unicode character to seek (as lower case).
        //
        //   startIndex:
        //     The search starting position.
        //
        //   count:
        //     The number of character positions to examine.
        //
        // Returns:
        //     The zero-based index position of value if that character is found, or -1
        //     if it is not.
        //
        // Exceptions:
        //   System.ArgumentOutOfRangeException:
        //     count or startIndex is negative.-or- startIndex is greater than the length
        //     of this string.-or-count is greater than the length of this string minus
        //     startIndex.
        [SecuritySafeCritical]
        public int IndexOf(char valueCaseIgnored, int startIndex, int count)
        { return s.IndexOf(Char.ToLower(valueCaseIgnored), startIndex, count); }
        //
        // Summary:
        //     Reports the zero-based index of the first occurrence of the specified string
        //     in this instance. The search starts at a specified character position and
        //     examines a specified number of character positions.
        //
        // Parameters:
        //   value:
        //     The string to seek.
        //
        //   startIndex:
        //     The search starting position.
        //
        //   count:
        //     The number of character positions to examine.
        //
        // Returns:
        //     The zero-based index position of value if that string is found, or -1 if
        //     it is not. If value is System.String.Empty, the return value is startIndex.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     value is null.
        //
        //   System.ArgumentOutOfRangeException:
        //     count or startIndex is negative.-or- startIndex is greater than the length
        //     of this string.-or-count is greater than the length of this string minus
        //     startIndex.
        public int IndexOf(LowerCaseString value, int startIndex, int count)
        { return s.IndexOf(value.s, startIndex, count); }
        //
        // Summary:
        //     Reports the zero-based index of the first occurrence of the specified string
        //     in the current System.String object. Parameters specify the starting search
        //     position in the current string and the type of search to use for the specified
        //     string.
        //
        // Parameters:
        //   value:
        //     The string to seek.
        //
        //   startIndex:
        //     The search starting position.
        //
        //   comparisonType:
        //     One of the enumeration values that specifies the rules for the search.
        //
        // Returns:
        //     The zero-based index position of the value parameter if that string is found,
        //     or -1 if it is not. If value is System.String.Empty, the return value is
        //     startIndex.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     value is null.
        //
        //   System.ArgumentOutOfRangeException:
        //     startIndex is less than 0 (zero) or greater than the length of this string.
        //
        //   System.ArgumentException:
        //     comparisonType is not a valid System.StringComparison value.
        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public int IndexOf(LowerCaseString value, int startIndex, StringComparison comparisonType)
        { return s.IndexOf(value.s, startIndex, comparisonType); }
        //
        // Summary:
        //     Reports the zero-based index of the first occurrence of the specified string
        //     in the current System.String object. Parameters specify the starting search
        //     position in the current string, the number of characters in the current string
        //     to search, and the type of search to use for the specified string.
        //
        // Parameters:
        //   value:
        //     The string to seek.
        //
        //   startIndex:
        //     The search starting position.
        //
        //   count:
        //     The number of character positions to examine.
        //
        //   comparisonType:
        //     One of the enumeration values that specifies the rules for the search.
        //
        // Returns:
        //     The zero-based index position of the value parameter if that string is found,
        //     or -1 if it is not. If value is System.String.Empty, the return value is
        //     startIndex.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     value is null.
        //
        //   System.ArgumentOutOfRangeException:
        //     count or startIndex is negative.-or- startIndex is greater than the length
        //     of this instance.-or-count is greater than the length of this string minus
        //     startIndex.
        //
        //   System.ArgumentException:
        //     comparisonType is not a valid System.StringComparison value.
        [SecuritySafeCritical]
        public int IndexOf(LowerCaseString value, int startIndex, int count, StringComparison comparisonType)
        { return s.IndexOf(value.s, startIndex, count, comparisonType); }
        //
        // Summary:
        //     Reports the zero-based index of the first occurrence in this instance of
        //     any character in a specified array of Unicode characters.
        //
        // Parameters:
        //   anyOfIgnoreCase:
        //     A Unicode character array containing one or more characters to seek (ignoring case).
        //
        // Returns:
        //     The zero-based index position of the first occurrence in this instance where
        //     any character in anyOf was found; -1 if no character in anyOf was found.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     anyOf is null.
        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public int IndexOfAny(char[] anyOfIgnoreCase)
        {
            return s.IndexOfAny(ToLowerSet(anyOfIgnoreCase));
        }

        //
        // Summary:
        //     Reports the zero-based index of the first occurrence in this instance of
        //     any character in a specified array of Unicode characters. The search starts
        //     at a specified character position.
        //
        // Parameters:
        //   anyOfIgnoreCase:
        //     A Unicode character array containing one or more characters to seek (ignoring case).
        //
        //   startIndex:
        //     The search starting position.
        //
        // Returns:
        //     The zero-based index position of the first occurrence in this instance where
        //     any character in anyOf was found; -1 if no character in anyOf was found.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     anyOf is null.
        //
        //   System.ArgumentOutOfRangeException:
        //     startIndex is negative.-or- startIndex is greater than the number of characters
        //     in this instance.
        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public int IndexOfAny(char[] anyOfIgnoreCase, int startIndex)
        {
            return s.IndexOfAny(ToLowerSet(anyOfIgnoreCase), startIndex);
        }

        //
        // Summary:
        //     Reports the zero-based index of the first occurrence in this instance of
        //     any character in a specified array of Unicode characters. The search starts
        //     at a specified character position and examines a specified number of character
        //     positions.
        //
        // Parameters:
        //   anyOfIgnoreCase:
        //     A Unicode character array containing one or more characters to seek (ignoring case).
        //
        //   startIndex:
        //     The search starting position.
        //
        //   count:
        //     The number of character positions to examine.
        //
        // Returns:
        //     The zero-based index position of the first occurrence in this instance where
        //     any character in anyOf was found; -1 if no character in anyOf was found.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     anyOf is null.
        //
        //   System.ArgumentOutOfRangeException:
        //     count or startIndex is negative.-or- count + startIndex is greater than the
        //     number of characters in this instance.
        [SecuritySafeCritical]
        public int IndexOfAny(char[] anyOfIgnoreCase, int startIndex, int count)
        {
            return s.IndexOfAny(ToLowerSet(anyOfIgnoreCase), startIndex, count);
        }

        //
        // Summary:
        //     Returns a new string in which a specified string is inserted at a specified
        //     index position in this instance.
        //
        // Parameters:
        //   startIndex:
        //     The zero-based index position of the insertion.
        //
        //   value:
        //     The string to insert.
        //
        // Returns:
        //     A new string that is equivalent to this instance, but with value inserted
        //     at position startIndex.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     value is null.
        //
        //   System.ArgumentOutOfRangeException:
        //     startIndex is negative or greater than the length of this instance.
        [SecuritySafeCritical]
        public LowerCaseString Insert(int startIndex, LowerCaseString value)
        { return new LowerCaseString(s.Insert(startIndex, value.s)); }

        //
        // Summary:
        //     Indicates whether the specified string is null or an System.String.Empty
        //     string.
        //
        // Parameters:
        //   value:
        //     The string to test.
        //
        // Returns:
        //     true if the value parameter is null or an empty string (""); otherwise, false.
        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public static bool IsNullOrEmpty(LowerCaseString value)
        {
            if (value == null)
            {
                return true;
            }
            return string.IsNullOrEmpty(value.s);
        }

        //
        // Summary:
        //     Indicates whether a specified string is null, empty, or consists only of
        //     white-space characters.
        //
        // Parameters:
        //   value:
        //     The string to test.
        //
        // Returns:
        //     true if the value parameter is null or System.String.Empty, or if value consists
        //     exclusively of white-space characters.
        public static bool IsNullOrWhiteSpace(LowerCaseString value)
        {
            if (value == null)
            {
                return true;
            }
            return string.IsNullOrWhiteSpace(value.s);
        }

        //
        // Summary:
        //     Concatenates the members of a constructed System.Collections.Generic.IEnumerable<T>
        //     collection of type System.String, using the specified separator between each
        //     member.
        //
        // Parameters:
        //   separator:
        //     The string to use as a separator.
        //
        //   values:
        //     A collection that contains the strings to concatenate.
        //
        // Returns:
        //     A string that consists of the members of values delimited by the separator
        //     string. If values has no members, the method returns System.String.Empty.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     values is null.
        [ComVisible(false)]
        public static LowerCaseString Join(LowerCaseString separator, IEnumerable<LowerCaseString> values)
        { return new LowerCaseString(string.Join(separator.s, ToString(values))); }

        //
        // Summary:
        //     Concatenates the members of a constructed System.Collections.Generic.IEnumerable<T>
        //     collection of type System.String, using the specified separator between each
        //     member.
        //
        // Parameters:
        //   separator:
        //     The string to use as a separator.
        //
        //   values:
        //     A collection that contains the strings to concatenate.
        //
        // Returns:
        //     A string that consists of the members of values delimited by the separator
        //     string. If values has no members, the method returns System.String.Empty.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     values is null.
        [ComVisible(false)]
        public static LowerCaseString Join(LowerCaseString separator, IEnumerable<string> values)
        { return new LowerCaseString(string.Join(separator.s, values)); }

        //
        // Summary:
        //     Concatenates the members of a collection, using the specified separator between
        //     each member.
        //
        // Parameters:
        //   separator:
        //     The string to use as a separator.
        //
        //   values:
        //     A collection that contains the objects to concatenate.
        //
        // Type parameters:
        //   T:
        //     The type of the members of values.
        //
        // Returns:
        //     A string that consists of the members of values delimited by the separator
        //     string. If values has no members, the method returns System.String.Empty.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     values is null.
        [ComVisible(false)]
        public static LowerCaseString Join<T>(LowerCaseString separator, IEnumerable<T> values)
        { return new LowerCaseString(string.Join<T>(separator.s, values)); }

        //
        // Summary:
        //     Concatenates the elements of an object array, using the specified separator
        //     between each element.
        //
        // Parameters:
        //   separator:
        //     The string to use as a separator.
        //
        //   values:
        //     An array that contains the elements to concatenate.
        //
        // Returns:
        //     A string that consists of the elements of values delimited by the separator
        //     string. If values is an empty array, the method returns System.String.Empty.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     values is null.
        [ComVisible(false)]
        public static LowerCaseString Join(LowerCaseString separator, params object[] values)
        { return new LowerCaseString(string.Join(separator.s, values)); }
        //
        // Summary:
        //     Concatenates all the elements of a string array, using the specified separator
        //     between each element.
        //
        // Parameters:
        //   separator:
        //     The string to use as a separator.
        //
        //   value:
        //     An array that contains the elements to concatenate.
        //
        // Returns:
        //     A string that consists of the elements in value delimited by the separator
        //     string. If value is an empty array, the method returns System.String.Empty.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     value is null.
        public static LowerCaseString Join(LowerCaseString separator, params string[] value)
        { return new LowerCaseString(string.Join(separator.s, value)); }


        public static LowerCaseString Join(LowerCaseString separator, params LowerCaseString[] value)
        { return new LowerCaseString(string.Join(separator.s, ToString(value))); }

        //
        // Summary:
        //     Concatenates the specified elements of a string array, using the specified
        //     separator between each element.
        //
        // Parameters:
        //   separator:
        //     The string to use as a separator.
        //
        //   value:
        //     An array that contains the elements to concatenate.
        //
        //   startIndex:
        //     The first element in value to use.
        //
        //   count:
        //     The number of elements of value to use.
        //
        // Returns:
        //     A string that consists of the strings in value delimited by the separator
        //     string. -or-System.String.Empty if count is zero, value has no elements,
        //     or separator and all the elements of value are System.String.Empty.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     value is null.
        //
        //   System.ArgumentOutOfRangeException:
        //     startIndex or count is less than 0.-or- startIndex plus count is greater
        //     than the number of elements in value.
        //
        //   System.OutOfMemoryException:
        //     Out of memory.
        [SecuritySafeCritical]
        public static LowerCaseString Join(LowerCaseString separator, string[] value, int startIndex, int count)
        { return new LowerCaseString(string.Join(separator.s, value, startIndex, count)); }


        public static LowerCaseString Join(LowerCaseString separator, LowerCaseString[] value, int startIndex, int count)
        { return new LowerCaseString(string.Join(separator.s, ToString(value), startIndex, count)); }

        //
        // Summary:
        //     Reports the zero-based index position of the last occurrence of a specified
        //     Unicode character within this instance.
        //
        // Parameters:
        //   valueIgnoreCase:
        //     The Unicode character to seek (ignoring case).
        //
        // Returns:
        //     The zero-based index position of value if that character is found, or -1
        //     if it is not.
        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public int LastIndexOf(char valueIgnoreCase) { return s.LastIndexOf(char.ToLower(valueIgnoreCase)); }
        //
        // Summary:
        //     Reports the zero-based index position of the last occurrence of a specified
        //     string within this instance.
        //
        // Parameters:
        //   value:
        //     The string to seek.
        //
        // Returns:
        //     The zero-based index position of value if that string is found, or -1 if
        //     it is not. If value is System.String.Empty, the return value is the last
        //     index position in this instance.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     value is null.
        public int LastIndexOf(LowerCaseString value)
        { return s.LastIndexOf(value.s); }

        //
        // Summary:
        //     Reports the zero-based index position of the last occurrence of a specified
        //     Unicode character within this instance. The search starts at a specified
        //     character position and proceeds backward toward the beginning of the string.
        //
        // Parameters:
        //   valueIgnoreCase:
        //     The Unicode character to seek (ignoring case).
        //
        //   startIndex:
        //     The starting position of the search. The search proceeds from startIndex
        //     toward the beginning of this instance.
        //
        // Returns:
        //     The zero-based index position of value if that character is found, or -1
        //     if it is not found or if the current instance equals System.String.Empty.
        //
        // Exceptions:
        //   System.ArgumentOutOfRangeException:
        //     The current instance does not equal System.String.Empty, and startIndex is
        //     less than zero or greater than or equal to the length of this instance.
        public int LastIndexOf(char valueIgnoreCase, int startIndex)
        { return s.LastIndexOf(char.ToLower(valueIgnoreCase), startIndex); }

        //
        // Summary:
        //     Reports the zero-based index position of the last occurrence of a specified
        //     string within this instance. The search starts at a specified character position
        //     and proceeds backward toward the beginning of the string.
        //
        // Parameters:
        //   value:
        //     The string to seek.
        //
        //   startIndex:
        //     The search starting position. The search proceeds from startIndex toward
        //     the beginning of this instance.
        //
        // Returns:
        //     The zero-based index position of value if that string is found, or -1 if
        //     it is not found or if the current instance equals System.String.Empty. If
        //     value is System.String.Empty, the return value is the smaller of startIndex
        //     and the last index position in this instance.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     value is null.
        //
        //   System.ArgumentOutOfRangeException:
        //     The current instance does not equal System.String.Empty, and startIndex is
        //     less than zero or greater than the length of the current instance. -or-The
        //     current instance equals System.String.Empty, and startIndex is greater than
        //     zero.
        public int LastIndexOf(LowerCaseString value, int startIndex)
        { return s.LastIndexOf(value.s, startIndex); }
        //
        // Summary:
        //     Reports the zero-based index of the last occurrence of a specified string
        //     within the current System.String object. A parameter specifies the type of
        //     search to use for the specified string.
        //
        // Parameters:
        //   value:
        //     The string to seek.
        //
        //   comparisonType:
        //     One of the enumeration values that specifies the rules for the search.
        //
        // Returns:
        //     The index position of the value parameter if that string is found, or -1
        //     if it is not. If value is System.String.Empty, the return value is the last
        //     index position in this instance.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     value is null.
        //
        //   System.ArgumentException:
        //     comparisonType is not a valid System.StringComparison value.
        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public int LastIndexOf(LowerCaseString value, StringComparison comparisonType)
        { return s.LastIndexOf(value.s, comparisonType); }
        //
        // Summary:
        //     Reports the zero-based index position of the last occurrence of the specified
        //     Unicode character in a substring within this instance. The search starts
        //     at a specified character position and proceeds backward toward the beginning
        //     of the string for a specified number of character positions.
        //
        // Parameters:
        //   valueIgnoreCase:
        //     The Unicode character to seek (ignoring case).
        //
        //   startIndex:
        //     The starting position of the search. The search proceeds from startIndex
        //     toward the beginning of this instance.
        //
        //   count:
        //     The number of character positions to examine.
        //
        // Returns:
        //     The zero-based index position of value if that character is found, or -1
        //     if it is not found or if the current instance equals System.String.Empty.
        //
        // Exceptions:
        //   System.ArgumentOutOfRangeException:
        //     The current instance does not equal System.String.Empty, and startIndex is
        //     less than zero or greater than or equal to the length of this instance.-or-The
        //     current instance does not equal System.String.Empty, and startIndex - count
        //     + 1 is less than zero.
        [SecuritySafeCritical]
        public int LastIndexOf(char valueIgnoreCase, int startIndex, int count)
        { return s.LastIndexOf(char.ToLower(valueIgnoreCase), startIndex, count); }
        //
        // Summary:
        //     Reports the zero-based index position of the last occurrence of a specified
        //     string within this instance. The search starts at a specified character position
        //     and proceeds backward toward the beginning of the string for a specified
        //     number of character positions.
        //
        // Parameters:
        //   value:
        //     The string to seek.
        //
        //   startIndex:
        //     The search starting position. The search proceeds from startIndex toward
        //     the beginning of this instance.
        //
        //   count:
        //     The number of character positions to examine.
        //
        // Returns:
        //     The zero-based index position of value if that string is found, or -1 if
        //     it is not found or if the current instance equals System.String.Empty. If
        //     value is System.String.Empty, the return value is the smaller of startIndex
        //     and the last index position in this instance.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     value is null.
        //
        //   System.ArgumentOutOfRangeException:
        //     count is negative.-or-The current instance does not equal System.String.Empty,
        //     and startIndex is negative.-or- The current instance does not equal System.String.Empty,
        //     and startIndex is greater than the length of this instance.-or-The current
        //     instance does not equal System.String.Empty, and startIndex - count + 1 specifies
        //     a position that is not within this instance.
        public int LastIndexOf(LowerCaseString value, int startIndex, int count)
        { return s.LastIndexOf(value.s, startIndex, count); }
        //
        // Summary:
        //     Reports the zero-based index of the last occurrence of a specified string
        //     within the current System.String object. The search starts at a specified
        //     character position and proceeds backward toward the beginning of the string.
        //     A parameter specifies the type of comparison to perform when searching for
        //     the specified string.
        //
        // Parameters:
        //   value:
        //     The string to seek.
        //
        //   startIndex:
        //     The search starting position. The search proceeds from startIndex toward
        //     the beginning of this instance.
        //
        //   comparisonType:
        //     One of the enumeration values that specifies the rules for the search.
        //
        // Returns:
        //     The index position of the value parameter if that string is found, or -1
        //     if it is not found or if the current instance equals System.String.Empty.
        //     If value is System.String.Empty, the return value is the smaller of startIndex
        //     and the last index position in this instance.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     value is null.
        //
        //   System.ArgumentOutOfRangeException:
        //     The current instance does not equal System.String.Empty, and startIndex is
        //     less than zero or greater than the length of the current instance. -or-The
        //     current instance equals System.String.Empty, and startIndex is greater than
        //     zero.
        //
        //   System.ArgumentException:
        //     comparisonType is not a valid System.StringComparison value.
        public int LastIndexOf(LowerCaseString value, int startIndex, StringComparison comparisonType)
        { return s.LastIndexOf(value.s, startIndex, comparisonType); }
        //
        // Summary:
        //     Reports the zero-based index position of the last occurrence of a specified
        //     string within this instance. The search starts at a specified character position
        //     and proceeds backward toward the beginning of the string for the specified
        //     number of character positions. A parameter specifies the type of comparison
        //     to perform when searching for the specified string.
        //
        // Parameters:
        //   value:
        //     The string to seek.
        //
        //   startIndex:
        //     The search starting position. The search proceeds from startIndex toward
        //     the beginning of this instance.
        //
        //   count:
        //     The number of character positions to examine.
        //
        //   comparisonType:
        //     One of the enumeration values that specifies the rules for the search.
        //
        // Returns:
        //     The index position of the value parameter if that string is found, or -1
        //     if it is not found or if the current instance equals System.String.Empty.
        //     If value is System.String.Empty, the return value is the smaller of startIndex
        //     and the last index position in this instance.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     value is null.
        //
        //   System.ArgumentOutOfRangeException:
        //     count is negative.-or-The current instance does not equal System.String.Empty,
        //     and startIndex is negative.-or- The current instance does not equal System.String.Empty,
        //     and startIndex is greater than the length of this instance.-or-The current
        //     instance does not equal System.String.Empty, and startIndex + 1 - count specifies
        //     a position that is not within this instance.
        //
        //   System.ArgumentException:
        //     comparisonType is not a valid System.StringComparison value.
        [SecuritySafeCritical]
        public int LastIndexOf(LowerCaseString value, int startIndex, int count, StringComparison comparisonType)
        { return s.LastIndexOf(value.s, startIndex, count, comparisonType); }


        //
        // Summary:
        //     Reports the zero-based index position of the last occurrence in this instance
        //     of one or more characters specified in a Unicode array.
        //
        // Parameters:
        //   anyOfIgnoreCase:
        //     A Unicode character array containing one or more characters to seek (ignoring case).
        //
        // Returns:
        //     The index position of the last occurrence in this instance where any character
        //     in anyOf was found; -1 if no character in anyOf was found.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     anyOf is null.
        public int LastIndexOfAny(char[] anyOfIgnoreCase)
        { return s.LastIndexOfAny(ToLowerSet(anyOfIgnoreCase)); }

        //
        // Summary:
        //     Reports the zero-based index position of the last occurrence in this instance
        //     of one or more characters specified in a Unicode array. The search starts
        //     at a specified character position and proceeds backward toward the beginning
        //     of the string.
        //
        // Parameters:
        //   anyOfIgnoreCase:
        //     A Unicode character array containing one or more characters to seek (ignoring case).
        //
        //   startIndex:
        //     The search starting position. The search proceeds from startIndex toward
        //     the beginning of this instance.
        //
        // Returns:
        //     The index position of the last occurrence in this instance where any character
        //     in anyOf was found; -1 if no character in anyOf was found or if the current
        //     instance equals System.String.Empty.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     anyOf is null.
        //
        //   System.ArgumentOutOfRangeException:
        //     The current instance does not equal System.String.Empty, and startIndex specifies
        //     a position that is not within this instance.
        public int LastIndexOfAny(char[] anyOfIgnoreCase, int startIndex)
        { return s.LastIndexOfAny(ToLowerSet(anyOfIgnoreCase), startIndex); }

        //
        // Summary:
        //     Reports the zero-based index position of the last occurrence in this instance
        //     of one or more characters specified in a Unicode array. The search starts
        //     at a specified character position and proceeds backward toward the beginning
        //     of the string for a specified number of character positions.
        //
        // Parameters:
        //   anyOfIgnoreCase:
        //     A Unicode character array containing one or more characters to seek (ignoring case).
        //
        //   startIndex:
        //     The search starting position. The search proceeds from startIndex toward
        //     the beginning of this instance.
        //
        //   count:
        //     The number of character positions to examine.
        //
        // Returns:
        //     The index position of the last occurrence in this instance where any character
        //     in anyOf was found; -1 if no character in anyOf was found or if the current
        //     instance equals System.String.Empty.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     anyOf is null.
        //
        //   System.ArgumentOutOfRangeException:
        //     The current instance does not equal System.String.Empty, and count or startIndex
        //     is negative.-or- The current instance does not equal System.String.Empty,
        //     and startIndex minus count specifies a position that is not within this instance.
        [SecuritySafeCritical]
        public int LastIndexOfAny(char[] anyOfIgnoreCase, int startIndex, int count)
        { return s.LastIndexOfAny(ToLowerSet(anyOfIgnoreCase), startIndex, count); }

        //
        // Summary:
        //     Returns a new string that right-aligns the characters in this instance by
        //     padding them with spaces on the left, for a specified total length.
        //
        // Parameters:
        //   totalWidth:
        //     The number of characters in the resulting string, equal to the number of
        //     original characters plus any additional padding characters.
        //
        // Returns:
        //     A new string that is equivalent to this instance, but right-aligned and padded
        //     on the left with as many spaces as needed to create a length of totalWidth.
        //     However, if totalWidth is less than the length of this instance, the method
        //     returns a reference to the existing instance. If totalWidth is equal to the
        //     length of this instance, the method returns a new string that is identical
        //     to this instance.
        //
        // Exceptions:
        //   System.ArgumentOutOfRangeException:
        //     totalWidth is less than zero.
        public LowerCaseString PadLeft(int totalWidth)
        { return new LowerCaseString(s.PadLeft(totalWidth)); }
        //
        // Summary:
        //     Returns a new string that right-aligns the characters in this instance by
        //     padding them on the left with a specified Unicode character, for a specified
        //     total length.
        //
        // Parameters:
        //   totalWidth:
        //     The number of characters in the resulting string, equal to the number of
        //     original characters plus any additional padding characters.
        //
        //   paddingCharLowerCase:
        //     A Unicode padding character (lower case varient will be used).
        //
        // Returns:
        //     A new string that is equivalent to this instance, but right-aligned and padded
        //     on the left with as many paddingChar characters as needed to create a length
        //     of totalWidth. However, if totalWidth is less than the length of this instance,
        //     the method returns a reference to the existing instance. If totalWidth is
        //     equal to the length of this instance, the method returns a new string that
        //     is identical to this instance.
        //
        // Exceptions:
        //   System.ArgumentOutOfRangeException:
        //     totalWidth is less than zero.
        public LowerCaseString PadLeft(int totalWidth, char paddingCharLowerCase)
        { return new LowerCaseString(s.PadLeft(totalWidth, char.ToLower(paddingCharLowerCase))); }
        //
        // Summary:
        //     Returns a new string that left-aligns the characters in this string by padding
        //     them with spaces on the right, for a specified total length.
        //
        // Parameters:
        //   totalWidth:
        //     The number of characters in the resulting string, equal to the number of
        //     original characters plus any additional padding characters.
        //
        // Returns:
        //     A new string that is equivalent to this instance, but left-aligned and padded
        //     on the right with as many spaces as needed to create a length of totalWidth.
        //     However, if totalWidth is less than the length of this instance, the method
        //     returns a reference to the existing instance. If totalWidth is equal to the
        //     length of this instance, the method returns a new string that is identical
        //     to this instance.
        //
        // Exceptions:
        //   System.ArgumentOutOfRangeException:
        //     totalWidth is less than zero.
        public LowerCaseString PadRight(int totalWidth)
        { return new LowerCaseString(s.PadRight(totalWidth)); }
        //
        // Summary:
        //     Returns a new string that left-aligns the characters in this string by padding
        //     them on the right with a specified Unicode character, for a specified total
        //     length.
        //
        // Parameters:
        //   totalWidth:
        //     The number of characters in the resulting string, equal to the number of
        //     original characters plus any additional padding characters.
        //
        //   paddingCharLowerCase:
        //     A Unicode padding character (lower case varient will be used).
        //
        // Returns:
        //     A new string that is equivalent to this instance, but left-aligned and padded
        //     on the right with as many paddingChar characters as needed to create a length
        //     of totalWidth. However, if totalWidth is less than the length of this instance,
        //     the method returns a reference to the existing instance. If totalWidth is
        //     equal to the length of this instance, the method returns a new string that
        //     is identical to this instance.
        //
        // Exceptions:
        //   System.ArgumentOutOfRangeException:
        //     totalWidth is less than zero.
        public LowerCaseString PadRight(int totalWidth, char paddingCharLowerCase)
        { return new LowerCaseString(s.PadRight(totalWidth, char.ToLower(paddingCharLowerCase))); }

        //
        // Summary:
        //     Returns a new string in which all the characters in the current instance,
        //     beginning at a specified position and continuing through the last position,
        //     have been deleted.
        //
        // Parameters:
        //   startIndex:
        //     The zero-based position to begin deleting characters.
        //
        // Returns:
        //     A new string that is equivalent to this string except for the removed characters.
        //
        // Exceptions:
        //   System.ArgumentOutOfRangeException:
        //     startIndex is less than zero.-or- startIndex specifies a position that is
        //     not within this string.
        public LowerCaseString Remove(int startIndex)
        { return new LowerCaseString(s.Remove(startIndex)); }
        //
        // Summary:
        //     Returns a new string in which a specified number of characters in the current
        //     this instance beginning at a specified position have been deleted.
        //
        // Parameters:
        //   startIndex:
        //     The zero-based position to begin deleting characters.
        //
        //   count:
        //     The number of characters to delete.
        //
        // Returns:
        //     A new string that is equivalent to this instance except for the removed characters.
        //
        // Exceptions:
        //   System.ArgumentOutOfRangeException:
        //     Either startIndex or count is less than zero.-or- startIndex plus count specify
        //     a position outside this instance.
        [SecuritySafeCritical]
        public LowerCaseString Remove(int startIndex, int count)
        { return new LowerCaseString(s.Remove(startIndex, count)); }

        //
        // Summary:
        //     Returns a new string in which all occurrences of a specified Unicode character
        //     in this instance are replaced with another specified Unicode character.
        //
        // Parameters:
        //   oldCharIgnoreCase:
        //     The Unicode character to be replaced (ignoring case).
        //
        //   newCharIgnoreCase:
        //     The Unicode character to replace all occurrences of oldChar (ignoring case).
        //
        // Returns:
        //     A string that is equivalent to this instance except that all instances of
        //     oldChar are replaced with newChar.
        public LowerCaseString Replace(char oldCharIgnoreCase, char newCharIgnoreCase)
        { return new LowerCaseString(s.Replace(char.ToLower(oldCharIgnoreCase), char.ToLower(newCharIgnoreCase))); }

        //
        // Summary:
        //     Returns a new string in which all occurrences of a specified string in the
        //     current instance are replaced with another specified string.
        //
        // Parameters:
        //   oldValue:
        //     The string to be replaced.
        //
        //   newValue:
        //     The string to replace all occurrences of oldValue.
        //
        // Returns:
        //     A string that is equivalent to the current string except that all instances
        //     of oldValue are replaced with newValue.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     oldValue is null.
        //
        //   System.ArgumentException:
        //     oldValue is the empty string ("").
        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public LowerCaseString Replace(LowerCaseString oldValue, LowerCaseString newValue)
        { return new LowerCaseString(s.Replace(oldValue.s, newValue.s)); }

        //
        // Summary:
        //     Returns a string array that contains the substrings in this instance that
        //     are delimited by elements of a specified Unicode character array.
        //
        // Parameters:
        //   separatorIgnoreCase:
        //     An array of Unicode characters that delimit the substrings in this instance,
        //     an empty array that contains no delimiters, or null. Case is ignored.
        //
        // Returns:
        //     An array whose elements contain the substrings in this instance that are
        //     delimited by one or more characters in separator. For more information, see
        //     the Remarks section.
        public LowerCaseString[] Split(params char[] separatorIgnoreCase)
        { return ToLower(s.Split(ToLowerSet(separatorIgnoreCase))).ToArray(); }

        //
        // Summary:
        //     Returns a string array that contains the substrings in this instance that
        //     are delimited by elements of a specified Unicode character array. A parameter
        //     specifies the maximum number of substrings to return.
        //
        // Parameters:
        //   separatorIgnoreCase:
        //     An array of Unicode characters that delimit the substrings in this instance,
        //     an empty array that contains no delimiters, or null. Case is ignored.
        //
        //   count:
        //     The maximum number of substrings to return.
        //
        // Returns:
        //     An array whose elements contain the substrings in this instance that are
        //     delimited by one or more characters in separator. For more information, see
        //     the Remarks section.
        //
        // Exceptions:
        //   System.ArgumentOutOfRangeException:
        //     count is negative.
        public LowerCaseString[] Split(char[] separatorIgnoreCase, int count)
        { return ToLower(s.Split(ToLowerSet(separatorIgnoreCase), count)).ToArray(); }
        //
        // Summary:
        //     Returns a string array that contains the substrings in this string that are
        //     delimited by elements of a specified Unicode character array. A parameter
        //     specifies whether to return empty array elements.
        //
        // Parameters:
        //   separatorIgnoreCase:
        //     An array of Unicode characters that delimit the substrings in this instance,
        //     an empty array that contains no delimiters, or null. Case is ignored.
        //
        //   options:
        //     System.StringSplitOptions.RemoveEmptyEntries to omit empty array elements
        //     from the array returned; or System.StringSplitOptions.None to include empty
        //     array elements in the array returned.
        //
        // Returns:
        //     An array whose elements contain the substrings in this string that are delimited
        //     by one or more characters in separator. For more information, see the Remarks
        //     section.
        //
        // Exceptions:
        //   System.ArgumentException:
        //     options is not one of the System.StringSplitOptions values.
        [ComVisible(false)]
        public LowerCaseString[] Split(char[] separatorIgnoreCase, StringSplitOptions options)
        { return ToLower(s.Split(ToLowerSet(separatorIgnoreCase), options)).ToArray(); }

        //
        // Summary:
        //     Returns a string array that contains the substrings in this string that are
        //     delimited by elements of a specified string array. A parameter specifies
        //     whether to return empty array elements.
        //
        // Parameters:
        //   separator:
        //     An array of strings that delimit the substrings in this string, an empty
        //     array that contains no delimiters, or null.
        //
        //   options:
        //     System.StringSplitOptions.RemoveEmptyEntries to omit empty array elements
        //     from the array returned; or System.StringSplitOptions.None to include empty
        //     array elements in the array returned.
        //
        // Returns:
        //     An array whose elements contain the substrings in this string that are delimited
        //     by one or more strings in separator. For more information, see the Remarks
        //     section.
        //
        // Exceptions:
        //   System.ArgumentException:
        //     options is not one of the System.StringSplitOptions values.
        [ComVisible(false)]
        public LowerCaseString[] Split(LowerCaseString[] separator, StringSplitOptions options)
        { return ToLower(s.Split(ToString(separator).ToArray(), options)).ToArray(); }
        //
        // Summary:
        //     Returns a string array that contains the substrings in this string that are
        //     delimited by elements of a specified Unicode character array. Parameters
        //     specify the maximum number of substrings to return and whether to return
        //     empty array elements.
        //
        // Parameters:
        //   separatorIgnoreCase:
        //     An array of Unicode characters that delimit the substrings in this instance,
        //     an empty array that contains no delimiters, or null. Case is ignored.
        //
        //   count:
        //     The maximum number of substrings to return.
        //
        //   options:
        //     System.StringSplitOptions.RemoveEmptyEntries to omit empty array elements
        //     from the array returned; or System.StringSplitOptions.None to include empty
        //     array elements in the array returned.
        //
        // Returns:
        //     An array whose elements contain the substrings in this string that are delimited
        //     by one or more characters in separator. For more information, see the Remarks
        //     section.
        //
        // Exceptions:
        //   System.ArgumentOutOfRangeException:
        //     count is negative.
        //
        //   System.ArgumentException:
        //     options is not one of the System.StringSplitOptions values.
        [ComVisible(false)]
        public LowerCaseString[] Split(char[] separatorIgnoreCase, int count, StringSplitOptions options)
        { return ToLower(s.Split(ToLowerSet(separatorIgnoreCase), count, options)).ToArray(); }
        //
        // Summary:
        //     Returns a string array that contains the substrings in this string that are
        //     delimited by elements of a specified string array. Parameters specify the
        //     maximum number of substrings to return and whether to return empty array
        //     elements.
        //
        // Parameters:
        //   separator:
        //     An array of strings that delimit the substrings in this string, an empty
        //     array that contains no delimiters, or null.
        //
        //   count:
        //     The maximum number of substrings to return.
        //
        //   options:
        //     System.StringSplitOptions.RemoveEmptyEntries to omit empty array elements
        //     from the array returned; or System.StringSplitOptions.None to include empty
        //     array elements in the array returned.
        //
        // Returns:
        //     An array whose elements contain the substrings in this string that are delimited
        //     by one or more strings in separator. For more information, see the Remarks
        //     section.
        //
        // Exceptions:
        //   System.ArgumentOutOfRangeException:
        //     count is negative.
        //
        //   System.ArgumentException:
        //     options is not one of the System.StringSplitOptions values.
        [ComVisible(false)]
        public LowerCaseString[] Split(LowerCaseString[] separator, int count, StringSplitOptions options)
        { return ToLower(s.Split(ToString(separator).ToArray(), count, options)).ToArray(); }
        //
        // Summary:
        //     Determines whether the beginning of this string instance matches the specified
        //     string.
        //
        // Parameters:
        //   value:
        //     The string to compare.
        //
        // Returns:
        //     true if value matches the beginning of this string; otherwise, false.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     value is null.
        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public bool StartsWith(LowerCaseString value) { return s.StartsWith(value.s); }

        //
        // Summary:
        //     Determines whether the beginning of this string instance matches the specified
        //     string when compared using the specified comparison option.
        //
        // Parameters:
        //   value:
        //     The string to compare.
        //
        //   comparisonType:
        //     One of the enumeration values that determines how this string and value are
        //     compared.
        //
        // Returns:
        //     true if this instance begins with value; otherwise, false.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     value is null.
        //
        //   System.ArgumentException:
        //     comparisonType is not a System.StringComparison value.
        [ComVisible(false)]
        [SecuritySafeCritical]
        public bool StartsWith(LowerCaseString value, StringComparison comparisonType) { return s.StartsWith(value.s, comparisonType); }
        //
        // Summary:
        //     Determines whether the beginning of this string instance matches the specified
        //     string when compared using the specified culture.
        //
        // Parameters:
        //   value:
        //     The string to compare.
        //
        //   culture:
        //     Cultural information that determines how this string and value are compared.
        //     If culture is null, the current culture is used.
        //
        // Returns:
        //     true if the value parameter matches the beginning of this string; otherwise,
        //     false.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     value is null.
        public bool StartsWith(LowerCaseString value, CultureInfo culture)
        { return s.StartsWith(value.s, true, culture); }

        //
        // Summary:
        //     Retrieves a substring from this instance. The substring starts at a specified
        //     character position.
        //
        // Parameters:
        //   startIndex:
        //     The zero-based starting character position of a substring in this instance.
        //
        // Returns:
        //     A string that is equivalent to the substring that begins at startIndex in
        //     this instance, or System.String.Empty if startIndex is equal to the length
        //     of this instance.
        //
        // Exceptions:
        //   System.ArgumentOutOfRangeException:
        //     startIndex is less than zero or greater than the length of this instance.
        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public LowerCaseString Substring(int startIndex)
        { return new LowerCaseString(s.Substring(startIndex)); }


        //
        // Summary:
        //     Retrieves a substring from this instance. The substring starts at a specified
        //     character position and has a specified length.
        //
        // Parameters:
        //   startIndex:
        //     The zero-based starting character position of a substring in this instance.
        //
        //   length:
        //     The number of characters in the substring.
        //
        // Returns:
        //     A string that is equivalent to the substring of length length that begins
        //     at startIndex in this instance, or System.String.Empty if startIndex is equal
        //     to the length of this instance and length is zero.
        //
        // Exceptions:
        //   System.ArgumentOutOfRangeException:
        //     startIndex plus length indicates a position not within this instance.-or-
        //     startIndex or length is less than zero.
        [SecuritySafeCritical]
        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public LowerCaseString Substring(int startIndex, int length)
        { return new LowerCaseString(s.Substring(startIndex, length)); }
        //
        // Summary:
        //     Copies the characters in this instance to a Unicode character array.
        //
        // Returns:
        //     A Unicode character array whose elements are the individual characters of
        //     this instance. If this instance is an empty string, the returned array is
        //     empty and has a zero length.
        [SecuritySafeCritical]
        public char[] ToCharArray() { return s.ToCharArray(); }
        //
        // Summary:
        //     Copies the characters in a specified substring in this instance to a Unicode
        //     character array.
        //
        // Parameters:
        //   startIndex:
        //     The starting position of a substring in this instance.
        //
        //   length:
        //     The length of the substring in this instance.
        //
        // Returns:
        //     A Unicode character array whose elements are the length number of characters
        //     in this instance starting from character position startIndex.
        //
        // Exceptions:
        //   System.ArgumentOutOfRangeException:
        //     startIndex or length is less than zero.-or- startIndex plus length is greater
        //     than the length of this instance.
        [SecuritySafeCritical]
        public char[] ToCharArray(int startIndex, int length) { return s.ToCharArray(startIndex, length); }

        //
        // Summary:
        //     Returns this instance of System.String; no actual conversion is performed.
        //
        // Returns:
        //     The current string.
        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public override string ToString() { return String.Copy(s); } //copy cause the returned value is a system string
        //
        // Summary:
        //     Returns this instance of System.String; no actual conversion is performed.
        //
        // Parameters:
        //   provider:
        //     (Reserved) An object that supplies culture-specific formatting information.
        //
        // Returns:
        //     The current string.
        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public string ToString(IFormatProvider provider) { return String.Copy(s.ToString(provider)); } //copy cause the returned value is a system string
        //
        // Summary:
        //     Returns a copy of this string converted to uppercase.
        //
        // Returns:
        //     The uppercase equivalent of the current string.
        public string ToUpper() { return s.ToUpper(); }
        //
        // Summary:
        //     Returns a copy of this string converted to uppercase, using the casing rules
        //     of the specified culture.
        //
        // Parameters:
        //   culture:
        //     An object that supplies culture-specific casing rules.
        //
        // Returns:
        //     The uppercase equivalent of the current string.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     culture is null.
        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public string ToUpper(CultureInfo culture) { return s.ToUpper(culture); }
        //
        // Summary:
        //     Returns a copy of this System.String object converted to uppercase using
        //     the casing rules of the invariant culture.
        //
        // Returns:
        //     The uppercase equivalent of the current string.
        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public string ToUpperInvariant() { return s.ToUpperInvariant(); }
        //
        // Summary:
        //     Removes all leading and trailing white-space characters from the current
        //     System.String object.
        //
        // Returns:
        //     The string that remains after all white-space characters are removed from
        //     the start and end of the current string.
        public LowerCaseString Trim() { return new LowerCaseString(s.Trim()); }
        //
        // Summary:
        //     Removes all leading and trailing occurrences of a set of characters specified
        //     in an array from the current System.String object.
        //
        // Parameters:
        //   trimCharsIgnoreCase:
        //     An array of Unicode characters to remove, or null. Case is ignored.
        //
        // Returns:
        //     The string that remains after all occurrences of the characters in the trimChars
        //     parameter are removed from the start and end of the current string. If trimChars
        //     is null or an empty array, white-space characters are removed instead.
        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public LowerCaseString Trim(params char[] trimCharsIgnoreCase)
        { return new LowerCaseString(s.Trim(ToLowerSet(trimCharsIgnoreCase))); }
        //
        // Summary:
        //     Removes all trailing occurrences of a set of characters specified in an array
        //     from the current System.String object.
        //
        // Parameters:
        //   trimCharsIgnoreCase:
        //     An array of Unicode characters to remove, or null. Case is ignored.
        //
        // Returns:
        //     The string that remains after all occurrences of the characters in the trimChars
        //     parameter are removed from the end of the current string. If trimChars is
        //     null or an empty array, Unicode white-space characters are removed instead.
        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public LowerCaseString TrimEnd(params char[] trimCharsIgnoreCase)
        { return new LowerCaseString(s.TrimEnd(ToLowerSet(trimCharsIgnoreCase))); }
        //
        // Summary:
        //     Removes all leading occurrences of a set of characters specified in an array
        //     from the current System.String object.
        //
        // Parameters:
        //   trimCharsIgnoreCase:
        //     An array of Unicode characters to remove, or null. Case is ignored.
        //
        // Returns:
        //     The string that remains after all occurrences of characters in the trimChars
        //     parameter are removed from the start of the current string. If trimChars
        //     is null or an empty array, white-space characters are removed instead.
        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public LowerCaseString TrimStart(params char[] trimCharsIgnoreCase)
        { return new LowerCaseString(s.TrimStart(ToLowerSet(trimCharsIgnoreCase))); }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)s).GetEnumerator();
        }

        IEnumerator<char> IEnumerable<char>.GetEnumerator()
        {
            return s.GetEnumerator();
        }
    }
}
