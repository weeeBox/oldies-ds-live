using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuckstazyLive.game.env
{
    public class EnvEffect3 : EnvEffect
	{
		private float t;
		// private Shape shape;
		
		public EnvEffect3()
		{			
			// shape = new Shape();
			t = 0.0f;
		}
		
		public override void update(float dt)
		{
			t+=dt*200*(power-0.5f);
			if(t>100)
				t-=100;
		}

		public override void draw(Canvas canvas)
		{
            //// Временные переменные.
            //float x;
            //bool c = false;
            //Graphics gr = shape.graphics;
			
            //gr.clear();
            //gr.lineStyle();
            //gr.beginFill(c1);
            //gr.drawRect(0.0f, 0.0f, 640.0, 400.0);
            //gr.endFill();
			
            //x = 512.0-t;
            //while(x>0.0f)
            //{
            //    gr.beginFill(c2);
            //    gr.drawCircle(320.0, 200.0, x);
            //    if(x>50.0) gr.drawCircle(320.0, 200.0, x-50.0);
            //    gr.endFill();
				
            //    x-=100.0;
            //}
			
            //canvas.draw(shape);            
		}
		
	}

}
