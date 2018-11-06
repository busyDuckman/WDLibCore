/*
 * The following code is Copyright 2018 Dr Warren Creemers (busyDuckman)
 * See LICENSE.md for more information.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WDToolbox.Data
{
    /// <summary>
    /// Represents something that has a version.
    /// </summary>
    public interface IHasVersion
    {
        Version Version { get;  }
    }
}
