/*
 * The following code is Copyright 2018 Dr Warren Creemers (busyDuckman)
 * See LICENSE.md for more information.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WDToolbox.AplicationFramework;

namespace WDToolbox
{
    public static class EventHandlerExtension
    {
        /// <summary>
        /// Calls an event, and guarantees code will continue.
        /// Useful for controls that generate events, and want to work regardless of issues in the event handler. 
        /// </summary>
        public static bool SafeCall(this EventHandler e, object sender, Action<Exception> onCallException)
        {
            if (e != null)
            {
                try
                {
                    e(sender, null);
                    return true;
                }
                catch (Exception ex)
                {
                    try
                    {
                        if (onCallException != null)
                        {
                            onCallException(ex);
                        }
                    }
                    catch (Exception ex2)
                    {
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Calls an event, and guarantees code will continue.
        /// Useful for controls that generate events, and want to work regardless of issues in the event handler. 
        /// </summary>
        public static bool SafeCall(this EventHandler e, object sender, bool logCallException = false)
        {
            if (e != null)
            {
                try
                {
                    e(sender, null);
                    return true;
                }
                catch (Exception ex)
                {
                    if (logCallException)
                    {
                        WDAppLog.logException(ErrorLevel.Error, ex);
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Calls an event, and guarantees code will continue.
        /// Useful for controls that generate events, and want to work regardless of issues in the event handler. 
        /// </summary>
        public static bool SafeCall<T>(this EventHandler<T> e, object sender, T arg, Action<Exception> onCallException)
        {
            if(e != null)
            {
                try
                {
                    e(sender, arg);
                    return true;
                }
                catch (Exception ex)
                {
                    try
                    {
                        if (onCallException != null)
                        {
                            onCallException(ex);
                        }
                    }
                    catch (Exception ex2)
                    {
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Calls an event, and guarantees code will continue.
        /// Useful for controls that generate events, and want to work regardless of issues in the event handler. 
        /// </summary>
        public static bool SafeCall<T>(this EventHandler<T> e, object sender, T arg, bool logCallException = false)
        {
            if (e != null)
            {
                try
                {
                    e(sender, arg);
                    return true;
                }
                catch (Exception ex)
                {
                    if (logCallException)
                    {
                        WDAppLog.logException(ErrorLevel.Error, ex);
                    }
                }
            }
            return false;
        }


    }
}
