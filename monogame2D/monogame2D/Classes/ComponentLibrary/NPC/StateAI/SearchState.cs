
using Microsoft.Xna.Framework;
using monogame2D.ComponentLibrary;
using monogame2D.JoeStar;

namespace monogame2D.StateAI
{
	public class SearchState : StandardState
	{
		/*				CONSTRUCTEUR			*/
		public SearchState(AI ai) : base(ai)
		{
			this.ai = ai;
		}
		public override void NextCase()
		{
			velocityOrigin = Vector2.Zero;
		}
		public override void SearchTarget()
		{
		}
	}
}