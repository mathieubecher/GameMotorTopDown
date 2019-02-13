using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace monogame2D.ATHLibrary
{
	class ATHFeuilleText : ATH
	{
		protected SpriteFont font;
		protected float sizeFont;
		protected string text;
		protected Color color;
		private Anchor align;

		public string Text { get => text; set => text = value; }

		protected ATHFeuilleText(ATH parent, Vector2 position, Vector2 size, string text, Color color, float sizeFont, Anchor anchor = Anchor.topleft, Anchor align = Anchor.center, int order = 0) : base(parent, position, size, anchor, order)
		{
			this.Text = text;
			this.color = color;
			this.sizeFont = sizeFont;
			this.align = align;
		}

		protected override void LoadContent()
		{
			font = game.Content.Load<SpriteFont>("File");
			base.LoadContent();
		}
		public override Rectangle RealPos()
		{
			// position de base de l'ath
			Rectangle realPos = base.RealPos();
			int x = (int)(realPos.X);
			int y = (int)(realPos.Y);
			// centre le texte en x
			if (align == Anchor.center || align == Anchor.top || align == Anchor.bottom)
				x += realPos.Width / 2 - (int)(this.font.MeasureString(Text).X * sizeFont/ 2);
			// positionne le texte à droite
			else if (align == Anchor.right || align == Anchor.topright || align == Anchor.bottomright)
				x += realPos.Width - (int)(this.font.MeasureString(Text).X * sizeFont);
			// centre le texte en y
			if (align == Anchor.center || align == Anchor.left || align == Anchor.right)
				y += realPos.Height / 2 - (int)(this.font.MeasureString(Text).Y * sizeFont/ 2);
			// positionne le texte en bas
			else if (align == Anchor.bottom || align == Anchor.bottomleft || align == Anchor.bottomright)
				y += realPos.Height - (int)(this.font.MeasureString(Text).Y * sizeFont);

			Rectangle textPos = new Rectangle(x,y, (int)(this.font.MeasureString(Text).X * sizeFont), (int)(this.font.MeasureString(Text).Y * sizeFont));
			return textPos;

		}

		public override void Draw(GameTime gameTime)
		{
			spriteBatch.Begin();
			
			spriteBatch.DrawString(
					this.font,
					this.Text,
					new Vector2(RealPos().X,RealPos().Y), 
					color,
					0,
					new Vector2(0,0),
					sizeFont,
					SpriteEffects.None,
					0
				);

			spriteBatch.End();

			base.Draw(gameTime);
		}

		// Création d'un athfeuilletext
		public static ATHFeuilleText Create(ATHComposite parent, Vector2 position, Vector2 size, string text, Color color, float sizeFont, Anchor anchor = Anchor.topleft, Anchor align = Anchor.center, int order = 0)
		{
			ATHFeuilleText ath = new ATHFeuilleText(parent, position, size, text, color, sizeFont, anchor,align, order);
			parent.game.Components.Add(ath);
			parent.Add(ath);
			return ath;
		}
		public override void Remove()
		{
			base.Remove();
		}
	}
}
