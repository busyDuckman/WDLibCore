/*
 * The following code is Copyright 2018 Dr Warren Creemers (busyDuckman)
 * See LICENSE.md for more information.
 */

namespace WDToolbox.AplicationFramework
{
    /// <summary>
    /// Used in FileBoundData.
    /// Represents an object that can be reloaded from a file.
    /// </summary>
    public interface IReloadFile
    {
        bool ReloadFile(string path);
    }
}
