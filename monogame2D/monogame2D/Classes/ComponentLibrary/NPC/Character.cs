using Microsoft.Xna.Framework;
using monogame2D.ComponentLibrary;
using monogame2D.MapLibrary;

namespace monogame2D.ComponentLibrary
{
	public class Character : Component
	{
		// Type
		protected CreatureType type;
		// vitesse de l'IA
		protected Speed speed;
		public Speed Speed { get => speed;}
		public CreatureType Type { get => type;}

		protected Character(Map map, Vector2 origin, Vector2 position, Vector2 size, Rectangle hitbox, CreatureType type = CreatureType.pacific, float height = 0, bool solid = true) : base(map, origin, position, size, hitbox, height, solid)
		{
			this.speed = new Speed(500);
			this.movable = true;
			this.type = type;
		}

		
	}
}