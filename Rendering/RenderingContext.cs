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

namespace WDToolbox.Rendering
{
    /// <summary>
    /// A version of an existing renderer with setting changed,
    /// Changes are restored on dispose.
    /// Works well with using clause.
    /// </summary>
    public sealed class RenderingContext : IRenderer, IDisposable
    {
        public IRenderer Renderer { get; private set; }

        private IRendererSettings orignialSetings;

        public RenderingContext(IRenderer r)
        {
            Renderer = r;
            orignialSetings = r.GetSettings();
        }

        public void Dispose()
        {
            Renderer.ApplySettings(orignialSetings);
        }


        #region IRenderer
        public AngleTypes AngleType
        {
            get
            {
                return Renderer.AngleType;
            }

            set
            {
                Renderer.AngleType = value;
            }
        }

        public int Width
        {
            get
            {
                return Renderer.Width;
            }
        }

        public int Height
        {
            get
            {
                return Renderer.Height;
            }
        }

        public bool SupportsMultiThreading
        {
            get
            {
                return Renderer.SupportsMultiThreading;
            }
        }
    
        public void Close()
        {
            Renderer.Close();
        }

        public void Flush()
        {
            Renderer.Flush();
        }

        public Bitmap RenderTargetAsGDIBitmap()
        {
            return Renderer.RenderTargetAsGDIBitmap();
        }

        public void DrawImage(Image image, Point[] destPoints)
        {
            Renderer.DrawImage(image, destPoints);
        }

        public void DrawImage(Image image, int x, int y)
        {
            Renderer.DrawImage(image, x, y);
        }

        public void DrawImage(Image image, Point[] destPoints, Rectangle srcRect)
        {
            Renderer.DrawImage(image, destPoints, srcRect);
        }

        public void DrawImage(Image image, Rectangle destRect, Rectangle srcRect)
        {
            Renderer.DrawImage(image, destRect, srcRect);
        }

        public void DrawImage(Image image, int x, int y, int width, int height)
        {
            Renderer.DrawImage(image, x, y, width, height);
        }

        public void DrawImage(Image image, int x, int y, Rectangle srcRect)
        {
            Renderer.DrawImage(image, x, y, srcRect);
        }

        public void DrawString(Color col, string s, Font font, int x, int y)
        {
            Renderer.DrawString(col, s, font, x, y);
        }

        public void PutPixel(Color col, int x, int y)
        {
            Renderer.PutPixel(col, x, y);
        }

        public void DrawArc(Color col, int lineSize, int x, int y, int width, int height, double startAngle, double sweepAngle)
        {
            Renderer.DrawArc(col, lineSize, x, y, width, height, startAngle, sweepAngle);
        }

        public void DrawBeziers(Color col, int lineSize, Point[] points)
        {
            Renderer.DrawBeziers(col, lineSize, points);
        }

        public void DrawLine(Color col, int lineSize, int x1, int y1, int x2, int y2)
        {
            Renderer.DrawLine(col, lineSize, x1, y1, x2, y2);
        }

        public void DrawPolyLine(Color col, int lineSize, Point[] points)
        {
            Renderer.DrawPolyLine(col, lineSize, points);
        }

        public void DrawCurve(Color col, int lineSize, Point[] points)
        {
            Renderer.DrawCurve(col, lineSize, points);
        }

        public void DrawClosedCurve(Color col, int lineSize, Point[] points)
        {
            Renderer.DrawClosedCurve(col, lineSize, points);
        }

        public void DrawEllipse(Color col, int lineSize, int x, int y, int width, int height)
        {
            Renderer.DrawEllipse(col, lineSize, x, y, width, height);
        }

        public void DrawCircle(Color col, int lineSize, int cx, int cy, int radius)
        {
            Renderer.DrawCircle(col, lineSize, cx, cy, radius);
        }

        public void DrawPie(Color col, int lineSize, int x, int y, int width, int height, double startAngle, double sweepAngle)
        {
            Renderer.DrawPie(col, lineSize, x, y, width, height, startAngle, sweepAngle);
        }

        public void DrawPolygon(Color col, int lineSize, Point[] points)
        {
            Renderer.DrawPolygon(col, lineSize, points);
        }

        public void DrawRectangle(Color col, int lineSize, int x, int y, int width, int height)
        {
            Renderer.DrawRectangle(col, lineSize, x, y, width, height);
        }

        public void FillClosedCurve(Color fillCol, Point[] points)
        {
            Renderer.FillClosedCurve(fillCol, points);
        }

        public void FillEllipse(Color fillCol, int x, int y, int width, int height)
        {
            Renderer.FillEllipse(fillCol, x, y, width, height);
        }

        public void FillCircle(Color fillCol, int cx, int cy, int radius)
        {
            Renderer.FillCircle(fillCol, cx, cy, radius);
        }

        public void FillPie(Color fillCol, int x, int y, int width, int height, double startAngle, double sweepAngle)
        {
            Renderer.FillPie(fillCol, x, y, width, height, startAngle, sweepAngle);
        }

        public void FillPolygon(Color fillCol, Point[] points)
        {
            Renderer.FillPolygon(fillCol, points);
        }

        public void FillRectangle(Color fillCol, int x, int y, int width, int height)
        {
            Renderer.FillRectangle(fillCol, x, y, width, height);
        }

        public void Clear(Color c)
        {
            Renderer.Clear(c);
        }

        public Color GetNearestColor(Color color)
        {
            return Renderer.GetNearestColor(color);
        }

        public void ResetTransform()
        {
            Renderer.ResetTransform();
        }

        public IRendererSettings GetSettings()
        {
            return Renderer.GetSettings();
        }

        public void ApplySettings(IRendererSettings setting)
        {
            Renderer.ApplySettings(setting);
        }

        public bool HighQuality
        {
            get
            {
                return Renderer.HighQuality;
            }
            set
            {
                Renderer.HighQuality = value;
            }
        }

        public RendererLineCapStyle LineEndCapStyle
        {
            get
            {
                return Renderer.LineEndCapStyle;
            }
            set
            {
                Renderer.LineEndCapStyle = value;
            }
        }

        public TransMatrix2D Transform
        {
            get
            {
                return Renderer.Transform;
            }
            set
            {
                Renderer.Transform = value;
            }
        }

        public Size MeasureString(string s, Font font)
        {
            return Renderer.MeasureString(s, font);
        }
        #endregion


    }
}
