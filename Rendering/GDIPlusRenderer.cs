/*
 * The following code is Copyright 2018 Dr Warren Creemers (busyDuckman)
 * See LICENSE.md for more information.
 */
//#define PROTECT_INNER_IMAGE

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WDToolbox.AplicationFramework;
using WDToolbox.Maths.Geometry;
using WDToolbox.Maths.Trigonometry;

namespace WDToolbox.Rendering
{
    public sealed class GDIPlusRenderer : IRenderer
    {
        private const int MaxXBound = 1073741951;  //source: some dude on the internet made a loop to find these
        private const int MinXBound = -1073741760;
        private const int MaxYBound = 1073741951;
        private const int MinYBound = -1073741760;

        Graphics graphics;
        Bitmap bitmap; //this may be null
        RendererLineCapStyle capStyle;

        public AngleTypes AngleType { get; set; }

        private double toDegrees(double angle)
        {
            return (AngleType == AngleTypes.Degrees) ? angle : (angle * Trig.Rad2Deg);
        }

        private bool highQuality;

        /// <summary>
        /// True if this GDIPlusRenderer was created using GDIPlusRenderer(Graphics g)
        /// In this mode the graphics object passed to the constructor is not disposed when the render is disposed.
        /// </summary>
        bool GDIProxyMode { get { return bitmap == null;} }

        public GDIPlusRenderer(Bitmap b)
        {
            graphics = Graphics.FromImage(b);
            bitmap = b;
            HighQuality = true;
            AngleType = AngleTypes.Degrees;
        }

        public GDIPlusRenderer(int width, int height)
        {
            try
            {
                bitmap = new Bitmap(width, height);
                graphics = Graphics.FromImage(bitmap);
                AngleType = AngleTypes.Degrees;
                HighQuality = true;
            }
            catch (Exception ex)
            {
                WDAppLog.logException(ErrorLevel.Error, ex);
                throw ex;
            }
        }

        /// <summary>
        ///  The graphics object passed to the constructor is not disposed when the render is disposed.
        /// </summary>
        /// <param name="g">Is not disposed when the render is disposed.</param>
        public GDIPlusRenderer(Graphics g)
        {
            graphics = g;
            bitmap = null;
            HighQuality = true;
            AngleType = AngleTypes.Degrees;
        }

        //--------------------------------------------------------------------------------
        // Pen and Brush management
        //--------------------------------------------------------------------------------
        private static Pen getPen(Color c, int width, RendererLineCapStyle cap)
        {
            //todo: some caching of pen objects, but they must be cached separately for multiple threads
            Pen p = new Pen(c, width);
            switch (cap)
	        {
		        case RendererLineCapStyle.Flat:
                    p.EndCap = System.Drawing.Drawing2D.LineCap.Flat;
                    break;
                case RendererLineCapStyle.Round:
                    p.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                    break;
	        }
            return p;
        }

        private static Brush getBrush(Color c)
        {
            //todo: some caching of brush objects, but they must be cached separately for multiple threads
            return new SolidBrush(c);
        }

        //when, if, caching is used this flags an item is not in use
        private static void freePen(Pen p)
        {
            p.Dispose();
        }

        //when, if, caching is used this flags an item is not in use
        private static void freeBrush(Brush brush)
        {
            brush.Dispose();
        }

         //when, if, caching is used; this is where the caches are emptied.
        private void emptyCaches()
        {
        }

        //--------------------------------------------------------------------------------
        // IRenderer
        //--------------------------------------------------------------------------------
        public void Close()
        {
            Flush();
            emptyCaches();
            if (!GDIProxyMode)
            {
                graphics.Dispose();
#if PROTECT_INNER_IMAGE
                bitmap.Dispose();
#endif
            }
        }

        public void Dispose()
        {
            Close();
        }

        public void Flush()
        {
            graphics.Flush();
        }


        public int Width
        {
            get
            {
                if (bitmap != null)
                {
                    return bitmap.Width;
                }

                return graphics.Clip.IsEmpty(graphics) ? 0 : (graphics.Clip.IsInfinite(graphics) ? -1 : (int)graphics.ClipBounds.Width);
            }
        }

