/*
 * The following code is Copyright 2018 Dr Warren Creemers (busyDuckman)
 * See LICENSE.md for more information.
 */
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using WDToolbox.Maths.Range;

namespace WDToolbox.Rendering.Colour
{
    /// <summary>
    /// A class for fast access to colours stored in the win32 (BGRA) format
    ///
    /// TODO: This is very old code, written originally as part of a grant to get a OpenGL graphics
    ///       engine ported to c#. My understanding of color theory made a few incorrect assumptions back then.
    ///
    /// This uses named colors... probably the old x11 colors? 
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    [Serializable]
    public struct QColor : IFormattable, IColour<QColor>
    {
        private static Random colorRand = new Random();

        #region data elements

        /// <summary>
        /// Represents the blue component
        /// </summary>
        public byte b;

        /// <summary>
        /// Represents the green component
        /// </summary>
        public byte g;

        /// <summary>
        /// Represents the red component
        /// </summary>
        public byte r;

        /// <summary>
        /// Represents the alpha component (0=transparent, 255=solid)
        /// </summary>
        public byte a;

        #endregion

        #region named colours

        #region tables

        #region color names table	

        static private string[] colorNames = new string[128]
        {
            "alice blue",
            "antique white",
            "aqua marine",
            "azure",
            "beige",
            "bisque",
            "black",
            "blanched almond",
            "blue",
            "blue violet",
            "blue violet",
            "brown",
            "burly wood",
            "cadet blue",
            "chartreuse",
            "chocolate",
            "coral",
            "cornflower blue",
            "corn silk",
            "cyan",
            "dark blue",
            "dark cyan",
            "dark golden rod",
            "dark gray",
            "dark green",
            "dark khaki",
            "dark magenta",
            "dark olive green",
            "dark orange",
            "dark orchid",
            "dark red",
            "dark salmon",
            "dark sea green",
            "dark slate blue",
            "dark slate gray",
            "dark turquoise",
            "dark violet",
            "deep pink",
            "deep sky blue",
            "dim gray",
            "dodger blue",
            "fire brick",
            "floral white",
            "forest green",
            "gainsboro",
            "ghost white",
            "gold",
            "golden rod",
            "green",
            "green yellow",
            "gray",
            "honeydew",
            "hotpink",
            "indianred",
            "ivory",
            "khaki",
            "lavender",
            "lavender blush",
            "lawn green",
            "lemon chiffon",
            "light blue",
            "light coral",
            "light cyan",
            "light goldenrod",
            "light golden rod yellow",
            "light gray",
            "light green",
            "light pink",
            "light salmon",
            "light sea green",
            "light sky blue",
            "light slate blue",
            "light slate gray",
            "light steel blue",
            "light yellow",
            "limeg reen",
            "linen",
            "magenta",
            "maroon",
            "midnight blue",
            "mint cream",
            "misty rose",
            "moccasin",
            "navajo white",
            "navy",
            "navy blue",
            "old lace",
            "olive drab",
            "orange",
            "orange red",
            "orchid",
            "pale golden rod",
            "pale green",
            "pale turquoise",
            "pale violet red",
            "papayawhip",
            "peach puff",
            "peru",
            "pink",
            "plum",
            "powder blue",
            "purple",
            "red",
            "rosy brown",
            "royal blue",
            "saddle brown",
            "salmon",
            "sandy brown",
            "sea green",
            "sea shell",
            "sienna",
            "sky blue",
            "slate blue",
            "slate gray",
            "snow",
            "spring green",
            "steel blue",
            "tan",
            "thistle",
            "tomato",
            "turquoise",
            "violet",
            "violetred",
            "wheat",
            "white",
            "white smoke",
            "yellow",
            "yellow green"
        };

        #endregion

        #region color values table

        static private byte[] colorValues = new byte[128 * 3]
        {
            240, 248, 255,
            250, 235, 215,
            127, 255, 212,
            240, 255, 255,
            245, 245, 220,
            255, 228, 196,
            0, 0, 0,
            255, 235, 205,
            0, 0, 255,
            138, 43, 226,
            138, 43, 226,
            165, 42, 42,
            222, 184, 135,
            95, 158, 160,
            127, 255, 0,
            210, 105, 30,
            255, 127, 80,
            100, 149, 237,
            255, 248, 220,
            0, 255, 255,
            0, 0, 139,
            0, 139, 139,
            184, 134, 11,
            169, 169, 169,
            0, 100, 0,
            189, 183, 107,
            139, 0, 139,
            85, 107, 47,
            255, 140, 0,
            153, 50, 204,
            139, 0, 0,
            233, 150, 122,
            143, 188, 143,
            72, 61, 139,
            47, 79, 79,
            0, 206, 209,
            148, 0, 211,
            255, 20, 147,
            0, 191, 255,
            105, 105, 105,
            30, 144, 255,
            178, 34, 34,
            255, 250, 240,
            34, 139, 34,
            220, 220, 220,
            248, 248, 255,
            255, 215, 0,
            218, 165, 32,
            0, 255, 0,
            173, 255, 47,
            127, 127, 127,
            240, 255, 240,
            255, 105, 180,
            205, 92, 92,
            255, 255, 240,
            240, 230, 140,
            230, 230, 250,
            255, 240, 245,
            124, 252, 0,
            255, 250, 205,
            173, 216, 230,
            240, 128, 128,
            224, 255, 255,
            238, 221, 130,
            250, 250, 210,
            211, 211, 211,
            144, 238, 144,
            255, 182, 193,
            255, 160, 122,
            32, 178, 170,
            135, 206, 250,
            132, 112, 255,
            119, 136, 153,
            176, 196, 222,
            255, 255, 224,
            50, 205, 50,
            250, 240, 230,
            255, 0, 255,
            176, 48, 96,
            25, 25, 112,
            245, 255, 250,
            255, 228, 225,
            255, 228, 181,
            255, 222, 173,
            0, 0, 128,
            0, 0, 128,
            253, 245, 230,
            107, 142, 35,
            255, 165, 0,
            255, 69, 0,
            218, 112, 214,
            238, 232, 170,
            152, 251, 152,
            175, 238, 238,
            219, 112, 147,
            255, 239, 213,
            255, 218, 185,
            205, 133, 63,
            255, 192, 203,
            221, 160, 221,
            176, 224, 230,
            160, 32, 240,
            255, 0, 0,
            188, 143, 143,
            65, 105, 225,
            139, 69, 19,
            250, 128, 114,
            244, 164, 96,
            46, 139, 87,
            255, 245, 238,
            160, 82, 45,
            135, 206, 235,
            106, 90, 205,
            112, 128, 144,
            255, 250, 250,
            0, 255, 127,
            70, 130, 180,
            210, 180, 140,
            216, 191, 216,
            255, 99, 71,
            64, 224, 208,
            238, 130, 238,
            208, 32, 144,
            245, 222, 179,
            255, 255, 255,
            245, 245, 245,
            255, 255, 0,
            154, 205, 50
        };

