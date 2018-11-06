/*
 * The following code is Copyright 2018 Dr Warren Creemers (busyDuckman)
 * See LICENSE.md for more information.
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WDToolbox.Maths.Geometry.Shapes.Basic;

namespace WDToolbox.Maths.Geometry.Shapes
{
    public enum InterSectionType { None, InsideOff, Intersect, Touch, Contains };
    public interface IShape : ITransformable2D, IPath, ICloneable
    {
        InterSectionType Intersects(IShape another);
        Polygon ToPolygon(double lineLenOnCurves);

        bool ContainsPoint(Point2D point);
        double DistanceToNearestEdge(Point2D point);

        double SurfaceArea { get; }
    }

    public static class IShapeExtension
    {
        /// <summary>
        /// Default conversion to polygon.
        /// </summary>
        /// <param name="shape"></param>
        /// <returns></returns>
        public static Polygon ToPolygon(this IShape shape)
        {
            double len = Math.Min(shape.BoundingBox.Width, shape.BoundingBox.Height);
            return shape.ToPolygon(len / 32.0);
        }
    }
    
}
