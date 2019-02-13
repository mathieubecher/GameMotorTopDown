using Microsoft.Xna.Framework;
using monogame2D.ComponentLibrary;
using monogame2D.JoeStar;
using System;
using System.Collections.Generic;

namespace monogame2D.StateAI
{
	
	public class StandardState
	{
		// ai traité
		protected AI ai;
		// timer entre chaque nouvelle instruction
		protected float distance = 0;
		// velocite de déplacement
		protected Vector2 velocityOrigin;

		/*				CONSTRUCTEUR			*/
		public StandardState(AI ai)
		{
			this.ai = ai;
			ai.Important = false;
		}

		// action a réaliser à chaque update
		public virtual void StandardAction(GameTime gameTime)
		{
			float s = ai.Speed.getSpeed(gameTime);
			NextCase();

			Vector2 velocity = velocityOrigin * s;
			if (velocity.X != 0 && velocity.Y != 0) velocity *= 0.7f;

			distance -= Math.Abs((velocity.X != 0) ? velocity.X : ((velocity.Y != 0) ? velocity.Y : 1));
			ai.Move(velocity);

			SearchTarget();
		}

		// prochaine case à atteindre
		public virtual void NextCase()
		{
			
			if (distance <= 0)
			{
				Random rng = new Random((int)DateTime.Now.Ticks);
				if (rng.Next(0, 10) > 5)
				{
					int x = rng.Next(-1, 2);
					int y = rng.Next(-1, 2);

					velocityOrigin = new Vector2(x, y);
				}

				distance = 1;
			}
		}

		// Recherche une cible
		public virtual void SearchTarget()
		{
			foreach (Character npc in ai.Map.Decors.GetCharacters())
			{
				if (npc.Type != ai.Type)
				{
					double distance = Math.Pow(npc.Origin.X - ai.Origin.X, 2) + Math.Pow(npc.Origin.Y - ai.Origin.Y, 2);
					if (distance < Math.Pow(ai.Vigilance, 2))
					{
						ai.Sprites = new List<Sprite>() ;
						Sprite sprite = new Sprite(ai, "boutonTrack");
						ai.Sprites.Add(sprite);
						sprite.Initialize();

						ai.State = new TrackingState(ai, npc);
						break;
					}
				}
			}
		}
		// non pris en compte pour le moment
		public virtual void TakeDamage()
		{
			
		}

	}
}