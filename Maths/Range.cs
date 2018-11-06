/*
 * The following code is Copyright 2018 Dr Warren Creemers (busyDuckman)
 * See LICENSE.md for more information.
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace WDToolbox.Maths.Range
{
    /// <summary>
    /// range checking functions
    /// </summary>
    public static class Range
    {
        #region range checking functions
        /// <summary>
        /// Clamps a value, 0 to max
        /// </summary>
        /// <param name="val">Value to clamp</param>
        /// <param name="max">Maximum range for the value.</param>
        /// <returns>aA value clamped into a specific range.</returns>
        public static int clamp(int val, int maxInclusive)
        {
            return clamp(val, 0, maxInclusive);
        }

        /// <summary>
        /// Clamps a value into a specified range.
        /// </summary>
        /// <param name="val">Value to clamp</param>
        /// <param name="min">Minimum range for the value.</param>
        /// <param name="max">Maximum range for the value.</param>
        /// <returns>aA value clamped into a specific range.</returns>
        public static int clamp(int val, int min, int maxInclusive)
        {
            if (val < min)
                return min;
            if (val > maxInclusive)
                return maxInclusive;
            return val;
        }

        /// <summary>
        /// Clamps a value into a specified range.
        /// </summary>
        /// <param name="val">Value to clamp</param>
        /// <param name="min">Minimum range for the value.</param>
        /// <param name="max">Maximum range for the value.</param>
        /// <returns>aA value clamped into a specific range.</returns>
        public static long clamp(long val, long min, long maxInclusive)
        {
            if (val < min)
                return min;
            if (val > maxInclusive)
                return maxInclusive;
            return val;
        }

        /// <summary>
        /// Clamps a value into a specified range.
        /// </summary>
        /// <param name="val">Value to clamp</param>
        /// <param name="min">Minimum range for the value.</param>
        /// <param name="max">Maximum range for the value.</param>
        /// <returns>aA value clamped into a specific range.</returns>
        public static ulong clamp(ulong val, ulong min, ulong maxInclusive)
        {
            if (val < min)
                return min;
            if (val > maxInclusive)
                return maxInclusive;
            return val;
        }

        /// <summary>
        /// Clamps a value into a specified range.
        /// </summary>
        /// <param name="val">Value to clamp</param>
        /// <param name="min">Minimum range for the value.</param>
        /// <param name="max">Maximum range for the value.</param>
        /// <returns>aA value clamped into a specific range.</returns>
        public static double clamp(double val, double min, double maxInclusive)
        {
            if (val < min)
                return min;
            if (val > maxInclusive)
                return maxInclusive;
            return val;
        }

        /// <summary>
        /// Clamps a value into a specified range.
        /// </summary>
        /// <param name="val">Value to clamp</param>
        /// <param name="min">Minimum range for the value.</param>
        /// <param name="max">Maximum range for the value.</param>
        /// <returns>aA value clamped into a specific range.</returns>
        public static float clamp(float val, float min, float maxInclusive)
        {
            if (val < min)
                return min;
            if (val > maxInclusive)
                return maxInclusive;
            return val;
        }

        /// <summary>
        /// Clamps a value into a specified range.
        /// </summary>
        /// <param name="val">Value to clamp</param>
        /// <param name="min">Minimum range for the value.</param>
        /// <param name="max">Maximum range for the value.</param>
        /// <returns>aA value clamped into a specific range.</returns>
        public static DateTime clamp(DateTime val, DateTime min, DateTime max)
        {
            if (val < min)
                return min;
            if (val > max)
                return max;
            return val;
        }

        /// <summary>
        /// Clamps a value into a specified range.
        /// </summary>
        /// <param name="val">Value to clamp</param>
        /// <param name="min">Minimum range for the value.</param>
        /// <param name="max">Maximum range for the value.</param>
        /// <returns>aA value clamped into a specific range.</returns>
        public static TimeSpan clamp(TimeSpan val, TimeSpan min, TimeSpan max)
        {
            if (val < min)
                return min;
            if (val > max)
                return max;
            return val;
        }

        public static CType clamp<CType>(CType val, CType min, CType max)
        where CType : IComparable
        {
            if (val.CompareTo(min) < 0)
                return min;
            if (val.CompareTo(max) > 0)
                return max;
            return val;
        }

#if UNSAFE
        public static unsafe void clamp(float* data, int count, float min, float max)
        {
            float* pos = data + count;
            for (; pos > data; pos--)
            {
                if (*pos < min)
                {
                    *pos = min;
                }
                else if (*pos > max)
                {
                    *pos = max;
                }
            }
        }
#endif 

        public static uint clampToUint(int val)
        {
            if (val < 0)
                return 0;
            return (uint)val;
        }

        public static int clampToInt(uint val)
        {
            if (val > int.MaxValue)
                return int.MaxValue;
            return (int)val;
        }

        /// <summary>
        /// Clamps a value to a specific upper bound.
        /// The lower bound is not checked.
        /// </summary>
        /// <param name="val">Value to clamp</param>
        /// <param name="cutoff">Maximum range for the value.</param>
        /// <returns>A value clamped to a specific upper bound.</returns>
        public static int clampIfBelow(int val, int cutoff)
        {
            if (val < cutoff)
                return cutoff;
            return val;
        }

        /// <summary>
        /// Gets the minimum of two values.
        /// </summary>
        /// <param name="a">A value</param>
        /// <param name="b">A value</param>
        /// <returns>The minimum of "a" and "b"</returns>
        public static float min(float a, float b)
        {
            return a < b ? a : b;
        }

        /// <summary>
        /// Gets the minimum of two values.
        /// </summary>
        /// <param name="a">A value</param>
        /// <param name="b">A value</param>
        /// <returns>The minimum of "a" and "b"</returns>
        public static int min(int a, int b)
        {
            return a < b ? a : b;
        }

        /// <summary>
        /// Gets the minimum of two values.
        /// </summary>
        /// <param name="a">A value</param>
        /// <param name="b">A value</param>
        /// <param name="c">A value</param>
        /// <returns>The minimum of "a", "b" and "c"</returns>
        public static float min(float a, float b, float c)
        {
            return min(min(a, b), c);
        }

        /// <summary>
        /// Gets the maximum of two values.
        /// </summary>
        /// <param name="a">A value</param>
        /// <param name="b">A value</param>
        /// <returns>The maximum of "a" and "b"</returns>
        public static float max(float a, float b)
        {
            return a > b ? a : b;
        }

        /// <summary>
        /// Gets the maximum of two values.
        /// </summary>
        /// <param name="a">A value</param>
        /// <param name="b">A value</param>
        /// <returns>The maximum of "a" and "b"</returns>
        public static int max(int a, int b)
        {
            return a > b ? a : b;
        }

        /// <summary>
        /// Gets the maximum of two values.
        /// </summary>
        /// <param name="a">A value</param>
        /// <param name="b">A value</param>
        /// <param name="c">A value</param>
        /// <returns>The maximum of "a", "b" and "c"</returns>
        public static float max(float a, float b, float c)
        {
            return max(max(a, b), c);
        }

        /// <summary>
        /// Transformes a value into a range by wrapping the value into the range
        /// </summary>
        /// <param name="val">A value</param>
        /// <param name="min">Minimum range for the value.</param>
        /// <param name="max">Maximum range for the value.</param>
        /// <returns>A value wrapped into a specific range.</returns>
        public static float wrap(float val, float min, float max)
        {
            float v = val;

            float MIN = Range.min(min, max);
            float MAX = Range.max(min, max);

            if (MAX == MIN)
            {
                return MIN;
            }

            float diff = MAX - MIN;
            while (v > MAX)
            {
                v -= diff;
            }

            while (v < MIN)
            {
                v += diff;
            }

            return v;
        }


        /// <summary>
        /// Determins if a value is a valid index for a specific list.
        /// </summary>
        /// <param name="index">Index to test for valididty.</param>
        /// <param name="list">List for which the index must be valid.</param>
        /// <returns>True if the index is valid; otherwise false.</returns>
        public static bool isValidIndex(int index, IList list)
        {
            return ((index >= 0) && (index < list.Count));
        }
#endregion
    }

    public class rangedList<CompType> : IList<CompType>
    where CompType : IComparable
    {
        CompType min;
        public CompType Min
        {
            get
            {
                return min;
            }
            set
            {
                min = value;
                if (min.CompareTo(max) > 0)
                {
                    max = min;
                }
                reValidateAllValues();
            }
        }

        CompType max;
        public CompType Max
        {
            get
            {
                return max;
            }
            set
            {
                max = value;
                if (max.CompareTo(min) < 0)
                {
                    min = max;
                }
                reValidateAllValues();
            }
        }

        List<CompType> values;

        public enum outOfRangeFunction { clamp = 0, remove, throwException /*wrap, randomiseToRange*/};

        public outOfRangeFunction onOutOfRangeBehaviour = outOfRangeFunction.clamp;

        /*
         * Passing a out of range value via the [] operator can either invoke the OutOfRangeFunction or preserve the old value;
         */
        public bool preserveInitialValueOnBadAlterations = false;

        public rangedList(CompType min, CompType max)
        {
            values = new List<CompType>();
#if DEBUG
            System.Diagnostics.Debug.Assert((min.CompareTo(min) == 0), "buggy comparable operator found in rangedList");
            System.Diagnostics.Debug.Assert((max.CompareTo(max) == 0), "buggy comparable operator found in rangedList");
#endif
            if (max.CompareTo(min) >= 0)
            {
                this.min = min;
                this.max = max;
            }
            else
            {
                //asume inputs are back to front
                this.min = max;
                this.max = min;
                //TODO: debug message...
            }
        }

        private bool validForMax(CompType value)
        {
            return (value.CompareTo(max) <= 0);
        }

        private bool validForMin(CompType value)
        {
            return (value.CompareTo(min) >= 0);
        }

        private bool processNewData(CompType item, out CompType newItem)
        {
            bool ignore;
            return processNewData(item, out newItem, out ignore);
        }
        /*
         * Returns false it the data is not to be added.
         */
        private bool processNewData(CompType item, out CompType newItem, out bool changeMade)
        {
            changeMade = false;
            if ((!validForMax(item)) || (!validForMin(item)))
            {
                changeMade = true;
                switch (onOutOfRangeBehaviour)
                {
                    case outOfRangeFunction.clamp:
                        newItem = Range.clamp<CompType>(item, min, max);
                        break;
                    case outOfRangeFunction.remove:
                        newItem = item;
                        return false;
                    case outOfRangeFunction.throwException:
                        throw new Exception(String.Format("Value ({0}) is not avlid for range ({1}, {2}).", item.ToString(), min.ToString(), max.ToString()));
                    default:
                        throw new Exception("Unhandled situation in rangedList");
                }
            }
            newItem = item;
            return true;
        }

        private void reValidateAllValues()
        {
            int i;
            CompType temp;
            List<int> removeAt = new List<int>();
            for (i = 0; i < values.Count; i++)
            {
                if (processNewData(values[i], out temp))
                {
                    values[i] = temp;
                }
                else
                {
                    //remove at goes from largest to smallest
                    removeAt.Insert(0, i);
                }
            }

            //if the list in in remove invalid values then this colection will have data to be purged                                   
            for (i = 0; i < removeAt.Count; i++)
            {
                values.RemoveAt(removeAt[i]);
            }
        }

