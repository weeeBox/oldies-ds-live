using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
			x = 640.0*Math.random();
			y = 400.0*Math.random();
			a = Math.random()*6.28;
			vx = 400.0*Math.Cos(a);
			vy = 400.0*Math.Sin(a);
			t = Math.random();
		}
		
		public void update(float dt, float power)
		{
			float delta = dt*power;

			x += vx*delta;
			y += vy*delta;

			if(x<-7.0) x += 654.0;
			else if(x>647.0) x-=654.0;

			if(y<-7.0) y += 414.0;
			else if(y>407.0) y-=414.0;

			t += 5.0*power*dt;
			if(t>=1.0) t -= int(t);
			
			delta = 1.0-y/400.0;
			color.alphaMultiplier = Math.Sqrt(delta);//*(0.5-power)*2.0;
		}
	};

}
