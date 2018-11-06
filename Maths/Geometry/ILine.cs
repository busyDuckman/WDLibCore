/*
 * The following code is Copyright 2018 Dr Warren Creemers (busyDuckman)
 * See LICENSE.md for more information.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WDToolbox.Maths.Geometry
{
    public interface ILine : IOpenPath
    {
        Point2D VecFromStartToEnd();

        Point2D UnitVecFromStartToEnd();

        double LengthSquared();

        double Length();

        bool IsVertical { get; }

        bool IsHorizontal { get; }

        bool EqualsIgnoreDirection(ILine other);
    }
}
