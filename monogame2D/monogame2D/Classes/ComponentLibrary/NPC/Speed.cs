using Microsoft.Xna.Framework;
using System;

namespace monogame2D.ComponentLibrary
{
	public class Speed
	{
		// durée en ms nécessaire pour parcourir une case
		private float timeTravelCase;

		public Speed(float timeTravelCase)
		{
			this.timeTravelCase = timeTravelCase;
			
		}
		public float getSpeed(GameTime gameTime)
		{
			float speed = (float)(gameTime.ElapsedGameTime.TotalMilliseconds) /timeTravelCase;				
			return speed;
		}
	}
}