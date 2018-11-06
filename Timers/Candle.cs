/*
 * The following code is Copyright 2018 Dr Warren Creemers (busyDuckman)
 * See LICENSE.md for more information.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WDToolbox.Timers
{
    /// <summary>
    /// Burns for so long (its a stopwatch type thing to time out loops)
    /// </summary>
    /// <example>
    /// Candle ok = Candle.StartNewFromSeconds(5);
    /// while(data.more() && ok) {
    ///   ...
    /// }
    /// </example>
    public class Candle
    {
        public DateTime StartTime { get; protected set; }
        public TimeSpan Duration { get; protected set; }

        public bool BurntOut
        {
            get { return (DateTime.Now - StartTime) >= Duration; }
        }

        public bool IsBurning
        {
            get { return (DateTime.Now - StartTime) < Duration; }
        }

        protected Candle(TimeSpan duration)
        {
            this.Duration = duration;
        }

        public static Candle StartNew(TimeSpan duration)
        {
            Candle c = new Candle(duration);
            c.ReStartAsync();
            return c;
        }

        public static Candle StartNewFromMs(int ms)
        {
            return StartNew(TimeSpan.FromMilliseconds(ms));
        }

        public static Candle StartNewFromSeconds(int sec)
        {
            return StartNew(TimeSpan.FromSeconds(sec));
        }

        public void ReStartAsync()
        {
            StartTime = DateTime.Now;
        }

        public static implicit operator bool(Candle c)
        {
            return c.IsBurning;
        }
    }
}