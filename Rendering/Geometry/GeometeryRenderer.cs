/*
 * The following code is Copyright 2018 Dr Warren Creemers (busyDuckman)
 * See LICENSE.md for more information.
 */
using System;
using System.Collections.Generic;
//using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WDToolbox.Maths.Geometry;
using WDToolbox.Maths.Geometry.Shapes;
using WDToolbox.Maths.Geometry.Shapes.Basic;
using Color = System.Drawing.Color;

namespace WDToolbox.Rendering.Geometry
{
    public class TypeSwitch
    {
        Dictionary<Type, Action<object>> matches = new Dictionary<Type, Action<object>>();
        public TypeSwitch Case<T>(Action<T> action) { matches.Add(typeof(T), (x) => action((T)x)); return this; }
        public void Switch(object x) { matches[x.GetType()](x); }
    }

    public class GeometeryRenderer<GT, PT, ST> :
        IGeometeryRenderer< IRenderableGeometry<GT>, GT,
                            IRenderablePath<PT>, PT,
                            IRenderableShape<ST>, ST>
        where GT : IGeometry
        where PT : IPath
        where ST : IShape
    {
        public GeometeryRenderer()
        {
            GDIPlusRenderer r = new GDIPlusRenderer(100, 100);
            Rectangle rec = new Rectangle(0,0,100,100);

            RenderableShape<IShape> rs = new RenderableShape<IShape>(rec);
            //Render(r, rs);

            RenderableShape<Rectangle> rs2 = new RenderableShape<Rectangle>(rec);
            //Render(r, rs2);
        }

        public void Render<T>(IRenderer r, IRenderableGeometry<GT> g) where T : IGeometry
        {
            throw new NotImplementedException();
        }

        public void Render<T>(IRenderer r, T s) where T : IPath
        {
            throw new NotImplementedException();
        }

        public void Render<T>(IRenderer r, IRenderableShape<ST> s) where T : IShape
        {
            TransMatrix2D transform = s.TransMatrix;
            //TODO: Transform
            if (s.Geomerty is Circle)
            {
                Circle c = s.Geomerty as Circle;
                r.DrawCircle(Color.Red, 1, (int)c.Centre.X, (int)c.Centre.Y, (int)c.Radius);
            }
            else
            {
                //meh, squares et al., are all polygons.
                //The class should cache the conversion.
                Polygon p = s.Geomerty.ToPolygon();
                //PointList p2 = transform.Transform();
                for (int i = 0; i < p.Points.Count; i++)
                {
                    //NB: p.points is padded with an extra indexable value, to close the shape
                    //so p.Points[i + 1] is valid on the last iteration
                    r.DrawLine(Color.Red, 1, p.Points[i], p.Points[i + 1]);
                }
            }
        }


      
    }



    


}
