/*
 * The following code is Copyright 2018 Dr Warren Creemers (busyDuckman)
 * See LICENSE.md for more information.
 */

//TODO: This looks like an unfinished version. Find the finished version, or finish the code. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WDToolbox.Maths.Geometry.Shapes.Basic
{
    public class Rectangle : Shape
    {
        double _xPos;
        double _yPos;
        double _width;
        double _height;

        public double Height
        {
            get
            {
                return _height;
            }

            set
            {
                _height = value;
                Invalidate();
            }
        }

        public double XPos
        {
            get
            {
                return _xPos;
            }

            set
            {
                _xPos = value;
                Invalidate();
            }
        }

        public double YPos
        {
            get
            {
                return _yPos;
            }

            set
            {
                _yPos = value;
                Invalidate();
            }
        }

        public double Width
        {
            get
            {
                return _width;
            }

            set
            {
                _width = value;
                Invalidate();
            }
        }

        public Rectangle(double x, double y, double width, double height)
        {
            _xPos = x;
            _yPos = y;
            _width = width;
            _height = height;
            Invalidate();
        }

        public Rectangle(Rectangle another)
        {
            _xPos = another._xPos;
            _yPos = another._yPos;
            _width = another._width;
            _height = another._height;
            Invalidate();
        }

        public override double PathLength { get { return (Width * 2) + (Height * 2); } }

        public override Point2D Start { get { return new Point2D(_xPos, _yPos); } }
       
        public override double SurfaceArea { get { return (Width * 2) + (Height * 2); } }

        public override object Clone() { return new Rectangle(this); }

        public override Polygon ToPolygon(double lineLenOnCurves)
        {
            return new Polygon();
        }
    }
}
