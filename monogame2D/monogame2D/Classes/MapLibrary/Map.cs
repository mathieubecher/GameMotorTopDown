using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using monogame2D.ComponentLibrary;

namespace monogame2D.MapLibrary
{
	public class Map : Floor
	{
		/*				VARIABLES				*/
		// ensemble des décors de la carte
		private CartoManagement cartos;
		private DecorManagement decors;
		
		// liste des éléments de la map à afficher
		
		private List<DrawableGameComponent> toDraw;
		private List<Map> neibhourgs;
		// outils de comparaison permettant de trier la list toDraw par DrawOrder
		private DrawOrderCompare drawOrderCompare;
		// position réel, qui se recalcule à chaque déplacement du joueur
		private bool isLastPos = false;
		private Rectangle realPos;
		/*				GETTER SETTER			*/
		public new Game1 Game { get => game;}
		public bool ShowFloor { get => showFloor; }
		public Rectangle ShowCase { get => showCase; }
		public DecorManagement Decors { get => decors; set => decors = value; }
		internal CartoManagement Cartos { get => cartos; set => cartos = value; }
		public List<DrawableGameComponent> ToDraw { get => toDraw; set => toDraw = value; }
		public bool IsLastPos { get => isLastPos; set => isLastPos = value; }
		public List<Map> Neibhourgs { get => neibhourgs; set => neibhourgs = value; }

		/*				CONSTRUCTEUR			*/
		private Map(Game1 game, string name, DimensionMap size, string texture, int nbSprite, int[,] spriteCase, bool fullPicture = false) : base(game,name,size,texture,nbSprite,spriteCase, fullPicture)
		{
			Neibhourgs = new List<Map>();
			decors = new DecorManagement(this);
			cartos = new CartoManagement(this);
			toDraw = new List<DrawableGameComponent>();
			drawOrderCompare = new DrawOrderCompare();
		}

		// Génération d'une carte
		public static Map Create(Game1 game, string name, DimensionMap size, string texture, int nbSprite, bool fullPicture = false)
		{
			int[,] spriteCase = new int[size.NbCaseX,size.NbCaseY];
			
			// Génération aléatoire des cases... pour le moment
			Random rng = new Random((int)DateTime.Now.Ticks);
			for (int x = 0; x < size.NbCaseX; x++)
			{
				for (int y = 0; y < size.NbCaseY; y++)
				{
					spriteCase[x, y] = rng.Next(1, nbSprite);
				}
			}
			Map map = new Map(game, name, size, texture, nbSprite, spriteCase, fullPicture);

			/* ICI SERA CREER LE CONTENU COMPLET DE LA MAP, A PARTIR DE FICHIER XML */
			game.maps.Add(map);
			game.Components.Add(map);

			return map;
		}
		/*				LOAD CONTENT			*/
		protected override void LoadContent()
		{
			base.LoadContent();

			/*			BASE DE TEST			*/
			// création d'un sol 1
			Carto.Create(this, "floor3", new DimensionMap(size.NbCaseX, size.NbCaseY, new Vector2(0, 0)), "sol3", 1f);
			Carto.Create(this, "floor2", new DimensionMap(size.NbCaseX, size.NbCaseY, new Vector2(0, 0)), "sol2", 0f);
			Carto.Create(this, "floor1", new DimensionMap(size.NbCaseX, size.NbCaseY, new Vector2(0, 0)), "sol1", 0.5f);
			Wall.Create(this, "wall", new DimensionMap(size.NbCaseX, size.NbCaseY, new Vector2(0, 0)), "wall1",1, 0.5f);


			// création de n décors sur la map
			Random rng = new Random((int)DateTime.Now.Ticks);
			for (int i = 0; i < Game1.NBDECOR; i++)
			{
				Decor.Create(this, new Vector2(rng.Next(0, size.NbCaseX), rng.Next(0, size.NbCaseY)), new Vector2(rng.Next(0, 100) / 100f, rng.Next(0, 100) / 100f), new Vector2(rng.Next(50, 200) / 100f, rng.Next(50, 200) / 100f), "bouton",false);

			}
			// création de n composant sur la map
			for (int i = 0; i < Game1.NBCOMPONENT; i++)
			{
				CreatureType type = (rng.Next(0, 2) < 2) ? CreatureType.monster : CreatureType.outlaw;
				Component.Create(this, new Vector2(rng.Next(0, size.NbCaseX), rng.Next(0, size.NbCaseY)), new Vector2(rng.Next(0, 100) / 100f, rng.Next(0, 100) / 100f), new Vector2(rng.Next(50, 200) / 100f, rng.Next(50, 200) / 100f), "boutonSolid");
			}
			// création de n AI sur la map
			for (int i = 0; i < Game1.NBAI; i++)
			{
				CreatureType type = (rng.Next(0, 2) < 2) ? CreatureType.monster : CreatureType.outlaw;
				AI.Create(this, new Vector2(rng.Next(0, size.NbCaseX), rng.Next(0, size.NbCaseY)), new Vector2(rng.Next(0, 100) / 100f, rng.Next(0, 100) / 100f), new Vector2(rng.Next(50, 200) / 100f, rng.Next(50, 200) / 100f), "boutonStandard", type, Game1.VIGILENCE,Game1.TENACITE,Game1.AISPEED);
			}

			foreach (Decor d in decors.Decors)
			{ 
				d.GestionHeight();
			}
			this.DrawOrder = (int)(size.Origin.Y + size.Origin.X);
		}

