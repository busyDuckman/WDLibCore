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
using WDToolbox.Data.DataStructures;

namespace WDToolbox.Rendering.Colour
{
    /// <summary>
    /// Stores and manipulates a list of colours.
    /// </summary>
    public class Palette : VList<Color>
    {

        int opacity;
        public int Opacity
        {
            get { return opacity; }
            set { opacity = value; fixAll(); }
        }

        public Palette(int opacity)
            : base()
        {
            this.opacity = opacity;
        }

        public static Palette generateSpectrumPalette(int size, int opacity)
        {
            double hue = 0, sat = 1.0, lum = 0.5;
            double step = 1.0 / (size + 1);

            Palette p = new Palette(opacity);
            for (int i = 0; i < size; i++)
            {
                //p.Add(new HSLColor(hue, sat, lum));
                p.Add(ColorFromHSV(hue*360, 1, 1));
                hue += step;
            }

            return p;
        }

        private static Color ColorFromHSV(double hue, double saturation, double value)
        {
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value = value * 255;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1 - saturation));
            int q = Convert.ToInt32(value * (1 - f * saturation));
            int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

            if (hi == 0)
                return Color.FromArgb(255, v, t, p);
            else if (hi == 1)
                return Color.FromArgb(255, q, v, p);
            else if (hi == 2)
                return Color.FromArgb(255, p, v, t);
            else if (hi == 3)
                return Color.FromArgb(255, p, q, v);
            else if (hi == 4)
                return Color.FromArgb(255, t, p, v);
            else
                return Color.FromArgb(255, v, p, q);
        }

        protected Color fix(Color c)
        {
            return Color.FromArgb(opacity, c);
        }

        protected void fixAll()
        {
            for (int i = 0; i < list.Count; i++)
            {
                list[i] = fix(list[i]);
            }
        }

        public Palette getResizedPalette(int n)
        {
            if (n <= this.Count)
            {
                //select colors
                Palette newPalette = new Palette(this.opacity);
                for (int i = 0; i < n; i++)
                {
                    newPalette.Add(this[i]);
                }
                return newPalette;
            }
            else
            {
                //interpolate colors
                Palette newPalette = new Palette(this.opacity);
                Bitmap b = new Bitmap(this.Count, 1);
                for (int i = 0; i < this.Count; i++)
                {
                    b.SetPixel(i, 0, getNonTranslucentColor(i));
                }
                Bitmap b2 = new Bitmap(b, n, 1);
                for (int i = 0; i < n; i++)
                {
                    newPalette.Add(b2.GetPixel(i, 0));
                }
                return newPalette;
            }
        }

        public Color getNonTranslucentColor(int i)
        {
            Color c = this[i];
            return Color.FromArgb(c.R, c.G, c.B);
        }

        //------------------------------------------------------------------------------------------
        // IList<Color>
        //------------------------------------------------------------------------------------------

        public override Color this[int index]
        {
            get
            {
                return fix(list[index]);
            }
            set
            {
                list[index] = fix(value);
            }
        }

        public void CopyTo(Color[] array, int arrayIndex)
        {
            fixAll();
            base.CopyTo(array, arrayIndex);
        }


        public override IEnumerator<Color> GetEnumerator()
        {
            fixAll();
            return list.GetEnumerator();
        }

    }
}
