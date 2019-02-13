using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace monogame2D.MapLibrary
{
	public class HeightCompare : IComparer<Carto>
	{
		public int Compare(Carto x, Carto y)
		{
			if (x.Height < y.Height) return -1;
			else if (x.Height > y.Height) return 1;
			else return 0;

		}
	}
}