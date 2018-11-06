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
using WDToolbox.Maths.Geometry.Lines;
using WDToolbox.Maths.Geometry.Shapes.Basic;

namespace WDToolbox.Maths.Geometry.Shapes
{
    /// <summary>
    /// General shape class.
    /// Novel shapes that can't do intersection do not need to override the
    /// various implemented methods in this class. So long as a working
    /// ToPolygon() exists, the shape will be converted back as needed.
    /// </summary>
    public abstract class Shape : IShape
    {
        //--------------------------------------------------------------------------------------------------
        // Transient data.
        //--------------------------------------------------------------------------------------------------
        [NonSerialized]
        protected Rectangle2D boundingBoxProxy = null;

        /// <summary>
        /// This must be called whenever the shape is mutated in some way.
        /// Causes cached polygon & bounding box etc to be discarded.
        /// </summary>MaxMax
        public virtual void Invalidate()
        {
            boundingBoxProxy = null;
        }

        //--------------------------------------------------------------------------------------------------
        // Bounding box
        //--------------------------------------------------------------------------------------------------
        public Rectangle2D BoundingBox
        {
            get
            {
                if(boundingBoxProxy == null)
                {
                    boundingBoxProxy = CalculateBoundingBox();
                }
                return this.ToPolygon().BoundingBox;
            }
        }

        public abstract double SurfaceArea { get; }
        public abstract double PathLength { get; }
        public abstract Point2D Start { get; }

        protected virtual Rectangle2D CalculateBoundingBox()
        {
            return this.ToPolygon().BoundingBox;
        }

        //--------------------------------------------------------------------------------------------------
        // IShape
        //--------------------------------------------------------------------------------------------------
        public virtual bool ContainsPoint(Point2D point)
        {
            return this.ToPolygon().ContainsPoint(point);
        }

        public virtual double DistanceToNearestEdge(Point2D point)
        {
            return this.ToPolygon().DistanceToNearestEdge(point);
        }

        public virtual InterSectionType Intersects(IShape another)
        {
            return this.ToPolygon().Intersects(another);
        }

        public abstract Polygon ToPolygon(double lineLenOnCurves);

        //--------------------------------------------------------------------------------------------------
        // IPath
        //--------------------------------------------------------------------------------------------------

        public virtual PolyLine ToPolyLine(double lineLenOnCurves)
        {
            return ToPolygon(lineLenOnCurves).ToPolyLine();
        }

        public Point2D PointOnPath(double percent)
        {
            throw new NotImplementedException();
        }

        public List<PathPoint> Intersections(IPath path)
        {
            throw new NotImplementedException();
        }

        public double DistanceToPath(Point2D point)
        {
            throw new NotImplementedException();
        }

        public abstract object Clone();
    }
}
