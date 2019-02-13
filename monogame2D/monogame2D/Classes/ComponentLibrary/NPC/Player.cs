using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using monogame2D.MapLibrary;
using System;
using System.Collections.Generic;

namespace monogame2D.ComponentLibrary
{
	public class Player : Character
	{
		/*				CONSTRUCTEUR			*/
		private Player(Map map, Vector2 origin, Vector2 position, Vector2 size, Rectangle hitbox, float height = 0,bool solid = true) : base(map,origin,position,size, hitbox)
		{
			this.speed = new Speed(500);
			this.type = CreatureType.pacific;
		}
		
		public static Player Create(Map map)
		{

			Player j = new Player(map, new Vector2(10, 10), Vector2.Zero, new Vector2(1f, 1f), new Rectangle(0,0,100,100));
			j.hitbox = new Rectangle(0,0,100,100);

			Sprite s = new Sprite(j, "boutonStandard");
			j.Initialize();
			j.Sprites.Add(s);
			s.Initialize();

			map.Decors.Add(j);

			return j;
		}
		// Renvoi la position du joueur sur la carte. Cette position est utilisé partout pour centrer l'environnement sur le joueur
		public Rectangle GetPos()
		{
			return new Rectangle((int)((Origin.X + map.Size.Origin.X) * (int)Game1.sizeCase), (int)((Origin.Y + map.Size.Origin.Y) * (int)Game1.sizeCase), (int)(this.size.X * (int)Game1.sizeCase), (int)(this.size.Y * (int)Game1.sizeCase));
		}
		/*				UPDATE			*/
		public override void Update(GameTime gameTime)
		{
			// calcul le nombre de case que le joueur doit parcourir cette frame (assure une vitesse constante même en cas de lag)
			float s = this.speed.getSpeed(gameTime);
			Vector2 velocity = Vector2.Zero;
			// capture les actions du joueur
			if (Keyboard.GetState().IsKeyDown(Keys.Z))
			{
				velocity.Y -= s;
			}
			if (Keyboard.GetState().IsKeyDown(Keys.Q))
			{
				velocity.X -= s;
			}
			if (Keyboard.GetState().IsKeyDown(Keys.S))
			{
				velocity.Y += s;
			}
			if (Keyboard.GetState().IsKeyDown(Keys.D))
			{
				velocity.X += s;
			}
			// modifie la vitesse en x et en y si le joueur se déplace en diagonal
			if (velocity.X != 0 && velocity.Y != 0) velocity = velocity * 0.7f;

			if (velocity.X != 0 || velocity.Y != 0)
			{
				// Gestion des collisions + déplacement
				Move(velocity);
			}
			
			// Mise à jour de la position réel des cartes (action nécessaire car elle permet de calculer RealPos() de chaque carte qu'une seul fois par frame
			for (int i = 0; i < game.maps.Count; i++) game.maps[i].IsLastPos = false;

			

			base.Update(gameTime);
		}
		
	}
}
