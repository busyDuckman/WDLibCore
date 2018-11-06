/*
 * The following code is Copyright 2018 Dr Warren Creemers (busyDuckman)
 * See LICENSE.md for more information.
 */

//TODO: This looks like an unfinished version. Find the finished version, or finish the code. 


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WDToolbox.Maths.Geometry.Lines
{
    public class PolyLine : IOpenPath
    {
        List<Point2D> points;

        public PolyLine(IEnumerable<Point2D> points)
        {
            points = new List<Point2D>(points);
        }

        public Rectangle2D BoundingBox { get { return Rectangle2D.BoundingBox(points); } }

        public Point2D End
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public double PathLength
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Point2D Start
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public double DistanceToPath(Point2D point)
        {
            throw new NotImplementedException();
        }

        public List<PathPoint> Intersections(IPath path)
        {
            throw new NotImplementedException();
        }

        public Point2D PointOnPath(double percent)
        {
            throw new NotImplementedException();
        }

        public IOpenPath Reverse()
        {
            throw new NotImplementedException();
        }

        public List<IOpenPath> Split(double percentPos)
        {
            throw new NotImplementedException();
        }

        public PolyLine ToPolyLine(double lineLenOnCurves)
        {
            throw new NotImplementedException();
        }
    }
}
