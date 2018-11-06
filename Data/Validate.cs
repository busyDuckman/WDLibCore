/*
 * The following code is Copyright 2018 Dr Warren Creemers (busyDuckman)
 * See LICENSE.md for more information.
 */
using System;
using System.Collections.Generic;
using WDToolbox.Data.DataStructures;

namespace WDToolbox.Data
{
    /// <summary>
    /// Handy numerical rules.
    /// Used by things such as Validate.check(3, IsOdd, e->log(e)); 
    /// </summary>
    public enum NumericRule { IsZero, IsNotZero, 
                             IsGreaterThanOrEqualZero, IsGreaterThanZero,
                             IsLessThanOrEqualZero, IsLesThanZero,
                             IsOdd, IsEven,
                             IsUnsignedByte, IsSignedByte,
                             IsUnsignedInt32, IsSignedInt32,
                             IsPow2
                            };
    
//    public enum TextRules { NoSpaces, NotEmpty };


    /// <summary>
    /// Useful data validation rules for user inputted data.
    /// </summary>
    public static class Validate
    {
        /// <summary>
        /// Applies a list of validation rules to a number.
        /// </summary>
        public static Why Check(double value, Action<string> onError=null, params NumericRule[] rules)
        {
            foreach(NumericRule rule in rules)
            {
                Why r = Check(value, rule, onError);
                if(!r)
                {
                    return r;
                }
            }
            return true;
        }

        /// <summary>
        /// Applies a validation rule to a number.
        /// </summary>
        public static Why Check(double value, NumericRule rule, Action<string> onError = null)
        {
            string reason = "";

            //handle -0
            double negativeZero = -1.0*0;
            if(BitConverter.DoubleToInt64Bits(0) == BitConverter.DoubleToInt64Bits(negativeZero))
            {
                value = 0;
            }

            //if ok, return true
            switch (rule)
            {
                case NumericRule.IsZero:
                    if (value == 0.0)
                    {
                        return true;
                    }
                    reason = string.Format("expecting zero, got: {0}", value);
                    break;
                case NumericRule.IsNotZero:
                    if (value != 0.0)  //nb -0 == 0 in c#
                    {
                        return true;
                    }
                    reason = string.Format("value can not be zero");
                    break;
                case NumericRule.IsGreaterThanOrEqualZero:
                    if (value >= 0.0)
                    {
                        return true;
                    }
                    reason = string.Format("expecting >= zero, got: {0}", value);
                    break;
                case NumericRule.IsGreaterThanZero:
                    if (value > 0.0)
                    {
                        return true;
                    }
                    reason = string.Format("expecting > zero, got: {0}", value);
                    break;
                case NumericRule.IsLessThanOrEqualZero:
                    if (value <= 0.0)
                    {
                        return true;
                    }
                    reason = string.Format("expecting <= zero, got: {0}", value);
                    break;
                case NumericRule.IsLesThanZero:
                    if (value < 0.0)
                    {
                        return true;
                    }
                    reason = string.Format("expecting < zero, got: {0}", value);
                    break;
                case NumericRule.IsOdd:
                    if (isWholeNumber(value))
                    {
                        if (isOdd((int)value))
                        {
                            return true;
                        }
                        reason = string.Format("expecting an odd number, got: {0}", value);
                    }
                    else
                    {
                        reason = string.Format("expecting odd number, but did not get a whole number {0}", value);
                    }
                    break;
                case NumericRule.IsEven:
                    if (isWholeNumber(value))
                    {
                        if (isEven((int)value))
                        {
                            return true;
                        }
                        reason = string.Format("expecting an even number, got: {0}", value);
                    }
                    else
                    {
                        reason = string.Format("expecting even number, but did not get a whole number {0}", value);
                    }
                    break;
                case NumericRule.IsUnsignedByte:
                    return CheckRangeInclusive(value, byte.MinValue, byte.MaxValue, onError);
                case NumericRule.IsSignedByte:
                    return CheckRangeInclusive(value, sbyte.MinValue, sbyte.MaxValue, onError);
                case NumericRule.IsUnsignedInt32:
                    return CheckRangeInclusive(value, uint.MinValue, uint.MaxValue, onError);
                case NumericRule.IsSignedInt32:
                    return CheckRangeInclusive(value, int.MinValue, int.MaxValue, onError);
                case NumericRule.IsPow2:
                    if (isWholeNumber(value))
                    {
                        if (IsPowerOfTwo((ulong)value))
                        {
                            return true;
                        }
                        reason = string.Format("expecting a power of 2, got: {0}", value);
                    }
                    else
                    {
                        reason = string.Format("expecting a power of 2, but did not get a whole number {0}", value);
                    }
                    break;
                default:
                    reason = string.Format("INVALID CHECK ({0})", rule);
                    break;
            }

            
            //here because the test failed
            doAction(onError, reason);
            return Why.FalseBecause(reason);
        }

