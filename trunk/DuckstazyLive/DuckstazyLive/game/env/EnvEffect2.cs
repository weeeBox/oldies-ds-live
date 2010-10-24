using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuckstazyLive.game.env
{
    	public class EnvEffect2 : EnvEffect
	{
		private float t;
		// private Shape shape;
		
		public EnvEffect2()
		{			
			// shape = new Shape();
			t = 0.0f;
		}
		
		public override void update(float dt)
		{
			t+=dt*6.28f*(power-0.5f);
			if(t>6.28f)
				t-=6.28f;
		}

		public override void draw(bool canvas)
		{
            //// Временные переменные.
            //float x;
            //float y;
            //float r;
            //Graphics gr = shape.graphics;
			
            //gr.clear();
            //gr.lineStyle();
            //gr.beginFill(c2);
            //gr.drawRect(0.0, 0.0, 640.0, 400.0);
            //gr.endFill();
			
            //x = 40.0;
            //y = 40.0;
            //r = 22.5+12.5*Math.Sin(t);
            //while(x<640.0)
            //{
            //    while(y<400.0)
            //    {
            //        gr.beginFill(c1);
            //        gr.drawCircle(x, y, r);
            //        gr.endFill();
            //        y+=80.0;
            //    }
            //    y = 40.0;
            //    x+=80.0;
            //}
			
            //x = 80.0;
            //y = 80.0;
            //r = 22.5-12.5*Math.Sin(t);
            //while(x<640.0)
            //{
            //    while(y<400.0)
            //    {
            //        gr.beginFill(c1);
            //        gr.drawCircle(x, y, r);
            //        gr.endFill();
            //        y+=80.0;
            //    }
            //    y = 80.0;
            //    x+=80.0;
            //}
			
            //canvas.draw(shape);
            throw new NotImplementedException();
		}
		
		// Р‘Р›Р®Р 
		/*private Shape shape;
		
		public EnvEffect2()
		{
			super();
			
			shape = new Shape();
		}
		
		public override void draw(bool canvas)
		{
			// Р’СЂРµРјРµРЅРЅС‹Рµ РїРµСЂРµРјРµРЅРЅС‹Рµ.
			Graphics gr = shape.graphics;

			gr.clear();
			gr.beginFill(c1, 1.8*(1.0-power)+0.1);
			gr.drawRect(0.0, 0.0, 640.0, 400.0);
			gr.endFill();

			canvas.draw(shape);
		}*/
		
	}

}
