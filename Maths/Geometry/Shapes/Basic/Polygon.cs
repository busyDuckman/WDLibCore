/*
 * The following code is Copyright 2018 Dr Warren Creemers (busyDuckman)
 * See LICENSE.md for more information.
 */

//TODO: This looks like an unfinished version. Find the finished version, or finish the code. 

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WDToolbox.Maths.Geometry.Lines;

namespace WDToolbox.Maths.Geometry.Shapes.Basic
{
    public class Polygon : Shape//, IList<Point2D>, INotifyCollectionChanged
    {
        public PointList Points { get; protected set; }

        int NumPoints { get { return Points.Count; } }


        [NonSerialized]
        private double surfaceAreaProxy, pathLengthProxy;

        public Polygon()
        {
            Invalidate();
            Points = new PointList();
            Points.OnNumberOfItemsChanged = (E, P) => Invalidate();
            Points.OnValueChanged = (E, P) => Invalidate();
        }

        public Polygon(IList<Point2D> points)
        {
            Invalidate();
            Points = new PointList(points);
            Points.OnNumberOfItemsChanged = (E, P) => Invalidate();
            Points.OnValueChanged = (E, P) => Invalidate();
        }

        protected Polygon(Polygon another)
        {
            Invalidate();
            Points = (PointList)another.Points.Clone();
            Points.OnNumberOfItemsChanged = (E, P) => Invalidate();
            Points.OnValueChanged = (E, P) => Invalidate();
        }
        public override object Clone() { return new Polygon(this); }

        public override void Invalidate()
        {
            surfaceAreaProxy = -1;
            pathLengthProxy = -1;
            base.Invalidate();
        }

        public override double SurfaceArea
        {
            get
            {
                if(surfaceAreaProxy < 0)
                {
                    refreshAreaCalculation();
                }
                return surfaceAreaProxy;
            }
        }

        private void refreshAreaCalculation()
        {
            double area = 0;
            int n = NumPoints;

            if (NumPoints >= 3)
            {
                for (int i = 1; i < n; i++)
                {
                    area += Points[i].X * (Points[i + 1].Y - Points[i - 1].Y);
                }
                area += Points[n].X * (Points[1].Y - Points[n - 1].Y);  // wrap-around term
                surfaceAreaProxy = area / 2.0;
            }
            else
            {
                // < 3 points
                surfaceAreaProxy = 0; //polygon is a line or a dot, or nothing.
            }
        }

        public override double PathLength
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override Point2D Start
        {
            get
            {
                throw new NotImplementedException();
            }
        }



        //because its basicly a polyline
        public PolyLine ToPolyLine()
        {
            return new PolyLine(Points);
        }

        public override Polygon ToPolygon(double lineLenOnCurves)
        {
            return this;
        }

        protected override Rectangle2D CalculateBoundingBox()
        {
            double minX = Points.Select(P => P.X).Min();
            double minY = Points.Select(P => P.Y).Min();
            double maxX = Points.Select(P => P.X).Max();
            double maxY = Points.Select(P => P.Y).Max();
            return new Rectangle2D(minX, minY, maxX-minX, maxY-minY);
        }
    }
}
