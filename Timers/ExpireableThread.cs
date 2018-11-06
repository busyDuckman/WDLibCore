/*
 * The following code is Copyright 2018 Dr Warren Creemers (busyDuckman)
 * See LICENSE.md for more information.
 */

using System;
using System.Threading;

namespace WDToolbox.Timers
{
    /// <summary>
    /// Runs a process, but kills it after a set amount of time.
    /// </summary>
    public class ExpireableThread
    {
        protected TimeSpan life;
        protected Thread monitor;
        protected Thread process;
        protected volatile bool refresh = false;

        /// <summary>
        /// Runs a ExpireableThread thread.
        /// </summary>
        public ExpireableThread(TimeSpan life, ThreadStart ts)
        {
            this.life = life;
            process = new Thread(ts);
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void start()
        {
            monitor = new Thread(new ThreadStart(monitorThread));
            process.Start();
            monitor.Start();
        }

        /// <summary>
        /// resets the experation count down.
        /// </summary>
        public void restart()
        {
            refresh = true;
        }

        /// <summary>
        /// A Thread to kill the main process.
        /// </summary>
        protected void monitorThread()
        {
            TimeSpan ts = TimeSpan.Zero;
            while ((ts < life) && (process.IsAlive))
            {
                Thread.Sleep(1);
                if (refresh)
                {
                    ts = TimeSpan.Zero;
                }
            }

            if (process.IsAlive)
            {
                try
                {
                    process.Abort();
                }
                catch (Exception ex)
                {
                    if (process.IsAlive)
                        throw ex;
                }
            }
        }
    }
}