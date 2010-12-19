using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuckstazyLive.game.levels.generator
{
    public class Setuper
	{        
        public UserCallback userCallback;	

		public virtual Pill start(float x, float y, Pill pill)
		{
			return null;
		}

	}

}
