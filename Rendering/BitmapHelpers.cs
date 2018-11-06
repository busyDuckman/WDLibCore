/*
 * The following code is Copyright 2018 Dr Warren Creemers (busyDuckman)
 * See LICENSE.md for more information.
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WDToolbox;
using WDToolbox.Data.DataStructures;
using WDToolbox.Maths.Range;
using WDToolbox.Rendering.Colour;
using WDToolbox.Rendering.FormattedText;

namespace WDToolbox.Rendering
{
    /// <summary>
    /// Helpers for misc Bitmap functionality.
    /// </summary>
    public static class BitmapHelpers
    {
        /// <summary>
        /// Used to create a stand-in image with a message on it.
        ///
        /// eg: GenerateDebugImage(1024,1024, "File not found: " + imageName); 
        /// </summary>        
        /// <returns></returns>
        public static Bitmap GenerateDebugImage(int width, int height, string message)
        {
            Bitmap bmp = new Bitmap(width, height);
            GDIPlusRenderer r =new GDIPlusRenderer(bmp);
            r.Clear(Color.Fuchsia);
            float fontSize = Math.Min(width, height)/10.0f;
            fontSize = Range.clamp(fontSize, 10, 45);
            using (Font f = new Font("Arial", fontSize, FontStyle.Bold))
            {
                TextFormat tf = new TextFormat(f, Color.Black, true, Color.White, false);
                tf.render(r, message, 0, height / 2);
                bmp = r.RenderTargetAsGDIBitmap();
            }
            return bmp;
        }

        /// <summary>
        /// Used to create "round buttons" for menus.
        /// Altering this method will affect visual stying across several projects.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="foreCol"></param>
        /// <param name="backCol"></param>
        /// <param name="message"></param>
        /// <param name="font"></param>
        /// <returns></returns>
        public static Bitmap GenerateMessageInCircle(int width, int height, Color foreCol, Color backCol, string message, Font font = null)
        {
            GDIPlusRenderer r = new GDIPlusRenderer(width, height);
            r.SetHighQuality(true);
            //circle
            int lineWidth = Range.clamp(Math.Min(width, height) / 128, 1, 10);
            int diamater = Math.Max(Math.Min(width, height) - lineWidth - 1, 1);
            int cX = width/2;
            int cY = height/2;
            r.FillCircle(backCol, cX, cY, diamater / 2);
            r.DrawCircle(foreCol, lineWidth, cX, cY, diamater / 2);

            //text
            if (!String.IsNullOrWhiteSpace(message))
            {
                
                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Center;
                stringFormat.LineAlignment = StringAlignment.Center;

                Font _font  = font;

                if (_font == null)
                {
                    float fontSize = Math.Min(width, height) / 10.0f;
                    fontSize = Range.clamp(fontSize, 10, 45);
                    _font = new Font("Arial", fontSize, FontStyle.Bold);
                }

                try
                {
                    r.DrawStringAligned(foreCol, message, _font, cX, cY, HorizontalAlignment.Centre, VerticalAlignment.Centre);
                }
                finally
                {
                    if(font == null)
                    {
                        //_font was created in this method, not supplied via font
                        _font.TryDispose();
                    }
                }                
            }

            return r.RenderTargetAsGDIBitmap();
        }

        /// <summary>
        /// Bitmap save routine, bridges Bitmap saving to BinaryWriter
        /// </summary>
        public static Why Save(Bitmap b, BinaryWriter s)
        {
            // validate inputs
            if (s == null)
            {
                return Why.FalseBecause("Save(Bitmap b, BinaryWriter s), BinaryWriter was null", true);
            }
            
            // Save b into s
            return Why.FromTry(delegate()
            {
                bool notNull = b != null;
                
                s.Write(notNull);

                if (notNull)
                {
                    using(MemoryStream ms = new MemoryStream())
                    {
                        b.Save(ms, ImageFormat.Bmp);
                        byte[] data = ms.ToArray();
                        s.Write(data.Length);
                        s.Write(data);
                    }
                };
            });
        }

        /// <summary>
        /// Bitmap load routine, bridges Bitmap loading to BinaryReader
        /// </summary>
        public static Why Load(BinaryReader r, out Bitmap b)
        {
            //validate inputs
            if (r == null)
            {
                b = null;
                return Why.FalseBecause("Load(BinaryReader r, out Bitmap b), BinaryReader was null", true);
            }
            
            // load b from r
            try
            {
                bool notNull = r.ReadBoolean();
                if (notNull)
                {
                    int len = r.ReadInt32();
                    byte[] data = r.ReadBytes(len);
                    using (MemoryStream ms = new MemoryStream(data))
                    {
                        b = new Bitmap(ms);
                        return true;
                    }
                }
                else
                {
                    b = null;
                    return true;
                }
            }
            catch (Exception ex)
            {
                b = null;
                return Why.FalseBecause(ex);
            }
        }
    }
}
