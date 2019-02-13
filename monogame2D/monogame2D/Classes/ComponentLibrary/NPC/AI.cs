using Microsoft.Xna.Framework;
using monogame2D.JoeStar;
using monogame2D.MapLibrary;
using monogame2D.StateAI;
using System;
using System.Collections.Generic;

namespace monogame2D.ComponentLibrary
{
	public class AI : Character
	{
		/*				VARIABLES				*/
		// Diagramme état d'une ia
		private StandardState state;
		// vélocité à appliqué jusqu'a ce que distance soit égal à zéro
		Vector2 velocityOrigin = Vector2.Zero;
		private float vigilance;
		private float tenacity;
		

		/*				CONSTRUCTEUR			*/
		protected AI(Map map, Vector2 origin, Vector2 position, Vector2 size, Rectangle hitbox, CreatureType type = CreatureType.pacific,float vigilance = 5, float tenacity = 8, float height = 0, bool solid = true, float speed = 1000) : base(map, origin, position, size, hitbox, type, height, solid)
		{
			this.speed = new Speed(speed);
			State = new StandardState(this);
			this.vigilance = vigilance;
			this.tenacity = tenacity;
			
		}

		public StandardState State { get => state; set => state = value; }
		public float Vigilance { get => vigilance;}
		public float Tenacity { get => tenacity;}

		// Génération d'une IA
		public static AI Create(Map map, Vector2 origin, Vector2 position, Vector2 size, string texture, CreatureType type, float vigilance, float tenacity, float speed = 1000)
		{
			AI d = new AI(map, origin, position, size,new Rectangle(0,0,100,100),type,vigilance, tenacity,speed);
			Sprite s = new Sprite(d, texture);
			d.Initialize();
			d.sprites.Add(s);
			s.Initialize();
			map.Decors.Add(d);
			return d;
		}
		// Génération d'une IA
		public static AI Create(Map map, Vector2 origin, Vector2 position, Vector2 size, string texture, Rectangle hitbox, CreatureType type, float vigilance, float tenacity, float speed = 1000)
		{
			AI d = new AI(map, origin, position, size,hitbox,type,vigilance,tenacity,speed);
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
			float lastHeight = height;
			base.Update(gameTime);

			if (ShowSprite || important)
			{
				State.StandardAction(gameTime);
			}
			if (lastHeight < height) Console.WriteLine("{BUG}");
		}
	}
}