        #endregion

        struct preDefinedColours
        {
            public static QColor aliceBlue = QColor.fromRGB(QColor.colorValues[0 * 3], QColor.colorValues[0 * 3 + 1],
                QColor.colorValues[0 * 3 + 2]);

            public static QColor antiqueWhite = QColor.fromRGB(QColor.colorValues[1 * 3], QColor.colorValues[1 * 3 + 1],
                QColor.colorValues[1 * 3 + 2]);

            public static QColor aquaMarine = QColor.fromRGB(QColor.colorValues[2 * 3], QColor.colorValues[2 * 3 + 1],
                QColor.colorValues[2 * 3 + 2]);

            public static QColor azure = QColor.fromRGB(QColor.colorValues[3 * 3], QColor.colorValues[3 * 3 + 1],
                QColor.colorValues[3 * 3 + 2]);

            public static QColor beige = QColor.fromRGB(QColor.colorValues[4 * 3], QColor.colorValues[4 * 3 + 1],
                QColor.colorValues[4 * 3 + 2]);

            public static QColor bisque = QColor.fromRGB(QColor.colorValues[5 * 3], QColor.colorValues[5 * 3 + 1],
                QColor.colorValues[5 * 3 + 2]);

            public static QColor black = QColor.fromRGB(QColor.colorValues[6 * 3], QColor.colorValues[6 * 3 + 1],
                QColor.colorValues[6 * 3 + 2]);

            public static QColor blanchedAlmond = QColor.fromRGB(QColor.colorValues[7 * 3],
                QColor.colorValues[7 * 3 + 1], QColor.colorValues[7 * 3 + 2]);

            public static QColor blue = QColor.fromRGB(QColor.colorValues[8 * 3], QColor.colorValues[8 * 3 + 1],
                QColor.colorValues[8 * 3 + 2]);

            public static QColor blueViolet = QColor.fromRGB(QColor.colorValues[9 * 3], QColor.colorValues[9 * 3 + 1],
                QColor.colorValues[9 * 3 + 2]);

            //public static qColor blueViolet = qColor.fromRGB(qColor.colorValues[10*3], qColor.colorValues[10*3+1], qColor.colorValues[10*3+2]);
            public static QColor brown = QColor.fromRGB(QColor.colorValues[11 * 3], QColor.colorValues[11 * 3 + 1],
                QColor.colorValues[11 * 3 + 2]);

            public static QColor burlyWood = QColor.fromRGB(QColor.colorValues[12 * 3], QColor.colorValues[12 * 3 + 1],
                QColor.colorValues[12 * 3 + 2]);

            public static QColor cadetBlue = QColor.fromRGB(QColor.colorValues[13 * 3], QColor.colorValues[13 * 3 + 1],
                QColor.colorValues[13 * 3 + 2]);

            public static QColor chartreuse = QColor.fromRGB(QColor.colorValues[14 * 3], QColor.colorValues[14 * 3 + 1],
                QColor.colorValues[14 * 3 + 2]);

            public static QColor chocolate = QColor.fromRGB(QColor.colorValues[15 * 3], QColor.colorValues[15 * 3 + 1],
                QColor.colorValues[15 * 3 + 2]);

            public static QColor coral = QColor.fromRGB(QColor.colorValues[16 * 3], QColor.colorValues[16 * 3 + 1],
                QColor.colorValues[16 * 3 + 2]);

            public static QColor cornflowerBlue = QColor.fromRGB(QColor.colorValues[17 * 3],
                QColor.colorValues[17 * 3 + 1], QColor.colorValues[17 * 3 + 2]);

            public static QColor cornSilk = QColor.fromRGB(QColor.colorValues[18 * 3], QColor.colorValues[18 * 3 + 1],
                QColor.colorValues[18 * 3 + 2]);

            public static QColor cyan = QColor.fromRGB(QColor.colorValues[19 * 3], QColor.colorValues[19 * 3 + 1],
                QColor.colorValues[19 * 3 + 2]);

            public static QColor darkBlue = QColor.fromRGB(QColor.colorValues[20 * 3], QColor.colorValues[20 * 3 + 1],
                QColor.colorValues[20 * 3 + 2]);

            public static QColor darkCyan = QColor.fromRGB(QColor.colorValues[21 * 3], QColor.colorValues[21 * 3 + 1],
                QColor.colorValues[21 * 3 + 2]);

            public static QColor darkGoldenRod = QColor.fromRGB(QColor.colorValues[22 * 3],
                QColor.colorValues[22 * 3 + 1], QColor.colorValues[22 * 3 + 2]);

            public static QColor darkGray = QColor.fromRGB(QColor.colorValues[23 * 3], QColor.colorValues[23 * 3 + 1],
                QColor.colorValues[23 * 3 + 2]);

