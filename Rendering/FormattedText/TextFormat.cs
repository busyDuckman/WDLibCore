/*
 * The following code is Copyright 2018 Dr Warren Creemers (busyDuckman)
 * See LICENSE.md for more information.
 */

using System;
using System.Drawing;
using System.Reflection.Metadata;
using WDToolbox.Maths.Space;

namespace WDToolbox.Rendering.FormattedText
{
    /// <summary>
    /// Encapsulates properties concerning the representation of text
    /// </summary>
    [Serializable]
    public class TextFormat : ICloneable, IDisposable
    {
        private Font _font;

        public Font font
        {
            get { return _font; }
            set { _font = value; }
        }

        private bool _showShadow;

        public bool showShadow
        {
            get { return _showShadow; }
            set { _showShadow = value; }
        }

        private bool _printVertical;

        public bool printVertical
        {
            get { return _printVertical; }
            set { _printVertical = value; }
        }

        private Color _textColour;

        public Color textColour
        {
            get { return _textColour; }
            set { _textColour = value; }
        }

        private Color _shadowColour;

        public Color shadowColour
        {
            get { return _shadowColour; }
            set { _shadowColour = value; }
        }


        private Octants2D _shadowDir = Octants2D.BottomRight;

        public Octants2D shadowDir
        {
            get { return _shadowDir; }
            set
            {
                _shadowDir = value;
                shadowOffsets = OctantsHelper.GetOffsets(_shadowDir);
            }
        }

        [NonSerialized] Point[] shadowOffsets = null;


        public TextFormat()
        {
            font = new Font("Ariel", 16);
            showShadow = false;
            printVertical = false;
            textColour = Color.Black;
            shadowColour = Color.FromArgb(128, Color.Black);
        }

        public TextFormat(Font font, Color textColour) : this(font, textColour, false, Color.FromArgb(128, Color.Black),
            false)
        {
        }

        public TextFormat(Font font, Color textColour, bool showShadow, Color shadowColour, bool printVertical)
        {
            this.font = (Font) font.Clone();
            this.showShadow = showShadow;
            this.printVertical = printVertical;
            this.textColour = textColour;
            this.shadowColour = shadowColour;
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="another">textStyle to copy</param>
        protected TextFormat(TextFormat another)
        {
            font = (Font) another.font.Clone();
            showShadow = another.showShadow;
            printVertical = another.printVertical;
            textColour = another.textColour;
            shadowColour = another.shadowColour;
        }

        public Brush generateBrush()
        {
            return new SolidBrush(textColour);
        }

        public Brush generateShadowBrush()
        {
            return new SolidBrush(shadowColour);
        }

        /// <summary>
        /// Renders text using the style (not finished yet).
        /// </summary>
        /// <param name="g"></param>
        /// <param name="s"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void render(Graphics g, string s, int x, int y)
        {
            if (shadowOffsets == null)
            {
                this.shadowOffsets = OctantsHelper.GetOffsets(this.shadowDir);
            }

            //draw the shadow
            int i;
            for (i = 0; i < shadowOffsets.Length; i++)
            {
                g.DrawString(s, this.font, this.generateShadowBrush(), x + shadowOffsets[i].X, y + shadowOffsets[i].Y);
            }


            g.DrawString(s, this.font, this.generateBrush(), x, y);
        }

        /// <summary>
        /// Renders text using the style (not finished yet).
        /// </summary>
        /// <param name="g"></param>
        /// <param name="s"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void render(IRenderer r, string s, int x, int y)
        {
            if (shadowOffsets == null)
            {
                this.shadowOffsets = OctantsHelper.GetOffsets(this.shadowDir);
            }

            //draw the shadow
            int i;
            for (i = 0; i < shadowOffsets.Length; i++)
            {
                r.DrawString(shadowColour, s, this.font, x + shadowOffsets[i].X, y + shadowOffsets[i].Y);
            }


            r.DrawString(textColour, s, this.font, x, y);
        }

        #region ICloneable Members

        /// <summary>
        /// Makes a clone of this object.
        /// </summary>
        /// <returns>clone of this object</returns>
        public object Clone()
        {
            return new TextFormat(this);
        }

        #endregion

        public void Dispose()
        {
            _font.Dispose();
        }
    }
}