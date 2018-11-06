/*
 * The following code is Copyright 2018 Dr Warren Creemers (busyDuckman)
 * See LICENSE.md for more information.
 */

using WDToolbox.Maths.Geometry;
using WDToolbox.Maths.Geometry.Shapes;

namespace WDToolbox.Rendering.Geometry
{
    public interface IGeometeryRenderer<G,GT,P,PT,S,ST>
        where G : IRenderableGeometry<GT>
        where GT : IGeometry
        where P : IRenderablePath<PT>
        where PT : IPath
        where S : IRenderableShape<ST>
        where ST : IShape
    {
        void Render<T>(IRenderer r, S s)
            where T : IShape;

        void Render<T>(IRenderer r, T s)
            where T : IPath;

        void Render<T>(IRenderer r, G g)
            where T : IGeometry;
    }
}