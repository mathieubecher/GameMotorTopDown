using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace monogame2D.ATHLibrary
{
	public class ATHFeuilleColor : ATH
	{
		protected Color color;
		protected Texture2D texture;
		public ATHFeuilleColor(ATH parent, Vector2 position, Vector2 size, Color color, Anchor anchor = Anchor.topleft, int order = 0) : base(parent, position, size, anchor, order)
		{
			this.anchor = anchor;
			this.color = color;
		}
		protected override void LoadContent()
		{
			base.LoadContent();
			texture = new Texture2D((this.Game).GraphicsDevice, 1, 1);
			texture.SetData(new Color[] { color });
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
		}
		public override void Draw(GameTime gameTime)
		{
			base.Draw(gameTime);
			spriteBatch.Begin();
			spriteBatch.Draw(texture, RealPos(), Color.White);
			spriteBatch.End();
		}
		public static ATHFeuilleColor Create(ATHComposite parent, Vector2 position, Vector2 size, Color color, Anchor anchor = Anchor.topleft, int order = 0)
		{
			ATHFeuilleColor ath = new ATHFeuilleColor(parent, position, size, color, anchor, order);
			parent.game.Components.Add(ath);
			parent.Add(ath);
			return ath;
		}
		public override void Remove()
		{
			texture.Dispose();
			base.Remove();
		}
	}

}