            public static QColor darkGreen = QColor.fromRGB(QColor.colorValues[24 * 3], QColor.colorValues[24 * 3 + 1],
                QColor.colorValues[24 * 3 + 2]);

            public static QColor darkKhaki = QColor.fromRGB(QColor.colorValues[25 * 3], QColor.colorValues[25 * 3 + 1],
                QColor.colorValues[25 * 3 + 2]);

            public static QColor darkMagenta = QColor.fromRGB(QColor.colorValues[26 * 3],
                QColor.colorValues[26 * 3 + 1], QColor.colorValues[26 * 3 + 2]);

            public static QColor darkOliveGreen = QColor.fromRGB(QColor.colorValues[27 * 3],
                QColor.colorValues[27 * 3 + 1], QColor.colorValues[27 * 3 + 2]);

            public static QColor darkOrange = QColor.fromRGB(QColor.colorValues[28 * 3], QColor.colorValues[28 * 3 + 1],
                QColor.colorValues[28 * 3 + 2]);

            public static QColor darkOrchid = QColor.fromRGB(QColor.colorValues[29 * 3], QColor.colorValues[29 * 3 + 1],
                QColor.colorValues[29 * 3 + 2]);

            public static QColor darkRed = QColor.fromRGB(QColor.colorValues[30 * 3], QColor.colorValues[30 * 3 + 1],
                QColor.colorValues[30 * 3 + 2]);

            public static QColor darkSalmon = QColor.fromRGB(QColor.colorValues[31 * 3], QColor.colorValues[31 * 3 + 1],
                QColor.colorValues[31 * 3 + 2]);

            public static QColor darkSeaGreen = QColor.fromRGB(QColor.colorValues[32 * 3],
                QColor.colorValues[32 * 3 + 1], QColor.colorValues[32 * 3 + 2]);

            public static QColor darkSlateBlue = QColor.fromRGB(QColor.colorValues[33 * 3],
                QColor.colorValues[33 * 3 + 1], QColor.colorValues[33 * 3 + 2]);

            public static QColor darkSlateGray = QColor.fromRGB(QColor.colorValues[34 * 3],
                QColor.colorValues[34 * 3 + 1], QColor.colorValues[34 * 3 + 2]);

            public static QColor darkTurquoise = QColor.fromRGB(QColor.colorValues[35 * 3],
                QColor.colorValues[35 * 3 + 1], QColor.colorValues[35 * 3 + 2]);

            public static QColor darkViolet = QColor.fromRGB(QColor.colorValues[36 * 3], QColor.colorValues[36 * 3 + 1],
                QColor.colorValues[36 * 3 + 2]);

            public static QColor deepPink = QColor.fromRGB(QColor.colorValues[37 * 3], QColor.colorValues[37 * 3 + 1],
                QColor.colorValues[37 * 3 + 2]);

            public static QColor deepSkyBlue = QColor.fromRGB(QColor.colorValues[38 * 3],
                QColor.colorValues[38 * 3 + 1], QColor.colorValues[38 * 3 + 2]);

            public static QColor dimGray = QColor.fromRGB(QColor.colorValues[39 * 3], QColor.colorValues[39 * 3 + 1],
                QColor.colorValues[39 * 3 + 2]);

            public static QColor dodgerBlue = QColor.fromRGB(QColor.colorValues[40 * 3], QColor.colorValues[40 * 3 + 1],
                QColor.colorValues[40 * 3 + 2]);

            public static QColor fireBrick = QColor.fromRGB(QColor.colorValues[41 * 3], QColor.colorValues[41 * 3 + 1],
                QColor.colorValues[41 * 3 + 2]);

            public static QColor floralWhite = QColor.fromRGB(QColor.colorValues[42 * 3],
                QColor.colorValues[42 * 3 + 1], QColor.colorValues[42 * 3 + 2]);

            public static QColor forestGreen = QColor.fromRGB(QColor.colorValues[43 * 3],
                QColor.colorValues[43 * 3 + 1], QColor.colorValues[43 * 3 + 2]);

            public static QColor gainsboro = QColor.fromRGB(QColor.colorValues[44 * 3], QColor.colorValues[44 * 3 + 1],
                QColor.colorValues[44 * 3 + 2]);

            public static QColor ghostWhite = QColor.fromRGB(QColor.colorValues[45 * 3], QColor.colorValues[45 * 3 + 1],
                QColor.colorValues[45 * 3 + 2]);

            public static QColor gold = QColor.fromRGB(QColor.colorValues[46 * 3], QColor.colorValues[46 * 3 + 1],
                QColor.colorValues[46 * 3 + 2]);

            public static QColor goldenRod = QColor.fromRGB(QColor.colorValues[47 * 3], QColor.colorValues[47 * 3 + 1],
                QColor.colorValues[47 * 3 + 2]);

            public static QColor green = QColor.fromRGB(QColor.colorValues[48 * 3], QColor.colorValues[48 * 3 + 1],
                QColor.colorValues[48 * 3 + 2]);

            public static QColor greenYellow = QColor.fromRGB(QColor.colorValues[49 * 3],
                QColor.colorValues[49 * 3 + 1], QColor.colorValues[49 * 3 + 2]);

            public static QColor gray = QColor.fromRGB(QColor.colorValues[50 * 3], QColor.colorValues[50 * 3 + 1],
                QColor.colorValues[50 * 3 + 2]);

            public static QColor honeydew = QColor.fromRGB(QColor.colorValues[51 * 3], QColor.colorValues[51 * 3 + 1],
                QColor.colorValues[51 * 3 + 2]);

            public static QColor hotpink = QColor.fromRGB(QColor.colorValues[52 * 3], QColor.colorValues[52 * 3 + 1],
                QColor.colorValues[52 * 3 + 2]);

            public static QColor indianred = QColor.fromRGB(QColor.colorValues[53 * 3], QColor.colorValues[53 * 3 + 1],
                QColor.colorValues[53 * 3 + 2]);

