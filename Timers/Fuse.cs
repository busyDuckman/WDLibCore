/*
 * The following code is Copyright 2018 Dr Warren Creemers (busyDuckman)
 * See LICENSE.md for more information.
 */

using System;
using System.Threading;

namespace WDToolbox.Timers
{
    /// <summary>
    /// Counts down then runs a process.
    /// </summary>
    public class Fuse
    {
        /// <summary>
        /// Process to run.
        /// </summary>
        protected ThreadStart ts;

        /// <summary>
        /// Delay before running process.
        /// </summary>
        protected TimeSpan delay;

        private volatile bool active = false;

        /// <summary>
        /// Creates a new fuse.
        /// </summary>
        /// <param name="ts">Process to run.</param>
        /// <param name="delaySeconds">Delay before running process, in seconds.</param>
        public Fuse(ThreadStart ts, int delaySeconds) : this(ts, TimeSpan.FromSeconds(delaySeconds))
        {
        }

        /// <summary>
        /// Creates a new fuse.
        /// </summary>
        /// <param name="ts">Process to run.</param>
        /// <param name="delay">Delay before running process.</param>
        public Fuse(ThreadStart ts, TimeSpan delay)
        {
            this.ts = ts;
            this.delay = delay;
        }

        /// <summary>
        /// Starts the fuse. This function returns afer fuse has expired and the assosiated process has finished;
        /// </summary>
        public void start()
        {
            if (!active)
            {
                active = true;
                Thread t = new Thread(ts);
                Thread.Sleep(delay);
                t.Start();
                while (t.IsAlive)
                {
                    Thread.Sleep(1);
                }

                active = false;
            }
        }

        /// <summary>
        /// Starts the fuse asyncronsly.
        /// </summary>
        public void light()
        {
            //lock(active)
            {
                //if(!active)
                {
                    Thread t = new Thread(new ThreadStart(start));
                    t.Start();
                }
            }
        }
    }
}