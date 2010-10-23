using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuckstazyLive.game
{
	public class EnvCloud
	{
		public float x;
		public float y;
		public float counter;
		public int id;
		//public var color:ColorTransform;
		
		public EnvCloud()
		{
			//color = new ColorTransform();
		}
		
		public void init(float _x)
		{
			x = _x;
			y = utils.rnd_float(40, 90);
			id = utils.rnd_(int)(0, 2);
			counter = Math.random();
		}
		
		public void update(float dt, float power)
		{
			x -= (0.75 + 0.25*Math.Sin(counter*6.2832))*(30.0+power*200.0)*dt;
			if(x<=-50.0)
			{
				x += 740;
				y = 40.0 + Math.random()*90.0;
				id = (int)(Math.random()*3.0);
			}
			counter += (0.1 + 0.9*power)*dt;
			if(counter>=1.0)
				counter -= (int)(counter);
			
			//color.alphaMultiplier = (0.5-power)*2.0;
		}
	};

}