#region IList<CompType> Members

        public int IndexOf(CompType item)
        {
            return values.LastIndexOf(item);
        }

        public void Insert(int index, CompType item)
        {
            CompType temp;
            if (processNewData(item, out temp))
            {
                values.Insert(index, temp);
            }
        }

        public void RemoveAt(int index)
        {
            values.RemoveAt(index);
        }

        public CompType this[int index]
        {
            get
            {
                return values[index];
            }
            set
            {
                CompType temp;
                bool changeMade;
                if (processNewData(value, out temp, out changeMade))
                {
                    if (!(changeMade && preserveInitialValueOnBadAlterations))
                    {
                        values[index] = temp;
                    }
                }
                else if (!preserveInitialValueOnBadAlterations)
                {
                    values.RemoveAt(index);
                }
            }
        }

#endregion

#region ICollection<CompType> Members

        public void Add(CompType item)
        {
            CompType temp;
            if (processNewData(item, out temp))
            {
                values.Add(temp);
            }
        }

        public void Clear()
        {
            values.Clear();
        }

        public bool Contains(CompType item)
        {
            if (validForMin(item) && validForMax(item))
            {
                return values.Contains(item);
            }
            else
            {
#if DEBUG
                System.Diagnostics.Debug.Assert(!values.Contains(item), "invalid value in rangedList, range constraint violated");
#endif
                return false;
            }
        }

        public void CopyTo(CompType[] array, int arrayIndex)
        {
            values.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return values.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(CompType item)
        {
            return values.Remove(item);
        }

#endregion

#region IEnumerable<CompType> Members

        public IEnumerator<CompType> GetEnumerator()
        {
            return values.GetEnumerator();
        }

#endregion

#region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return values.GetEnumerator();
        }

