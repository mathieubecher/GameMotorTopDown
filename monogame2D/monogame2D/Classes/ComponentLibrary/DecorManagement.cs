using Microsoft.Xna.Framework;
using monogame2D.MapLibrary;
using System.Collections.Generic;

namespace monogame2D.ComponentLibrary
{
	public class DecorManagement : GameComponent
	{
		/*				VARIABLES				*/
		// map ou se trouve les décors
		private Map map;
		// listes des décors
		private List<Decor> decors;

		public List<Decor> Decors { get => decors;}

		/*				CONSTRUCTEUR			*/
		public DecorManagement(Map map) : base(map.Game)
		{
			this.map = map;
			decors = new List<Decor>();
		}

		/*					UPDATE				*/
		public override void Update(GameTime gameTime)
		{
			for (int i = 0; i < decors.Count; i++)
			{

				Decor d = decors[i];
				Vector2 pos = d.Origin;
				d.Update(gameTime);
				
				if (d.ShowSprite)
				{
					if (d.Movable && pos != d.Origin)
					{
						d.DrawToMap();
						d.GestionHeight();
					}
					else map.ToDraw.Add(d);
				}
			}
			

		}

		// ajout d'un décor
		public void Add(Decor d)
		{
			decors.Add(d);
		}
		// supresion d'un décor
		public void Remove(Decor d)
		{
			decors.Remove(d);
		}
		public List<Character> GetCharacters()
		{
			List<Character> npcs = new List<Character>();
			for (int i = 0; i < decors.Count; i++)
			{
				if (decors[i] is Character) npcs.Add((Character)decors[i]);
			}
			return npcs;
		}

	}
}