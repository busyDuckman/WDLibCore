/*
 * The following code is Copyright 2018 Dr Warren Creemers (busyDuckman)
 * See LICENSE.md for more information.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WDToolbox.AplicationFramework;

namespace WDToolbox//.DotNetExtension
{
    public static class IDisposableExtension
    {
        public delegate void ExceptionThrownDelegate(Exception ex);
        
        /// <summary>
        /// When the proverbial has hit the fan, you just want your clean up code  o try and dispose and move on.
        /// This makes that a one-liner.
        /// </summary>
        public static void TryDispose(this IDisposable item, ExceptionThrownDelegate onFail)
        {
            if (item == null)
            {
                return;
            }
            try
            {
                item.Dispose();
            }
            catch(Exception ex)
            {
                if (onFail != null)
                {
                    onFail(ex);
                }
            }
        }

        /// <summary>
        /// When the proverbial has hit the fan, you just want your clean up code  o try and dispose and move on.
        /// This makes that a one-liner.
        /// </summary>
        public static void TryDispose(this IDisposable item, bool logError=true)
        {
            if (item == null)
            {
                return;
            }

            try
            {
                item.Dispose();
            }
            catch(Exception ex)
            {
                if (logError)
                {
                    WDAppLog.logException(ErrorLevel.Error, ex);
                }
            }
        }
    }
}
