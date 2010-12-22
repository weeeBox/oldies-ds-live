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
		
		public Setuper setuper;
		public Placer(Setuper _setuper, float _x, float _y)
		{
			x = _x;
			y = _y;
			setuper = _setuper;
		}
		
		public Pill place(Pill pill)
		{
			return setuper.start(x, y, pill);
		}
		
		public Pill placeAvoidHero(Pill pill, float distSqr)
		{
			Pills pills = Pills.instance;
			Pill p = null;
			
			if(distSqr<=0 || !pills.tooCloseHero(x, y, distSqr))
				p = setuper.start(x, y, pill);
			
			return p;
		}
	}

}
