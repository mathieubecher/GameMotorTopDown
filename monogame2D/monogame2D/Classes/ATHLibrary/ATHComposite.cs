using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace monogame2D.ATHLibrary
{
	public class ATHComposite :ATH
	{
		protected List<ATH> children;
		public ATHComposite(Game1 game, Vector2 position, Vector2 size, Anchor anchor = Anchor.topleft, int order = 0) : base(game, position, size,anchor,order)
		{
			children = new List<ATH>();
		}
		public ATHComposite(ATH parent, Vector2 position, Vector2 size, Anchor anchor = Anchor.topleft, int order = 0) : base(parent, position, size, anchor, order)
		{
			children = new List<ATH>();
		}
		public void Add(ATH ath)
		{
			this.children.Add(ath);
		}
		public void Remove(ATH ath)
		{
			this.children.Remove(ath);
		}
		public override void Remove()
		{
			foreach (ATH ath in children) ath.Remove();
			base.Remove();
		}
	}
}
