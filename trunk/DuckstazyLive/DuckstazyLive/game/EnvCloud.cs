using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckstazyLive.app;

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
			id = utils.rnd_int(0, 2);
			counter = utils.rnd();
		}
		
		public void update(float dt, float power)
		{
			x -= (float)((0.75 + 0.25*Math.Sin(counter*6.2832))*(30.0f+power*200.0f)*dt);
			if(x<=-50.0f)
			{
				x += 740;
				y = 40.0f + utils.rnd()*90.0f;
				id = (int)(utils.rnd()*3.0f);
			}
			counter += (0.1f + 0.9f*power)*dt;
			if(counter>=1.0f)
				counter -= (int)(counter);
			
			//color.alphaMultiplier = (0.5-power)*2.0f;
		}
	};

}
