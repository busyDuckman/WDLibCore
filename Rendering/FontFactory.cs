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

namespace WDToolbox.Rendering
{
    public static class FontFactory
    {
        /// <summary>
        /// A font creation height resolver, suited to raster orientated work(where pixel sizes are known).
        ///
        /// 
        /// </summary>
        /// <param name="fontFamily"></param>
        /// <param name="style"></param>
        /// <param name="pixelHeight"> Font height, in pixels</param>
        /// <param name="intergralHeight"> True if the font size should be an integer</param>
        /// <returns></returns>
        public static Font GetFontByTotalHeight(FontFamily fontFamily, FontStyle style, int pixelHeight, bool intergralHeight = true)
        {
            double ascent = fontFamily.GetCellAscent(FontStyle.Regular);
            double descent = fontFamily.GetCellDescent(FontStyle.Regular);

            double fontTestSize = 48;
            double ascentPixel = fontTestSize * ascent / fontFamily.GetEmHeight(FontStyle.Regular);
            double descentPixel = fontTestSize * descent / fontFamily.GetEmHeight(FontStyle.Regular);

            //remove +descentPixel line for basline
            double fontTestSizePixels = ascentPixel + descentPixel;
            
            double fontSize = (fontTestSize / fontTestSizePixels) * (double) pixelHeight;

            fontSize *= 0.8;  //kludge

            return new Font(fontFamily, intergralHeight ? ((float)Math.Floor(fontSize)) : ((float)fontSize), style);
        }
    }
}
