/*
 * The following code is Copyright 2018 Dr Warren Creemers (busyDuckman)
 * See LICENSE.md for more information.
 */


using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Linq;
using System.Collections;
using System.Runtime.CompilerServices;

namespace WDToolbox.Maths.Geometry
{
    //--------------------------------------------------------------------------------------------------
    // Point2D
    //--------------------------------------------------------------------------------------------------
    public struct Point2D : IEquatable<Point2D>, ICloneable, IReadOnlyList<double>
    {
        //--------------------------------------------------------------------------------------------------
        // Static data
        //--------------------------------------------------------------------------------------------------
        public static Point2D Origin { get { return new Point2D(0, 0); } }

        //--------------------------------------------------------------------------------------------------
        // Instance data
        //--------------------------------------------------------------------------------------------------
        public double X { get; set; }
        public double Y { get; set; }

        //--------------------------------------------------------------------------------------------------
        // Accessors
        //--------------------------------------------------------------------------------------------------
        double Theta { get { return Math.Atan2(Y, X); } }

        //--------------------------------------------------------------------------------------------------
        // Constructors and factory methods
        //--------------------------------------------------------------------------------------------------
        public Point2D(double x, double y) : this()
        {
            X = x;
            Y = y;
        }

        private Point2D(Point2D another)
            : this()
        {
            X = another.X;
            Y = another.Y;
        }

        public Point2D(Point p)
            : this()
        {
            X = p.X;
            Y = p.Y;
        }

        public Point2D(PointF p)
            : this()
        {
            X = p.X;
            Y = p.Y;
        }


        public static Point2D FromLenTheta(double len, double radians)
        {
            return new Point2D(len * Math.Cos(radians), len * Math.Sin(radians));
        }

        //.net compatibility
        public Point AsPoint()
        {
            return new Point((int)X, (int)Y);
        }

        //.net compatibility
        public PointF AsPointF()
        {
            return new PointF((float)X, (float)Y);
        }

        #region Operators

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2D operator +(Point2D a, Point2D b)
        {
            return new Point2D(a.X + b.X, a.Y + b.Y);
        }

        public static Point2D operator +(Point2D a, Point b)
        {
            return new Point2D(a.X + b.X, a.Y + b.Y);
        }

        public static Point2D operator +(Point a, Point2D b)
        {
            return new Point2D(a.X + b.X, a.Y + b.Y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2D operator -(Point2D a, Point2D b)
        {
            return new Point2D(a.X - b.X, a.Y - b.Y);
        }

        public static Point2D operator -(Point2D a, Point b)
        {
            return new Point2D(a.X - b.X, a.Y - b.Y);
        }

        public static Point2D operator -(Point a, Point2D b)
        {
            return new Point2D(a.X - b.X, a.Y - b.Y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2D operator *(Point2D a, double d)
        {
            return new Point2D(a.X * d, a.Y * d);
        }

        public static Point2D operator *(Point2D a, int d)
        {
            return a * (double) d;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2D operator *(double d, Point2D a)
        {
            return new Point2D(a.X * d, a.Y * d);
        }

        public static Point2D operator *(int d, Point2D a)
        {
            return a * (double)d;
        }


        #endregion

        public override string ToString()
        {
            return "("+X+", "+Y+")";
        }

        public Point2D RotateAboutOrigin(double clockwiseRadians)
        {
            double s = Math.Sin(clockwiseRadians);
            double c = Math.Cos(clockwiseRadians);
            double x = X * c + Y * s;
            double y = -X * s + Y * c;
            return new Point2D(x, y);
        }

        public static Point2D operator-(Point2D p)
        {
            return new Point2D(-p.X,-p.Y);
        }

        public override int GetHashCode()
        {
            return (X.GetHashCode() >> 1) + (31 * Y.GetHashCode());
        }

        public override bool Equals(object obj)
        {
            return (obj is Point2D) ? (this == (Point2D)obj) : false;
        }

        public bool Equals(Point2D point)
        {
            return this == point;
        }

        public static bool operator ==(Point2D a, Point2D b)
        {
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            return ((a.X == b.X) && (a.Y == b.Y));
        }

        public static bool operator !=(Point2D a, Point2D b)
        {
            return !(a == b);
        }

        public double LengthSquared()
        {
            return (X * X) + (Y * Y);
        }

        public double Length()
        {
            return Math.Sqrt((X * X) + (Y * Y));
        }

        public double DistanceTo(Point2D another)
        {
            double dX = this.X - another.X;
            double dY = this.Y - another.Y;
            return Math.Sqrt((dX * dX) + (dY * dY));
        }

        public double DistanceSquared(Point2D another)
        {
            double dX = this.X - another.X;
            double dY = this.Y - another.Y;
            return (dX * dX) + (dY * dY);
        }

        public Point2D UnitVector()
        {
            double len = Length();
            if (len > 0.0)
            {
                return new Point2D(X / len, Y / len);
            }
            else
            {
                return new Point2D(0, 0);
            }
        }

        public Point2D Transpose()
        {
            return new Point2D(Y, X);
        }

        public object Clone()
        {
            return new Point2D(this);
        }

        #region IReadOnlyList<double>

        public IEnumerator<double> GetEnumerator()
        {
            for (int i = 0; i < 2; i++)
            {
                yield return (i==0) ? X : Y;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            for (int i = 0; i < 2; i++)
            {
                yield return (i == 0) ? X : Y;
            }
        }

        public int Count { get { return 2; } }

        public double this[int index]
        {
            get
            {
                switch(index)
                {
                    case 0: return X; 
                    case 1: return Y;
                }
                throw new IndexOutOfRangeException("Index of a point can only be 0 or 1.");
            }
        }

        #endregion

        public Size ToSize(IntegerConversionMode mode)
        {
            return new Size(X.ToInteger(mode), Y.ToInteger(mode));
        }
    }


    public static class Point2DExtension
    {
        public static Point[] ToDrawingPoint(this Point2D[] points)
        {
            Point[] res = new Point[points.Length];
            for (int i = 0; i < points.Length; i++)
            {
                res[i] = new Point((int)Math.Floor(points[i].X),
                                   (int)Math.Floor(points[i].Y));
            }
            return res;
        }

        public static IList<Point> ToDrawingPoints(this IEnumerable<Point2D> points)
        {
            List<Point> res = new List<Point>();
            foreach (Point2D point in points)
            {
                Point p = new Point((int)Math.Floor(point.X),
                                    (int)Math.Floor(point.Y));

                res.Add(p);
            }
            return res;
        }

        public static Point[] ToDrawingPoint(this Point2D[] points, IntegerConversionMode mode)
        {
            Point[] res = new Point[points.Length];
            for (int i = 0; i < points.Length; i++)
            {
                res[i] = new Point(points[i].X.ToInteger(mode),
                                   points[i].Y.ToInteger(mode));
            }
            return res;
        }

        public static IList<Point> ToDrawingPoints(this IEnumerable<Point2D> points, IntegerConversionMode mode)
        {
            List<Point> res = new List<Point>();
            foreach (Point2D point in points)
            {
                Point p = new Point(point.X.ToInteger(mode),
                                    point.Y.ToInteger(mode));

                res.Add(p);
            }
            return res;
        }

    }

}
