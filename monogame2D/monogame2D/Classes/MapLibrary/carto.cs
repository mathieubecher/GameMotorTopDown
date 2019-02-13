using Microsoft.Xna.Framework;
using System;

namespace monogame2D.MapLibrary
{
	public class Carto : Floor
	{
		private const int CENTER = 1;
		private const int TOP = 3;
		private const int BOTTOM = 7;
		private const int LEFT = 9;
		private const int RIGHT = 5;

		private const int CORNERTL = 2;
		private const int CORNERTR = 4;
		private const int CORNERBL = 8;
		private const int CORNERBR = 6;

		private const int CAVITYTL = 13;
		private const int CAVITYTR = 12;
		private const int CAVITYBL = 11;
		private const int CAVITYBR = 10;

		private const int NBSPRITE = 13;

		// height du sol
		protected float height;
		// map sur laquelle se trouve le sol
		protected Map map;
		// Indique si le sol doit etre affiché ou non
		private bool show;
		// S'il est impossible de monter dessus
		protected bool blocked = false;

		/*				GETTER SETTER			*/
		public float Height { get => height; set => height = value; }
		public bool ShowSprite { get => show; }
		public bool Blocked { get => blocked;}

		/*				CONSTRUCTEUR			*/
		protected Carto(Map father, string name, DimensionMap size, string texture, int[,] spriteCase, float height):base(father.Game, name,size, texture, NBSPRITE, spriteCase)
		{
			this.show = false;
			this.map = father;
			this.height = height;
		}
		protected Carto(Map father, string name, DimensionMap size, string texture, int[,] spriteCase, int nbSprite, float height) : base(father.Game, name, size, texture, nbSprite, spriteCase)
		{
			this.show = false;
			this.map = father;
			this.height = height;
		}

		// Génération d'une carte
		public static Carto Create(Map map, string name, DimensionMap size, string texture, float height = 0)
		{
			int[,] forme = new int[size.NbCaseX, size.NbCaseY];
			int[,] spriteCase = new int[size.NbCaseX, size.NbCaseY];

			/*				ALIMENTATION POUR LES TESTS					*/
			// génération alléatoire de zone 
			Random rng = new Random((int)DateTime.Now.Ticks);
			for (int x = (size.Origin.X == 0) ? 0 : 1; x < size.NbCaseX - 1 - ((size.Origin.X + size.NbCaseX == map.Size.NbCaseX) ?0:1); x = x + 2)
			{
				for (int y = (size.Origin.Y == 0) ? 0 : 1; y < size.NbCaseY - 1 - ((size.Origin.Y + size.NbCaseY == map.Size.NbCaseY) ? 0 : 1); y = y + 2)
				{
					int value = 1;
					forme[x, y] = (rng.Next(0,100)< 95)?0:value;
					forme[x + 1, y] = forme[x, y];
					forme[x, y + 1] = forme[x, y];
					forme[x + 1, y + 1] = forme[x, y];

					spriteCase[x, y] = 0;
				}
			}

			// gestion automatique des sprites avec le contenu de forme
			for (int x = 0; x < size.NbCaseX; x++) {
				for (int y = 0; y < size.NbCaseY; y++) {
					if (forme[x, y] > 0)
					{
						spriteCase[x, y] = CENTER;
						// si on est pas en haut ou en bas de la carte
						if (y > 0 && y < size.NbCaseY - 1)
						{
							// HAUT
							if (forme[x, y - 1] == 0)
							{
								if ((x == 0 || forme[x - 1, y - 1] == 0) && (x == size.NbCaseX -1 || forme[x + 1, y - 1] == 0))
									spriteCase[x, y - 1] = TOP;
								else
								{
									if (x == 0 || forme[x - 1, y - 1] == 0) spriteCase[x, y - 1] = CAVITYTL;
									else spriteCase[x, y - 1] = CAVITYTR;
								}
							}
							// BAS
							if (forme[x, y + 1] == 0)
							{
								if ((x == 0 || forme[x - 1, y + 1] == 0) && (x == size.NbCaseX - 1 || forme[x + 1, y + 1] == 0))
									spriteCase[x, y + 1] = BOTTOM;
								else
								{
									if (x == 0 || forme[x - 1, y + 1] == 0) spriteCase[x, y + 1] = CAVITYBL;
									else spriteCase[x, y + 1] = CAVITYBR;
								}
							}
						}

						// sin on est pas à gauche ou à droite de la carte
						if(x > 0 && x < size.NbCaseX - 1)
						{
							// GAUCHE
							if (forme[x - 1, y] == 0 && spriteCase[x - 1, y] == 0) spriteCase[x - 1, y] = LEFT;
							//DROITE
							if (forme[x + 1, y] == 0 && spriteCase[x + 1, y] == 0) spriteCase[x + 1, y] = RIGHT;

						}
						// Les cases en coin
						// HAUT GAUCHE
						if(x > 0 && y > 0)
							if (forme[x - 1, y - 1] == 0 && forme[x - 1, y] == 0 && forme[x, y - 1] == 0) spriteCase[x - 1, y - 1] = CORNERTL;
						// HAUT DROITE
						if(x < size.NbCaseX -1 && y > 0)
							if (forme[x + 1, y - 1] == 0 && forme[x + 1, y] == 0 && forme[x, y - 1] == 0) spriteCase[x + 1, y - 1] = CORNERTR;
						// BAS DROITE
						if (x < size.NbCaseX - 1 && y < size.NbCaseY - 1)
							if (forme[x + 1, y + 1] == 0 && forme[x, y + 1] == 0 && forme[x + 1, y] == 0) spriteCase[x + 1, y + 1] = CORNERBR;
						// BAS GAUCHE
						if (x > 0 && y < size.NbCaseY - 1)
							if (forme[x - 1, y + 1] == 0 && forme[x - 1, y] == 0 && forme[x, y + 1] == 0) spriteCase[x - 1, y + 1] = CORNERBL;
					}
				}
			}

			Carto carto = new Carto(map, name, size, texture, spriteCase,height);
			carto.Initialize();
			map.Cartos.Add(carto);

			return carto;
		}

