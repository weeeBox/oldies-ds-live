using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckstazyLive.app;

namespace DuckstazyLive.game
{
	public class EnvStar
	{
		public float x;
		public float y;
		public float vx;
		public float vy;
		public float t;
		public float a;
		public ColorTransform color;

        private const float MIN_X = -7.0f;
        private const float MIN_Y = -7.0f;
        private const float MAX_X = Constants.ENV_WIDTH_UNSCALE + 7.0f;
        private const float MAX_Y = Constants.ENV_HEIGHT_UNSCALE + 7.0f;
		
		public EnvStar()
		{
			color = new ColorTransform();
			x = Constants.ENV_WIDTH_UNSCALE*utils.rnd();
			y = Constants.ENV_HEIGHT_UNSCALE*utils.rnd();
			a = utils.rnd()*6.28f;
			vx = 0.5f * (float)(Constants.ENV_WIDTH_UNSCALE*Math.Cos(a));
			vy = 0.5f * (float)(Constants.ENV_HEIGHT_UNSCALE*Math.Sin(a));
			t = utils.rnd();
		}
		
		public void update(float dt, float power)
		{
			float delta = dt*power;

			x += vx*delta;
			y += vy*delta;

			if(x < MIN_X) x += MAX_X;
			else if(x>MAX_X) x-=MAX_X;

			if(y<-MIN_Y) y += MAX_Y;
			else if(y>MAX_Y) y-=MAX_Y;

			t += 5.0f*power*dt;
			if(t>=1.0f) t -= (int)t;
			
			delta = 1.0f-y/Constants.ENV_HEIGHT_UNSCALE;
			color.alphaMultiplier = (float)Math.Sqrt(delta);//*(0.5-power)*2.0f;
		}
	};

}
