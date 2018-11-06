/*
 * The following code is Copyright 2018 Dr Warren Creemers (busyDuckman)
 * See LICENSE.md for more information.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WDToolbox.Maths.Geometry;
using WDToolbox.Maths.Geometry.Shapes;
using Color = System.Drawing.Color;

namespace WDToolbox.Rendering.Geometry
{
    public interface IRenderableGeometry<T>
        where T : IGeometry
    {
        T Geomerty { get; }
        TransMatrix2D TransMatrix { get; set; }
        Rectangle2D BoundingBoxWS { get; }
    }
    public interface IRenderablePath<P> : IRenderableGeometry<P>
        where P : IPath
    {
    }
    public interface IRenderableShape<S> : IRenderableGeometry<S>
        where S : IShape
    {
    }



        public abstract class RenderableGeometry<T> : IRenderableGeometry<T>
        where T : IGeometry
    {
        public T Geomerty { get; protected set; }
        public TransMatrix2D TransMatrix { get; set; }
        public Rectangle2D BoundingBoxWS { get { return TransMatrix.Transform(Geomerty.BoundingBox).BoundingBox; } }



        public RenderableGeometry(T geometry)
        {
            TransMatrix = TransMatrix2D.Identity;
            this.Geomerty = geometry;
        }
    }
    public class RenderablePath<P> : RenderableGeometry<P>
        where P : IPath
    {
        public Color LineColor { get; set; }
        public Color Pointolor { get; set; }
        public int LineWidth { get; set; }
        public int PointSize { get; set; }

        public RenderablePath(P path) : base(path)
        {

        }
    }
    public class RenderableShape<S> : RenderablePath<S>
        where S : IShape
    {
        public Color FillColor { get; set; }

        public RenderableShape(S shape) : base(shape)
        {

        }
    }
}
