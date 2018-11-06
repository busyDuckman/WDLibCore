/*
 * The following code is Copyright 2018 Dr Warren Creemers (busyDuckman)
 * See LICENSE.md for more information.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WDToolbox.Maths.Geometry;
using WDToolbox.Rendering.FormattedText;
using GDIMatrix = System.Drawing.Drawing2D.Matrix;
using PointF = System.Drawing.PointF;

namespace WDToolbox.Rendering
{
    public enum RendererLineCapStyle
    {
        Flat, Round
    }

    public enum AngleTypes
    {
        Degrees = 0, Radians
    }

    public enum HorizontalAlignment {Left=0, Centre, Right};
    public enum VerticalAlignment {Above=0, Centre, Below};

    /// <summary>
    /// An Interface to common rendering API's
    /// Importantly this reduces dependencies on things like GDI+
    /// It is expected the IRenderer will be in high quality mode when created.
    /// </summary>
    /// <remarks>
    /// IDisposable is implemented, and should ONLY call the close method.
    /// </remarks>
    public interface IRenderer : IDisposable
    {
        /// <summary>
        /// Will flush all output and free cached resourcves.
        /// May finalise the rendering context of the API. The IRenderer may prevent this for whatever reason (normally when the graphics context relates to a screen, not a file or software device).
        /// Can be called multiple times without issue.
        /// </summary>
        void Close();
        void Flush();
        Bitmap RenderTargetAsGDIBitmap();

        /// <summary>
        /// Quality is either High or low. see SetHighQuality(..).
        ///	    Low quality should not cause any interpolation of pixels when bitmaps are rendered.
        ///	    Low quality will disable all antialiasing.
        ///	    SetHighQuality(..) can be called at any time, and results aplicable only to items rendered after the setting is changed.
        ///	    High quality sets all features of a rendering API to their highest setting.
        /// </summary>
        bool HighQuality { get; set; }

        AngleTypes AngleType { get; set; }
        RendererLineCapStyle LineEndCapStyle { get; set; }
        TransMatrix2D Transform { get; set; }
        /// <summary>
        /// Width in pixels, -1 if not aplicable
        /// </summary>
        int Width  {get;}

        /// <summary>
        /// Width in pixels, -1 if not aplicable
        /// </summary>
        int Height {get;}

        /// <summary>
        /// True if the API supports being altered by threads other than the one that created the context 
        /// </summary>
        bool SupportsMultiThreading { get; }

        void DrawImage(Image image, Point[] destPoints);
        void DrawImage(Image image, int x, int y);
        void DrawImage(Image image, Point[] destPoints, Rectangle srcRect);
        void DrawImage(Image image, Rectangle destRect, Rectangle srcRect);
        void DrawImage(Image image, int x, int y, int width, int height);
        void DrawImage(Image image, int x, int y, Rectangle srcRect);

        void DrawString(Color col, string s, Font font, int x, int y);

        void PutPixel(Color col, int x, int y);

        void DrawArc(Color col, int lineSize, int x, int y, int width, int height, double startAngle, double sweepAngle);
        void DrawBeziers(Color col, int lineSize, Point[] points);
        void DrawLine(Color col, int lineSize, int x1, int y1, int x2, int y2);
        void DrawPolyLine(Color col, int lineSize, Point[] points);
        void DrawCurve(Color col, int lineSize, Point[] points);

        void DrawClosedCurve(Color col, int lineSize, Point[] points);
        void DrawEllipse(Color col, int lineSize, int x, int y, int width, int height);
        void DrawCircle(Color col, int lineSize, int cx, int cy, int radius);
        void DrawPie(Color col, int lineSize, int x, int y, int width, int height, double startAngle, double sweepAngle);
        void DrawPolygon(Color col, int lineSize, Point[] points);
        void DrawRectangle(Color col, int lineSize, int x, int y, int width, int height);

        void FillClosedCurve(Color fillCol, Point[] points);
        void FillEllipse(Color fillCol, int x, int y, int width, int height);
        void FillCircle(Color fillCol, int cx, int cy, int radius);
        void FillPie(Color fillCol, int x, int y, int width, int height, double startAngle, double sweepAngle);
        void FillPolygon(Color fillCol, Point[] points);
        void FillRectangle(Color fillCol, int x, int y, int width, int height);

        void Clear(Color c);

        /// <summary>
        /// Support for a reduced colour palate. 
        /// </summary>
        /// <param name="color">The desired color.</param>
        /// <returns>The best match color, supported by the output device.</returns>
        Color GetNearestColor(Color color);

        void ResetTransform();

        IRendererSettings GetSettings();
        void ApplySettings(IRendererSettings settings);

        Size MeasureString(string s, Font font);
    }

    public interface IRendererSettings
    {
        AngleTypes AngleType { get; }
        RendererLineCapStyle LineEndCapStyle { get;}
        bool HighQuality {get;}
        TransMatrix2D Transform {get;}
    }

    public class RendererSettings : IRendererSettings
    {
        public AngleTypes AngleType { get; protected set; }
        public RendererLineCapStyle LineEndCapStyle { get; protected set; }
        public bool HighQuality { get; protected set; }
        public TransMatrix2D Transform { get; protected set; }

        public RendererSettings(IRenderer r)
        {
            AngleType = r.AngleType;
            LineEndCapStyle = r.LineEndCapStyle;
            HighQuality = r.HighQuality;
            Transform = r.Transform;
        }

        public virtual void Apply(IRenderer r)
        {
            r.AngleType = AngleType;
            r.LineEndCapStyle = LineEndCapStyle;
            r.HighQuality = HighQuality;
            r.Transform = Transform;
        }
    }

    /// <summary>
    /// Function overloads for IRenderer
    /// </summary>
    public static class IRendererExtension
    {

        /// <summary>
        /// IRenderer was updated to use acessors, an extension for the old 
        /// set method reduces the amount of updates required.
        /// </summary>
        public static void SetHighQuality(this IRenderer renderer, bool enable)
        {
            renderer.HighQuality = enable;
        }

        /// <summary>
        /// IRenderer was updated to use acessors, an extension for the old 
        /// set method reduces the amount of updates required.
        /// </summary>
        public static void SetTransform(this IRenderer renderer, TransMatrix2D transform)
        {
            renderer.Transform = transform;
        }

        public static RenderingContext GetContext(this IRenderer renderer)
        {
            return new RenderingContext(renderer);
        }


        public static void FillRectangleOverCanvas(this IRenderer renderer, Color c)
        {
            if((renderer.Width > 0) && (renderer.Height > 0))
            {
                Rectangle r = new Rectangle(0, 0, renderer.Width, renderer.Height);
                renderer.FillRectangle(c, r);
            }
        }

        public static void DrawImage(this IRenderer renderer, Image image, Point point)
        {
            renderer.DrawImage(image, point.X, point.Y);
        }

        public static void DrawImage(this IRenderer renderer, Image image, Rectangle rect)
        {
            renderer.DrawImage(image, rect.X, rect.Y, rect.Width, rect.Height);
        }

        public static void DrawLine(this IRenderer renderer, Color col, int lineSize, Point pt1, Point pt2)
        {
            renderer.DrawLine(col, lineSize, pt1.X, pt1.Y, pt2.X, pt2.Y);
        }

        public static void DrawLine(this IRenderer renderer, Color col, int lineSize, Point2D pt1, Point2D pt2)
        {
            renderer.DrawLine(col, lineSize, (int)pt1.X, (int)pt1.Y, (int)pt2.X, (int)pt2.Y);
        }

        public static void DrawArc(this IRenderer renderer, Color col, int lineSize, Rectangle rect, double startAngle, double sweepAngle)
        {
            renderer.DrawArc(col, lineSize, rect.X, rect.Y, rect.Width, rect.Height,
                             startAngle, sweepAngle);
        }
        public static void DrawBezier(this IRenderer renderer, Color col, int lineSize, Point pt1, Point pt2, Point pt3, Point pt4)
        {
            renderer.DrawBeziers(col, lineSize, new Point[] { pt1, pt2, pt3, pt4 });
        }

        public static void DrawString(this IRenderer renderer, Color col, string s, Font font, Point point)
        {
            renderer.DrawString(col, s, font, point.X, point.Y);
        }

        public static void DrawStringAligned(this IRenderer renderer, Color col, string s, Font font, Point point, 
                                             HorizontalAlignment hAlign, VerticalAlignment vAlign = VerticalAlignment.Above)
        {
            DrawStringAligned(renderer, col, s, font, point.X, point.Y, hAlign, vAlign);
        }

        public static void DrawStringAligned(this IRenderer renderer, Color col, string s, Font font, int x, int y,
                                             HorizontalAlignment hAlign, VerticalAlignment vAlign = VerticalAlignment.Above)
        {
            Size size = renderer.MeasureString(s, font);
            
            int adjust = (hAlign == HorizontalAlignment.Centre) ? size.Width/2 : size.Width;
            int aX = (hAlign == HorizontalAlignment.Left) ? x : x - adjust;
            adjust = (vAlign == VerticalAlignment.Centre) ? size.Height/2 : size.Height;
            int aY = (vAlign == VerticalAlignment.Above) ? x : x - adjust;

            renderer.DrawString(col, s, font, aX, aY);
        }


        public static void DrawString(this IRenderer renderer, TextFormat format, string s, Point point)
        {
            renderer.DrawString(format, s, point.X, point.Y);
        }

        public static void DrawString(this IRenderer renderer, TextFormat format, string s, int x, int y)
        {
            format.render(renderer, s, x, y);
        }

        public static void DrawEllipse(this IRenderer renderer, Color col, int lineSize, Rectangle rect)
        {
            renderer.DrawEllipse(col, lineSize, rect.X, rect.Y, rect.Width, rect.Height);
        }
        public static void DrawPie(this IRenderer renderer, Color col, int lineSize, Rectangle rect, double startAngle, double sweepAngle)
        {
            renderer.DrawPie(col, lineSize, rect.X, rect.Y, rect.Width, rect.Height, startAngle, sweepAngle);
        }
        public static void DrawRectangle(this IRenderer renderer, Color col, int lineSize, Rectangle rect)
        {
            renderer.DrawRectangle(col, lineSize, rect.X, rect.Y, rect.Width, rect.Height);
        }

        public static void FillEllipse(this IRenderer renderer, Color fillCol, Rectangle rect)
        {
            renderer.FillEllipse(fillCol, rect.X, rect.Y, rect.Width, rect.Height);
        }

        public static void FillPie(this IRenderer renderer, Color fillCol, Rectangle rect, double startAngle, double sweepAngle)
        {
            renderer.FillPie(fillCol, rect.X, rect.Y, rect.Width, rect.Height, startAngle, sweepAngle);
        }

        public static void FillRectangle(this IRenderer renderer, Color fillCol, Rectangle rect)
        {
            renderer.FillRectangle(fillCol, rect.X, rect.Y, rect.Width, rect.Height);
        }

        public static void FillTransformedRectangle(this IRenderer renderer, Color fillCol, TransformedRectangle2D rect)
        {
            if (rect.IsAlignedOrthogonally)
            {
                //this should be quicker
                renderer.FillRectangle(fillCol, rect.BoundingBox.AsSystemRecRoundedOutward());
            }
            else
            {
                renderer.FillPolygon(fillCol, rect.Points.ToDrawingPoints().ToArray());
            }
        }


        //--------------------------------------------------------------------------------------------------------------
        // Performance benchmarking
        //--------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Just to mak sure the code is compiled etc
        /// </summary>
        /// <returns>A hash code that just prevents a too-smart compiler for removing redundent code. </returns>
        public static double PerformanceTestGDIConversion(int numberOfTests)
        {
            //TODO: HOW THE HELL DOES THIS TAKE 2MS TO COMPLETE???
            double[] numbers = new double[] {5, 8, 232.5, 123321, 12, -18};
            double hash=0;

            for (int i = 1; i < numberOfTests; i++)
            {
                //I want to know that the transformation matrix is valid in GDI plus conversions.
                TransMatrix2D t1 = TransMatrix2D.FromTRS(
                    new Point2D(22.25, 31.75),
                    numbers[i % numbers.Length],
                    new Point2D(51, 37),
                    new Point2D(0.5, 0.25));

                GDIMatrix g1 = t1.ToGDIPlusMatrix();
                TransMatrix2D t2 = TransMatrix2D.FromGDIPlusMatrix(g1);

                Point2D test = new Point2D(7.1, numbers[(i+2) % numbers.Length]);
                Point2D p1 = t1.Transform(test);

                var temp = new PointF[] { test.AsPointF() };
                g1.TransformPoints(temp);
                Point2D p2 = new Point2D(temp[0]);

                Point2D p3 = t2.Transform(test);

                hash += p1.X + p2.X - p3.X;
            }

            return hash;
        }

        //--------------------------------------------------------------------------------------------------------------
        // Transformable Operations
        //--------------------------------------------------------------------------------------------------------------
        /*
        public static void DrawImage(this IView2D renderer, TransMatrix2D trans, Image image, Point[] destPoints)
        {
            renderer.DrawImage(image, trans.Transform(destPoints).Floor());
        }

        public static void DrawImage(this IRenderer renderer, TransMatrix2D trans, Image image, int x, int y)
        {
            double xt=x, yt=y;
            trans.Transform(ref xt, ref yt);
            renderer(x, y);
        }
        public static void DrawImage(this IRenderer renderer, TransMatrix2D trans, Image image, Point[] destPoints, Rectangle srcRect)
        {
            double xt=x, yt=y;
            trans.Transform(ref xt, ref yt);
            renderer(x, y);
        }

        public static void DrawImage(this IRenderer renderer, TransMatrix2D trans, Image image, Rectangle destRect, Rectangle srcRect)
        {
            double xt=x, yt=y;
            trans.Transform(ref xt, ref yt);
            renderer(x, y);
        }

        public static void DrawImage(this IRenderer renderer, TransMatrix2D trans, Image image, int x, int y, int width, int height)
        {
            double xt=x, yt=y;
            trans.Transform(ref xt, ref yt);
            renderer(x, y);
        }
        public static void DrawImage(this IRenderer renderer, TransMatrix2D trans, Image image, int x, int y, Rectangle srcRect)
        {
            double xt=x, yt=y;
            trans.Transform(ref xt, ref yt);
            renderer(x, y);
        }

        public static void DrawString(this IRenderer renderer, TransMatrix2D trans, Color col, string s, Font font, int x, int y)
        {
            double xt=x, yt=y;
            trans.Transform(ref xt, ref yt);
            renderer(x, y);
        }

        public static void PutPixel(this IRenderer renderer, TransMatrix2D trans, Color col, int x, int y)
        {
            double xt=x, yt=y;
            trans.Transform(ref xt, ref yt);
            renderer(x, y);
        }

        public static void DrawArc(this IRenderer renderer, TransMatrix2D trans, Color col, int lineSize, int x, int y, int width, int height, double startAngle, double sweepAngle)
        {
            double xt=x, yt=y;
            trans.Transform(ref xt, ref yt);
            renderer(x, y);
        }

        public static void DrawBeziers(this IRenderer renderer, TransMatrix2D trans, Color col, int lineSize, Point[] points)
        {
            double xt=x, yt=y;
            trans.Transform(ref xt, ref yt);
            renderer(x, y);
        }

        public static void DrawLine(this IRenderer renderer, TransMatrix2D trans, Color col, int lineSize, int x1, int y1, int x2, int y2)
        {
            double xt=x, yt=y;
            trans.Transform(ref xt, ref yt);
            renderer(x, y);
        }
        public static void DrawPolyLine(this IRenderer renderer, TransMatrix2D trans, Color col, int lineSize, Point[] points)
        {
            double xt=x, yt=y;
            trans.Transform(ref xt, ref yt);
            renderer(x, y);
        }

        public static void DrawCurve(this IRenderer renderer, TransMatrix2D trans, Color col, int lineSize, Point[] points)
        {
            double xt=x, yt=y;
            trans.Transform(ref xt, ref yt);
            renderer(x, y);
        }

        public static void DrawClosedCurve(this IRenderer renderer, TransMatrix2D trans, Color col, int lineSize, Point[] points)
        {
            double xt=x, yt=y;
            trans.Transform(ref xt, ref yt);
            renderer(x, y);
        }

        public static void DrawEllipse(this IRenderer renderer, TransMatrix2D trans, Color col, int lineSize, int x, int y, int width, int height)
        {
            double xt=x, yt=y;
            trans.Transform(ref xt, ref yt);
            renderer(x, y);
        }

        public static void DrawCircle(this IRenderer renderer, TransMatrix2D trans, Color col, int lineSize, int cx, int cy, int radius)
        {
            double xt=x, yt=y;
            trans.Transform(ref xt, ref yt);
            renderer(x, y);
        }

        public static void DrawPie(this IRenderer renderer, TransMatrix2D trans, Color col, int lineSize, int x, int y, int width, int height, double startAngle, double sweepAngle)
        {
            double xt=x, yt=y;
            trans.Transform(ref xt, ref yt);
            renderer(x, y);
        }

        public static void DrawPolygon(this IRenderer renderer, TransMatrix2D trans, Color col, int lineSize, Point[] points)
        {
            double xt=x, yt=y;
            trans.Transform(ref xt, ref yt);
            renderer(x, y);
        }

        public static void DrawRectangle(this IRenderer renderer, TransMatrix2D trans, Color col, int lineSize, int x, int y, int width, int height)
        {
            double xt=x, yt=y;
            trans.Transform(ref xt, ref yt);
            renderer(x, y);
        }

        public static void FillClosedCurve(this IRenderer renderer, TransMatrix2D trans, Color fillCol, Point[] points)
        {
            double xt=x, yt=y;
            trans.Transform(ref xt, ref yt);
            renderer(x, y);
        }

        public static void FillEllipse(this IRenderer renderer, TransMatrix2D trans, Color fillCol, int x, int y, int width, int height)
        {
            double xt=x, yt=y;
            trans.Transform(ref xt, ref yt);
            renderer(x, y);
        }

        public static void FillCircle(this IRenderer renderer, TransMatrix2D trans, Color fillCol, int cx, int cy, int radius)
        {
            double xt=x, yt=y;
            trans.Transform(ref xt, ref yt);
            renderer(x, y);
        }

        public static void FillPie(this IRenderer renderer, TransMatrix2D trans, Color fillCol, int x, int y, int width, int height, double startAngle, double sweepAngle)
        {
            double xt=x, yt=y;
            trans.Transform(ref xt, ref yt);
            renderer(x, y);
        }

        public static void FillPolygon(this IRenderer renderer, TransMatrix2D trans, Color fillCol, Point[] points)
        {
            double xt=x, yt=y;
            trans.Transform(ref xt, ref yt);
            renderer(x, y);
        }

        public static void FillRectangle(this IRenderer renderer, TransMatrix2D trans, Color fillCol, int x, int y, int width, int height)
        {
            double xt=x, yt=y;
            trans.Transform(ref xt, ref yt);
            renderer(x, y);
        }
        */
    }
    
}
