/*
 * The following code is Copyright 2018 Dr Warren Creemers (busyDuckman)
 * See LICENSE.md for more information.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WDToolbox.Maths.Geometry;
using System.Drawing;
using WDToolbox;
using GDIMatrix = System.Drawing.Drawing2D.Matrix;


namespace WDToolbox.Maths.Geometry
{
    /// <summary>
    /// A basic transformation matrix.
    /// For 3x3 homogenous coordinates
    /// Row major [row, column]
    /// This class is "somewhat" thread safe
    /// </summary>
    public sealed class TransMatrix2D : ICloneable, IEquatable<TransMatrix2D>
    {
        //--------------------------------------------------------------------------------------------------
        // Static data and constants
        //--------------------------------------------------------------------------------------------------
        public static TransMatrix2D Identity { get { return TransMatrix2D.FromScale(1); } }

        private static Point2D NoScale = new Point2D(1, 1);

        private const int A1 = 0;
        private const int A2 = 1;
        private const int A3 = 2;
        private const int B1 = 3;
        private const int B2 = 4;
        private const int B3 = 5;
        private const int C1 = 6;
        private const int C2 = 7;
        private const int C3 = 8;

        //--------------------------------------------------------------------------------------------------
        // Instance data
        //--------------------------------------------------------------------------------------------------
        private double[] data;
        /* Point2D scale;
         Point2D translate;
         double clockwiseRotation; //in radians
         Point2D rotationPos;*/

        //--------------------------------------------------------------------------------------------------
        // Transient data
        //--------------------------------------------------------------------------------------------------
        [NonSerialized]
        private object dataLock = new object();

        //--------------------------------------------------------------------------------------------------
        // Accessors
        //--------------------------------------------------------------------------------------------------
        #region accessors

        /* public Point2D Scale
         {
             get { return scale; }
             set { scale = value; rebuildMatrix(); }
         }

         public Point2D Translate
         {
             get { return translate; }
             set { translate = value; rebuildMatrix(); }
         }
         /// <summary>
         /// Clockwise rotation in radians
         /// </summary>
         public double ClockwiseRotation
         {
             get { return clockwiseRotation; }
             set { clockwiseRotation = value; rebuildMatrix(); }
         }

         public Point2D RotationPos
         {
             get { return rotationPos; }
             set { rotationPos = value; rebuildMatrix(); }
         }
         * */
        #endregion


        //--------------------------------------------------------------------------------------------------
        // Constructors and factory methods
        //--------------------------------------------------------------------------------------------------
        private TransMatrix2D()
        {
            data = new double[9];
            Point2D scale = new Point2D(1, 1);
            Point2D translate = Point2D.Origin;
            double clockwiseRotation = 0;
            Point2D rotationPos = Point2D.Origin;
            rebuildMatrix(scale, translate, clockwiseRotation, rotationPos);
        }

        protected TransMatrix2D(TransMatrix2D another)
        {
#if NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP2_0
            this.data = new double[another.data.Length];
            for(int i=0; i<data.Length; i++) {
                this.data[i] = another.data[i];
            }
#else
            this.data = another.data.DeepCopyValues();
#endif
        }
        public object Clone() { return new TransMatrix2D(this); }


        public static TransMatrix2D FromGDIPlusMatrix(GDIMatrix matrix)
        {
            TransMatrix2D m = new TransMatrix2D();

            //m11, m12, m21, m22, dx, and dy
            m[0, 0] = matrix.Elements[0];
            m[1, 0] = matrix.Elements[1];
            m[0, 1] = matrix.Elements[2];
            m[1, 1] = matrix.Elements[3];
            m[0, 2] = matrix.Elements[4];
            m[1, 2] = matrix.Elements[5];
            

            return m;
        }

        public GDIMatrix ToGDIPlusMatrix()
        {
            return new GDIMatrix((float)this[0, 0],
                      (float)this[1, 0],
                      (float)this[0, 1],
                      (float)this[1, 1],
                      (float)this[0, 2],
                      (float)this[1, 2]);
        }

        /// <summary>
        /// A translation about the origin
        /// </summary>
        /// <param name="clockWiseRotation">In Radians</param>
        /// <returns>A new transformation matrix.</returns>
        public static TransMatrix2D FromRotation(double clockWiseRotation, Point2D point)
        {
            TransMatrix2D t = new TransMatrix2D();
            //t.clockwiseRotation = clockWiseRotation;
            //t.rotationPos = point;
            t.rebuildMatrix(NoScale ,Point2D.Origin,  clockWiseRotation, point);
            return t;
        }

        public static TransMatrix2D FromTranslation(double x, double y)
        {
            TransMatrix2D t = new TransMatrix2D();
            //t.translate = new Point2D(x, y);
            t.rebuildMatrix(NoScale, new Point2D(x, y), 0, Point2D.Origin);
            return t;
        }
        
        public static TransMatrix2D FromScale(double scale)
        {
            return FromScale(scale, scale); 
        }

        public static TransMatrix2D FromScale(double sx, double sy)
        {
            TransMatrix2D t = new TransMatrix2D();
            //t.scale = new Point2D(sx, sy);
            t.rebuildMatrix(new Point2D(sx, sy), Point2D.Origin, 0, Point2D.Origin);
            return t;
        }


        /*public static TransMatrix2D FromTRS(double transformX, 
                                            double transformY, 
                                            double clockwiseRotation, 
                                            double scaleX,
                                            double scaleY)
        {
            double cos = Math.Cos(clockwiseRotation);
            double sin = Math.Sin(clockwiseRotation);
            TransMatrix2D t = new TransMatrix2D();
            t[0, 0] = scaleX * cos;
            t[0, 1] = -scaleY * sin;
            t[0, 1] = transformX;
            t[1, 0] = scaleX * sin;
            t[1, 1] = scaleY * cos;
            t[1, 2] = transformY;
            return t;
        }*/

        public static TransMatrix2D FromTRS(Point2D transform,
                                            double clockwiseRotation,
                                            Point2D rotationPos,
                                            Point2D scale)
        {
            TransMatrix2D t = new TransMatrix2D();
            /*t.scale = scale;
            t.translate = transform;
            t.clockwiseRotation = clockwiseRotation;
            t.rotationPos = rotationPos;
            t.rebuildMatrix();*/
            t.rebuildMatrix(scale, transform, clockwiseRotation, rotationPos);
            return t;
        }

        //--------------------------------------------------------------------------------------------------
        // Manipuulation
        //--------------------------------------------------------------------------------------------------
        public double this[int r, int c]
        {
            get { return data[(r * 3) + c]; }
            private set { data[(r * 3) + c] = value; }
        }

        //--------------------------------------------------------------------------------------------------
        // Object
        //--------------------------------------------------------------------------------------------------
        public override string ToString()
        {
            return "[" + data.ListAll(", ") + "]";
        }

        public override int GetHashCode()
        {
            return data.GetQuickashCode();
        }

        //--------------------------------------------------------------------------------------------------
        // Transforms
        //--------------------------------------------------------------------------------------------------
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Point2D _transform(Point point)
        {
            double a = point.X;
            double b = point.Y;
            double c = 1.0;

            double x = (a * data[A1]) + (b * data[A2]) + (c * data[A3]);
            double y = (a * data[B1]) + (b * data[B2]) + (c * data[B3]);
            double s = (a * data[C1]) + (b * data[C2]) + (c * data[C3]);
            return new Point2D(s * x, s * y);
        }

        public void Transform(ref double x, ref double y)
        {
            double a = x;
            double b = y;
            double c = 1.0;

            x = (a * data[A1]) + (b * data[A2]) + (c * data[A3]);
            y = (a * data[B1]) + (b * data[B2]) + (c * data[B3]);
            double s = (a * data[C1]) + (b * data[C2]) + (c * data[C3]);
            x *= s;
            y *= s;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Point2D _transform(Point2D point)
        {
            double a = point.X;
            double b = point.Y;
            double c = 1.0;

            double x = (a * data[A1]) + (b * data[A2]) + (c * data[A3]);
            double y = (a * data[B1]) + (b * data[B2]) + (c * data[B3]);
            double s = (a * data[C1]) + (b * data[C2]) + (c * data[C3]);
            return new Point2D(s*x, s*y);

            /*double x2 = 0, y2 = 0, t = this[2,2];

            x2 = this[0, 0] * x + this[0, 1] * y + this[0, 2];
            y2 = this[1, 0] * x + this[1, 1] * y + this[1, 2];
            x2 *= t;
            y2 *= t;
            return new Point2D(x2, y2);*/
        }

        public Point2D Transform(Point2D point)
        {
            lock (dataLock)
            {
                return _transform(point);
            }
        }

        public Point2D Transform(Point point)
        {
            lock (dataLock)
            {
                return _transform(point);
            }
        }

        public static Point2D operator* (TransMatrix2D t, Point2D point)
        {
            return t._transform(point); 
        }

        public List<Point2D> Transform(IList<Point2D> points)
        {
            lock (dataLock)
            {
                List<Point2D> res = new List<Point2D>();
                foreach (Point2D point in points)
                {
                    res.Add(_transform(point));
                }
            return res;
            }
        }

        public Point2D[] Transform(Point2D[] points)
        {
            lock (dataLock)
            {
                Point2D[] res = new Point2D[points.Length];
                for(int i=0; i<points.Length; i++)
                {
                    res[i] = _transform(points[i]);
                }
                return res;
            }
        }

        public IList<Point2D> Transform(IList<Point> points)
        {
            lock (dataLock)
            {
                List<Point2D> res = new List<Point2D>();
                foreach (Point point in points)
                {
                    res.Add(_transform(point));
                }
                return res;
            }
        }

        public Point2D[] Transform(Point[] points)
        {
            lock (dataLock)
            {
                Point2D[] res = new Point2D[points.Length];
                for(int i=0; i<points.Length; i++)
                {
                    res[i] = _transform(points[i]);
                }
                return res;
            }
        }

        /// <summary>
        /// Transforms the corner points of a rectange
        /// </summary>
        /// <param name="rec"></param>
        /// <returns> Corner points (topLeft, topRight, lowerRight, lowerLeft)</returns>
        public TransformedRectangle2D Transform(Rectangle2D rec)
        {
            List<Point2D> points = new List<Point2D>() { rec.TopLeft, 
                                                         rec.TopRight, 
                                                         rec.LowerRight,
                                                         rec.LowerLeft};

            points = Transform(points);

            return new TransformedRectangle2D(points[0], points[1], points[2], points[3]);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Point2D _invTransform(Point2D point)
        {
            //return Point2D.Origen;////////////////////////////todo
            return Inverse().Transform(point);
        }

        public Point2D InvTransform(Point2D point)
        {
            lock (dataLock)
            {
                return _invTransform(point);
            }
        }

        public List<Point2D> InvTransform(IList<Point2D> points)
        {
            lock (dataLock)
            {
                List<Point2D> res = new List<Point2D>();
                foreach (Point2D point in points)
                {
                    res.Add(_invTransform(point));
                }
                return res;
            }
        }

        
        //----------------------------------------------------------------------------------------
        // private functions
        //----------------------------------------------------------------------------------------
       // private void rebuildMatrix()
        private void rebuildMatrix(Point2D scale, Point2D translate, double clockwiseRotation, Point2D rotationPos)
        {

            //based on page 195 of Hern & Baker 2nd Ed
            lock (dataLock)
            {
                double cos = Math.Cos(clockwiseRotation);
                double sin = Math.Sin(clockwiseRotation);

                double sxCos = scale.X * cos;
                double syCos = scale.Y * cos;
                double sxSin = scale.X * sin;
                double sySin = scale.Y * sin;

                this[0, 0] = sxCos;
                this[0, 1] = -sySin;
                this[0, 2] = rotationPos.X * (1.0 - sxCos) +
                             rotationPos.Y * sySin +
                             translate.X;

                this[1, 0] = sxSin;
                this[1, 1] = syCos;
                this[1, 2] = rotationPos.Y * (1.0 - syCos) -
                             rotationPos.X * sxSin + 
                             translate.Y;

                this[2, 0] = 0;
                this[2, 1] = 0;
                /*this[0, 0] = sxCos;
                this[1, 0] = -sySin;
                this[0, 1] = rotationPos.X * (1.0 - sxCos) +
                             rotationPos.Y * sySin +
                             translate.X;

                this[1, 0] = sxSin;
                this[1, 1] = syCos;
                this[1, 2] = rotationPos.Y * (1.0 - syCos) -
                             rotationPos.X * sxSin +
                             translate.Y;

                this[2, 0] = 0;
                this[2, 1] = 0;*/
                this[2, 2] = 1.0;
            }
        }

        public TransMatrix2D Inverse()
        {
            //return TransMatrix2D.FromTRS(-Translate, -clockwiseRotation, RotationPos, new Point2D(1.0/Scale.X, 1.0/scale.Y));
            double tmp = 1.0 / Determinate();
            TransMatrix2D inv = new TransMatrix2D();
            SCALE_ADJOINT_3X3(inv.data, tmp, data);
            return inv;
        }

        //port of glut implementation, cause I can't be bothered typing it all out.
        private static void SCALE_ADJOINT_3X3(double[] a, double scale, double[] m)
        {                               
            a[A1] = (scale) * (m[B2] * m[C3] - m[B3] * m[C2]);
            a[B1] = (scale) * (m[B3] * m[C1] - m[B1] * m[C3]);
            a[C1] = (scale) * (m[B1] * m[C2] - m[B2] * m[C1]);

            a[A2] = (scale) * (m[A3] * m[C2] - m[A2] * m[C3]);
            a[B2] = (scale) * (m[A1] * m[C3] - m[A3] * m[C1]);
            a[C2] = (scale) * (m[A2] * m[C1] - m[A1] * m[C2]);

            a[A3] = (scale) * (m[A2] * m[B3] - m[A3] * m[B2]); 
            a[B3] = (scale) * (m[A3] * m[B1] - m[A1] * m[B3]); 
            a[C3] = (scale) * (m[A1] * m[B2] - m[A2] * m[B1]); 
        }

        public double Determinate()
        {
            return (data[A1] * data[B2] * data[C3]) -
                    (data[A2] * data[B3] * data[C2]) -
                    (data[A2] * data[B1] * data[C3]) +
                    (data[A2] * data[B3] * data[C1]) +
                    (data[A3] * data[B1] * data[C2]) -
                    (data[A3] * data[B2] * data[C1]);
        }

        private static double determinate2(double a11, double a12, double a21, double a22)
        {
            return (a11 * a22) - (a12 * a21);
        }

        //----------------------------------------------------------------------------------------
        // IEquatable<TransMatrix2D>
        //----------------------------------------------------------------------------------------
        public override bool Equals(object obj)
        {
            return (obj is Point2D) ? (this == (TransMatrix2D)obj) : false;
        }

        public bool Equals(TransMatrix2D other)
        {
            return this == other;
        }

        public static bool operator ==(TransMatrix2D a, TransMatrix2D b)
        {
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            return a.data.SequenceEqual(b.data);
        }

        public static bool operator !=(TransMatrix2D a, TransMatrix2D b)
        {
            return !(a == b);
        }
    }
}