            public static QColor ivory = QColor.fromRGB(QColor.colorValues[54 * 3], QColor.colorValues[54 * 3 + 1],
                QColor.colorValues[54 * 3 + 2]);

            public static QColor khaki = QColor.fromRGB(QColor.colorValues[55 * 3], QColor.colorValues[55 * 3 + 1],
                QColor.colorValues[55 * 3 + 2]);

            public static QColor lavender = QColor.fromRGB(QColor.colorValues[56 * 3], QColor.colorValues[56 * 3 + 1],
                QColor.colorValues[56 * 3 + 2]);

            public static QColor lavenderBlush = QColor.fromRGB(QColor.colorValues[57 * 3],
                QColor.colorValues[57 * 3 + 1], QColor.colorValues[57 * 3 + 2]);

            public static QColor lawnGreen = QColor.fromRGB(QColor.colorValues[58 * 3], QColor.colorValues[58 * 3 + 1],
                QColor.colorValues[58 * 3 + 2]);

            public static QColor lemonChiffon = QColor.fromRGB(QColor.colorValues[59 * 3],
                QColor.colorValues[59 * 3 + 1], QColor.colorValues[59 * 3 + 2]);

            public static QColor lightBlue = QColor.fromRGB(QColor.colorValues[60 * 3], QColor.colorValues[60 * 3 + 1],
                QColor.colorValues[60 * 3 + 2]);

            public static QColor lightCoral = QColor.fromRGB(QColor.colorValues[61 * 3], QColor.colorValues[61 * 3 + 1],
                QColor.colorValues[61 * 3 + 2]);

            public static QColor lightCyan = QColor.fromRGB(QColor.colorValues[62 * 3], QColor.colorValues[62 * 3 + 1],
                QColor.colorValues[62 * 3 + 2]);

            public static QColor lightGoldenrod = QColor.fromRGB(QColor.colorValues[63 * 3],
                QColor.colorValues[63 * 3 + 1], QColor.colorValues[63 * 3 + 2]);

            public static QColor lightGoldenRodYellow = QColor.fromRGB(QColor.colorValues[64 * 3],
                QColor.colorValues[64 * 3 + 1], QColor.colorValues[64 * 3 + 2]);

            public static QColor lightGray = QColor.fromRGB(QColor.colorValues[65 * 3], QColor.colorValues[65 * 3 + 1],
                QColor.colorValues[65 * 3 + 2]);

            public static QColor lightGreen = QColor.fromRGB(QColor.colorValues[66 * 3], QColor.colorValues[66 * 3 + 1],
                QColor.colorValues[66 * 3 + 2]);

            public static QColor lightPink = QColor.fromRGB(QColor.colorValues[67 * 3], QColor.colorValues[67 * 3 + 1],
                QColor.colorValues[67 * 3 + 2]);

            public static QColor lightSalmon = QColor.fromRGB(QColor.colorValues[68 * 3],
                QColor.colorValues[68 * 3 + 1], QColor.colorValues[68 * 3 + 2]);

            public static QColor lightSeaGreen = QColor.fromRGB(QColor.colorValues[69 * 3],
                QColor.colorValues[69 * 3 + 1], QColor.colorValues[69 * 3 + 2]);

            public static QColor lightSkyBlue = QColor.fromRGB(QColor.colorValues[70 * 3],
                QColor.colorValues[70 * 3 + 1], QColor.colorValues[70 * 3 + 2]);

            public static QColor lightSlateBlue = QColor.fromRGB(QColor.colorValues[71 * 3],
                QColor.colorValues[71 * 3 + 1], QColor.colorValues[71 * 3 + 2]);

            public static QColor lightSlateGray = QColor.fromRGB(QColor.colorValues[72 * 3],
                QColor.colorValues[72 * 3 + 1], QColor.colorValues[72 * 3 + 2]);

            public static QColor lightSteelBlue = QColor.fromRGB(QColor.colorValues[73 * 3],
                QColor.colorValues[73 * 3 + 1], QColor.colorValues[73 * 3 + 2]);

            public static QColor lightYellow = QColor.fromRGB(QColor.colorValues[74 * 3],
                QColor.colorValues[74 * 3 + 1], QColor.colorValues[74 * 3 + 2]);

            public static QColor limegReen = QColor.fromRGB(QColor.colorValues[75 * 3], QColor.colorValues[75 * 3 + 1],
                QColor.colorValues[75 * 3 + 2]);

            public static QColor linen = QColor.fromRGB(QColor.colorValues[76 * 3], QColor.colorValues[76 * 3 + 1],
                QColor.colorValues[76 * 3 + 2]);

            public static QColor magenta = QColor.fromRGB(QColor.colorValues[77 * 3], QColor.colorValues[77 * 3 + 1],
                QColor.colorValues[77 * 3 + 2]);

            public static QColor maroon = QColor.fromRGB(QColor.colorValues[78 * 3], QColor.colorValues[78 * 3 + 1],
                QColor.colorValues[78 * 3 + 2]);

            public static QColor midnightBlue = QColor.fromRGB(QColor.colorValues[79 * 3],
                QColor.colorValues[79 * 3 + 1], QColor.colorValues[79 * 3 + 2]);

            public static QColor mintCream = QColor.fromRGB(QColor.colorValues[80 * 3], QColor.colorValues[80 * 3 + 1],
                QColor.colorValues[80 * 3 + 2]);

            public static QColor mistyRose = QColor.fromRGB(QColor.colorValues[81 * 3], QColor.colorValues[81 * 3 + 1],
                QColor.colorValues[81 * 3 + 2]);

            public static QColor moccasin = QColor.fromRGB(QColor.colorValues[82 * 3], QColor.colorValues[82 * 3 + 1],
                QColor.colorValues[82 * 3 + 2]);

            public static QColor navajoWhite = QColor.fromRGB(QColor.colorValues[83 * 3],
                QColor.colorValues[83 * 3 + 1], QColor.colorValues[83 * 3 + 2]);

