using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace monogame2D.ATHLibrary
{
	public class ATHCompositeColor : ATHComposite
	{
		protected Color color;
		protected Texture2D texture;
		public ATHCompositeColor(Game1 game, Vector2 position, Vector2 size, Color color, Anchor anchor = Anchor.topleft, int order = 0) : base(game, position, size, anchor, order)
		{
			this.color = color;
		}
		public ATHCompositeColor(ATH parent, Vector2 position, Vector2 size, Color color, Anchor anchor = Anchor.topleft, int order = 0) : base(parent, position, size,anchor, order)
		{
			this.color = color;
		}
		public override void Initialize()
		{
			base.Initialize();
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
		public static ATHCompositeColor Create(Game1 game, Vector2 position, Vector2 size, Color color, Anchor anchor = Anchor.topleft, int order = 0)
		{
			ATHCompositeColor ath = new ATHCompositeColor(game, position, size, color, anchor, order);
			game.Components.Add(ath);
			return ath;
		}
		public static ATHCompositeColor Create(ATHComposite parent, Vector2 position, Vector2 size, Color color, Anchor anchor = Anchor.topleft, int order = 0)
		{
			ATHCompositeColor ath = new ATHCompositeColor(parent, position, size, color, anchor, order);
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
