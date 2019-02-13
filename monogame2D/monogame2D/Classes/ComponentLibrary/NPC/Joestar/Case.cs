using Microsoft.Xna.Framework;
using monogame2D.MapLibrary;
using System;

namespace monogame2D.JoeStar
{
	class Case
	{
		public int x;
		public int y;
		public float distance;
		private Map map;
		private Vector2 destination;
		public Case parent;
		public float height = 0;
		public int distanceToOrigin;

		public Case(Map map, int x, int y, Vector2 destination)
		{
			this.map = map;
			this.x = x;
			this.y = y;
			this.destination = destination;

			CalculDistance();
			
			foreach (Carto carto in map.Cartos.Cartos)
			{
				if (carto.Height > height && carto.GetCase(x, y) > 0) height = carto.Height;
			}
		}
		public void AddParent(Case parent)
		{
			this.parent = parent;
			distanceToOrigin = parent.distanceToOrigin + 1;
		}
		private void CalculDistance()
		{
			distance = (float)Math.Sqrt(Math.Pow(Math.Abs(destination.X - x),2) + Math.Pow(Math.Abs(destination.Y - y),2));
		}
		public Vector2 getFirstCase()
		{
			//Console.Write("[" + x + "; " + y + "; " + distance + "], ");
			if (distanceToOrigin > 1) return parent.getFirstCase();
			else return new Vector2(x, y);
		}
	}
}