            public static QColor navy = QColor.fromRGB(QColor.colorValues[84 * 3], QColor.colorValues[84 * 3 + 1],
                QColor.colorValues[84 * 3 + 2]);

            public static QColor navyBlue = QColor.fromRGB(QColor.colorValues[85 * 3], QColor.colorValues[85 * 3 + 1],
                QColor.colorValues[85 * 3 + 2]);

            public static QColor oldLace = QColor.fromRGB(QColor.colorValues[86 * 3], QColor.colorValues[86 * 3 + 1],
                QColor.colorValues[86 * 3 + 2]);

            public static QColor oliveDrab = QColor.fromRGB(QColor.colorValues[87 * 3], QColor.colorValues[87 * 3 + 1],
                QColor.colorValues[87 * 3 + 2]);

            public static QColor orange = QColor.fromRGB(QColor.colorValues[88 * 3], QColor.colorValues[88 * 3 + 1],
                QColor.colorValues[88 * 3 + 2]);

            public static QColor orangeRed = QColor.fromRGB(QColor.colorValues[89 * 3], QColor.colorValues[89 * 3 + 1],
                QColor.colorValues[89 * 3 + 2]);

            public static QColor orchid = QColor.fromRGB(QColor.colorValues[90 * 3], QColor.colorValues[90 * 3 + 1],
                QColor.colorValues[90 * 3 + 2]);

            public static QColor paleGoldenRod = QColor.fromRGB(QColor.colorValues[91 * 3],
                QColor.colorValues[91 * 3 + 1], QColor.colorValues[91 * 3 + 2]);

            public static QColor paleGreen = QColor.fromRGB(QColor.colorValues[92 * 3], QColor.colorValues[92 * 3 + 1],
                QColor.colorValues[92 * 3 + 2]);

            public static QColor paleTurquoise = QColor.fromRGB(QColor.colorValues[93 * 3],
                QColor.colorValues[93 * 3 + 1], QColor.colorValues[93 * 3 + 2]);

            public static QColor paleVioletRed = QColor.fromRGB(QColor.colorValues[94 * 3],
                QColor.colorValues[94 * 3 + 1], QColor.colorValues[94 * 3 + 2]);

            public static QColor papayawhip = QColor.fromRGB(QColor.colorValues[95 * 3], QColor.colorValues[95 * 3 + 1],
                QColor.colorValues[95 * 3 + 2]);

            public static QColor peachPuff = QColor.fromRGB(QColor.colorValues[96 * 3], QColor.colorValues[96 * 3 + 1],
                QColor.colorValues[96 * 3 + 2]);

            public static QColor peru = QColor.fromRGB(QColor.colorValues[97 * 3], QColor.colorValues[97 * 3 + 1],
                QColor.colorValues[97 * 3 + 2]);

            public static QColor pink = QColor.fromRGB(QColor.colorValues[98 * 3], QColor.colorValues[98 * 3 + 1],
                QColor.colorValues[98 * 3 + 2]);

            public static QColor plum = QColor.fromRGB(QColor.colorValues[99 * 3], QColor.colorValues[99 * 3 + 1],
                QColor.colorValues[99 * 3 + 2]);

            public static QColor powderBlue = QColor.fromRGB(QColor.colorValues[100 * 3],
                QColor.colorValues[100 * 3 + 1], QColor.colorValues[100 * 3 + 2]);

            public static QColor purple = QColor.fromRGB(QColor.colorValues[101 * 3], QColor.colorValues[101 * 3 + 1],
                QColor.colorValues[101 * 3 + 2]);

            public static QColor red = QColor.fromRGB(QColor.colorValues[102 * 3], QColor.colorValues[102 * 3 + 1],
                QColor.colorValues[102 * 3 + 2]);

            public static QColor rosyBrown = QColor.fromRGB(QColor.colorValues[103 * 3],
                QColor.colorValues[103 * 3 + 1], QColor.colorValues[103 * 3 + 2]);

            public static QColor royalBlue = QColor.fromRGB(QColor.colorValues[104 * 3],
                QColor.colorValues[104 * 3 + 1], QColor.colorValues[104 * 3 + 2]);

            public static QColor saddleBrown = QColor.fromRGB(QColor.colorValues[105 * 3],
                QColor.colorValues[105 * 3 + 1], QColor.colorValues[105 * 3 + 2]);

            public static QColor salmon = QColor.fromRGB(QColor.colorValues[106 * 3], QColor.colorValues[106 * 3 + 1],
                QColor.colorValues[106 * 3 + 2]);

            public static QColor sandyBrown = QColor.fromRGB(QColor.colorValues[107 * 3],
                QColor.colorValues[107 * 3 + 1], QColor.colorValues[107 * 3 + 2]);

            public static QColor seaGreen = QColor.fromRGB(QColor.colorValues[108 * 3], QColor.colorValues[108 * 3 + 1],
                QColor.colorValues[108 * 3 + 2]);

            public static QColor seaShell = QColor.fromRGB(QColor.colorValues[109 * 3], QColor.colorValues[109 * 3 + 1],
                QColor.colorValues[109 * 3 + 2]);

            public static QColor sienna = QColor.fromRGB(QColor.colorValues[110 * 3], QColor.colorValues[110 * 3 + 1],
                QColor.colorValues[110 * 3 + 2]);

            public static QColor skyBlue = QColor.fromRGB(QColor.colorValues[111 * 3], QColor.colorValues[111 * 3 + 1],
                QColor.colorValues[111 * 3 + 2]);

            public static QColor slateBlue = QColor.fromRGB(QColor.colorValues[112 * 3],
                QColor.colorValues[112 * 3 + 1], QColor.colorValues[112 * 3 + 2]);

            public static QColor slateGray = QColor.fromRGB(QColor.colorValues[113 * 3],
                QColor.colorValues[113 * 3 + 1], QColor.colorValues[113 * 3 + 2]);

