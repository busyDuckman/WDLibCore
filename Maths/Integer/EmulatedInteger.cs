/*
 * The following code is Copyright 2018 Dr Warren Creemers (busyDuckman)
 * See LICENSE.md for more information.
 */
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WDToolbox.Maths.Integer
{
    public enum EdianType { Big, Small };


    /// <summary>
    /// C# integer arithmetic casts up before performing arithmetic on integer types greater than 32bit.
    /// This creates issues for those who were expecting an overflow...
    /// So I created this.
    /// 
    /// I extended it to emulate various integer types
    /// </summary>
    public struct EmulatedInteger : IComparable, IFormattable, IConvertible, IComparable<EmulatedInteger>, IEquatable<EmulatedInteger>
    {
        /// <summary>
        /// The overflow exception.
        /// </summary>
        public class EmulatedIntegerOverflowException : OverflowException
        {
            public EmulatedIntegerOverflowException() : base() { }
            public EmulatedIntegerOverflowException(string message) : base(message) { }
            public EmulatedIntegerOverflowException(string message, Exception innerException) : base(message, innerException) { }

            internal static void OverflowCheck(string operationDescripton, long value, long minInclusive, long maxInclusive)
            {
                if (value < minInclusive)
                {
                    throw new EmulatedIntegerOverflowException(string.Format("Error performing [{0}]: {1} must be >= {2}",
                                                                (operationDescripton ?? "operation").Trim(),
                                                                value,
                                                                minInclusive));
                }
                if (value > maxInclusive)
                {
                    throw new EmulatedIntegerOverflowException(string.Format("Error performing [{0}]: {1} must be <= {2}",
                                                                (operationDescripton ?? "operation").Trim(),
                                                                value,
                                                                maxInclusive));
                }
            }
        }

        /// <summary>
        /// Internally hold the data for the integer.
        /// </summary>
        private long valueLong;
        
        public long ValueLong
        {
            get { return valueLong; }
            set
            {
                EmulatedIntegerOverflowException.OverflowCheck("assignment", valueLong, MinValue, MaxValue);

                //all good
                valueLong = value;
            }
        }

        public int Bits { get; private set; }
        public bool Signed { get; private set; }
        //public EdianType Edian { get; private set; }
        //public static EdianType DefaultEdian { get { return EdianType.Big; } }
        private readonly long MaxValue;
        private readonly long MinValue;

        public EmulatedInteger(long value, int bits, bool signed)
            : this()
        {
            ValueLong = value;
            Bits = bits;
            Signed = signed;

            MaxValue = Signed ? (1 << (bits - 1)) - 1 : 1 << bits;
            MinValue = Signed ? -(1 << (bits - 1)) : 0;
        }

        private EmulatedInteger(long value, EmulatedInteger baseType)
            : this()
        {
            ValueLong = value;
            Bits = baseType.Bits;
            Signed = baseType.Signed;

            MaxValue = baseType.MaxValue;
            MinValue = baseType.MinValue;
        }

        public static EmulatedInteger UInt16(ushort value)
        {
            return new EmulatedInteger(value, 16, false);
        }
        public static EmulatedInteger SInt16(short value)
        {
            return new EmulatedInteger(value, 16, true);
        }
        public static EmulatedInteger UInt8(byte value)
        {
            return new EmulatedInteger(value, 8, false);
        }
        public static EmulatedInteger SInt8(sbyte value)
        {
            return new EmulatedInteger(value, 8, true);
        }

        public override int GetHashCode() { return valueLong.GetHashCode(); }
        public override string ToString() { return ValueLong.ToString(); }




        /// <summary>
        /// Bit access.
        /// </summary>
        /// <param name="i">nth bit to access.</param>
        /// <returns></returns>
        public bool this[int i]
        {
            get
            {
                return (valueLong & (1 << i)) != 0;
            }
        }

        private static EmulatedInteger FromOperation(long value, EmulatedInteger a, EmulatedInteger b, string operationName)
        {
            try
            {
                if (!a.isSameType(b))
                {
                    throw CreateTypeClashException(operationName);
                }

                EmulatedIntegerOverflowException.OverflowCheck(operationName, value, a.MinValue, a.MaxValue);
            }
            catch (EmulatedIntegerOverflowException ex)
            {
                throw new InvalidOperationException(string.Format("Error performing operator: {0} {2} {1}: {3}", 
                                                                   a, b, operationName ?? "?", ex.Message), 
                                                    ex);
            }

            return new EmulatedInteger(value, a);
        }

        private static EmulatedInteger FromOperation(long value, EmulatedInteger a, int b, string operationName)
        {
            try
            {
                EmulatedIntegerOverflowException.OverflowCheck(operationName, value, a.MinValue, a.MaxValue);
            }
            catch (EmulatedIntegerOverflowException ex)
            {
                throw new InvalidOperationException(string.Format("Error performing operator: {0} {2} {1}", a, b, operationName ?? "?"), ex);
            }

            return new EmulatedInteger(value, a);
        }

        private static EmulatedInteger FromOperation(long value, EmulatedInteger i, string operationName)
        {
            try
            {
                EmulatedIntegerOverflowException.OverflowCheck(operationName, value, i.MinValue, i.MaxValue);
            }
            catch (EmulatedIntegerOverflowException ex)
            {
                throw new InvalidOperationException(string.Format("Error performing operator: \"{1}\" on {0} ", i, operationName ?? "?"), ex);
            }

            return new EmulatedInteger(value, i);
        }

        private bool isSameType(EmulatedInteger another)
        {
            return (Bits == another.Bits) && (Signed == another.Signed);//&& (Edian == another.Edian);
        }

        private static Exception CreateTypeClashException(string operation)
        {
            return new InvalidOperationException("EmulatedInteger can not perform " + operation + " if values are not of the same type.");
        }


        #region parsing
        public static long Parse(string s) { return long.Parse(s); }
        public static long Parse(string s, NumberStyles style) { return long.Parse(s, style); }
        public static long Parse(string s, NumberStyles style, IFormatProvider provider) { return long.Parse(s, style, provider); }
        public static bool TryParse(string s, out long result) { return long.TryParse(s, out result); }
        public static bool TryParse(string s, NumberStyles style, IFormatProvider provider, out long result) { return long.TryParse(s, style, provider, out result); }
        #endregion

        #region implementing interfaces
        public int CompareTo(object obj)
        {
            return ValueLong.CompareTo(obj);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return ValueLong.ToString(format, formatProvider);
        }

        public int CompareTo(EmulatedInteger other)
        {
            return ValueLong.CompareTo(other.ValueLong);
        }

        public bool Equals(EmulatedInteger other)
        {
            return ValueLong == other.ValueLong;
        }

        public TypeCode GetTypeCode()
        {
            return TypeCode.Object;
        }

        public bool ToBoolean(IFormatProvider provider)
        {
            return ((IConvertible)ValueLong).ToBoolean(provider);
        }

        public byte ToByte(IFormatProvider provider)
        {
            return ((IConvertible)ValueLong).ToByte(provider);
        }

        public char ToChar(IFormatProvider provider)
        {
            return ((IConvertible)ValueLong).ToChar(provider);
        }

        public DateTime ToDateTime(IFormatProvider provider)
        {
            return ((IConvertible)ValueLong).ToDateTime(provider);
        }

        public decimal ToDecimal(IFormatProvider provider)
        {
            return ((IConvertible)ValueLong).ToDecimal(provider);
        }

        public double ToDouble(IFormatProvider provider)
        {
            return ((IConvertible)ValueLong).ToDouble(provider);
        }

        public short ToInt16(IFormatProvider provider)
        {
            return ((IConvertible)ValueLong).ToInt16(provider);
        }

        public int ToInt32(IFormatProvider provider)
        {
            return ((IConvertible)ValueLong).ToInt32(provider);
        }

        public long ToInt64(IFormatProvider provider)
        {
            return ((IConvertible)ValueLong).ToInt64(provider);
        }

        public sbyte ToSByte(IFormatProvider provider)
        {
            return ((IConvertible)ValueLong).ToSByte(provider);
        }

        public float ToSingle(IFormatProvider provider)
        {
            return ((IConvertible)ValueLong).ToSingle(provider);
        }

        public string ToString(IFormatProvider provider)
        {
            return ((IConvertible)ValueLong).ToString(provider);
        }

        public object ToType(Type conversionType, IFormatProvider provider)
        {
            return ((IConvertible)ValueLong).ToType(conversionType, provider);
        }

        public ushort ToUInt16(IFormatProvider provider)
        {
            return ((IConvertible)ValueLong).ToUInt16(provider);
        }

        public uint ToUInt32(IFormatProvider provider)
        {
            return ((IConvertible)ValueLong).ToUInt32(provider);
        }

        public ulong ToUInt64(IFormatProvider provider)
        {
            return ((IConvertible)ValueLong).ToUInt64(provider);
        }

        #endregion

        #region unary operators
        public static EmulatedInteger operator --(EmulatedInteger i)
        {
            return FromOperation(i.valueLong - 1, i, "--");
        }

        public static EmulatedInteger operator ++(EmulatedInteger i)
        {
            return FromOperation(i.valueLong + 1, i, "--");
        }

        public static EmulatedInteger operator -(EmulatedInteger i)
        {
            return FromOperation(-i.valueLong, i, "-");
        }

        public static EmulatedInteger operator +(EmulatedInteger i)
        {
            return FromOperation(+i.valueLong, i, "-");
        }

        public static EmulatedInteger operator ~(EmulatedInteger i)
        {
            return FromOperation(~i.valueLong, i, "~");
        }
        #endregion

        #region binary operators (EmulatedInteger and EmulatedInteger)
        public static EmulatedInteger operator +(EmulatedInteger a, EmulatedInteger b)
        {
            return FromOperation(a.valueLong + b.valueLong, a, b, "+");
        }

        public static EmulatedInteger operator -(EmulatedInteger a, EmulatedInteger b)
        {
            return FromOperation(a.valueLong - b.valueLong, a, b, "-");
        }

        public static EmulatedInteger operator *(EmulatedInteger a, EmulatedInteger b)
        {
            return FromOperation(a.valueLong * b.valueLong, a, b, "*");
        }

        public static EmulatedInteger operator /(EmulatedInteger a, EmulatedInteger b)
        {
            return FromOperation(a.valueLong / b.valueLong, a, b, "/");
        }

        public static EmulatedInteger operator %(EmulatedInteger a, EmulatedInteger b)
        {
            return FromOperation(a.valueLong % b.valueLong, a, b, "%");
        }

        public static EmulatedInteger operator &(EmulatedInteger a, EmulatedInteger b)
        {
            return FromOperation(a.valueLong & b.valueLong, a, b, "&");
        }

        public static EmulatedInteger operator |(EmulatedInteger a, EmulatedInteger b)
        {
            return FromOperation(a.valueLong | b.valueLong, a, b, "|");
        }

        public static EmulatedInteger operator ^(EmulatedInteger a, EmulatedInteger b)
        {
            return FromOperation(a.valueLong ^ b.valueLong, a, b, "^");
        }

        public static EmulatedInteger operator <<(EmulatedInteger a, int b)
        {
            long foo = a.valueLong << b;
            return FromOperation(foo, a, b, "<<(int)");
        }

        public static EmulatedInteger operator >>(EmulatedInteger a, int b)
        {
            long foo = a.valueLong >> b;
            return FromOperation(foo, a, b, ">>(int)");
        }
        #endregion

        #region comparison operators (EmulatedInteger and EmulatedInteger)
        public static bool operator ==(EmulatedInteger a, EmulatedInteger b)
        {
            return a.valueLong == b.valueLong;
        }

        public static bool operator !=(EmulatedInteger a, EmulatedInteger b)
        {
            return a.valueLong != b.valueLong;
        }

        public static bool operator <(EmulatedInteger a, EmulatedInteger b)
        {
            return a.valueLong < b.valueLong;
        }

        public static bool operator >(EmulatedInteger a, EmulatedInteger b)
        {
            return a.valueLong > b.valueLong;
        }

        public static bool operator <=(EmulatedInteger a, EmulatedInteger b)
        {
            return a.valueLong <= b.valueLong;
        }

        public static bool operator >=(EmulatedInteger a, EmulatedInteger b)
        {
            return a.valueLong >= b.valueLong;
        }
        #endregion

        #region binary operators that permit other types

        #endregion

        #region casting
        public static explicit operator long(EmulatedInteger i)
        {
            EmulatedIntegerOverflowException.OverflowCheck("cast to long", i.valueLong, long.MinValue, long.MaxValue);
            return i.valueLong;
        }

        public static explicit operator ulong(EmulatedInteger i)
        {
            EmulatedIntegerOverflowException.OverflowCheck("cast to ulong", i.valueLong, 0, long.MaxValue);
            return (ulong)i.valueLong;
        }

        public static explicit operator int(EmulatedInteger i)
        {
            EmulatedIntegerOverflowException.OverflowCheck("cast to int", i.valueLong, int.MinValue, int.MaxValue);
            return (int)i.valueLong;
        }

        public static explicit operator uint(EmulatedInteger i)
        {
            EmulatedIntegerOverflowException.OverflowCheck("cast to uint", i.valueLong, uint.MinValue, uint.MaxValue);
            return (uint)i.valueLong;
        }

        public static explicit operator short(EmulatedInteger i)
        {
            EmulatedIntegerOverflowException.OverflowCheck("cast to short", i.valueLong, short.MinValue, short.MaxValue);
            return (short)i.valueLong;
        }

        public static explicit operator ushort(EmulatedInteger i)
        {
            EmulatedIntegerOverflowException.OverflowCheck("cast to ushort", i.valueLong, ushort.MinValue, ushort.MaxValue);
            return (ushort)i.valueLong;
        }

        public static explicit operator sbyte(EmulatedInteger i)
        {
            EmulatedIntegerOverflowException.OverflowCheck("cast to sbyte", i.valueLong, sbyte.MinValue, sbyte.MaxValue);
            return (sbyte)i.valueLong;
        }

        public static explicit operator byte(EmulatedInteger i)
        {
            EmulatedIntegerOverflowException.OverflowCheck("cast to byte", i.valueLong, byte.MinValue, byte.MaxValue);
            return (byte)i.valueLong;
        }
        #endregion

    }
}
