using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using monogame2D.MapLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace monogame2D.ComponentLibrary
{
	public class Sprite : DrawableGameComponent
	{
		protected Decor father;
		protected string texture;
		protected Texture2D sprite;
		protected Vector2 position;
		protected Vector2 size;
		protected static SpriteBatch spriteBatch;

		public Sprite(Decor father, Vector2 position, string texture, Vector2 size) : base(father.Game)
		{
			this.texture = texture;
			this.position = position;
			this.size = size;
			this.father = father;
		}
		public Sprite(Decor father, string texture) : base(father.Game)
		{
			this.texture = texture;
			this.position = new Vector2(0,0);
			this.size = new Vector2(100, 100) ;
			this.father = father;
		}
		public override void Initialize()
		{
			base.Initialize();
		}
		protected override void LoadContent()
		{
			base.LoadContent();
			// ne crée qu'un seul spriteBatch pour toutes les cartes
			if (Map.SpriteBatch == null && spriteBatch == null) spriteBatch = new SpriteBatch(father.Game.GraphicsDevice);
			else if (spriteBatch == null) spriteBatch = Map.SpriteBatch;
			// création du sprite
			this.sprite = Game1.creerTexture(this.father.Game, texture);
		}
		public override void Draw(GameTime gameTime)
		{
			// début du dessin
			spriteBatch.Begin();
			spriteBatch.Draw(sprite,
							RealPos(),
							Color.White);
			spriteBatch.End();
			
			base.Draw(gameTime);
		}
		public Rectangle RealPos()
		{
			Rectangle pos = father.RealPos();
			pos.X += (int)(position.X * father.Size.X * (int)Game1.sizeCase / 100f);
			pos.Y += (int)(position.Y * father.Size.Y * (int)Game1.sizeCase / 100f - father.Height * (int)Game1.sizeCase);
			pos.Width = (int)(size.X * father.Size.X * (int)Game1.sizeCase / 100f);
			pos.Height = (int)(size.Y * father.Size.Y * (int)Game1.sizeCase / 100f);
			return pos;
		}
	}
}
