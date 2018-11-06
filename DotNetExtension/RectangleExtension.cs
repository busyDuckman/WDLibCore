/*
 * The following code is Copyright 2018 Dr Warren Creemers (busyDuckman)
 * See LICENSE.md for more information.
 */
using System.Drawing;
using WDToolbox.Maths.Geometry;

namespace WDToolbox//.DotNetExtension
{
    public static class RectangleExtension
    {
        public static int CalculateSurfaceArea(this Rectangle rec)
        {
            return rec.Width * rec.Height;
        }

        public static Point2D Middle(this Rectangle rec)
        { 
            double mx = rec.X + (rec.Width/2.0);
            double my = rec.Y + (rec.Height/2.0);
            return new Point2D(mx, my);
        }
    }
}
