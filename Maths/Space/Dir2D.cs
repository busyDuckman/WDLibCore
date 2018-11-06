/*
 * The following code is Copyright 2018 Dr Warren Creemers (busyDuckman)
 * See LICENSE.md for more information.
 */

using System;

namespace WDToolbox.Maths.Space
{
    /// <summary>
    ///Represents one or more directios.
    /// Used in the Point Extension and OrthoDirection.
    /// </summary>
    [Flags]
    public enum Dir2D
    {
        None = 0,
        Up = 1,
        Down = 2,
        Left = 4,
        Right = 8,
        
        UpDown = Up | Down,
        LeftRight = Left | Right
    };

}