/*
 * The following code is Copyright 2018 Dr Warren Creemers (busyDuckman)
 * See LICENSE.md for more information.
 */
using System;
using System.IO;
using System.Text;

namespace WDToolbox.Data.Text
{
    /// <summary>
    /// A stream handler that redirects all characters written to an event handler.
    /// For the times when you have a stream, but wanted events.
    /// </summary>
    public class TextWriter2Event : TextWriter
    {
        public override Encoding Encoding
        {
            get { return Encoding.ASCII; }
        }

        StringBuilder currentLine = new StringBuilder();

        public EventHandler<char> OnChar { get; set; }
        public EventHandler<string> OnLine { get; set; }

        public TextWriter2Event(EventHandler<string> _onLine)
        {
            this.OnLine = _onLine;
        }

        public TextWriter2Event(EventHandler<char> _onChar)
        {
            this.OnChar = _onChar;
        }

        /// <summary>
        /// Apparently this is the only method in TextWriter that needs to be overridden.
        /// AQll other write methods call this.
        /// </summary>
        /// <param name="value"></param>
        public override void Write(char value)
        {
            OnChar.SafeCall(this, value);

            if ((value == '\r') || (value == '\n'))
            {
                if (currentLine.Length > 0)
                {
                    OnLine.SafeCall(this, currentLine.ToString());
                }
                currentLine.Clear();
            }
            else
            {
                currentLine.Append(value);
            }
        }
    }
}
