using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace monogame2D.ATHLibrary
{
	public class ATHCompositeButton : ATHComposite
	{
		// Permet de savoir si l'utilisateur appui sur le bouton gauche de la souris ou non
		protected bool press = false;
		// Indique l'action à réalisé lors du clic
		protected ButtonType type;

		protected ATHCompositeButton(Game1 game, Vector2 position, Vector2 size, Anchor anchor = Anchor.topleft, int order = 0, ButtonType type = ButtonType.back) : base(game, position, size, anchor, order)
		{
			this.type = type;
		}
		protected ATHCompositeButton(ATH parent, Vector2 position, Vector2 size, Anchor anchor = Anchor.topleft, int order = 0, ButtonType type = ButtonType.back) : base(parent, position, size, anchor, order)
		{
			this.type = type;
		}
		public override void Update(GameTime gameTime)
		{
			Rectangle hitbox = new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 20, 20);
			// ACTION BOUTON
			if (RealPos().Intersects(hitbox))
			{
				if (Mouse.GetState().LeftButton.CompareTo(ButtonState.Pressed) == 0 && !press)
				{
					switch (type) {
						case ButtonType.exit: game.Exit(this); break;
						case ButtonType.newGame: game.NewGame(this); break;
						case ButtonType.loadGame: game.LoadGame(this); break;
						case ButtonType.saveGame: game.SaveGame(this); break;
						default: game.Back(this); break;
					}
					press = true;
					
				}
			}
			// Evite de cliquer plusieurs fois, il faut que le clic soit relaché avant que l'action soit à nouveau pris en compte
			if (Mouse.GetState().LeftButton.CompareTo(ButtonState.Pressed) != 0 && press)
			{
				press = false;

			}
			base.Update(gameTime);
		}
		// Création de bouton, quand celui ci n'est pas dans un ATH
		public static ATHCompositeButton Create(Game1 game, Vector2 position, Vector2 size, Anchor anchor = Anchor.topleft, int order = 0, ButtonType type = ButtonType.back)
		{
			ATHCompositeButton ath = new ATHCompositeButton(game, position, size, anchor, order, type);
			game.Components.Add(ath);
			return ath;
		}
		// Création de bouton, quand celui ci est dans un ATH
		public static ATHCompositeButton Create(ATHComposite parent, Vector2 position, Vector2 size, Anchor anchor = Anchor.topleft, int order = 0, ButtonType type = ButtonType.back)
		{
			ATHCompositeButton ath = new ATHCompositeButton(parent, position, size, anchor, order, type);
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
