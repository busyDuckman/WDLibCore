/*
 * The following code is Copyright 2018 Dr Warren Creemers (busyDuckman)
 * See LICENSE.md for more information.
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace WDToolbox.Rendering.Colour
{
	interface IColour<colType> where colType : IColour<colType>
	{
		float mesureColorantContrast(colType b);
		QColor mix(colType a, float per);
		QColor rgbMix(colType a, float per);
		void toHSV(out float H, out float S, out float V);
		string toName();
		string ToString();
	}
}