        public int Height
        {
            get 
            {
                if (bitmap != null)
                {
                    return bitmap.Height;
                }

                return graphics.Clip.IsEmpty(graphics) ? 0 : (graphics.Clip.IsInfinite(graphics) ? -1 : (int)graphics.ClipBounds.Height);
            }
        }


        public Color GetNearestColor(Color color)
        {
            return graphics.GetNearestColor(color);
        }

        public Bitmap RenderTargetAsGDIBitmap()
        {
#if PROTECT_INNER_IMAGE
            return new Bitmap(bitmap);
#else
            return bitmap;
#endif
        }

        public void SetLineEndCap(RendererLineCapStyle capStyle)
        {
            this.capStyle = capStyle;
        }

        public bool SupportsMultiThreading { get { return false; } }

        public RendererLineCapStyle LineEndCapStyle
        {
            get
            {
                return capStyle;
            }

            set
            {
                capStyle = value;
            }
        }

        public bool HighQuality
        {
            get
            {
                return highQuality;
            }

            set
            {
                highQuality = value;
                if (value)
                {
                    graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                    graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                    graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                }
                else
                {
                    graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
                    graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
                    graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bilinear;
                    graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighSpeed;
                    graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
                }
            }
        }

        //--- Drawing
        public void DrawImage(Image image, Point[] destPoints)
        {
            graphics.DrawImage(image, destPoints);
        }

        public void DrawImage(Image image, int x, int y)
        {
            graphics.DrawImage(image, x, y);
        }

        public void DrawImage(Image image, Point[] destPoints, Rectangle srcRect)
        {
            graphics.DrawImage(image, destPoints, srcRect, GraphicsUnit.Pixel);
        }

        public void DrawImage(Image image, Rectangle destRect, Rectangle srcRect)
        {
            graphics.DrawImage(image, destRect, srcRect, GraphicsUnit.Pixel);
        }

        public void DrawImage(Image image, int x, int y, int width, int height)
        {
            graphics.DrawImage(image, x, y, width, height);
        }

        public void DrawImage(Image image, int x, int y, Rectangle srcRect)
        {
            graphics.DrawImage(image, x, y, srcRect, GraphicsUnit.Pixel);
        }

        public void DrawString(Color col, string s, Font font, int x, int y)
        {
            Brush brush = getBrush(col);
            graphics.DrawString(s, font, brush, x, y);
            freeBrush(brush);
        }

        public void DrawArc(Color col, int lineSize, int x, int y, int width, int height, double startAngle, double sweepAngle)
        {
            Pen pen = getPen(col, lineSize, this.capStyle);
            graphics.DrawArc(pen, x, y, width, height, (float)toDegrees(startAngle), (float)toDegrees(sweepAngle));
            freePen(pen);
        }

        public void DrawBeziers(Color col, int lineSize, Point[] points)
        {
            Pen pen = getPen(col, lineSize, this.capStyle);
            graphics.DrawBeziers(pen, points);
            freePen(pen);
        }

        public void DrawLine(Color col, int lineSize, int x1, int y1, int x2, int y2)
        {
            Pen pen = getPen(col, lineSize, this.capStyle);
            graphics.DrawLine(pen, x1, y1, x2, y2);
            freePen(pen);
        }

        public void DrawPolyLine(Color col, int lineSize, Point[] points)
        {
            Pen pen = getPen(col, lineSize, this.capStyle);
            graphics.DrawLines(pen, points);
            freePen(pen);
        } 

        public void DrawCurve(Color col, int lineSize, Point[] points)
        {
            Pen pen = getPen(col, lineSize, this.capStyle);
            graphics.DrawCurve(pen, points);
            freePen(pen);
        }

        public void DrawClosedCurve(Color col, int lineSize, Point[] points)
        {
            Pen pen = getPen(col, lineSize, this.capStyle);
            graphics.DrawClosedCurve(pen, points);
            freePen(pen);
        }

        public void DrawEllipse(Color col, int lineSize, int x, int y, int width, int height)
        {
            Pen pen = getPen(col, lineSize, this.capStyle);
            graphics.DrawEllipse(pen, x, y, width, height);
            freePen(pen);
        }

