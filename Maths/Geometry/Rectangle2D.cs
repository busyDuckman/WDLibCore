/*
 * The following code is Copyright 2018 Dr Warren Creemers (busyDuckman)
 * See LICENSE.md for more information.
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace WDToolbox.Maths.Geometry
{
    public class Rectangle2D
    {
        public double X {get; set;}
        public double Y { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }

        public double Right { 
            get { return X + Width; }
            set { Width = value - X; }
        }

        public double Bottom { 
            get { return Y + Height; }
            set { Height = value - Y; }
        }

        public double Left
        {
            get { return X; }
            set { double r = Right; X = value; Right = r; }
        }

        public double Top
        {
            get { return Y; }
            set { double b = Bottom; Y = value; Bottom = b; }
        }

        public Point2D Middle { 
            get { return new Point2D(X + (Width/2.0), Y + (Height/2.0)); }
            set { Point2D off = value - Middle; X = X + off.X; Y = Y + off.Y; }
        }

        public Point2D Location
        {
            get { return new Point2D(X, Y); }
            set { X = value.X; Y = value.Y; }
        }

        /// <summary>
        /// NOTE: the set changes the size of the rectange. Use Location if this is not your intention.
        /// </summary>
        public Point2D TopLeft { 
            get { return new Point2D(X, Y); }
            set { Left = value.X; Top = value.Y; }
        }

        public Point2D TopRight { 
            get { return new Point2D(X+Width, Y); }
            set { Right = value.X; Top = value.Y; }
        }
        
        public Point2D LowerLeft { 
            get { return new Point2D(X, Y+Height); }
            set { Left = value.X; Bottom = value.Y; }
        }

        public Point2D LowerRight { 
            get { return new Point2D(X+Width, Y+Height); }
            set { Right = value.X; Bottom = value.Y; }
        }

        public Rectangle2D(double x, double y, double width, double height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Creates a new Rectangle2D that bounds a given set of points.
        /// Returns null if the list of points is null or empty.
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public static Rectangle2D BoundingBox(IReadOnlyList<Point2D> points)
        {
            if ((points == null) || (points.Count == 0))
            {
                return null;
            }
            double minX = points[0].X, maxX = points[0].X,
                          minY = points[0].Y, maxY = points[0].Y;

            for (int i = 1; i < points.Count; i++)//yes starting at 1, not 0
            {
                double x = points[i].X;
                double y = points[i].Y;

                if (x > maxX) { maxX = x; }
                if (x < minX) { minX = x; }
                if (y > maxY) { maxY = y; }
                if (y < minY) { minY = y; }
            }

            return new Rectangle2D(minX, minY, maxX - minX, maxY - minY);
        }

        /// <summary>
        /// Gets a whole number rectangle which is the floor of the top left corner 
        /// and the ceiling of the lower RightCorner
        /// </summary>
        /// <returns></returns>
        public Rectangle AsSystemRecRoundedOutward()
        {
            int x = (int)Math.Floor(X);
            int y = (int)Math.Floor(Y);
            int x2 = (int)Math.Ceiling(Right);
            int y2 = (int)Math.Ceiling(Bottom);

            return new Rectangle(x, y, x2 - x, y2 - y);
        }


        public static Rectangle2D FromSizeAtOrigin(Size size)
        {
            return new Rectangle2D(0, 0, size.Width, size.Height);
        }

        public static Rectangle2D FromSizeAtOrigin(Point2D size)
        {
            return new Rectangle2D(0, 0, size.X, size.Y);
        }

        public static Rectangle2D EmptyAtOrigin { get { return new Rectangle2D(0, 0, 0, 0); } }

        public static explicit operator Rectangle(Rectangle2D rec)
        {
            return new Rectangle((int)rec.X, (int)rec.Y, (int)rec.Width, (int)rec.Height);
        }

        public static implicit operator Rectangle2D(Rectangle rec)
        {
            return new Rectangle2D(rec.X, rec.Y, rec.Width, rec.Height);
        }

        public bool Contains(Point2D pos)
        {
            return (pos.X >= X) && (pos.Y >= Y) && (pos.X <= Right) && (pos.Y <= Bottom);
        }

        public void Normalise()
        {
            if (Width < 0)
            {
                X = X + Width;
                Width = -Width;
            }

            if (Height < 0)
            {
                Y = Y + Height;
                Height = -Height;
            }
        }


        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 7919;
                hash = hash * 16777619 + X.GetHashCode();
                hash = hash * 16777619 + Y.GetHashCode();
                hash = hash * 16777619 + Width.GetHashCode();
                hash = hash * 16777619 + Height.GetHashCode();
                return hash;
            }
        }

        public override bool Equals(object obj)
        {
            return (obj is Rectangle2D) ? (this == (Rectangle2D)obj) : false;
        }

        public bool Equals(Rectangle2D rec)
        {
            return this == rec;
        }

        public static bool operator ==(Rectangle2D a, Rectangle2D b)
        {
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            return ((a.X == b.X) && (a.Y == b.Y) && (a.Width == b.Width) && (a.Height == b.Height));
        }

        public static bool operator !=(Rectangle2D a, Rectangle2D b)
        {
            return !(a == b);
        }

        public override string ToString()
        {
            return string.Format("pos=({0}, {1}) size=({2}, {3})", Left, Top, Width, Height);
        }
    }
}
