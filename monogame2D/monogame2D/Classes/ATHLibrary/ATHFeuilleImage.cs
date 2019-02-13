using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace monogame2D.ATHLibrary
{
	public class ATHFeuilleImage : ATH
	{
		protected string texture;
		protected Texture2D sprite;
		public ATHFeuilleImage(ATH parent, Vector2 position, Vector2 size, string texture, Anchor anchor = Anchor.topleft, int order = 0) : base(parent, position, size, anchor, order)
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
		public static ATHFeuilleImage Create(ATHComposite parent, Vector2 position, Vector2 size, string texture, Anchor anchor = Anchor.topleft, int order = 0)
		{
			ATHFeuilleImage ath = new ATHFeuilleImage(parent, position, size, texture, anchor, order);
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
