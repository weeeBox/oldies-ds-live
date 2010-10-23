using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuckstazyLive.game.env
{
    	public class EnvEffect1 : EnvEffect
	{
		private float t;
		private Shape shape;
		
		public EnvEffect1()
		{
			super();
			
			shape = new Shape();
			t = 0.0;
		}
		
		public override void update(float dt)
		{
			t+=dt*160.0*(power-0.5);
			if(t>80.0)
				t-=80.0;
		}

		public override void draw(bool canvas)
		{
            // Временные переменные.
			float x;
			bool c = false;
			Graphics gr = shape.graphics;
			
			gr.clear();
			gr.lineStyle();
			gr.beginFill(c1);
			gr.drawRect(0.0, 0.0, 640.0, 400.0);
			gr.endFill();
			
			x = -160.0+t;
			while(x<400.0)
			{
				gr.beginFill(c2);
				gr.moveTo(0.0, x);
				gr.lineTo(640.0, x+40.0);
				gr.lineTo(640.0, x+80.0);
				gr.lineTo(0.0, x+40.0);
				gr.endFill();
				
				c = !c;
				x+=80.0;
			}
			
			canvas.draw(shape);
		}
	}

}