            public static QColor snow = QColor.fromRGB(QColor.colorValues[114 * 3], QColor.colorValues[114 * 3 + 1],
                QColor.colorValues[114 * 3 + 2]);

            public static QColor springGreen = QColor.fromRGB(QColor.colorValues[115 * 3],
                QColor.colorValues[115 * 3 + 1], QColor.colorValues[115 * 3 + 2]);

            public static QColor steelBlue = QColor.fromRGB(QColor.colorValues[116 * 3],
                QColor.colorValues[116 * 3 + 1], QColor.colorValues[116 * 3 + 2]);

            public static QColor tan = QColor.fromRGB(QColor.colorValues[117 * 3], QColor.colorValues[117 * 3 + 1],
                QColor.colorValues[117 * 3 + 2]);

            public static QColor thistle = QColor.fromRGB(QColor.colorValues[118 * 3], QColor.colorValues[118 * 3 + 1],
                QColor.colorValues[118 * 3 + 2]);

            public static QColor tomato = QColor.fromRGB(QColor.colorValues[119 * 3], QColor.colorValues[119 * 3 + 1],
                QColor.colorValues[119 * 3 + 2]);

            public static QColor turquoise = QColor.fromRGB(QColor.colorValues[120 * 3],
                QColor.colorValues[120 * 3 + 1], QColor.colorValues[120 * 3 + 2]);

            public static QColor violet = QColor.fromRGB(QColor.colorValues[121 * 3], QColor.colorValues[121 * 3 + 1],
                QColor.colorValues[121 * 3 + 2]);

            public static QColor violetred = QColor.fromRGB(QColor.colorValues[122 * 3],
                QColor.colorValues[122 * 3 + 1], QColor.colorValues[122 * 3 + 2]);

            public static QColor wheat = QColor.fromRGB(QColor.colorValues[123 * 3], QColor.colorValues[123 * 3 + 1],
                QColor.colorValues[123 * 3 + 2]);

            public static QColor white = QColor.fromRGB(QColor.colorValues[124 * 3], QColor.colorValues[124 * 3 + 1],
                QColor.colorValues[124 * 3 + 2]);

            public static QColor whiteSmoke = QColor.fromRGB(QColor.colorValues[125 * 3],
                QColor.colorValues[125 * 3 + 1], QColor.colorValues[125 * 3 + 2]);

            public static QColor yellow = QColor.fromRGB(QColor.colorValues[126 * 3], QColor.colorValues[126 * 3 + 1],
                QColor.colorValues[126 * 3 + 2]);

            public static QColor yellowGreen = QColor.fromRGB(QColor.colorValues[127 * 3],
                QColor.colorValues[127 * 3 + 1], QColor.colorValues[127 * 3 + 2]);
        }

        #endregion

        #endregion

        #region static from* methods

        /// <summary>
        /// Creates a new qColour
        /// </summary>
        /// <param name="red">0-255</param>
        /// <param name="green">0-255</param>
        /// <param name="blue">0-255</param>
        /// <returns></returns>
        public static QColor fromRGB(int red, int green, int blue)
        {
            QColor c;
            c.b = (byte) blue;
            c.g = (byte) green;
            c.r = (byte) red;
            c.a = 255;
            return c;
        }

        /// <summary>
        /// Creates a new qColour
        /// </summary>
        /// <param name="red">0-255</param>
        /// <param name="green">0-255</param>
        /// <param name="blue">0-255</param>
        /// <param name="alpha">0-255 (0=transparent, 255=solid)</param>
        /// <returns></returns>
        public static QColor fromRGBA(int red, int green, int blue, int alpha)
        {
            QColor c;
            c.b = (byte) blue;
            c.g = (byte) green;
            c.r = (byte) red;
            c.a = (byte) alpha;
            return c;
        }

        /// <summary>
        /// Creates a new qColour
        /// </summary>
        /// <param name="H">Hue</param>
        /// <param name="S">Saturation</param>
        /// <param name="V">Value</param>
        /// <returns></returns>
        public static QColor fromHSV(float H, float S, float V)
        {
            return fromHSVA(H, S, V, 255);
        }

        /// <summary>
        /// Creates a new qColour
        /// </summary>
        /// <param name="H">Hue</param>
        /// <param name="S">Saturation</param>
        /// <param name="V">Value</param>
        /// <param name="alpha">0-255 (0=transparent, 255=solid)</param>
        /// <returns></returns>
        public static QColor fromHSVA(float H, float S, float V, byte alpha)
        {
            QColor c;
            c.a = alpha;
            if (S == 0) //HSV values = From 0 to 1
            {
                c.r = (byte) (V * 255.0f); //RGB results = From 0 to 255
                c.g = (byte) (V * 255.0f);
                c.b = (byte) (V * 255.0f);
            }
            else
            {
                float var_h = H * 6;
                float var_i = (int) var_h; //Or ... var_i = floor( var_h )
                float var_1 = V * (1 - S);
                float var_2 = V * (1 - S * (var_h - var_i));
                float var_3 = V * (1 - S * (1 - (var_h - var_i)));

                float var_r = V, var_g = var_1, var_b = var_2;
                if (var_i == 0)
                {
                    var_r = V;
                    var_g = var_3;
                    var_b = var_1;
                }
                else if (var_i == 1)
                {
                    var_r = var_2;
                    var_g = V;
                    var_b = var_1;
                }
                else if (var_i == 2)
                {
                    var_r = var_1;
                    var_g = V;
                    var_b = var_3;
                }
                else if (var_i == 3)
                {
                    var_r = var_1;
                    var_g = var_2;
                    var_b = V;
                }
                else if (var_i == 4)
                {
                    var_r = var_3;
                    var_g = var_1;
                    var_b = V;
                }
                //else                   { var_r = V     ; var_g = var_1 ; var_b = var_2; }

                c.r = (byte) (var_r * 255.0f); //RGB results = From 0 to 255
                c.g = (byte) (var_g * 255.0f);
                c.b = (byte) (var_b * 255.0f);
            }

            return c;
        }

