using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace monogame2D.ATHLibrary
{
	public class ATH :DrawableGameComponent
	{
		// parent de l'ath
		protected ATH parent;
		// position de l'ath calculé en fonction du parent
		protected Vector2 position;
		// size de l'ath
		protected Vector2 size;
		// position par rapport au parent
		protected Anchor anchor;
		// ordre d'affichage
		protected int order;
		// game ou se trouve l'ath
		public Game1 game;
		// outil permettant d'afficher l'ath
		protected static SpriteBatch spriteBatch;

		// Constructeur sans parent
		protected ATH(Game1 game, Vector2 position, Vector2 size, Anchor anchor = Anchor.topleft, int order = 0) : base(game)
		{
			// place le parent en null, on considère ici que le parent est game
			this.parent = null;
			this.position = position;
			this.size = size;
			this.anchor = anchor;
			this.game = game;
			
			// Gestion de l'ordre d'affichage (plus order est grand, plus il sera appelé tard et donc plus il sera en avant)
			// C'est un peu foireux mais ça marche si le nombre de fils est inférieur à 10 et que il y'a moins de 5 couche d'ath
			this.order = (int)(order*1000/Math.Pow(10,NbParent()));
			resize(new Vector2(game.GraphicsDevice.PresentationParameters.BackBufferWidth, game.GraphicsDevice.PresentationParameters.BackBufferHeight));
		}
		// Constructeur avec parent
		protected ATH(ATH parent, Vector2 position, Vector2 size, Anchor anchor = Anchor.topleft, int order = 0) : base(parent.game)
		{
			this.parent = parent;
			this.position = position;
			this.size = size;
			this.anchor = anchor;
			this.game = parent.game;

			// Gestion de l'ordre d'affichage (plus order est grand, plus il sera appelé tard et donc plus il sera en avant
			// Un enfant passera toujours au dessus d'un parent, d'ou le "+ 1" et sera ensuite replacé en fonction de son order(précisé dans les paramètres du constructeur)
			this.order = (int)(parent.order + 1 + order * 1000 / Math.Pow(10, NbParent()));
			resize(parent.size);
			
		}
		// Redimentionne l'ath pour qu'il remplisse son père si les dimension dans le constructeur sont a zéro
		private void resize(Vector2 parentSize)
		{
			if (size.X == 0 || size.Y == 0)
			{
				if (this.anchor == Anchor.center)
				{
					this.size = parentSize - position * 2;
					this.position = Vector2.Zero;
				}
				else
				{
					this.size = parentSize - position;
				}
			}
		}

		protected override void LoadContent()
		{
			base.LoadContent();

			// Création du spriteBatch si il n'existe pas encore
			if (spriteBatch == null) spriteBatch = new SpriteBatch(GraphicsDevice);

			// Ajout de la taille de la fenètre pour être sur que l'ATH passe par dessus le jeu
			this.DrawOrder = game.GraphicsDevice.PresentationParameters.BackBufferHeight * game.GraphicsDevice.PresentationParameters.BackBufferWidth + order;
		}

		public virtual Rectangle RealPos()
		{
			Rectangle parentSize;
			// si l'ATH a un père
			if (parent != null) {
				parentSize = parent.RealPos();
			}
			// si l'ATH n'a pas de père, ses positions et dimensions réel sont calculé en fonction de la taille de la fenètre
			else parentSize = new Rectangle(0,0,game.GraphicsDevice.PresentationParameters.BackBufferWidth, game.GraphicsDevice.PresentationParameters.BackBufferHeight);
			Rectangle realPos;

			// calcul de la position de l'ATH à fonction de l'ancre indiqué lors de sa création
			switch (this.anchor)
			{
				case Anchor.top:
					
					realPos = new Rectangle((int)(parentSize.X + parentSize.Width/2 - this.size.X/2 + this.position.X),
								(int)(parentSize.Y + this.position.Y), 
								(int)size.X, (int)size.Y);
					break;
				case Anchor.topright:
					realPos = new Rectangle((int)(parentSize.X + parentSize.Width - this.size.X + this.position.X),
								(int)(parentSize.Y + this.position.Y),
								(int)size.X, (int)size.Y);
					break;
				case Anchor.right:
					realPos = new Rectangle((int)(parentSize.X + parentSize.Width - this.size.X + this.position.X),
								(int)(parentSize.Y + parentSize.Height / 2 - this.size.Y / 2 + this.position.Y),
								(int)size.X, (int)size.Y);
					break;
				case Anchor.bottomright:
					realPos = new Rectangle((int)(parentSize.X + parentSize.Width - this.size.X + this.position.X),
								(int)(parentSize.Y + parentSize.Height - this.size.Y + this.position.Y), 
								(int)size.X, (int)size.Y);
					break;
				case Anchor.bottom:
					realPos = new Rectangle((int)(parentSize.X + parentSize.Width / 2 - this.size.X / 2 + this.position.X),
								(int)(parentSize.Y + parentSize.Height - this.size.Y + this.position.Y), 
								(int)size.X, (int)size.Y);
					break;
				case Anchor.bottomleft:
					realPos = new Rectangle((int)(parentSize.X + this.position.X),
								(int)(parentSize.Y + parentSize.Height - this.size.Y + this.position.Y), 
								(int)size.X, (int)size.Y);
					break;
				case Anchor.left:
					realPos = new Rectangle((int)(parentSize.X + this.position.X),
								(int)(parentSize.Y + parentSize.Height / 2 - this.size.Y / 2 + this.position.Y), 
								(int)size.X, (int)size.Y);
					break;
				case Anchor.center:
					realPos = new Rectangle((int)(parentSize.X + parentSize.Width / 2 - this.size.X / 2 + this.position.X),
								(int)(parentSize.Y + parentSize.Height / 2 - this.size.Y / 2 + this.position.Y),
								(int)size.X, (int)size.Y);
					break;
				default:
					realPos = new Rectangle((int)(parentSize.X + this.position.X),
								(int)(parentSize.Y + this.position.Y), 
								(int)size.X, (int)size.Y);
					break;
			}

			return realPos;

		}
		// Retrouve la base de l'arbre
		public ATH getBase() {
			if (parent == null) return this;
			else return parent.getBase();
		}
		// Cherche le nombre de branche séparant la base de l'arbre à l'ATH
		public int NbParent()
		{
			if (parent == null) return 0;
			else return parent.NbParent()+1;
		}
		// Supprime l'ATH
		public virtual void Remove()
		{
			this.Dispose();
			game.Components.Remove(this);
			this.UnloadContent();
		}

		protected override void UnloadContent()
		{
			Console.WriteLine("Fermeture de l'ath " + this);
			base.UnloadContent();
		}
		// Méthode test, amusez vous
		public static void CreateATHTest(Game1 game)
		{
			ATHCompositeColor ath = ATHCompositeColor.Create(game, new Vector2(0, 0), new Vector2(0, 0), Color.Red);
			ATHCompositeButton button = ATHCompositeButton.Create(ath, new Vector2(50, -100), new Vector2(500, 200), Anchor.center, 0);
			ATHFeuilleColor.Create(button, new Vector2(0, 0), new Vector2(0, 0), Color.Aqua, Anchor.topleft, 0);
			ATHFeuilleColor.Create(button, new Vector2(0, 0), new Vector2(20, 20), Color.Red, Anchor.topleft, 0);
			ATHFeuilleColor.Create(button, new Vector2(20, 20), new Vector2(0, 0), Color.Blue, Anchor.center, 0);
			ATHFeuilleText.Create(button, new Vector2(20, 20), new Vector2(0, 0), "test centre", Color.Black, 1, Anchor.center, Anchor.center);
			ATHFeuilleText.Create(button, new Vector2(20, 0), new Vector2(0, 0), "test gauche", Color.Black, 1, Anchor.left, Anchor.left);
			ATHFeuilleText.Create(button, new Vector2(-20, 0), new Vector2(0, 0), "test droite", Color.Black, 1, Anchor.right, Anchor.right);
			ATHFeuilleText.Create(button, new Vector2(0, -20), new Vector2(0, 0), "test bas", Color.Black, 1, Anchor.bottom, Anchor.bottom);
			ATHFeuilleText.Create(button, new Vector2(0, 20), new Vector2(0, 0), "test haut", Color.Black, 1, Anchor.top, Anchor.top);
			ATHFeuilleText.Create(button, new Vector2(20, 20), new Vector2(0, 0), "test haut gauche", Color.Black, 1, Anchor.topleft, Anchor.topleft);
			ATHFeuilleText.Create(button, new Vector2(-20, 20), new Vector2(0, 0), "test haut droite", Color.Black, 1, Anchor.topright, Anchor.topright);
			ATHFeuilleText.Create(button, new Vector2(20, -20), new Vector2(0, 0), "test bas gauche", Color.Black, 1, Anchor.bottomleft, Anchor.bottomleft);
			ATHFeuilleText.Create(button, new Vector2(-20, -20), new Vector2(0, 0), "test bas droite", Color.Black, 1, Anchor.bottomright, Anchor.bottomright);

			ATHCompositeColor test = ATHCompositeColor.Create(ath, new Vector2(20, 20), new Vector2(100, 50), Color.AntiqueWhite, Anchor.topleft, 5);
			ATHFeuilleText.Create(test, new Vector2(10, 10), new Vector2(0, 0), "test hautgauche", Color.Black, 0.5f, Anchor.center, Anchor.center);

			test = ATHCompositeColor.Create(ath, new Vector2(0, 20), new Vector2(100, 50), Color.AntiqueWhite, Anchor.top, 5);
			ATHFeuilleText.Create(test, new Vector2(10, 10), new Vector2(0, 0), "test haut", Color.Black, 0.5f, Anchor.center, Anchor.center);

			test = ATHCompositeColor.Create(ath, new Vector2(-20, 20), new Vector2(100, 50), Color.AntiqueWhite, Anchor.topright, 5);
			ATHFeuilleText.Create(test, new Vector2(10, 10), new Vector2(0, 0), "test hautdroite", Color.Black, 0.5f, Anchor.center, Anchor.center);

			test = ATHCompositeColor.Create(ath, new Vector2(-20, 0), new Vector2(100, 50), Color.AntiqueWhite, Anchor.right, 5);
			ATHFeuilleText.Create(test, new Vector2(10, 10), new Vector2(0, 0), "test droite", Color.Black, 0.5f, Anchor.center, Anchor.center);

			test = ATHCompositeColor.Create(ath, new Vector2(-20, -20), new Vector2(100, 50), Color.AntiqueWhite, Anchor.bottomright, 5);
			ATHFeuilleText.Create(test, new Vector2(10, 10), new Vector2(0, 0), "test basdroite", Color.Black, 0.5f, Anchor.center, Anchor.center);

			test = ATHCompositeColor.Create(ath, new Vector2(0, -20), new Vector2(100, 50), Color.AntiqueWhite, Anchor.bottom, 5);
			ATHFeuilleText.Create(test, new Vector2(10, 10), new Vector2(0, 0), "test bas", Color.Black, 0.5f, Anchor.center, Anchor.center);

			test = ATHCompositeColor.Create(ath, new Vector2(20, -20), new Vector2(100, 50), Color.AntiqueWhite, Anchor.bottomleft, 5);
			ATHFeuilleText.Create(test, new Vector2(10, 10), new Vector2(0, 0), "test basgauche", Color.Black, 0.5f, Anchor.center, Anchor.center);

			test = ATHCompositeColor.Create(ath, new Vector2(20, 0), new Vector2(100, 50), Color.AntiqueWhite, Anchor.left, 5);
			ATHFeuilleText.Create(test, new Vector2(10, 10), new Vector2(0, 0), "test gauche", Color.Black, 0.5f, Anchor.center, Anchor.center);

			ATHCompositeImage test2 = ATHCompositeImage.Create(ath, new Vector2(10, 0), new Vector2(50, 50), "bouton", Anchor.left, 6);
			test2 = ATHCompositeImage.Create(ath, new Vector2(-10, 0), new Vector2(50, 50), "bouton", Anchor.right, 4);
			test2 = ATHCompositeImage.Create(ath, new Vector2(0, 10), new Vector2(50, 50), "bouton", Anchor.top, 1);
			test2 = ATHCompositeImage.Create(ath, new Vector2(0, -10), new Vector2(50, 50), "bouton", Anchor.bottom, 8);
			test2 = ATHCompositeImage.Create(ath, new Vector2(10, 10), new Vector2(50, 50), "bouton", Anchor.topleft, 6);
			test2 = ATHCompositeImage.Create(ath, new Vector2(-10, 10), new Vector2(50, 50), "bouton", Anchor.topright, 4);
			test2 = ATHCompositeImage.Create(ath, new Vector2(10, -10), new Vector2(50, 50), "bouton", Anchor.bottomleft, 2);
			test2 = ATHCompositeImage.Create(ath, new Vector2(-10, -10), new Vector2(50, 50), "bouton", Anchor.bottomright, 6);
		}

	}
}
