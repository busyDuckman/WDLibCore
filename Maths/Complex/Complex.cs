/*
 * The following code is Copyright 2018 Dr Warren Creemers (busyDuckman)
 * See LICENSE.md for more information.
 */

using System;

namespace WDToolbox.Maths.Complex
{
    /// <summary>
    /// Handles Complex numbers
    /// </summary>
    public struct Complex
    {
        /// <summary>
        /// euler position of the complex number.
        /// </summary>
        public double x { get; set; }

        public double y { get; set; }
        
        /// <summary>
        /// Creates a new complex number.
        /// </summary>
        /// <param name="a">x euler position of the complex number</param>
        /// <param name="b">y euler position of the complex number</param>
        public Complex(double a, double b)
        {
            x = a;
            y = b;
        }

        /// <summary>
        /// Multiplies two complex numbers
        /// </summary>
        /// <param name="a">a complex number.</param>
        /// <param name="b">a complex number.</param>
        /// <returns>a*b</returns>
        public static Complex operator *(Complex a, Complex b)
        {
            return new Complex(a.x * b.x - a.y * b.y, a.x * b.y + b.x * a.y);
        }

        /// <summary>
        /// Adds two complex numbers
        /// </summary>
        /// <param name="a">a complex number.</param>
        /// <param name="b">a complex number.</param>
        /// <returns>a+b</returns>	
        public static Complex operator +(Complex a, Complex b)
        {
            return new Complex(a.x + b.x, a.y + b.y);
        }

        /// <summary>
        /// sqr operation for complex numbers
        /// </summary>
        /// <returns>A new complex number.</returns>
        public Complex getSqr()
        {
            return new Complex((x * x) - (y * y), x * y * 2);
        }

        /// <summary>
        /// SqrMag operation for complex numbers
        /// </summary>
        /// <returns>A scalar value.</returns>
        public double getSqrMag()
        {
            return ((x * x) + (y * y));
        }
    }
}