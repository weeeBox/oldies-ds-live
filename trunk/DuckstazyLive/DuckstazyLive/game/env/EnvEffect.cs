using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuckstazyLive.game.env
{
	public class EnvEffect
	{		
		public float power;
		public uint c1;
		public uint c2;
		public float peak;
		
		public EnvEffect()
		{
			power = 0.0f;
			c1 = 0x000000;
			c2 = 0x000000;
			peak = 0.0f;
		}
		
		public virtual void update(float dt)
		{
		}

		public virtual void draw(Canvas canvas)
		{
		}

	}
}
