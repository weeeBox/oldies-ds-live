using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuckstazyLive.game.env
{
 	public class EnvEffect4 : EnvEffect
	{
		private float t;
		// private Shape shape;
		
		public EnvEffect4()
		{			
			// shape = new Shape();
			t = 0.0f;
		}
		
		public override void update(float dt)
		{
			t+=dt*1.256f*(power-0.5f);
			if(t>6.28f)
				t-=6.28f;
		}

		public override void draw(bool canvas)
		{
            //// Временные переменные.
            //float c = 0.0f;
            //float a = t;
            //float a2 = t+0.314f;
            //Graphics gr = shape.graphics;
			
            //gr.clear();
            //gr.lineStyle();
            //gr.beginFill(c2);
            //gr.drawRect(0.0, 0.0, 640.0, 400.0);
            //gr.endFill();
			
            //while(c<6.28)
            //{
            //    gr.beginFill(c1);
            //    gr.moveTo(320.0 + 512.0*Math.Cos(a), 200.0 + 512.0*Math.Sin(a));
            //    gr.lineTo(320.0, 200.0);
            //    gr.lineTo(320.0 + 512.0*Math.Cos(a2), 200.0 + 512.0*Math.Sin(a2));
            //    gr.endFill();
				
            //    a+=0.628;
            //    a2+=0.628;
            //    c+=0.628;
            //}
			
            //gr.beginFill(c1);
            //gr.drawCircle(320.0, 200.0, peak*25.0);
            //gr.endFill();
			
            //canvas.draw(shape);
            throw new NotImplementedException();
		}
		
	}

}
