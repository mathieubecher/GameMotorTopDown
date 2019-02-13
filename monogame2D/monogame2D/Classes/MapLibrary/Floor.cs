using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace monogame2D.MapLibrary
{
	public class Floor : DrawableGameComponent
	{
		

		/*				VARIABLES				*/
		// game ou se trouve le sol
		protected Game1 game;
		// nom du sol
		protected string name;
		// dimension du sol
		protected DimensionMap size;
		// texture du sol
		protected string texture;
		protected Texture2D sprite;
		
		// affichage du sol
		protected static SpriteBatch spriteBatch;
		// tableau indiquant la position du sprite
		protected int[,] spriteCase;
		// nombre de sprite différent existant sur la texture
		protected int nbSprite;
		// ensemble des cases de la carte affiché à l'écran
		protected Rectangle showCase;
		// si la carte est affiché
		protected bool showFloor = true;        
		// si le fond de la carte est une image complète
		protected bool fullPicture;


		public string Name { get => name; }
		public DimensionMap Size { get => size; }
		public static SpriteBatch SpriteBatch { get => spriteBatch;}
		public int[,] SpriteCase { get => spriteCase; set => spriteCase = value; }

		/*				CONSTRUCTEUR			*/
		public Floor(Game1 game, string name, DimensionMap size, string texture, int nbSprite, int[,] spriteCase, bool fullPicture = false) : base(game)
		{
			this.size = size;
			this.texture = texture;
			this.name = name;
			this.game = game;
			this.spriteCase = spriteCase;
			this.nbSprite = nbSprite;
			showCase = new Rectangle(0, 0, 0, 0);
			this.fullPicture = false;
		}
		/*				LOAD CONTENT			*/
		protected override void LoadContent()
		{
			base.LoadContent();
			// ne crée qu'un seul spriteBatch pour toutes les cartes
			if (spriteBatch == null) spriteBatch = new SpriteBatch(game.GraphicsDevice);
			// création du sprite
			this.sprite = Game1.creerTexture(game, texture);
		}
		/*					UPDATE				*/
		public override void Update(GameTime gameTime)
		{
			Rectangle realPos = RealPos();
			// détermine les cases qui seront visible à l'écran
			showCase.X = (int)((-realPos.X) / (int)Game1.sizeCase - 2);
			showCase.Y = (int)((-realPos.Y) / (int)Game1.sizeCase - 2);
			showCase.Width = (int)game.nbCaseAffiche.X + 5;
			showCase.Height = (int)game.nbCaseAffiche.Y + 5;
			base.Update(gameTime);
		}

		// Calcul la position réel de la carte à l'écran
		public virtual Rectangle RealPos()
		{
			Rectangle pos = size.RealPos();
			Rectangle playerPos = game.player.GetPos();
			pos.X += -playerPos.X - playerPos.Width / 2 + game.GraphicsDevice.PresentationParameters.BackBufferWidth / 2;
			pos.Y += -playerPos.Y - playerPos.Height / 2 + game.GraphicsDevice.PresentationParameters.BackBufferHeight / 2;
			return pos;
			
		}

		// Affiche la case positionné en x y
		public virtual void DrawCase(int x, int y, Rectangle realPos)
		{
			DrawCase(x, y, realPos, 0);
		}
		public void DrawCase(int x, int y, Rectangle realPos, float height)
		{
			if (fullPicture || spriteCase[x, y] > 0)
			{
				// taille de la case
				Rectangle sizeCase = SizeCase(x, y, realPos, height);
				// taille du sprite de la case
				Rectangle sizeSprite;
				// si la map est dessiner à partir d'une image complète 
				if (fullPicture) sizeSprite = new Rectangle((sprite.Width / size.NbCaseX) * x, (sprite.Height / size.NbCaseY) * y, (int)sprite.Width / size.NbCaseX, (int)sprite.Height / size.NbCaseY);
				// si la case n'est pas vide
				else sizeSprite = new Rectangle((sprite.Width / nbSprite) * ((spriteCase[x, y] - 1) % nbSprite),
												(sprite.Width / nbSprite) * (((spriteCase[x, y] - 1) - (spriteCase[x, y] - 1) % nbSprite) / nbSprite),
												(int)(Math.Floor((float)(sprite.Width / nbSprite))),
												(int)(Math.Floor((float)(sprite.Width * (height + 1)
												/ nbSprite))));


				spriteBatch.Draw(sprite,
						sizeCase,
						sizeSprite,
						Color.White);
			}
		}
		// Dimension et position réel de la case
		public virtual Rectangle SizeCase(int x, int y, Rectangle realPos,float height)
		{
			Rectangle sizeCase = new Rectangle(realPos.X + (int)Game1.sizeCase * x,
														realPos.Y + (int)Game1.sizeCase * y - (int)(height * (int)Game1.sizeCase),
														(int)Game1.sizeCase,
														(int)(Math.Ceiling((Game1.sizeCase * (height + 1)))));
			return sizeCase;
		}

	}
}