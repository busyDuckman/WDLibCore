/*
 * The following code is Copyright 2018 Dr Warren Creemers (busyDuckman)
 * See LICENSE.md for more information.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using WDToolbox.AplicationFramework;

namespace WDToolbox.Maths.Units
{
    /// <summary>
    /// Represents a method to present a unit's unit of mesurement eg cm or " or km or kilometers
    /// </summary>
    public class DistanceUnitInfo
    {
        public string UnitText { get; protected set; }
        public DistanceUnits Unit { get; protected set; }
        public bool PreferedShortFormat { get; protected set; }
        public bool PreferedLongFormat { get; protected set; }

        public static explicit operator string(DistanceUnitInfo info) { return info.UnitText; }

        private static IList<DistanceUnitInfo> unitsTable;

        protected DistanceUnitInfo()
        {
            PreferedLongFormat = false;
            PreferedShortFormat = false;
        }


        public DistanceUnitInfo(DistanceUnits unit, string unitString)
            : this()
        {
            UnitText = unitString;
            Unit = unit;
        }

        public DistanceUnitInfo(DistanceUnits unit, string unitString, bool preferedShortFormat, bool preferedLongFormat)
            : this(unit, unitString)
        {
            PreferedShortFormat = preferedShortFormat;
            PreferedLongFormat = preferedLongFormat;
        }

        static DistanceUnitInfo()
        {
            unitsTable = new List<DistanceUnitInfo>();
            unitsTable.Add(new DistanceUnitInfo(DistanceUnits.ImpMils, "mils", true, true));
            unitsTable.Add(new DistanceUnitInfo(DistanceUnits.ImpMils, "thou"));
            unitsTable.Add(new DistanceUnitInfo(DistanceUnits.ImpMils, "mil"));

            unitsTable.Add(new DistanceUnitInfo(DistanceUnits.ImpFeet, "'", true, false));
            unitsTable.Add(new DistanceUnitInfo(DistanceUnits.ImpFeet, "ft", false, true));
            unitsTable.Add(new DistanceUnitInfo(DistanceUnits.ImpFeet, "foot"));
            unitsTable.Add(new DistanceUnitInfo(DistanceUnits.ImpFeet, "feet"));

            unitsTable.Add(new DistanceUnitInfo(DistanceUnits.ImpInches, "\"", true, false));
            unitsTable.Add(new DistanceUnitInfo(DistanceUnits.ImpInches, "in", false, true));
            unitsTable.Add(new DistanceUnitInfo(DistanceUnits.ImpInches, "inch"));
            unitsTable.Add(new DistanceUnitInfo(DistanceUnits.ImpInches, "inchs"));

            unitsTable.Add(new DistanceUnitInfo(DistanceUnits.ImpMiles, "mi", true, false));
            unitsTable.Add(new DistanceUnitInfo(DistanceUnits.ImpMiles, "mile"));
            unitsTable.Add(new DistanceUnitInfo(DistanceUnits.ImpMiles, "miles", false, true));
            
            //Metres, CentiMetres, MilliMetres, KiloMetres
            unitsTable.Add(new DistanceUnitInfo(DistanceUnits.MilliMetres, "mm", true, true));
            unitsTable.Add(new DistanceUnitInfo(DistanceUnits.CentiMetres, "cm", true, true));
            unitsTable.Add(new DistanceUnitInfo(DistanceUnits.KiloMetres, "km", true, true));
            unitsTable.Add(new DistanceUnitInfo(DistanceUnits.Metres, "m", true, true));
            unitsTable.Add(new DistanceUnitInfo(DistanceUnits.Metres, "metre"));
            unitsTable.Add(new DistanceUnitInfo(DistanceUnits.Metres, "metres"));

            AddMetricViaPrefix("milli", DistanceUnits.MilliMetres);
            AddMetricViaPrefix("kilo", DistanceUnits.KiloMetres);
            AddMetricViaPrefix("centi", DistanceUnits.CentiMetres);
        }

        private static void AddMetricViaPrefix(string prefix, DistanceUnits unit)
        {
            unitsTable.Add(new DistanceUnitInfo(unit, prefix + "metres"));
            unitsTable.Add(new DistanceUnitInfo(unit, prefix + "metre"));
            unitsTable.Add(new DistanceUnitInfo(unit, prefix + " metre"));
            unitsTable.Add(new DistanceUnitInfo(unit, prefix + "-metre"));
            unitsTable.Add(new DistanceUnitInfo(unit, prefix + " metres"));
            unitsTable.Add(new DistanceUnitInfo(unit, prefix + "-metres"));
        }

        public static DistanceUnits? ParseUnit(string unitText)
        {
            var info = unitsTable.FirstOrDefault(U => U.UnitText == unitText);
            if(info == null)
            {
                info = unitsTable.FirstOrDefault(U => U.UnitText.ToLower().Trim() == unitText.ToLower().Trim());
            }

            return (info != null) ? (DistanceUnits?)info.Unit : null;
        }

        public static String GetPreferedLongUnitString(DistanceUnits unit)
        {
            var info = from U in unitsTable where U.Unit == unit select U;
            var theInfo = info.FirstOrDefault(U => U.PreferedLongFormat) ?? info.First();
            if (theInfo != null)
            {
                return theInfo.UnitText;
            }
            else
            {
                WDAppLog.LogNeverSupposedToBeHere();
                return ("(unnamed unit: " + unit.ToString() + ")");
            }
        }

        public static String GetPreferedShortUnitString(DistanceUnits unit)
        {
            var info = from U in unitsTable where U.Unit == unit select U;
            var theInfo = info.FirstOrDefault(U => U.PreferedShortFormat) ?? info.First();
            if (theInfo != null)
            {
                return theInfo.UnitText;
            }
            else
            {
                WDAppLog.LogNeverSupposedToBeHere();
                return ("(unnamed unit: " + unit.ToString() + ")");
            }
        }
    }

}