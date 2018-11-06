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

namespace WDToolbox
{
    public static  class ColorExtension
    {
        public static Color WithNewAlpha(this Color color, int alpha)
        {
            return Color.FromArgb(alpha, color);
        }

        /// <summary>
        /// Quickly gets a luminance (approximation) (2 parts red, 1 part blue, 3 parts green
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static byte GetQuickLuminance(this Color color)
        {
            return  (byte) ((color.R + color.R + 
                             color.B +
                             color.G + color.G + color.G
                             ) / 6);
        }
    }
}
