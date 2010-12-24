using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuckstazyLive.game.levels.generator
{
	public class Placer
	{
		public float x;
		public float y;
		
		protected Setuper setuper;
		public Placer(Setuper setuper, float x, float y)
		{
			this.x = x;
			this.y = y;
			this.setuper = setuper;
		}
		
		public Pill place(Pill pill)
		{
			return start(pill);
		}
		
		public Pill placeAvoidHero(Pill pill, float distSqr)
		{
			Pills pills = Pills.instance;
			Pill p = null;
			
			if(distSqr<=0 || !pills.tooCloseHero(x, y, distSqr))
				p = start(pill);
			
			return p;
		}

        protected virtual Pill start(Pill pill)
        {
            return setuper.start(x, y, pill);
        }
	}

}
