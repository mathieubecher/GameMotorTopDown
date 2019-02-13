using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace monogame2D.ATHLibrary
{
	public class ATHCompositeImage : ATHComposite
	{
		protected string texture;
		protected Texture2D sprite;
		protected ATHCompositeImage(Game1 game, Vector2 position, Vector2 size, string texture, Anchor anchor = Anchor.topleft, int order = 0) : base(game, position, size, anchor, order)
		{
			this.texture = texture;
		}
		protected ATHCompositeImage(ATH parent, Vector2 position, Vector2 size,string texture, Anchor anchor = Anchor.topleft, int order = 0) : base(parent, position, size, anchor, order)
		{
			this.texture = texture;
		}
		protected override void LoadContent()
		{
			base.LoadContent();
			this.sprite = Game1.creerTexture(game,texture);
		}
		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
		}
		public override void Draw(GameTime gameTime)
		{
			base.Draw(gameTime);
			spriteBatch.Begin();
			spriteBatch.Draw(sprite, RealPos(), Color.White);
			spriteBatch.End();
		}
		public static ATHCompositeImage Create(Game1 game, Vector2 position, Vector2 size, string texture, Anchor anchor = Anchor.topleft, int order = 0)
		{
			ATHCompositeImage ath = new ATHCompositeImage(game, position, size,texture, anchor, order);
			game.Components.Add(ath);
			return ath;
		}
		public static ATHCompositeImage Create(ATHComposite parent, Vector2 position, Vector2 size, string texture, Anchor anchor = Anchor.topleft, int order = 0)
		{
			ATHCompositeImage ath = new ATHCompositeImage(parent, position, size,texture, anchor, order);
			parent.game.Components.Add(ath);
			parent.Add(ath);
			return ath;
		}
		public override void Remove()
		{
			sprite.Dispose();
			base.Remove();
		}
	}
}
