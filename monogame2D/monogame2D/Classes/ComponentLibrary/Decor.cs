using Microsoft.Xna.Framework;
using monogame2D.MapLibrary;
using System;
using System.Collections.Generic;

namespace monogame2D.ComponentLibrary
{
	public class Decor : DrawableGameComponent
	{
		/*				VARIABLES				*/
		protected Game1 game;
		protected Map map;
		// case ou se trouve le décor
		protected Vector2 origin;

		// taille de l'objet défini par la taille d'une case
		protected Vector2 size;
		// indique si le décor est plat ou non
		private bool flat;
		// liste des sprites
		protected List<Sprite> sprites;
		// détermine si l'objet doit être affiché ou non
		private bool show;
		protected float height;
		// précise si l'objet est déplaçable ou non
		protected bool movable = false;
		// détermine si l'objet est solide
		protected bool solid = false;

		/*				GETTER SETTER			*/
		public new Game1 Game { get => game; set => game = value; }
		public bool Flat { get => flat; }
		public bool ShowSprite { get => show; }
		public Map Map { get => map; set => map = value; }
		public float Height { get => height; set => height = value; }
		public List<Sprite> Sprites { get => sprites; set => sprites = value; }
		public bool Movable { get => movable; set => movable = value; }
		public Vector2 Origin { get => origin;  }
		public Vector2 Size { get => size; set => size = value; }
		public bool Solid { get => solid; }

		/*				CONSTRUCTEUR			*/
		protected Decor(Map map, Vector2 origin, Vector2 position, Vector2 size, bool flat = true, float height = 0) : base(map.Game)
		{
			this.map = map;
			this.origin = origin + position;
			this.size = size;
			this.flat = flat;
			this.game = map.Game;
			show = false;
			sprites = new List<Sprite>();
			this.height = height;
		}

		// Génération d'un décor
		public static Decor Create(Map map, Vector2 origin, Vector2 position, Vector2 size, string texture, bool flat = false, float height = 0)
		{
			Decor d = new Decor(map, origin, position, size, flat, height);
			Sprite s = new Sprite(d, texture);
			d.Initialize();
			d.sprites.Add(s);
			s.Initialize();
			map.Decors.Add(d);

			return d;
		}



		/*					UPDATE				*/
		public override void Update(GameTime gameTime)
		{

			// si l'objet est présent dans la fenètre
			if (map.ShowCase.X <= origin.X + size.X && map.ShowCase.X + map.ShowCase.Width >= origin.X &&
				map.ShowCase.Y <= origin.Y + size.Y - height && map.ShowCase.Y + map.ShowCase.Height >= origin.Y - height)
			{
				// préciser qu'il est visible
				if (!show)
					show = true;


				// détermination de son ordre d'affichage
				CalculDrawOrder();

			}
			// préciser qu'il n'est pas visible
			else if (show) show = false;



			base.Update(gameTime);
		}

		/*					DRAW				*/
		public override void Draw(GameTime gameTime)
		{
			// Lance le dessin de tous les sprites
			for (int i = 0; i < sprites.Count; i++) sprites[i].Draw(gameTime);
		}

		// Met à jour automatiquement la hauteur d'un objet si il est plus bas que la plateforme sur laquel il se trouve
		public virtual void GestionHeight()
		{
			float minheight = 0;
			for (int x = (int)Origin.X; x < (int)(Math.Ceiling(Origin.X + Size.X)); x++)
			{
				int y = (int)(Math.Floor(Origin.Y + Size.Y));

				foreach (Carto f in map.Cartos.Cartos)
				{
					if (f.GetCase(x, y) > 0 && f.Height > minheight)
					{
						minheight = f.Height;
					}
				}
			}
			height = minheight;
		}
		// Calcul l'odre d'affichage du décor
		public virtual void CalculDrawOrder()
		{
			if (Flat) this.DrawOrder = RealPos().Y * game.GraphicsDevice.PresentationParameters.BackBufferWidth + RealPos().X;
			else this.DrawOrder = (RealPos().Y + RealPos().Height) * game.GraphicsDevice.PresentationParameters.BackBufferWidth + RealPos().X;
		}

		// position réel du décor
		public Rectangle RealPos()
		{
			Rectangle mapPos = map.RealPos();
			return new Rectangle((int)(Math.Floor((this.origin.X) * (int)Game1.sizeCase + mapPos.X)), (int)(Math.Floor((this.origin.Y) * (int)Game1.sizeCase + mapPos.Y)), (int)(size.X * (int)Game1.sizeCase), (int)(size.Y * (int)Game1.sizeCase));
		}
		public override void Initialize()
		{
			base.Initialize();
		}

		// Suppression du décor
		public void Remove()
		{
			game.Components.Remove(this);
			this.Dispose();
			foreach (Sprite sprite in sprites)
			{
				sprite.Dispose();
				game.Components.Remove(sprite);
			}
		}

		// Gère les changements de cartes des objets déplaçable
		public void DrawToMap()
		{
			int i = 0;
			while (i < game.maps.Count && !RealPos().Intersects(game.maps[i].RealPos())) ++i;
			if (i < game.maps.Count)
			{
				game.maps[i].ToDraw.Add(this);
				if (map != game.maps[i])
				{
					origin += -game.maps[i].Size.Origin + map.Size.Origin;
					map.Decors.Remove(this);
					map = game.maps[i];
					map.Decors.Add(this);
				}
			}
		}

	}
}
