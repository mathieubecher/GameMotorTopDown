using System;
using Microsoft.Xna.Framework;

namespace monogame2D.MapLibrary
{
	public class Wall : Carto
	{
		/*				CONSTRUCTEUR			*/
		private Wall(Map father, string name, DimensionMap size, string texture, int[,] spriteCase, int nbSprite, float height) : base(father, name, size, texture, spriteCase, nbSprite, height)
		{
			blocked = true;

		}
		public static new Wall Create(Map map, string name, DimensionMap size, string texture, int nbSprite, float height)
		{
			int[,] forme = new int[size.NbCaseX, size.NbCaseY];
			int[,] spriteCase = new int[size.NbCaseX, size.NbCaseY];

			/*				ALIMENTATION POUR LES TESTS					*/
			// génération alléatoire de zone 
			Random rng = new Random((int)DateTime.Now.Ticks);
			for (int x = (size.Origin.X == 0) ? 0 : 1; x < size.NbCaseX  - ((size.Origin.X + size.NbCaseX == map.Size.NbCaseX) ? 0 : 1); x = ++x)
			{
				for (int y = (size.Origin.Y == 0) ? 0 : 1; y < size.NbCaseY - ((size.Origin.Y + size.NbCaseY == map.Size.NbCaseY) ? 0 : 1); ++y)
				{
					int value = 1;
					forme[x, y] = (rng.Next(0, 100) < 95) ? 0 : value;
					spriteCase[x, y] = forme[x, y];
				}
			}

			Wall wall = new Wall(map, name, size, texture, spriteCase, nbSprite, height);
			wall.Initialize();
			map.Cartos.Add(wall);

			return wall;
		}

		// renvoi les dimensions et la position réel de la case en x y
		public override Rectangle SizeCase(int x, int y, Rectangle realPos, float height)
		{

			Rectangle sizeCase = base.SizeCase(x, y, realPos, height);
			float floorHeight = 0;
			foreach (Carto c in map.Cartos.Cartos)
			{
				if (c.Height > floorHeight && c.GetCase(x, y) > 0 && !c.Blocked) floorHeight = c.Height;
			}
			sizeCase.Y -= (int)(floorHeight*Game1.sizeCase);

			return sizeCase;
		}
	}
}