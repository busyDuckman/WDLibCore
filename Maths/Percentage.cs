/*
 * The following code is Copyright 2018 Dr Warren Creemers (busyDuckman)
 * See LICENSE.md for more information.
 */

namespace WDToolbox.Maths
{
    /// <summary>
    /// For all those times a percentage (0% to 100%) needs to be handled.
    /// </summary>
    public class Percentage
    {
        double value;

        public double Value
        {
            get { return this.value; }
            set { if (IsValidPercentage(value)) { this.value = value; } }
        }

        public double HundredValue
        {
            get { return this.value*100.0; }
            set { Value = value * 0.01; }
        }

        private Percentage(double val) { value = val; }

        public static Percentage FromZeroToOne(double val)
        {
            return IsValidPercentage(val) ? new Percentage(val) : null;
        }

        public static Percentage FromZeroToHundred(double val)
        {
            return FromZeroToOne(val * 0.01);
        }

        public static implicit operator double(Percentage p) {return p.value;}

        public static explicit operator Percentage(double val) { return FromZeroToOne(val); }

        public static bool IsValidPercentage(double value)
        {
            return ((value >= 0.0) && (value <= 1.0));
        }

        public override string ToString()
        {
            return "" + (value * 100.0) + "%";
        }


        public byte ScaleToByte()
        {
            return (byte)Range.Range.clamp(0, 255, (int)(this.value * 255.0));
        }
    }
}
