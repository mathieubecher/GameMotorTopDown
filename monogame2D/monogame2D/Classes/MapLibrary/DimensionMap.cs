using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace monogame2D.MapLibrary
{
	public class DimensionMap
	{
		/*				VARIABLES				*/
		// position d'origine de la carte
		private Vector2 origin;
		// nombre de case en X de la carte
		private int nbCaseX;
		// nombre de case en Y de la carte
		private int nbCaseY;

		/*				GETTER SETTER			*/
		public Vector2 Origin { get => origin; }
		public int NbCaseX { get => nbCaseX; }
		public int NbCaseY { get => nbCaseY; }

		/*				CONSTRUCTEUR			*/
		public DimensionMap(int nbCaseX, int nbCaseY, Vector2 origin)
		{
			this.nbCaseX = nbCaseX;
			this.nbCaseY = nbCaseY;
			this.origin = origin;
		}
		// Calcul la dimension réel de la carte
		public Rectangle RealPos()
		{
			return new Rectangle((int)(origin.X* (int)Game1.sizeCase),(int)(origin.Y * (int)Game1.sizeCase), (int)(nbCaseX * (int)Game1.sizeCase),(int)(nbCaseY * (int)Game1.sizeCase));
		}
	}
}
