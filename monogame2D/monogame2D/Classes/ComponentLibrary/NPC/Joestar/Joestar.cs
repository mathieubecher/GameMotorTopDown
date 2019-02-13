using Microsoft.Xna.Framework;
using monogame2D.MapLibrary;
using System;
using monogame2D.ComponentLibrary;
using System.Collections.Generic;

namespace monogame2D.JoeStar
{
	public class Joestar
	{
		private Component component;

		private Vector2 destination;
		private Map map;
		private int[,] treats;
		private List<Case> toTreat;
		public bool couldFind = true;
		public bool near = false;
		private float tenacity;

		public Joestar(Component component, Map map, float tenacity)
		{
			this.component = component;
			this.map = map;
			this.tenacity = tenacity;
		}

		public Vector2 NextCase(Component target, float heightDestination)
		{

			// Si le composant peut atteindre sa cible
			couldFind = true;
			// centre de la cible
			this.destination = target.Origin;
			// distance réel entre le centre du composant et le centre de la cible
			float originalDistance = (float)Math.Pow(target.Origin.X + target.Size.X / 2 - (component.Origin.X + component.Size.X / 2), 2) +
									 (float)Math.Pow(target.Origin.Y + target.Size.Y / 2 - (component.Origin.Y + component.Size.Y / 2), 2);
			
			// si le composant est trop loin de la cible pour avancer en ligne droite
			if (originalDistance > 4)
			{
				// case de dépard du parcours
				Case start = new Case(map, (int)Math.Floor(component.Origin.X), (int)Math.Floor(component.Origin.Y), destination);
				if (start.x < 0 || start.y < 0 || start.x > map.Size.NbCaseX || start.y > map.Size.NbCaseY)
				{
					Vector2 direction = new Vector2(0,0);
					if (start.x < 0) direction.X = 1;
					else if (start.x > map.Size.NbCaseX) direction.X = -1;
					if (start.y < 0) direction.Y = 1;
					else if (start.y > map.Size.NbCaseY) direction.Y = -1;
					return direction;
				}
				// le composant n'est pas proche de la cible
				near = false;
				// 
				destination = new Vector2((int)Math.Floor(destination.X), (int)Math.Floor(destination.Y));
				if (component.Height >= heightDestination && couldFind)
				{
					// Liste des cases déjà traités
					treats = new int[map.Size.NbCaseX, map.Size.NbCaseY];
					// Liste des cases à traiter
					toTreat = new List<Case>();
					// Case de départ
					toTreat.Add(start);
					// Tant qu'il reste des cases à traiter
					while (toTreat.Count > 0)
					{
						// Case qui sera traité
						Case actualPos = toTreat[0];
						// Cherche si un des voisins de la case est la case du centre de la cible
						for (int x = actualPos.x - 1; x <= actualPos.x + 1; x++)
							for (int y = actualPos.y - 1; y <= actualPos.y + 1; y++)
								if (x == destination.X && y == destination.Y)
								{
									// Continue de charger le composant même s'il est en dehors de la fenètre
									component.Important = true;
									// Envoi la première direction du parcours
									return actualPos.getFirstCase() - new Vector2((int)Math.Floor(component.Origin.X), (int)Math.Floor(component.Origin.Y));
								}

						// TRAITEMENT DES VOISIN
						bool left = TreatNeighbour(actualPos, actualPos.x - 1, actualPos.y);
						bool top = TreatNeighbour(actualPos, actualPos.x, actualPos.y - 1);
						bool right = TreatNeighbour(actualPos, actualPos.x + 1, actualPos.y);
						bool bottom = TreatNeighbour(actualPos, actualPos.x, actualPos.y + 1);

						if (left && top) TreatNeighbour(actualPos, actualPos.x - 1, actualPos.y - 1);
						if (right && top) TreatNeighbour(actualPos, actualPos.x + 1, actualPos.y - 1);
						if (left && bottom) TreatNeighbour(actualPos, actualPos.x - 1, actualPos.y + 1);
						if (right && bottom) TreatNeighbour(actualPos, actualPos.x + 1, actualPos.y + 1);


						// Suppression de la case traité dans toTreat et ajout dans treat
						toTreat.Remove(actualPos);
						toTreat.Sort(new DistanceCompare());
						treats[actualPos.x, actualPos.y] = 1;
					}
				}
				// Supprime le contenu des 2 tableaux
				toTreat = null;
				treats = null;
				
				// On indique que le composant n'a pas réussi à atteindre la cible
				component.Important = false;
				couldFind = false;

				// Renvoi un vecteur nul
				return Vector2.Zero;

			}
			// Si le composant est assez proche de sa cible, il va se diriger en ligne droite vers lui
			else
			{
				near = true;
				return destination - (component.Origin);
			}
		}

		private bool TreatNeighbour(Case actualPos, int x, int y)
		{
			// Indique si le composant peut atteindre cette case (util pour calculer ou non les diagonales)
			bool couldGo = false;
			// si les coordonnées sont dans la carte
			if (x >= 0 && x < map.Size.NbCaseX && y >= 0 && y < map.Size.NbCaseY)
			{
				// Création de la nouvelle case
				Case c = new Case(map, x, y, destination);

				// Si le composant ne rencontre pas de mur sur cette case et que la distance avec la cible sur cette case est inférieur à sa limite de tenacité
				if (Component.TryCase(component, new Vector2(x, y) - new Vector2((float)Math.Floor(component.Origin.X), (float)Math.Floor(component.Origin.Y))).Count == 0 && c.distance < tenacity * 10)
				{
					// si la case voisine n'est pas dans la list treats
					if (treats[x, y] == 0)
					{
						// si la case voisine n'est pas dans la liste toTreat
						int i = 0;
						while (i < toTreat.Count && !(toTreat[i].x == x && toTreat[i].y == y) && toTreat[i].distance <= c.distance) ++i;
						if (i == toTreat.Count || toTreat[i].distance > c.distance)
						{
							// on ajoute le parent à la case 
							c.AddParent(actualPos);
							// ajoute la case voisine dans la liste des cases à traiter puis tri le tableau
							toTreat.Add(c);
							toTreat.Sort(new DistanceCompare());
							// confirme que le composant peut atteindre cette case
							couldGo = true;

						}
					}
				}
				// si le composant ne peut pas atteindre la case voisine, on considère la case comme traité pour éviter de la calculer à nouveau
				else treats[x, y] = 1;
			}
			// indique si le composant peut bien atteindre la case voisine
			return couldGo;
		}
	}
}