        /// <summary>
        /// Creates a colour from a string, Returns black when an error occurs
        /// String can be int the form (i)(i a)(r g b)(r g b a) OR a prenamed color string 
        /// </summary>
        /// <param name="s">String to parse</param>
        /// <returns></returns>
        public static QColor fromString(string s)
        {
            try
            {
                string[] elements = s.Trim().Replace("  ", " ").Split(" ".ToCharArray());

                if (s.IndexOfAny("0123456789".ToCharArray()) != -1)
                {
                    //numerical information string
                    switch (elements.Length)
                    {
                        case 0:
                            return QColor.fromRGB(0, 0, 0);
                        case 1:
                            return QColor.fromRGB(byte.Parse(elements[0]), byte.Parse(elements[0]),
                                byte.Parse(elements[0]));
                        case 2:
                            //assume c c c a (intensity + opacity)
                            return QColor.fromRGBA(byte.Parse(elements[0]), byte.Parse(elements[0]),
                                byte.Parse(elements[0]), byte.Parse(elements[1]));
                        case 3:
                            return QColor.fromRGB(byte.Parse(elements[0]), byte.Parse(elements[1]),
                                byte.Parse(elements[2]));
                        default:
                            //four or more
                            return QColor.fromRGBA(byte.Parse(elements[0]), byte.Parse(elements[1]),
                                byte.Parse(elements[2]), byte.Parse(elements[2]));
                    }
                }
                else
                {
                    //non numerical string
                    QColor c = QColor.fromRGB(0, 0, 0);
                    string lowerCase = s.ToLower();
                    int i;
                    for (i = 0; i < colorNames.Length; i++)
                    {
                        if (lowerCase.CompareTo(colorNames[i]) == 0)
                        {
                            //unsafe
                            {
                                c.r = colorValues[i * 3];
                                c.g = colorValues[i * 3 + 1];
                                c.b = colorValues[i * 3 + 2];
                                c.a = 255;
                            }
                        }
                    }

                    return c;
                }
            }
            catch
            {
                return QColor.fromRGB(0, 0, 0);
            }
        }

        #endregion

        #region operators

        /// <summary>
        /// Cast to a windows colour
        /// </summary>
        /// <param name="c">a qColor</param>
        /// <returns>a windows colour</returns>
#if UNSAFE
        public static unsafe explicit operator System.Drawing.Color(QColor c)
		{
		//FIX:
		return System.Drawing.Color.FromArgb(*((int*)&c.b));
		//return Color.FromArgb(c.a, c.r, c.g, c.b);
		}
#else
        public static explicit operator System.Drawing.Color(QColor c)
        {
            return Color.FromArgb(c.a, c.r, c.g, c.b);
        }
#endif

        /// <summary>
        /// Cast from a windows colour
        /// </summary>
        /// <param name="c">a windows colour</param>
        /// <returns>a qColor</returns>
        public static implicit operator QColor(System.Drawing.Color c)
        {
            QColor r;
            r.a = c.A;
            r.r = c.R;
            r.g = c.G;
            r.b = c.B;
            return r;
        }

        #endregion

        #region internal functions

        private float min(float a, float b, float c)
        {
            return min(min(a, b), c);
        }

        private float min(float a, float b)
        {
            return (a < b) ? a : b;
        }

        private float max(float a, float b, float c)
        {
            return max(max(a, b), c);
        }

        private float max(float a, float b)
        {
            return (a > b) ? a : b;
        }

        #endregion

        #region to* converters

        /// <summary>
        /// Returns Hue saturation and value information
        /// </summary>
        /// <param name="H">Hue</param>
        /// <param name="S">Saturation</param>
        /// <param name="V">Value</param>
        public void toHSV(out float H, out float S, out float V)
        {
            float var_R = ((float) r / 255.0f); //RGB values = From 0 to 255
            float var_G = ((float) g / 255.0f);
            float var_B = ((float) b / 255.0f);

            float var_Min = min(var_R, var_G, var_B); //Min. value of RGB
            float var_Max = max(var_R, var_G, var_B); //Max. value of RGB
            float del_Max = var_Max - var_Min; //Delta RGB value

            V = var_Max;

            if (del_Max == 0) //This is a gray, no chroma...
            {
                H = 0; //HSV results = From 0 to 1
                S = 0;
            }
            else //Chromatic data...
            {
                S = del_Max / var_Max;

                float del_R = (((var_Max - var_R) / 6) + (del_Max / 2)) / del_Max;
                float del_G = (((var_Max - var_G) / 6) + (del_Max / 2)) / del_Max;
                float del_B = (((var_Max - var_B) / 6) + (del_Max / 2)) / del_Max;

                if (var_R == var_Max) H = del_B - del_G;
                else if (var_G == var_Max) H = (1 / 3) + del_R - del_B;
                else //if ( var_B == var_Max ) 
                    H = (2 / 3) + del_G - del_R;

                if (H < 0) H += 1;
                if (H > 1) H -= 1;
            }
        }

        /// <summary>
        /// Returns a string representing the colour.
        /// </summary>
        /// <returns>A sring in the format "r g b a"</returns>
        public override string ToString()
        {
            return String.Format("{0} {1} {2} {3}", r, g, b, a);
        }

        /// <summary>
        /// Returns the english name of the colour.
        /// </summary>
        /// <returns>An named version of the colour or custom if no name exists.</returns>
        public string toName()
        {
            int i;
            for (i = 0; i < colorValues.Length; i += 3)
            {
                if (colorValues[i] == r)
                    if (colorValues[i + 1] == g)
                        if (colorValues[i + 2] == b)
                        {
                            return colorNames[i / 3];
                        }
            }

            return "custom";
        }

        #endregion

        #region random generation

