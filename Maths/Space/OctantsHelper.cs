using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;

namespace WDToolbox.Maths.Space
{
    /// <summary>
    /// Helpers for Octants2D
    /// </summary>
    public static class OctantsHelper
    {
        private static readonly Point[] getOffsetsCache = new Point[]
        {
            new Point(0, 0), //none
            new Point(0, 1), //top
            new Point(0, -1), //down
            new Point(-1, 0), //left
            new Point(1, 0), //right
            new Point(-1, 1), //top left
            new Point(1, 1), //top right
            new Point(-1, -1), //bottom left
            new Point(1, -1) //bottom right
        };

        /// <summary>
        /// Gets a set of 2d offsets as specified by the direction.
        /// The magnitude of the offsets is NOT always one.
        /// NOTE: Top right is designated (+1, +1)
        /// </summary>
        /// <param name="dir">The dir flag.</param>
        /// <returns>A list of points</returns>
        public static Point[] GetOffsets(Octants2D dir)
        {
            List<Point> offsets = new List<Point>();
            BitVector32 bv = new BitVector32((int) dir);
            int i;
            for (i = 1; i < getOffsetsCache.Length; i++)
            {
                if (bv[1 << (i - 1)])
                {
                    offsets.Add(getOffsetsCache[i]);
                }
            }

            return offsets.ToArray();
        }
    }
}