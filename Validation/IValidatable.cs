/*
 * The following code is Copyright 2018 Dr Warren Creemers (busyDuckman)
 * See LICENSE.md for more information.
 */
using WDToolbox.Data.DataStructures;

namespace WDToolbox.Validation
{
    public interface IValidatable
    {
        Why Valid();
    }
}
