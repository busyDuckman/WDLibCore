/*
 * The following code is Copyright 2018 Dr Warren Creemers (busyDuckman)
 * See LICENSE.md for more information.
 */

//TODO: This looks like an unfinished version. Find the finished version, or finish the code. 

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WDToolbox.Maths.Geometry.Lines
{
    public struct LineToPointInfo
    {
        public double disatance;
        public enum SideEnum { left = -1, on = 0, right = 1 };
        public double distanceAlongLinetoNearestPointOnLine;
        public int relativePos;
        public Point2D pointOfIntersection;
        public SideEnum side;
    }

    public struct LineLineIntersectInfo
    {
        public bool linesAreIdentical;
        public bool linesIntersect;
        public Line ShortestLineFromLineAtoLineB;
    }

    //--------------------------------------------------------------------------------------------------
    // Line
    //--------------------------------------------------------------------------------------------------
    public sealed class Line : IOpenPath, IEquatable<Line>, ICloneable, IReadOnlyList<Point2D>
    {
        //--------------------------------------------------------------------------------------------------
        // Accessors
        //--------------------------------------------------------------------------------------------------
        //public bool Closed {  get { return false; } }

        public Point2D End { get; private set; }

        public double PathLength { get { return Length(); } }

        public Point2D Start { get; private set; }

        //--------------------------------------------------------------------------------------------------
        // Constructors and factory methods.
        //--------------------------------------------------------------------------------------------------
        public Line(Point2D start, Point2D end)
        {
            this.Start = start;
            this.End = end;
        }

        private Line(Line another)
        {
            Start = another.Start;
            End = another.End;
        }

        //--------------------------------------------------------------------------------------------------
        // IPath
        //--------------------------------------------------------------------------------------------------
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

        public List<IOpenPath> Split(double percentPos)
        {
            throw new NotImplementedException();
        }

        public PolyLine ToPolyLine(double lineLenOnCurves)
        {
            throw new NotImplementedException();
        }

        public IOpenPath Reverse()
        {
            return new Line(End, Start);
        }


        //--------------------------------------------------------------------------------------------------
        // ILine
        //--------------------------------------------------------------------------------------------------
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point2D VecFromStartToEnd()
        {
            return End - Start;
        }

        public Point2D UnitVecFromStartToEnd()
        {
            return VecFromStartToEnd() * (1.0 / Length());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double LengthSquared()
        {
            Point2D v = VecFromStartToEnd();
            return (v.X * v.X) + (v.Y * v.Y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double Length()
        {
            return Math.Sqrt(LengthSquared());
        }

        public bool IsVertical
        {
            get { return Start.X == End.X; }
        }

        public bool IsHorizontal
        {
            get { return Start.Y == End.Y; }
        }

        public bool EqualsIgnoreDirection(ILine other)
        {
            if (System.Object.ReferenceEquals(this, other))
            {
                return true;
            }

            if (((object)this == null) || ((object)other == null))
            {
                return false;
            }

            return ((this.Start == other.Start) && (this.End == other.End)) ||
                   ((this.Start == other.End) && (this.End == other.Start));
        }

        public Rectangle2D BoundingBox { get { return Rectangle2D.BoundingBox(this); } }


        //--------------------------------------------------------------------------------------------------
        // Object
        //--------------------------------------------------------------------------------------------------
        public override int GetHashCode()
        {
            return Start.GetHashCode() + End.GetHashCode();
        }

        /// <summary>
        /// Get string representation of the class.
        /// </summary>
        /// <returns>Returns string, which contains values of the like in readable form.</returns>
        public override string ToString()
        {
            return string.Format("Line from {0} to {1}", Start, End);
        }

        //--------------------------------------------------------------------------------------------------
        // IEquatable<Line>
        //--------------------------------------------------------------------------------------------------
        public override bool Equals(object obj)
        {
            return (obj is Line) ? (this == (Line)obj) : false;
        }

        public bool Equals(Line other)
        {
            return this == other;
        }

        public static bool operator ==(Line a, Line b)
        {
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            return ((a.Start == b.Start) && (a.End == b.End));
        }

        public static bool operator !=(Line a, Line b)
        {
            return !(a == b);
        }

        //--------------------------------------------------------------------------------------------------
        // ICloneable
        //--------------------------------------------------------------------------------------------------
        public object Clone()
        {
            return new Line(this);
        }

        //--------------------------------------------------------------------------------------------------
        // IReadOnlyList<Point2D>
        //--------------------------------------------------------------------------------------------------
        public IEnumerator<Point2D> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Point2D this[int index]
        {
            get
            {
                throw new NotImplementedException();
            }
        }


    }
}