        private static bool isWholeNumber(double d)
        {
            //mega strict zero
            return (d % 1.0) == 0.0;
        }

        private static bool isEven(int value)
        {
            //mega strict zero
            return (value % 2) == 0;
        }

        private static bool isOdd(int value)
        {
            return !isEven(value);
        }

        private static bool IsPowerOfTwo(ulong x)
        {
            return (x != 0) && ((x & (x - 1)) == 0);
        }

        public static Why CheckRangeInclusive(double value, 
                                                double minInclusive, 
                                                double maxInclusive, 
                                                Action<string> onError = null)
        {
            if (value < minInclusive)
            {
                string reason = string.Format("Value can not be < {1}, got: {0}", value, minInclusive);
                doAction(onError, reason);
                return Why.FalseBecause(reason);
            }
            if (value > maxInclusive)
            {
                string reason = string.Format("Value can not be < {1}, got: {0}", value, maxInclusive);
                doAction(onError, reason);
                return Why.FalseBecause(reason);
            }
            return true;
        }

        public static Why CheckCustom(double value, 
                                    Dictionary<string, Predicate<double>> checks, 
                                    Action<string> onError = null)
        {
            foreach(var check in checks)
            {
                Why r = CheckCustom(value, check.Key, check.Value, onError);
                if (!r)
                {
                    return r;
                }
            }

            return true;
        }

        public static Why CheckCustom(double value, string error, Predicate<double> check, Action<string> onError = null)
        {
            if (!check(value))
            {
                string reason = string.Format("{1} [got: {0}]", value, error);
                doAction(onError, reason);
                return Why.FalseBecause(reason);
            }
            return true;
        }

        /// <summary>
        /// Checks is an index iv valid.
        /// </summary>
        public static Why CheckIndex<T>(int index, IList<T> array, Action<string> onError = null)
        {
            if (index < 0)
            {
                string reason = string.Format("Index can not be negative, got: {0}", index);
                doAction(onError, reason);
                return Why.FalseBecause(reason);
            }
            if (index >= array.Count)
            {
                string reason = string.Format("Index out of bounts 0 <= index < {1}, got: {0}", index, array.Count);
                doAction(onError, reason);
                return Why.FalseBecause(reason);
            }
            return true;
        }

        /// <summary>
        /// Checks if a key is found in a dictionary. 
        /// </summary>
        public static Why CheckIsKey<K,V>(K key, IDictionary<K, V> dictionary, Action<string> onError = null)
        {
            if (dictionary.ContainsKey(key))
            {
                string reason = string.Format("Key not found: {0}", key);
                doAction(onError, reason);
                return Why.FalseBecause(reason);
            }
            return true;
        }
        
        private static void doAction(Action<string> onError, string reason)
        {
            if(onError != null)
            {
                try
                {
                    onError(reason);
                }
                catch (Exception)
                {

                }
            }
        }

    }
}