        /// <summary>
        /// Generates an array of random colours (with random opacity).
        /// </summary>
        /// <param name="amount">Number of colours to generate.</param>
        /// <returns>An array of random colours.</returns>
        public static QColor[] generateRandomColors(int amount)
        {
            Random r = new Random();
            return generateRandomColors(ref r, amount);
        }

        /// <summary>
        /// Generates an array of random colours (with random opacity).
        /// </summary>
        /// <param name="seed">seed value for random colour generation.</param>
        /// <param name="amount">Number of colours to generate.</param>
        /// <returns>An array of random colours.</returns>
        public static QColor[] generateRandomColors(int seed, int amount)
        {
            Random r = new Random(seed);
            return generateRandomColors(ref r, amount);
        }

        /// <summary>
        /// Generates an array of random colours (with random opacity).
        /// </summary>
        /// <param name="rGenerator">A random number generator to use.</param>
        /// <param name="amount">Number of colours to generate.</param>
        /// <returns>An array of random colours.</returns>
        public static QColor[] generateRandomColors(ref System.Random rGenerator, int amount)
        {
            QColor[] colors = new QColor[amount];
            int i;
            for (i = 0; i < amount; i++)
                colors[i] = generateRandom(rGenerator);
            return colors;
        }

        /// <summary>
        /// Generates an array of random colours (with full opacity).
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static QColor[] generateRandomColorsOpaque(int amount)
        {
            Random r = new Random();
            return generateRandomColorsOpaque(ref r, amount);
        }

        /// <summary>
        /// Generates an array of random colours (with full opacity).
        /// </summary>
        /// <param name="seed"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static QColor[] generateRandomColorsOpaque(int seed, int amount)
        {
            Random r = new Random(seed);
            return generateRandomColorsOpaque(ref r, amount);
        }

        /// <summary>
        /// Generates an array of random colours (with full opacity).
        /// </summary>
        /// <param name="rGenerator"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static QColor[] generateRandomColorsOpaque(ref System.Random rGenerator, int amount)
        {
            QColor[] colors = new QColor[amount];
            int i;
            for (i = 0; i < amount; i++)
                colors[i] = generateRandomOpaque(ref rGenerator);
            return colors;
        }


        /// <summary>
        /// Generates an array of random colours (with random opacity).
        /// </summary>
        /// <returns></returns>
        public static QColor generateRandom()
        {
            return generateRandom(QColor.colorRand);
        }

        /// <summary>
        /// Generates an array of random colours (with random opacity).
        /// </summary>
        /// <param name="rGenerator"></param>
        /// <returns></returns>
        public static QColor generateRandom(System.Random rGenerator)
        {
            return QColor.fromRGBA(rGenerator.Next(0, 255), rGenerator.Next(0, 255), rGenerator.Next(0, 255),
                rGenerator.Next(0, 255));
        }

        /// <summary>
        /// Generates an array of random colours (with random opacity).
        /// </summary>
        /// <returns></returns>
        public static QColor generateRandomOpaque(int min, int max)
        {
            min = Range.clamp(min, 0, 255);
            max = Range.clamp(max, 0, 255);
            return QColor.fromRGBA(QColor.colorRand.Next(min, max), QColor.colorRand.Next(min, max),
                QColor.colorRand.Next(min, max), 255);
        }


        /// <summary>
        /// Generates an array of random colours (with full opacity).
        /// </summary>
        /// <param name="rGenerator"></param>
        /// <returns></returns>
        public static QColor generateRandomOpaque(ref System.Random rGenerator)
        {
            return QColor.fromRGB(rGenerator.Next(0, 255), rGenerator.Next(0, 255), rGenerator.Next(0, 255));
        }

        #endregion

        #region IFormattable Members

        /// <summary>
        /// Returns a string representing the colour.
        /// </summary>
        /// <param name="format">unused</param>
        /// <param name="formatProvider">an IFormatProvider, used when turning byte values into strings.</param>
        /// <returns>A sring in the format "r g b a"</returns>
        string System.IFormattable.ToString(string format, IFormatProvider formatProvider)
        {
            return String.Format(formatProvider, "{0} {1} {2} {3}", r, g, b, a);
        }

        #endregion

        #region mixing

        public static QColor mix(QColor a, QColor b, float per)
        {
            QColor c = new QColor();
            c.a = (byte) (a.a + (byte) ((b.a - a.a) * per));
            c.r = (byte) (a.r + (byte) ((b.r - a.r) * per));
            c.g = (byte) (a.g + (byte) ((b.g - a.g) * per));
            c.b = (byte) (a.b + (byte) ((b.b - a.b) * per));
            return c;
        }

        public QColor mix(QColor a, float per)
        {
            return QColor.mix(this, a, per);
        }


        public static QColor rgbMix(QColor a, QColor b, float per)
        {
            QColor c = new QColor();
            c.a = a.a;
            c.r = (byte) (a.r + (byte) ((b.r - a.r) * per));
            c.g = (byte) (a.g + (byte) ((b.g - a.g) * per));
            c.b = (byte) (a.b + (byte) ((b.b - a.b) * per));
            return c;
        }

        public QColor rgbMix(QColor a, float per)
        {
            return QColor.rgbMix(this, a, per);
        }

        #endregion

        #region helpers

        private static int abs(int a, int b)
        {
            if (a > b)
                return a - b;
            else
                return b - a;
        }

        #endregion

        #region stats

        /// <summary>
        /// Calculates the colorant contrast of a colour. This is a metric used in many colour calculations.
        /// </summary>
        /// <param name="b">A colour to be mesured.</param>
        /// <returns>The Colorant Contrast of a colour.</returns>
        public float mesureColorantContrast(QColor b)
        {
            return (QColor.abs(r, b.r) + QColor.abs(g, b.g) + QColor.abs(this.b, b.b)) * (1.0f / (3.0f * 255.0f));
        }

        #endregion
    }
}