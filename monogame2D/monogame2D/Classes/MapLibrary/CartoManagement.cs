using Microsoft.Xna.Framework;
using monogame2D.ComponentLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace monogame2D.MapLibrary
{
	class CartoManagement : GameComponent
	{
		/*				VARIABLES				*/
		// map ou se trouve les décors
		private Map map;
		// listes des décors
		private List<Carto> cartos;
		// outils de comparaison permettant de trier la list toDraw par DrawOrder
		private HeightCompare heightCompare;

		public List<Carto> Cartos { get => cartos; }

		/*				CONSTRUCTEUR			*/
		public CartoManagement(Map map) : base(map.Game)
		{
			this.heightCompare = new HeightCompare();
			this.map = map;
			cartos = new List<Carto>();
		}
		/*					UPDATE				*/
		public override void Update(GameTime gameTime)
		{
			for (int i = 0; i < cartos.Count; i++)
			{
				cartos[i].Update(gameTime);
				//if (cartos[i].ShowSprite) map.ToDraw.Add(cartos[i]);
			}

		}
		// ajout d'un décor
		public void Add(Carto c)
		{
			cartos.Add(c);
			cartos.Sort(heightCompare);
		}
		// supresion d'un décor
		public void Remove(Carto c)
		{
			cartos.Remove(c);
		}
	}
}
