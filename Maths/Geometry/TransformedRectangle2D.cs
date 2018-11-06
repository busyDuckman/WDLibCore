/*
 * The following code is Copyright 2018 Dr Warren Creemers (busyDuckman)
 * See LICENSE.md for more information.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WDToolbox.Maths.Geometry
{
    /// <summary>
    /// A rectangle which can be put through affine transformations
    /// </summary>
    public class TransformedRectangle2D : IReadOnlyList<Point2D>, IGeometry
    {
        const int TopLeftIndex = 0, TopRightIndex = 1, LowerRightIndex = 2, LowerLeftIndex = 3;
        public Point2D NewTopLeft { get {return points[TopLeftIndex];} }
        public Point2D NewTopRight { get { return points[TopRightIndex]; } }
        public Point2D NewLowerLeft { get { return points[LowerLeftIndex]; } }
        public Point2D NewLowerRight { get { return points[LowerRightIndex]; } }

        
        protected readonly List<Point2D> points;
        public IReadOnlyList<Point2D> Points { get { return points.AsReadOnly(); } }

        public TransformedRectangle2D(Point2D newTopLeft, Point2D newTopRight, Point2D newLowerRight, Point2D newLowerLeft)
        {
            points = new List<Point2D>();
            
            //this used to be an array, until the enumerator got funny
            /*points[TopLeftIndex] = newTopLeft;
            points[TopRightIndex] = newTopRight;
            points[LowerRightIndex] = newLowerRight;
            points[LowerLeftIndex] = newLowerLeft;*/

            points.Add(newTopLeft);
            points.Add(newTopRight);
            points.Add(newLowerRight);
            points.Add(newLowerLeft);
            
        }
        
        /// <summary>
        /// eg, True if ithe the rotation is a multiple of 90 deg.
        /// </summary>
        public bool IsAlignedOrthogonally => Rectangle2D.BoundingBox(new Point2D[] { NewTopLeft, NewLowerRight }).Equals(Rectangle2D.BoundingBox(new Point2D[] { NewTopRight, NewLowerLeft }));


        public Point2D this[int index]
        {
            get 
            {
                switch (index)
                {
                    case TopLeftIndex:
                        return NewTopLeft;
                    case TopRightIndex:
                        return NewTopRight;
                    case LowerRightIndex:
                        return NewLowerRight;
                    case LowerLeftIndex:
                        return NewLowerLeft;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
        }

        public int Count => 4;

        public Rectangle2D BoundingBox => Rectangle2D.BoundingBox(points);

        public IEnumerator<Point2D> GetEnumerator()
        {
            return points.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ((System.Collections.IEnumerable)points).GetEnumerator();
        }
    }
}
