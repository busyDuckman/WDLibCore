/*
 * The following code is Copyright 2018 Dr Warren Creemers (busyDuckman)
 * See LICENSE.md for more information.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WDToolbox.Maths.Geometry;
using DistanceUnitLut = System.Collections.Generic.Dictionary<WDToolbox.Maths.Units.DistanceUnits, string>;


namespace WDToolbox.Maths.Units
{
    /// <summary>
    /// Works with Surface Area units
    /// </summary>
    public class Dimensions2D
    {
        protected static DistanceUnitLut areaUnitLut;

        public Distance Width { get; protected set; }
        public Distance Height { get; protected set; }

        static Dimensions2D()
	    {
            string sqrd = "\u00B2";
            areaUnitLut = new DistanceUnitLut();
            
            areaUnitLut.Add(DistanceUnits.ImpFeet, "cm" + sqrd);
            areaUnitLut.Add(DistanceUnits.ImpInches, "in" + sqrd);
            areaUnitLut.Add(DistanceUnits.ImpMiles, "ft" + sqrd);
            areaUnitLut.Add(DistanceUnits.ImpMils, "mi" + sqrd);
            areaUnitLut.Add(DistanceUnits.KiloMetres, "km" + sqrd);
            areaUnitLut.Add(DistanceUnits.Metres, "m" + sqrd);
            areaUnitLut.Add(DistanceUnits.MilliMetres, "mm" + sqrd);
            areaUnitLut.Add(DistanceUnits.CentiMetres, "cm" + sqrd);
	    }

        public Dimensions2D(Distance width, Distance height)
        {
            Width = width;
            Height = height;
        }

        public static Dimensions2D FromMetres(double width, double height)
        {
            return new Dimensions2D(Distance.FromMetres(width), Distance.FromMetres(height));
        }

        public static Dimensions2D From(double width, double height, DistanceUnits unit)
        {
            return new Dimensions2D(Distance.FromDistance(width, unit), Distance.FromDistance(height, unit));
        }

        public static Dimensions2D FromMetres(Rectangle2D size)
        {
            return new Dimensions2D(Distance.FromMetres(size.Width), Distance.FromMetres(size.Height));
        }

        public static Dimensions2D From(Rectangle2D size, DistanceUnits unit)
        {
            return new Dimensions2D(Distance.FromDistance(size.Width, unit), Distance.FromDistance(size.Height, unit));
        }

        public string ToString(DistanceUnits unit, bool longVersion)
        {
            return string.Format("{0} by {1}", Width.ToString(unit, longVersion), Height.ToString(unit, longVersion));
        }

        public override string ToString()
        {
            return ToString(DistanceUnits.Metres, false);
        }

        public string AreaToString(DistanceUnits unit, int decimalPlaces)
        {
            double num = Width.In(unit) * Height.In(unit);
            decimalPlaces = Range.Range.clamp(decimalPlaces, 0, 10);
            return string.Format("{0} {1}", num.ToString("N"+decimalPlaces), areaUnitLut[unit]);
        }

        public string AreaToString()
        {
            return AreaToString(DistanceUnits.Metres, 2);
        }
    }
}
