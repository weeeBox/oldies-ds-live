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
		
		public void init(float x)
		{
			this.x = x;
			this.y = utils.rnd_float(40, 90);
			id = utils.rnd_int(0, 2);
			counter = utils.rnd();
		}
		
		public void update(float dt, float power)
		{
			x -= (float)((0.75 + 0.25*Math.Sin(counter*6.2832))*(30.0f+power*200.0f)*dt);
			float border = 50.0f;
			if(x<=-border)
			{
				x += Constants.ENV_WIDTH_UNSCALE + 2 * border;
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
