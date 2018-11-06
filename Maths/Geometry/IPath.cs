/*
 * The following code is Copyright 2018 Dr Warren Creemers (busyDuckman)
 * See LICENSE.md for more information.
 */

 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WDToolbox.Maths.Geometry.Lines;
using WDToolbox.Maths.Geometry.Shapes;

namespace WDToolbox.Maths.Geometry
{
    public class PathPoint
    {
        public Point2D Position { get; protected set; }
        public double PercentAlongPath { get; protected set; }
    }

    public interface IGeometry //: ITransformable2D
    {
        Rectangle2D BoundingBox { get; }
    }

    public interface IPath : IGeometry
    {
        Point2D Start { get; }
        double PathLength { get; }

        Point2D PointOnPath(double percent);
        List<PathPoint> Intersections(IPath path);
        double DistanceToPath(Point2D point);

        PolyLine ToPolyLine(double lineLenOnCurves); 
    }

    public interface IOpenPath : IPath
    {
        Point2D End { get; }
        List<IOpenPath> Split(double percentPos);
        IOpenPath Reverse();
    }

    /*public interface IOpenOrClosedPath : IPath
    {
        Point2D End { get; }
        bool Closed { get; }
    }*/

    public static class IPathExtension
    {
        public static IPath SubLine(this IOpenPath path, double perStart, double perEnd)
        {
            IOpenPath p = path.Split(perStart).Last().Split(perEnd-perStart).First();
            return p;
        }

        public static List<IOpenPath> SplitOnDistance(this IOpenPath path, double posDistance)
        {
            return path.Split(posDistance / path.PathLength);
        }

        public static Point2D PointOnPathByDistance(this IPath path, double posDistance)
        {
            return path.PointOnPath(posDistance / path.PathLength);
        }
    }
}