#endregion
    }

    public sealed class RangeInt
    {
        public static readonly RangeInt Nothing = new RangeInt(0, 0);

        public int Min {get; private set;}
        public int Max { get { return IsNothing() ? 0 : Min + Length - 1; } }
        public int Length { get; private set; }

        private RangeInt(int min, int len)
        {
            if (len == 0)
            {
                Min = 0;
                Length = 0;
            }
        }

        public bool IsNothing()
        {
            return Length == 0;
        }

        public static RangeInt FromTwoInclusiveExtremes(int a, int b)
        {
            if (b >= a)
            {
                return new RangeInt(a, (b - a)+1);
            }
            else
            {
                return new RangeInt(b, (a - b)+1);
            }
        }

        public static RangeInt FromStartAndLength(int start, int len)
        {
            if (len > 0)
            {
                return new RangeInt(start, len);
            }
            else if (len < 0)
            {
                return new RangeInt(start+1-len, len);
            }
            return RangeInt.Nothing;
        }


        public bool Intersects(RangeInt range)
        {
            return !((range.Max < this.Min) || (range.Min > this.Max));
        }

        public bool Contains(int value)
        {
            return (value >= Min) && (value <= Max);
        }

        public bool Contains(RangeInt range)
        {
            return (range.Min >= Min) && (range.Max <= Max);
        }
        
    }
}
