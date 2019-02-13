using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using monogame2D.ATHLibrary;
using monogame2D.ComponentLibrary;
using monogame2D.MapLibrary;
using System;
using System.Collections.Generic;

namespace monogame2D
{
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class Game1 : Game
	{

		// VARIABLE CONSTANTE DE TEST
		public const int NBDECOR = 1000;
		public const int NBCOMPONENT = 100;
		public const int NBAI = 100;

		public const int VIGILENCE = 5;
		public const int TENACITE = 7;
		public const int AISPEED = 1000;

		public const int FRONTIERS = 2;

		/*				VARIABLES				*/
		GraphicsDeviceManager graphics;
		public SpriteBatch spriteBatch;
		// liste des cartes déja chargé
		public List<Map> maps;
		// Player
		public Player player;
		// taille d'une case
		public static float sizeCase = 0;
		// ensemble des textures chargé dans le jeu
		public static List<Texture2D> texture2Ds = new List<Texture2D>();
		// détermine le nombre de case affiché à l'écran
		public Vector2 nbCaseAffiche;
		// conteur d'FPS
		private FPS _frameCounter;

		/*				CONSTRUCTEUR			*/
		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			
			Content.RootDirectory = "Content";

			//graphics.IsFullScreen = true;
			this.IsMouseVisible = true;

			maps = new List<Map>();
			graphics.GraphicsProfile = GraphicsProfile.HiDef;
			nbCaseAffiche = new Vector2(0, 0);
		}
		/*				INITIALIZE				*/
		protected override void Initialize()
		{
			// permet de limité le nombre de case afficher sur la map
			nbCaseAffiche.X = 15;
			sizeCase = (int)(Math.Ceiling(this.GraphicsDevice.PresentationParameters.BackBufferWidth / nbCaseAffiche.X));
			nbCaseAffiche.Y = (int)(nbCaseAffiche.X * this.GraphicsDevice.PresentationParameters.BackBufferHeight / this.GraphicsDevice.PresentationParameters.BackBufferWidth);
			// Chargement du jeu
			LoadGame();
			_frameCounter = new FPS(this);

			base.Initialize();
		}
		/*				LOAD CONTENT			*/
		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);
		}
		/*					UPDATE				*/
		protected override void Update(GameTime gameTime)
		{
			// taile de la fenètre
			Rectangle sizeWindow = new Rectangle(0, 0, this.GraphicsDevice.PresentationParameters.BackBufferWidth, this.GraphicsDevice.PresentationParameters.BackBufferHeight);
			
			// Vérifie si un carte doit être affiché ou non
			foreach (Map map in maps) {
				Rectangle realPos = map.RealPos();
				// cache la carte
				if (!realPos.Intersects(sizeWindow))
				{
					map.Hide();
				}
				// affiche la carte
				else if (realPos.Intersects(sizeWindow))
				{
					map.Show();
				}
			}
			// lecture des touches. A retirer à terme
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();


			base.Update(gameTime);

		}
		/*					DRAW				*/
		protected override void Draw(GameTime gameTime)
		{
			float fps = _frameCounter.CurrentFramesPerSecond;
			var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
			_frameCounter.Update(deltaTime);


			GraphicsDevice.BlendState = BlendState.Opaque;
			GraphicsDevice.DepthStencilState = DepthStencilState.Default;
			GraphicsDevice.Clear(Color.CornflowerBlue);
			base.Draw(gameTime);
		}

		// méthode à appelé quand on charge une texture. A l'instar d'un singleton, elle permet de ne charger qu'une seul fois chaque texture
		public static Texture2D creerTexture(Game1 game, String file)
		{
			// recherche si la texture n'existe pas déjà
			int i = 0;
			while (i < texture2Ds.Count)
			{
				if (texture2Ds[i].Name == file) return texture2Ds[i];
				++i;
			}
			// créé la texture si celle ci n'est pas encore chargé
			Texture2D nouvelle = game.Content.Load<Texture2D>(file);
			// ajoute la texture au groupe
			texture2Ds.Add(nouvelle);
			return nouvelle;
		}

		public void Exit(ATHCompositeButton ath)
		{
			Console.WriteLine("tu clique sur Exit");
			ath.getBase().Remove();
			Exit();
		}
		public void NewGame(ATHCompositeButton ath)
		{
			Console.WriteLine("tu clique sur NewGame");
			ath.getBase().Remove();
		}
		public void LoadGame(ATHCompositeButton ath)
		{
			Console.WriteLine("tu clique sur LoadGame");
			ath.getBase().Remove();
			LoadGame();
		}
		public void LoadGame()
		{
			// Création de carte de test

			Map m1 = Map.Create(this, "test1", new DimensionMap(90, 90, new Vector2(0, 0)), "testSprite1",10);
			Map m2 = Map.Create(this, "test2", new DimensionMap( 90, 120, new Vector2(0, -120)), "testSprite2",10);
			Map m3 = Map.Create(this, "test3", new DimensionMap( 120, 90, new Vector2(-120, 0)), "testSprite3",10);
			Map m4 = Map.Create(this, "test4", new DimensionMap( 120, 120, new Vector2(-120, -120)), "testSprite4",10);
			Map m5 = Map.Create(this, "test5", new DimensionMap( 90, 90, new Vector2(-210, 0)), "testSprite1", 10);
			Map m6 = Map.Create(this, "test6", new DimensionMap( 90, 120, new Vector2(-210, -120)), "testSprite2", 10);
			Map m7 = Map.Create(this, "test7", new DimensionMap( 120, 90, new Vector2(-330, 0)), "testSprite3", 10);
			Map m8 = Map.Create(this, "test8", new DimensionMap( 120, 120, new Vector2(-330, -120)), "testSprite4", 10);
			Map m9 = Map.Create(this, "test9", new DimensionMap( 90, 90, new Vector2(0, -210)), "testSprite1", 10);
			Map m10 = Map.Create(this, "test10", new DimensionMap( 90, 120, new Vector2(0, -330)), "testSprite2", 10);
			Map m11 = Map.Create(this, "test11", new DimensionMap( 120, 90, new Vector2(-120, -210)), "testSprite3", 10);
			Map m12 = Map.Create(this, "test12", new DimensionMap( 120, 120, new Vector2(-120, -330)), "testSprite4", 10);
			Map m13 = Map.Create(this, "test13", new DimensionMap( 90, 90, new Vector2(-210, -210)), "testSprite1", 10);
			Map m14 = Map.Create(this, "test14", new DimensionMap( 90, 120, new Vector2(-210, -330)), "testSprite2", 10);
			Map m15 = Map.Create(this, "test15", new DimensionMap( 120, 90, new Vector2(-330, -210)), "testSprite3", 10);
			Map m16 = Map.Create(this, "test16", new DimensionMap( 120, 120, new Vector2(-330, -330)), "testSprite4", 10);

			// Truc vraiment chiant a faire pour créer les relations entre les maps... ça sera automatisé à terme ou stocké dans un fichier.
			Map.CreateNeibhourg(m1, m2); Map.CreateNeibhourg(m1, m3); Map.CreateNeibhourg(m1, m4);
			Map.CreateNeibhourg(m2, m4); Map.CreateNeibhourg(m2, m3); Map.CreateNeibhourg(m2, m11); Map.CreateNeibhourg(m2, m9); 
			Map.CreateNeibhourg(m3, m4); Map.CreateNeibhourg(m3, m5); Map.CreateNeibhourg(m3, m6);
			Map.CreateNeibhourg(m4, m5); Map.CreateNeibhourg(m4, m6); Map.CreateNeibhourg(m4, m13); Map.CreateNeibhourg(m4, m11); Map.CreateNeibhourg(m4, m9);
			Map.CreateNeibhourg(m5, m6); Map.CreateNeibhourg(m5, m8); Map.CreateNeibhourg(m5, m7);
			Map.CreateNeibhourg(m6, m7); Map.CreateNeibhourg(m6, m8); Map.CreateNeibhourg(m6, m15); Map.CreateNeibhourg(m6, m13); Map.CreateNeibhourg(m6, m11);
			Map.CreateNeibhourg(m7, m8);
			Map.CreateNeibhourg(m8, m15); Map.CreateNeibhourg(m8, m13);
			Map.CreateNeibhourg(m9, m10); Map.CreateNeibhourg(m9, m12); Map.CreateNeibhourg(m9, m11);
			Map.CreateNeibhourg(m10, m11); Map.CreateNeibhourg(m10, m12);
			Map.CreateNeibhourg(m11, m14); Map.CreateNeibhourg(m11, m12); Map.CreateNeibhourg(m11, m13);
			Map.CreateNeibhourg(m12, m14); Map.CreateNeibhourg(m12, m13);
			Map.CreateNeibhourg(m13, m14); Map.CreateNeibhourg(m13, m16); Map.CreateNeibhourg(m13, m15);
			Map.CreateNeibhourg(m14, m16); Map.CreateNeibhourg(m14, m15);
			Map.CreateNeibhourg(m15, m16);
			this.player = Player.Create(m1);
		}
		public void Back(ATHCompositeButton ath)
		{
			Console.WriteLine("tu clique sur Back");
			ath.getBase().Remove();
		}
		public void SaveGame(ATHCompositeButton ath)
		{
			Console.WriteLine("tu clique sur SaveGame");
		}
	}
}
