using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;

namespace monogame2D.ComponentLibrary
{
	public class DrawOrderCompare : IComparer<DrawableGameComponent>
	{
		public int Compare(DrawableGameComponent x, DrawableGameComponent y)
		{
			if (x.DrawOrder < y.DrawOrder) return -1;
			else if (x.DrawOrder > y.DrawOrder) return 1;
			else return 0;

		}
	}
}