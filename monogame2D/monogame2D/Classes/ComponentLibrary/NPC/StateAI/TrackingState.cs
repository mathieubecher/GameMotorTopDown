using Microsoft.Xna.Framework;
using monogame2D.ComponentLibrary;
using monogame2D.JoeStar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace monogame2D.StateAI
{
	public class TrackingState : StandardState
	{
		// Composant que l'ia poursuit
		private Component target;
		// Algorithme du plus court chemin
		private Joestar joestar;

		/*				CONSTRUCTEUR			*/
		public TrackingState(AI ai, Component target) : base(ai)
		{
			joestar = new Joestar(ai, ai.Map, ai.Tenacity);
			this.target = target;
			ai.Important = true;
		}

		// Recherche de la prochaine case à atteindre
		public override void NextCase()
		{
			if (distance <= 0)
			{
				velocityOrigin = joestar.NextCase(target, ai.Map.Game.player.Height);
				distance = 1;
				if (joestar.near) distance = 0;
				if (velocityOrigin.X > 0.5f) velocityOrigin.X = 1;
				else if (velocityOrigin.X < -0.5f) velocityOrigin.X = -1;
				if (velocityOrigin.Y > 0.5f) velocityOrigin.Y = 1;
				else if (velocityOrigin.Y < -0.5f) velocityOrigin.Y = -1;

				
				if (!joestar.couldFind)
				{
					ai.Sprites = new List<Sprite>();
					Sprite sprite = new Sprite(ai, "boutonSearch");
					ai.Sprites.Add(sprite);
					sprite.Initialize();

					ai.State = new SearchState(ai);
				}
			}
		}

		// Recherche la cible
		public override void SearchTarget()
		{

			double distance = Math.Pow(target.Origin.X - ai.Origin.X, 2) + Math.Pow(target.Origin.Y - ai.Origin.Y, 2);
			if (target.Map.Name != ai.Map.Name || distance > Math.Pow(ai.Tenacity, 2))
			{
				ai.Sprites = new List<Sprite>();
				Sprite sprite = new Sprite(ai, "boutonStandard");
				ai.Sprites.Add(sprite);
				sprite.Initialize();

				ai.State = new StandardState(ai);
			}

		}
	}
}
