/*
 * The following code is Copyright 2018 Dr Warren Creemers (busyDuckman)
 * See LICENSE.md for more information.
 */

using System;

namespace WDToolbox.Maths.Space
{
    /// <summary>
    /// Represents one or more octants.
    /// </summary>
    [Flags]
    public enum Octants2D
    {
        None = 0,
        Up = 1,
        Down = 2,
        Left = 4,
        Right = 8,
        TopLeft = 16,
        TopRight = 32,
        BottomLeft = 64,
        BottomRight = 128,
        
        Orthogonal = Up | Down | Left | Right,
        Diagonals = TopLeft | TopRight | BottomLeft | BottomRight,
        All = Up | Down | Left | Right | TopLeft | TopRight | BottomLeft | BottomRight
    };
}