		/*					UPDATE				*/
		public override void Update(GameTime gameTime)
		{

			// si l'objet est présent dans la fenètre
			if (map.ShowCase.X <= size.Origin.X + size.NbCaseX && map.ShowCase.X + map.ShowCase.Width >= size.Origin.X &&
				map.ShowCase.Y <= size.Origin.Y + size.NbCaseY && map.ShowCase.Y + map.ShowCase.Height >= size.Origin.Y)
			{
				// préciser qu'il est visible
				if (!show)
					show = true;
				base.Update(gameTime);
				showCase = map.ShowCase;
			}
			// préciser qu'il n'est pas visible
			else if (show) show = false;			
		}



		/*					DRAW				*/
		public override void Draw(GameTime gameTime)
		{
			// Cadre de la carte
			Rectangle realPos = RealPos();

			// Commence à dessiner
			spriteBatch.Begin();

			// Pour toute les case de la fenètre
			for (int x = showCase.X; x < showCase.X + showCase.Width; x++)
			{
				for (int y = showCase.Y; y < showCase.Y + showCase.Height; y++)
				{
					DrawCase(x, y, realPos);
				}
			}
			// fin du dessin
			spriteBatch.End();
		}
		
		// Calcul la position réel de la carte à l'écran
		public override Rectangle RealPos()
		{
			Rectangle fatherPos = map.RealPos();
			Rectangle pos = size.RealPos();
			pos.X += fatherPos.X;
			pos.Y += fatherPos.Y;

			return pos;
		}

		// renvoie la valeur de la case en x y sur la map sur le sol (le sol ne fais pas forcément les mêmes dimensions que la carte sur laquelle il se trouve)
		public int GetCase(int x, int y)
		{
			x -= (int)size.Origin.X;
			y -= (int)size.Origin.Y;
			if (x < 0 || y < 0 || x >= size.NbCaseX || y >= size.NbCaseY) return 0;
			else return spriteCase[x, y];
		}
		// dessine la case
		public override void DrawCase(int x, int y, Rectangle realPos)
		{
			if(GetCase(x,y) > 0)
				base.DrawCase((int)(x - size.Origin.X),(int)(y - size.Origin.Y),RealPos(),this.height);
		}
	}
}
