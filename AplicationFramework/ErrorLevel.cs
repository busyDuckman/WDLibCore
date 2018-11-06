/*
 * The following code is Copyright 2018 Dr Warren Creemers (busyDuckman)
 * See LICENSE.md for more information.
 */
namespace WDToolbox.AplicationFramework
{
    /// <summary>
    /// Error types.
    /// </summary>
    public enum ErrorLevel : byte
    {
        /// <summary> debugging related information, not necessarily an error </summary>
        Debug = 0,

        /// <summary> A behaviour seemed unusual but not necessary invalid</summary>
        Warning,

        /// <summary> error was small and mostly internalised, user was unlikely to notice</summary>
        SmallError,

        /// <summary> The application suffered a serious error BUT DID NOT SHUT DOWN. </summary>
        Error,

        /// <summary> Critical error, the application had to close.</summary>
        TerminalError,

        /// <summary> The system may no longer be stable (computer needs rebooting)</summary>
        SystemError
    };
}
