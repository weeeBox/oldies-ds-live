using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckstazyLive.app;

namespace DuckstazyLive.game.env
{
    	public class EnvEffect1 : EnvEffect
	{
		private float t;
		// private Shape shape;

		public EnvEffect1()
		{
			// super();
			
			// shape = new Shape();
			t = 0.0f;
		}
		
		public override void update(float dt)
		{
			t+=dt*160*(power-0.5f);
			if(t>80)
				t-=80;
		}

		public override void draw(Canvas canvas)
		{
            base.draw(canvas);

            // Временные переменные.            
            bool c = false;
            //Graphics gr = shape.graphics;

            //gr.clear();
            //gr.lineStyle();
            //gr.beginFill(c1);
            //gr.drawRect(0.0, 0.0, 640.0, 400.0);
            //gr.endFill();

            ColorTransform trans = new ColorTransform(c1);
            trans.alphaMultiplier = 0.7f;
            DrawMatrix m = new DrawMatrix(true);

            float x = Constants.SAFE_OFFSET_X_UNSCALE;
            float y = -160.0f + t;
            while (y < 400.0f)
            {
                //gr.beginFill(c2);
                //gr.moveTo(0.0, x);
                //gr.lineTo(640.0, x + 40.0);
                //gr.lineTo(640.0, x + 80.0);
                //gr.lineTo(0.0, x + 40.0);
                //gr.endFill();

                m.translate(x, y);
                canvas.draw(Res.IMG_EFFECT_LINE, m, trans);

                y += 80.0f;
            }
			
            //canvas.draw(shape);            
		}
	}

}