        public void DrawPie(Color col, int lineSize, int x, int y, int width, int height, double startAngle, double sweepAngle)
        {
            Pen pen = getPen(col, lineSize, this.capStyle);
            graphics.DrawPie(pen, x, y, width, height, (float)toDegrees(startAngle), (float)toDegrees(sweepAngle));
            freePen(pen);
        }

        public void DrawPolygon(Color col, int lineSize, Point[] points)
        {
            Pen pen = getPen(col, lineSize, this.capStyle);
            graphics.DrawPolygon(pen, points);
            freePen(pen);
        }

        public void DrawRectangle(Color col, int lineSize, int x, int y, int width, int height)
        {
            Pen pen = getPen(col, lineSize, this.capStyle);
            graphics.DrawRectangle(pen, x, y, width, height);
            freePen(pen);
        }

        public void FillClosedCurve(Color fillCol, Point[] points)
        {
            Brush brush = getBrush(fillCol);
            graphics.FillClosedCurve(brush, points);
            freeBrush(brush);
        }

        public void FillEllipse(Color fillCol, int x, int y, int width, int height)
        {
            Brush brush = getBrush(fillCol);
            graphics.FillEllipse(brush, x, y, width, height);
            freeBrush(brush);
        }

        public void FillPie(Color fillCol, int x, int y, int width, int height, double startAngle, double sweepAngle)
        {
            Brush brush = getBrush(fillCol);
            graphics.FillPie(brush, x, y, width, height, (float)toDegrees(startAngle), (float)toDegrees(sweepAngle));
            freeBrush(brush);
        }

        public void FillPolygon(Color fillCol, Point[] points)
        {
            Brush brush = getBrush(fillCol);
            graphics.FillPolygon(brush, points);
            freeBrush(brush);
        }

        public void FillRectangle(Color fillCol, int x, int y, int width, int height)
        {
            Brush brush = getBrush(fillCol);
            graphics.FillRectangle(brush, x, y, width, height);
            freeBrush(brush);
        }

        //--- non native GDI+ patches

        public void PutPixel(Color col, int x, int y)
        {
            FillRectangle(col, x, y, 1, 1);
        }

        public void DrawCircle(Color col, int lineSize, int cx, int cy, int radius)
        {
            int x = cx - radius;
            int y = cy - radius;
            int diamater = radius * 2;
            DrawEllipse(col, lineSize, x, y, diamater, diamater);
        }

        public void FillCircle(Color fillCol, int cx, int cy, int radius)
        {
            int x = cx - radius;
            int y = cy - radius;
            int diamater = radius * 2;
            FillEllipse(fillCol, x, y, diamater, diamater);
        }

        public void Clear(Color color)
        {
            FillRectangle(color, 0, 0, (int)graphics.ClipBounds.Width, (int)graphics.ClipBounds.Height); //todo this interface needs a get size
        }


        /*public void DrawImage(Bitmap Image, int x, int y, TransMatrix2D transform)
        {
            Transform(transform);
            DrawImage(Image, x, y);
            graphics.Transform.Reset();
        }*/

        public void ResetTransform()
        {
            //graphics.Transform.Reset();
            graphics.Transform = new Matrix();
        }

        private TransMatrix2D GetTransform()
        {
            return TransMatrix2D.FromGDIPlusMatrix(graphics.Transform);
        }

        private void SetTransform(TransMatrix2D transform)
        {
            graphics.Transform = transform.ToGDIPlusMatrix();
        }

        public TransMatrix2D Transform
        {
            get
            {
                return GetTransform();
            }
            set
            {
                SetTransform(value);
            }
        }

        public IRendererSettings GetSettings()
        {
            return new RendererSettings(this);
        }

        public void ApplySettings(IRendererSettings settings)
        {
            if (settings != null)
            {
                RendererSettings rs = settings as RendererSettings;
                if (rs != null)
                {
                    rs.Apply(this);
                }
                else
                {
                    AngleType = settings.AngleType;
                    LineEndCapStyle = settings.LineEndCapStyle;
                    HighQuality = settings.HighQuality;
                    Transform = settings.Transform;
                }
            }
        }

        public Size MeasureString(string s, Font font)
        {
            return graphics.MeasureString(s, font).ToSize();
        }
    }
}
