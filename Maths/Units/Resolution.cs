/*
 * The following code is Copyright 2018 Dr Warren Creemers (busyDuckman)
 * See LICENSE.md for more information.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WDToolbox.Data.DataStructures;

namespace WDToolbox.Maths.Units
{
    public class Resolution
    {
        public double Dots { get; private set; }
        public Distance MesuredDistance { get; private set; }

        public double DPI { get { return Dots / MesuredDistance.ImpInchs; } }

        public double DotsPerMetre { get { return Dots / MesuredDistance.Metres; } }
        public double DotsPerMilliMetre { get { return Dots / MesuredDistance.MilliMetres; } }

        public Resolution(double dots, Distance per)
        {
            MesuredDistance = per.Absolute();
            Dots = Math.Abs(dots);
        }

        public static Resolution fromDPI(double dpi)
        {
            return new Resolution(dpi, Distance.FromImpInchs(1));
        }

        public static Resolution fromDotsPerMetre(double dpm)
        {
            return new Resolution(dpm, Distance.FromMetres(1));
        }

        public static Resolution fromMesurement(double dots, Distance per)
        {
            return new Resolution(dots, per);
        }

        public static Resolution fromMesurement(double dots, double meters)
        {
            return fromMesurement(dots, Distance.FromMetres(meters));
        }

        public override string ToString()
        {
            return string.Format("{0} DPI", DPI);
        }

        public Why Save(BinaryWriter s)
        {
            return Why.FromTry(delegate()
            {
                s.Write(Dots);
                s.Write(MesuredDistance.Metres);
            });
        }

        public static Why Load(BinaryReader s, out Resolution res)
        {
            try
            {
                double dots = s.ReadDouble();
                double meters = s.ReadDouble();
                res = Resolution.fromMesurement(dots, meters);
            }
            catch (Exception ex)
            {
                res = null;
                return Why.FalseBecause(ex);
            }

            return true;
        }
        
    }
}