		/*					UPDATE				*/
		public override void Update(GameTime gameTime)
		{
			Random rng = new Random((int)DateTime.Now.Ticks);
			// La liste des objets à affiché est vidé
			toDraw = new List<DrawableGameComponent>();
			// Gestion des décors et cartos de la map
			cartos.Update(gameTime);
			decors.Update(gameTime);

			base.Update(gameTime);

			// Tous les objet à afficher sont trié par ordre d'affichage
			toDraw.Sort(drawOrderCompare);
		}
		/*					DRAW				*/
		public override void Draw(GameTime gameTime)
		{
			// Cadre de la carte
			Rectangle realPos = RealPos();

			// Commence à dessiner
			spriteBatch.Begin();

			// Pour toute les case de la fenètre
			for (int x = (showCase.X < 0) ? 0 : showCase.X; x < ((showCase.X + showCase.Width >= size.NbCaseX) ? size.NbCaseX : showCase.X + showCase.Width); x++)
			{
				for (int y = (showCase.Y < 0) ? 0 : showCase.Y; y < ((showCase.Y + showCase.Height >= size.NbCaseY) ? size.NbCaseY : showCase.Y + showCase.Height); y++)
				{
					DrawCase(x, y, realPos);
				}
			}
			// fin du dessin
			spriteBatch.End();

			base.Draw(gameTime);

			// dessinne les objets sur la cartes
			foreach (Carto f in cartos.Cartos)
			{
				if (!f.Blocked && f.Height == 0) f.Draw(gameTime);
			}

			for (int y = (showCase.Y < 0) ? 0 : showCase.Y; y < ((showCase.Y + showCase.Height + 2 >= size.NbCaseY) ? size.NbCaseY : showCase.Y + showCase.Height + 2); y++)
			{
				for (int x = (showCase.X < 0) ? 0 : showCase.X; x < ((showCase.X + showCase.Width >= size.NbCaseX) ? size.NbCaseX : showCase.X + showCase.Width); x++)
				{
					int nbdraw = 0;
					spriteBatch.Begin();
					foreach (Carto f in cartos.Cartos)
					{
						if (f.Height > 0)
						{
							if (f.GetCase(x, y) > 0) nbdraw++;
							f.DrawCase(x, y, f.RealPos());

						}
					}
					foreach (Carto f in cartos.Cartos)
					{
						if (f.Blocked)
						{
							if (f.GetCase(x, y) > 0) nbdraw++;
							f.DrawCase(x, y, f.RealPos());

						}
					}
					spriteBatch.End();
				}

				int drawOrderCase = (int)(realPos.Y + (int)Game1.sizeCase * (y+1.1f)) * game.GraphicsDevice.PresentationParameters.BackBufferWidth;
				int lastIndex = 0;
				while (toDraw.Count > lastIndex && toDraw[lastIndex].DrawOrder < drawOrderCase )
				{
					toDraw[lastIndex].Draw(gameTime);
					toDraw.Remove(toDraw[lastIndex]);
				
				}
				
			}
			while (toDraw.Count > 0)
			{
				
				toDraw[0].Draw(gameTime);
				toDraw.Remove(toDraw[0]);
			}
		}

		// Affiche la carte
		public void Show()
		{
			if (!showFloor)
			{
				Console.WriteLine("Affiche la carte " + this.name);
				showFloor = true;
				game.Components.Add(this);
			}
		}
		// Cache la carte
		public void Hide()
		{
			if (showFloor)
			{
				Remove();
			}
		}
		// Suppression de la carte
		public virtual void Remove()
		{
			Console.WriteLine("N'affiche plus la carte " + this.name);
			showFloor = false;
			game.Components.Remove(this);
			this.Dispose();
		}

		public override Rectangle RealPos()
		{
			if (!isLastPos)
			{
				isLastPos = true;
				realPos = base.RealPos();
			}
			return realPos;
		}
		public static void CreateNeibhourg(Map m1, Map m2)
		{
			m1.Neibhourgs.Add(m2);
			m2.Neibhourgs.Add(m1);
		}



	}
}
