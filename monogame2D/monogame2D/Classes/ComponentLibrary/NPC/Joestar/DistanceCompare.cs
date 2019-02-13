using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace monogame2D.JoeStar
{
	class DistanceCompare : IComparer<Case>
	{
		public int Compare(Case x, Case y)
		{
			if (x.distance < y.distance) return -1;
			else if (x.distance > y.distance) return 1;
			else return 0;

		}
	}
}
