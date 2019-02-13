using Microsoft.Xna.Framework;
using monogame2D.MapLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace monogame2D.ComponentLibrary
{
	public class Component : Decor
	{
		/*				VARIABLES				*/
		// détermine si l'objet doit être mis à jour même si il n'est pas affiché
		protected bool important = false;
		// hitbox de l'objet
		protected Rectangle hitbox;

		public bool Important { get => important; set => important = value; }
		public Rectangle Hitbox { get => hitbox; }

		/*				CONSTRUCTEUR			*/
		protected Component(Map map, Vector2 origin, Vector2 position, Vector2 size, float height = 0,bool solid = true) : base(map, origin, position, size, false, height)
		{
			this.hitbox = new Rectangle(0, 0,100, 100);
			this.solid = solid;
		}
		protected Component(Map map, Vector2 origin, Vector2 position, Vector2 size, Rectangle hitbox, float height = 0, bool solid = true) : base(map, origin, position, size, false, height)
		{
			this.hitbox = hitbox;
			this.solid = solid;
		}
		// Génération d'un composant
		public static Component Create(Map map, Vector2 origin, Vector2 position, Vector2 size, string texture)
		{
			Component c = new Component(map, origin, position, size);

			Sprite s = new Sprite(c, new Vector2(0, 0), texture, new Vector2(100, 100));
			c.Initialize();
			c.Sprites.Add(s);
			s.Initialize();
			map.Decors.Add(c);
			

			return c;
		}
		// Génération d'un composant
		public static Component Create(Map map, Vector2 origin, Vector2 position, Vector2 size, string texture, Rectangle hitbox)
		{
			Component c = Component.Create(map, origin, position, size, texture);
			c.hitbox = hitbox;

			return c;
		}

		// Met à jour automatiquement la hauteur d'un objet si il est plus bas que la plateforme sur laquel il se trouve (comme pour décor mais avec la hibox)
		public override void GestionHeight()
		{
			float minheight = 0;
			for (int x = (int)(Math.Floor(Origin.X + hitbox.X * size.X / 100)); x < (int)(Math.Ceiling(Origin.X + (hitbox.X + hitbox.Width) * size.X / 100)); ++x)
			{
				for (int y = (int)(Math.Floor(Origin.Y + hitbox.Y * size.Y / 100)); y < (int)(Math.Ceiling(Origin.Y + (hitbox.Y + hitbox.Height) * size.Y / 100)); ++y)
				{

					foreach (Carto f in map.Cartos.Cartos)
					{
						if (f.GetCase(x, y) > 0 && f.Height > minheight)
						{
							minheight = f.Height;
						}
					}
				}

			}
			height = minheight;
		}
		// Calcul l'odre d'affichage du décor (comme pour décor mais avec la hibox)
		public override void CalculDrawOrder()
		{
			this.DrawOrder = (int)((RealPos().Y + (hitbox.Y + hitbox.Height) * Size.Y / 100 * (int)Game1.sizeCase) * game.GraphicsDevice.PresentationParameters.BackBufferWidth + RealPos().X + (hitbox.X + hitbox.Width) * Size.X / 100 * (int)Game1.sizeCase);
		}

		// Déplacement de l'objet après avoir vérifié si il ne rentre pas en collision avec quelque chose
		public void Move(Vector2 velocity)
		{
			velocity = TryCollisionFloor(this, velocity);
			velocity = TryComponent(velocity);
			List<Map> collideMap = collideMapNeighbourg(velocity);
			
			foreach (Map m in collideMap)
			{
				Component c = new Component(m, Origin, Vector2.Zero, size, height, solid);
				c.origin += map.Size.Origin - m.Size.Origin;

				m.Decors.Add(c);
				velocity = Component.TryCollisionFloor(c,velocity);
				velocity = c.TryComponent(velocity);
				m.Decors.Remove(c);
			}
			this.origin += velocity;
		}

		private List<Map> collideMapNeighbourg(Vector2 velocity)
		{
			List<Map> collideMap = new List<Map>();

			Vector2 newOrigin = Origin + velocity;
			if (newOrigin.X < Game1.FRONTIERS || newOrigin.Y < Game1.FRONTIERS || newOrigin.X + size.X > map.Size.NbCaseX - Game1.FRONTIERS || newOrigin.Y + size.Y > map.Size.NbCaseY - Game1.FRONTIERS)
			{
				foreach (Map m in map.Neibhourgs)
				{
					Vector2 result = SizeIntersect(newOrigin.X + map.Size.Origin.X - m.Size.Origin.X, newOrigin.Y + map.Size.Origin.Y - m.Size.Origin.Y, size.X, size.Y, -Game1.FRONTIERS, -Game1.FRONTIERS, m.Size.NbCaseX + Game1.FRONTIERS, m.Size.NbCaseY + Game1.FRONTIERS);
					if (result.X > 0 && result.Y > 0) collideMap.Add(m);
				}
			}

			return collideMap;
		}

		// Test si l'objet ne rentre pas en collision avec un sol plus élevé
		public static Vector2 TryCollisionFloor(Component c, Vector2 velocity)
		{
			return TryCollisionFloor(c, velocity, c.Map);
		}
		public static Vector2 TryCollisionFloor(Component c, Vector2 velocity, Map map)
		{
			// Liste des cases surélevé que l'objet percute
			List<Vector2> caseIntersect = TryCase(c, velocity);
			// Test plusieurs fois l'algoritme pour être sure que l'objet ne percute plus aucun sol surélevé
			int i = 0;
			while (i < 2 && caseIntersect.Count > 0)
			{
				// Repositionnement de l'objet pour éviter des collision
				velocity = c.ActionCollide(caseIntersect, velocity);
				// Vérifie si la correction est suffisante
				caseIntersect = TryCase(c,velocity);
				i++;
			}
			// Si il y'a toujours collision, alors l'objet ne bouge pas
			if (i >= 2) velocity = Vector2.Zero;

			return velocity;
		}

		public static List<Vector2> TryCase(Component c, Vector2 velocity)
		{
			return TryCase(c, velocity, c.Map);
		}
		// Teste une à une toute les cases que l'objet touche pour vérifier si il n'entre pas en collision avec un sol surélevé
		public static List<Vector2> TryCase(Component c, Vector2 velocity, Map map)
		{
			// Recherche de toute les cases que l'objet touche
			int minX = (int)(Math.Floor(c.Origin.X + c.Hitbox.X * c.Size.X / 100 + velocity.X));
			int maxX = (int)(Math.Ceiling(c.Origin.X + (c.Hitbox.X + c.Hitbox.Width) * c.Size.X / 100 + velocity.X));
			int minY = (int)(Math.Floor(c.Origin.Y + c.Hitbox.Y * c.Size.Y / 100 + velocity.Y));
			int maxY = (int)(Math.Ceiling(c.Origin.Y + (c.Hitbox.Y + c.Hitbox.Height) * c.Size.Y / 100 + velocity.Y));

			List<Vector2> caseTouche = new List<Vector2>();

			// Pour toute les cases que l'objet touche
			for (int x = minX; x < maxX; ++x)
			{
				for (int y = minY; y < maxY; ++y)
				{
					// Pour tous les sols de la map
					for(int f = 0; f < map.Cartos.Cartos.Count; ++f)
					{
						// Si il y'a une case surélevé ici
						if (map.Cartos.Cartos[f].GetCase(x, y) > 0 && (map.Cartos.Cartos[f].Blocked || map.Cartos.Cartos[f].Height > c.Height))
						{
							// Ajout de la case dans la liste des cases ou il y a collision
							int i = 0;
							while (i < caseTouche.Count && (caseTouche[i].X != x || caseTouche[i].Y != y)) i++;
							if (i == caseTouche.Count) caseTouche.Add(new Vector2(x, y));
						}
					}
				}
			}
			return caseTouche;
		}
		// Correction du déplacement si il y a collision
		public Vector2 ActionCollide(List<Vector2> caseIntersect, Vector2 velocity)
		{
			// Collision avec une case (très complexe)
			if (caseIntersect.Count == 1)
			{
				velocity = CollideMonoCase(caseIntersect[0], velocity);
			}
			// Collision avec plusieurs case (très simple)
			else
			{
				velocity = CollideMultiCase(caseIntersect, velocity);

			}

			return velocity;
		}
		// Collision avec une case (très complexe)
		public Vector2 CollideMonoCase(Vector2 caseIntersect, Vector2 velocity)
		{
			// si l'objet ne se déplace que sur l'axe x
			if (velocity.X != 0 && velocity.Y == 0)
			{
				velocity.X = ReviseX(velocity.X);
			}
			// si l'objet ne se déplace que sur l'axe y
			else if (velocity.Y != 0 && velocity.X == 0)
			{
				velocity.Y = ReviseY(velocity.Y);
			}
			// sinon c'est la fète du slip...
			else
			{
				// Calcul de la taille du rectangle résultant de l'intersection entre la hitbox et la case (exprimé en case)

				Vector2 xy1 = Origin + new Vector2(hitbox.X, hitbox.Y) * Size / 100 + velocity;
				Vector2 wh1 = new Vector2(hitbox.Width, hitbox.Height) * Size / 100;
				Vector2 xy2 = new Vector2(caseIntersect.X, caseIntersect.Y);
				Vector2 wh2 = new Vector2(1, 1);

				velocity = Revise(xy1, wh1, xy2, wh2, velocity);
			}
			return velocity;
		}
		// Collision avec plusieurs case (très simple)
		public Vector2 CollideMultiCase(List<Vector2> caseIntersect, Vector2 velocity)
		{
			// Recherche le nombre de x et de y différent parmis les cases que touchent l'objet
			Vector2 v = new Vector2(velocity.X, velocity.Y);
			List<float> X = new List<float>();
			List<float> Y = new List<float>();
			foreach (Vector2 c in caseIntersect)
			{
				int i = 0;
				while (i < X.Count && c.X != X[i]) ++i;
				if (i == X.Count) X.Add(c.X);

				i = 0;
				while (i < Y.Count && c.Y != Y[i]) ++i;
				if (i == Y.Count) Y.Add(c.Y);

			}
			// Si il y'a plusieurs y différent, on corrige le déplacement de l'objet sur l'axe x
			if (Y.Count > 1) v.X = ReviseX(velocity.X);
			// Si il y'a plusieurs x différent, on corrige le déplacement de l'objet sur l'axe y
			if (X.Count > 1) v.Y = ReviseY(velocity.Y);
			return v;
		}

		// Regarde parmis les objets solides si l'objet n'est pas en collision avec un autre après un déplacement
		public Vector2 TryComponent(Vector2 velocity)
		{
			Vector2 xy1 = Origin + new Vector2(hitbox.X,hitbox.Y) * Size / 100 + velocity;
			Vector2 wh1 = new Vector2(hitbox.Width, hitbox.Height) * Size / 100;

			foreach (Decor d in map.Decors.Decors)
			{
				if (d is Component && d != this && d.Solid /* && d.Height == height*/)
				{
					Component c = (Component)d;
					Vector2 xy2 = c.Origin + new Vector2(c.hitbox.X, c.hitbox.Y) * c.Size / 100;
					Vector2 wh2 = new Vector2(c.hitbox.Width, c.hitbox.Height) * c.Size / 100;

					velocity = Revise(xy1,wh1,xy2,wh2,velocity);
				}
			}
			return velocity;
		}

		// Corrige la vélocité d'un objet 1 en réponse à sa collision avec un objet 2
		public Vector2 Revise(Vector2 xy1, Vector2 wh1, Vector2 xy2, Vector2 wh2, Vector2 velocity)
		{
			Vector2 sizeIntersect = SizeIntersect(xy1.X, xy1.Y, wh1.X, wh1.Y, xy2.X, xy2.Y, wh2.X, wh2.Y);

			if (sizeIntersect.X > 0 && sizeIntersect.Y > 0)
			{
				if (velocity.X != 0 && velocity.Y == 0) velocity.X = ReviseX(velocity.X);
				else if (velocity.Y != 0 && velocity.X == 0) velocity.Y = ReviseY(velocity.Y);
				else
				{
					if (sizeIntersect.Y <= Math.Abs(velocity.Y) && sizeIntersect.X > Math.Abs(velocity.X)) velocity.Y = ReviseY(velocity.Y);
					else if (sizeIntersect.X <= Math.Abs(velocity.X) && sizeIntersect.Y > Math.Abs(velocity.Y)) velocity.X = ReviseY(velocity.X);
					else
					{
						if (xy1.X - velocity.X + wh1.X <= xy2.X || xy1.X - velocity.X >= xy2.X + wh2.X) velocity.X = ReviseX(velocity.X);
						else if (xy1.Y - velocity.Y + wh1.Y <= xy2.Y || xy1.Y - velocity.Y >= xy2.Y + wh2.Y) velocity.Y = ReviseY(velocity.Y);
						else velocity = Vector2.Zero;
					}
				}

			}
			return velocity;
		}

		// Cacul les dimensions du rectangle d'intersection
		public Vector2 SizeIntersect(float x1, float y1, float w1, float h1, float x2, float y2, float w2, float h2)
		{
			Vector2 sizeIntersect = Vector2.Zero;
			if (x1 <= x2 && x1 + w1 >= x2 + w2) sizeIntersect.X = w2;
			else if (x1 < x2 + w2 && x1 + w1 >= x2 + w2) sizeIntersect.X = x2 + w2 - x1;
			else if (x1 + w1 > x2 && x1 + w1 <= x2 + w2) sizeIntersect.X = x1 + w1 - x2;
			else if (x2 <= x1 && x2 + w2 >= x1 + w1) sizeIntersect.X = w1;
			else sizeIntersect.X = 0;

			if (y1 <= y2 && y1 + h1 >= y2 + h2) sizeIntersect.Y = h2;
			else if (y1 < y2 + h2 && y1 + h1 >= y2 + h2) sizeIntersect.Y = y2 + h2 - y1;
			else if (y1 + h1 > y2 && y1 + h1 <= y2 + h2) sizeIntersect.Y = y1 + h1 - y2;
			else if (y2 <= y1 && y2 + h2 >= y1 + h1) sizeIntersect.Y = h1;
			else sizeIntersect.Y = 0;

			return sizeIntersect;
		}

		// Correction du déplacement sur l'axe x
		public float ReviseX(float x)
		{
			return 0;
		}
		// Correction du déplacement sur l'axe y
		public float ReviseY(float y)
		{
			return 0;
		}

	}
}
