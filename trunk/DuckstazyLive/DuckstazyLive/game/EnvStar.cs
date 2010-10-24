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
		
		public EnvStar()
		{
			color = new ColorTransform();
			x = 640.0f*utils.rnd();
			y = 400.0f*utils.rnd();
			a = utils.rnd()*6.28f;
			vx = (float)(400.0f*Math.Cos(a));
			vy = (float)(400.0f*Math.Sin(a));
			t = utils.rnd();
		}
		
		public void update(float dt, float power)
		{
			float delta = dt*power;

			x += vx*delta;
			y += vy*delta;

			if(x<-7.0f) x += 654.0f;
			else if(x>647.0f) x-=654.0f;

			if(y<-7.0f) y += 414.0f;
			else if(y>407.0f) y-=414.0f;

			t += 5.0f*power*dt;
			if(t>=1.0f) t -= (int)t;
			
			delta = 1.0f-y/400.0f;
			color.alphaMultiplier = (float)Math.Sqrt(delta);//*(0.5-power)*2.0f;
		}
	};

}
