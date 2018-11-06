/*
 * The following code is Copyright 2018 Dr Warren Creemers (busyDuckman)
 * See LICENSE.md for more information.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WDToolbox.Rendering
{
    /// <summary>
    /// Indicates something can be rendered
    /// </summary>
    public interface IRenderable
    {
        void Render(IRenderer r, Rectangle where);
        Size PreferedSizeInPixels { get; }
    }

    public static class IRenderableExtension
    {
        public static void Render(this IRenderable item, IRenderer r)
        {
            item.Render(r, new Rectangle(0, 0, item.PreferedSizeInPixels.Width, item.PreferedSizeInPixels.Height));
        }

        public static void Render(this IRenderable item, IRenderer r, Point location)
        {
            item.Render(r, new Rectangle(location.X, location.Y, item.PreferedSizeInPixels.Width, item.PreferedSizeInPixels.Height));
        }


        public static Bitmap RenderAsGDIBitmap(this IRenderable item)
        {
            Bitmap b = new Bitmap(item.PreferedSizeInPixels.Width, item.PreferedSizeInPixels.Height);
            IRenderer r = new GDIPlusRenderer(b);
            item.Render(r, new Rectangle(0, 0, r.Width, r.Height));
            r.Close();

            return b;
        }
    }
}
