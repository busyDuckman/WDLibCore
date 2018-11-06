/*
 * The following code is Copyright 2018 Dr Warren Creemers (busyDuckman)
 * See LICENSE.md for more information.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics.Contracts;
using WDToolbox.Maths.Range;
using System.Runtime.InteropServices;

namespace WDToolbox//.DotNetExtension
{
    public static class BitmapExtension
    {
        public static Bitmap ConvetTo(this Bitmap bitmap,  PixelFormat format)
        {
            Bitmap newBitmap = new Bitmap(bitmap.Width, bitmap.Height, format);
            Graphics g = Graphics.FromImage(newBitmap);
            g.DrawImageUnscaled(bitmap, 0, 0);
            g.Dispose();
            return newBitmap;
        }

        /// <summary>
        /// Removes any weird file locks windows creates.
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static Bitmap GetUnFuckedVersion(this Bitmap bitmap)
        {
            Bitmap b2 = new Bitmap(bitmap.Width, bitmap.Height, bitmap.PixelFormat);

            //NB: This approach is dpi stable for win 8
            b2.SetPixelsFromBytesARGB32(bitmap.GetCopyOfBytesARGB32());
            return b2;
        }

        /// <summary>
        /// Gets a sub image. Result cliped to image bounds
        /// </summary>
        /// <param name="x">Desired top left corner of sub image. (will be cliped)</param>
        /// <param name="y">Desired top left corner of sub image. (will be cliped)</param>
        /// <param name="width">Desired width of sub image. (will be cliped)</param>
        /// <param name="height">Desired height of sub image. (will be cliped)</param>
        /// <returns></returns>
        public static Bitmap GetSubImageClipped(this Bitmap bmp, int x, int y, int width, int height)
        {
            Rectangle rec = bmp.ClipRectangeInsideImage(x, y, width, height);

            if (rec.CalculateSurfaceArea() == 0)
            {
                return null;
            }
            return bmp.Clone(rec, bmp.PixelFormat);
        }

        public static Rectangle ClipRectangeInsideImage(this Bitmap bmp, int x, int y, int width, int height)
        {
            int x2 = x + width;
            int y2 = y + height;
            if (x < 0)
            {
                x2 -= x;
                x = 0;
            }

            if (y < 0)
            {
                y2 -= y;
                y = 0;
            }

            if (x2 >= bmp.Width)
            {
                x2 = bmp.Width;// -1;
            }

            if (y2 >= bmp.Height)
            {
                y2 = bmp.Height;// -1;
            }
            Rectangle rec = new Rectangle(x, y, x2 - x, y2 - y);
            return rec;
        }

        public static Bitmap AppendImageBelow(this Bitmap a, Bitmap b)
        {
            Bitmap newImage = new Bitmap(Math.Max(a.Width, b.Width), 
                                         a.Height + b.Height,
                                         a.PixelFormat);

            Graphics g = Graphics.FromImage(newImage);
            if(a.Width != b.Width)
            {
                g.Clear(Color.Transparent);
            }

            g.DrawImageUnscaled(a, 0, 0);
            g.DrawImageUnscaled(b, 0, a.Height);
            g.Dispose();
            return newImage;
        }
        
        public static Bitmap AppendImageAbove(this Bitmap a, Bitmap b)
        {
            Bitmap newImage = new Bitmap(Math.Max(a.Width, b.Width),
                                         a.Height + b.Height,
                                         a.PixelFormat);

            Graphics g = Graphics.FromImage(newImage);
            if (a.Width != b.Width)
            {
                g.Clear(Color.Transparent);
            }

            g.DrawImageUnscaled(b, 0, 0);
            g.DrawImageUnscaled(a, 0, b.Height);
            g.Dispose();
            return newImage;
        }

        public static Bitmap AppendImageRight(this Bitmap a, Bitmap b)
        {
            Bitmap newImage = new Bitmap(a.Width + b.Width,
                                         Math.Max(a.Height, b.Height),
                                         a.PixelFormat);

            Graphics g = Graphics.FromImage(newImage);
            if (a.Width != b.Width)
            {
                g.Clear(Color.Transparent);
            }

            g.DrawImageUnscaled(a, 0, 0);
            g.DrawImageUnscaled(b, a.Width, 0);
            g.Dispose();
            return newImage;
        }

        public static Bitmap AppendImageLeft(this Bitmap a, Bitmap b)
        {
            Bitmap newImage = new Bitmap(a.Width + b.Width,
                                         Math.Max(a.Height, b.Height),
                                         a.PixelFormat);

            Graphics g = Graphics.FromImage(newImage);
            if (a.Width != b.Width)
            {
                g.Clear(Color.Transparent);
            }

            g.DrawImageUnscaled(b, 0, 0);
            g.DrawImageUnscaled(a, b.Width, 0);
            g.Dispose();
            return newImage;
        }

        public static Bitmap GetBlankClone(this Bitmap b)
        {
            return new Bitmap(b.Width, b.Height, b.PixelFormat); 
        }

#if UNSAFE
        /// <summary>
        /// Scales an images opacity
        /// </summary>
        /// <param name="val">ie. pixel.opacity *= val;</param>
        /// <returns></returns>
        public static Bitmap MultiplyOpacity(this Bitmap b, double val)
        {
            //Contract.Requires<ArgumentNullException>(b != null, "image cannot be null.");
            val = Range.clamp(val, 0, 1);

            Bitmap res = b.GetBlankClone();

            using (LockedImageData src = b.GetLockedData_Format32bppArgb(ImageLockMode.ReadWrite))
            using (LockedImageData dest = res.GetLockedData_Format32bppArgb(ImageLockMode.ReadWrite))
            unsafe
            {
                for (int y = 0; y < src.Height; y++)
                {
                    int _off = (y * src.Stride);
                    byte* pSrc = (byte*)src.Scan0 + _off;
                    byte* pDest = (byte*)dest.Scan0 + _off;

                    for (int i = 0; i < src.Width; i++)
                    {
                        //b
                        *pDest = *pSrc;
                        pDest++; pSrc++;
                        //g
                        *pDest = *pSrc;
                        pDest++; pSrc++;
                        //r
                        *pDest = *pSrc;
                        pDest++; pSrc++;
                        //a
                        *pDest = (byte)Range.clamp((int)((*pSrc) * val), 0, 255);
                        pDest++; pSrc++;
                    }
                }
            }

            return res;
        }
#endif


        public static LockedImageData GetLockedData(this Bitmap bitmap, Rectangle rect, ImageLockMode flags, PixelFormat format)
        {
            //Contract.Requires<ArgumentNullException>(bitmap != null, "image cannot be null.");
            return new LockedImageData(bitmap, rect, flags, format);
        }

        public static LockedImageData GetLockedData_imageFormat(this Bitmap bitmap, ImageLockMode flags)
        {
            //Contract.Requires<ArgumentNullException>(bitmap != null, "image cannot be null.");
            return new LockedImageData(bitmap, new Rectangle(0, 0, bitmap.Width, bitmap.Height), flags, bitmap.PixelFormat);
        }

        public static LockedImageData GetLockedData_Format32bppArgb(this Bitmap bitmap, ImageLockMode flags)
        {
            //Contract.Requires<ArgumentNullException>(bitmap != null, "image cannot be null.");
            return new LockedImageData(bitmap, new Rectangle(0, 0, bitmap.Width, bitmap.Height), flags, PixelFormat.Format32bppArgb);
        }

        /// <summary>
        /// Gets bytes to represent an image.
        /// This is not a FAST method suitable for realtime systems.
        /// The stride is removed and the array is just sequential pixels.
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static byte[] GetCopyOfBytesARGB32(this Bitmap bitmap)
        {
            //Contract.Requires<ArgumentNullException>(bitmap != null, "image cannot be null.");
            if (bitmap == null)
            {
                return null;
            }
            BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            bool flip = data.Stride < 0;
            int actualStride = Math.Abs(data.Stride);
            int pixelSize = 4;
            byte[] rgbValues = new byte[bitmap.Width * bitmap.Height * pixelSize];
            for (int y = 0; y < bitmap.Height; y++)
            {
                //resolve the next image line (may be flipped)
                int line = flip ? (bitmap.Height - 1 - y) : y;
                //get the next source line
                IntPtr linePtr = data.Scan0 + (line * actualStride);

                Marshal.Copy(linePtr, rgbValues, (y * bitmap.Width * pixelSize), bitmap.Width * pixelSize);
            }

            bitmap.UnlockBits(data);
            return rgbValues;
        }

        public static int[] GetCopyOfIntsARGB32(this Bitmap bitmap)
        {
            //Contract.Requires<ArgumentNullException>(bitmap != null, "image cannot be null.");
            if (bitmap == null)
            {
                return null;
            }
            BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            bool flip = data.Stride < 0;
            int actualStride = Math.Abs(data.Stride);
            int[] rgbValues = new int[bitmap.Width * bitmap.Height];
            for (int y = 0; y < bitmap.Height; y++)
            {
                //resolve the next image line (may be flipped)
                int line = flip ? (bitmap.Height - 1 - y) : y;
                //get the next source line
                IntPtr linePtr = data.Scan0 + (line * actualStride);

                Marshal.Copy(linePtr,
                             rgbValues, 
                             (y * bitmap.Width), 
                             bitmap.Width);
            }

            bitmap.UnlockBits(data);
            return rgbValues;
        }

        /// <summary>
        /// Gets bytes to represent an image.
        /// This is not a FAST method suitable for realtime systems.
        /// The stride is removed and the array is just sequential pixels.
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static byte[] GetCopyOfBytesABGR32(this Bitmap bitmap)
        {
            byte[] data = GetCopyOfBytesARGB32(bitmap);
            TwizzleRedBlue(data);
            return data;
        }

        private static void TwizzleRedBlue(byte[] data)
        {
            int s1 = 0;
            int s2 = 2;
            for (int i = 0; i < data.Length; i += 4)
            {
                byte t = data[i + s1];
                data[i + s1] = data[i + s2];
                data[i + s2] = t;
            }
        }

        /// <summary>
        /// Got an array of pixels (no seperate stride), need a bitmap.
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="data"></param>
        /// <param name="dataStartPos"></param>
        public static void SetPixelsFromBytesARGB32(this Bitmap bitmap, byte[] data, int dataStartPos=0)
        {
            Bitmap result = new Bitmap(bitmap.Width, bitmap.Height, PixelFormat.Format32bppArgb);
            BitmapData bitmapData = bitmap.LockBits(new Rectangle(0,0,bitmap.Width,bitmap.Height), 
                                                    ImageLockMode.ReadWrite,
                                                    PixelFormat.Format32bppArgb);

            bool flip = bitmapData.Stride < 0;
            int actualStride = Math.Abs(bitmapData.Stride);
            int pixelSize = 4;
            byte[] rgbValues = new byte[bitmap.Width * bitmap.Height * pixelSize];
            for (int y = 0; y < bitmap.Height; y++)
            {
                //resolve the next image line (may be flipped)
                int line = flip ? (bitmap.Height - 1 - y) : y;
                //get the next destination line
                IntPtr linePtr = bitmapData.Scan0 + (line * actualStride);

                Marshal.Copy(data, 
                             dataStartPos + (y * bitmap.Width * pixelSize), 
                             linePtr, 
                             bitmap.Width * pixelSize);
            }

            bitmap.UnlockBits(bitmapData);
        }

        public static void SetPixelsFromBytesABGR32(this Bitmap bitmap, byte[] data, int dataStartPos=0)
        {
            int pixelSize = 4;
            byte[] data2 = new byte[bitmap.Width * bitmap.Height * pixelSize];
            Array.Copy(data, dataStartPos, data2, 0, data2.Length);
            TwizzleRedBlue(data2);
            SetPixelsFromBytesARGB32(bitmap, data2, 0);
        }

        public static void SetPixelsFromIntsARGB32(this Bitmap bitmap, int[] data, int dataStartPos = 0)
        {
            Bitmap result = new Bitmap(bitmap.Width, bitmap.Height, PixelFormat.Format32bppArgb);
            BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                                                    ImageLockMode.ReadWrite,
                                                    PixelFormat.Format32bppArgb);

            bool flip = bitmapData.Stride < 0;
            int actualStride = Math.Abs(bitmapData.Stride);
            //byte[] rgbValues = new byte[bitmap.Width * bitmap.Height * pixelSize];
            for (int y = 0; y < bitmap.Height; y++)
            {
                //resolve the next image line (may be flipped)
                int line = flip ? (bitmap.Height - 1 - y) : y;
                //get the next destination line
                IntPtr linePtr = bitmapData.Scan0 + (line * actualStride);

                Marshal.Copy(data,
                             dataStartPos + (y * bitmap.Width),
                             linePtr,
                             bitmap.Width);
            }

            bitmap.UnlockBits(bitmapData);
        }

        public class LockedImageData : IDisposable
        {
            private readonly Bitmap _bitmap;
            private readonly BitmapData _data;

            internal LockedImageData(Bitmap bitmap, Rectangle rect, ImageLockMode flags, PixelFormat format)
            {
                //Contract.Requires<ArgumentNullException>(bitmap != null, "image cannot be null.");
                _bitmap = bitmap;
                _data = bitmap.LockBits(rect, flags, format);
            }

            public void Dispose()
            {
                _bitmap.UnlockBits(_data);
            }

            public IntPtr Scan0
            {
                get { return _data.Scan0; }
            }

            public int Stride
            {
                get { return _data.Stride; }
            }

            public int Width
            {
                get { return _data.Width; }
            }

            public int Height
            {
                get { return _data.Height; }
            }

            public PixelFormat PixelFormat
            {
                get { return _data.PixelFormat; }
            }

            public int Reserved
            {
                get { return _data.Reserved; }
            }
        }   
    }
}
