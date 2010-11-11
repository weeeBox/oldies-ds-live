using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using DuckstazyLive.app;

namespace DuckstazyLive.game.env
{
   	public class EnvEffect2 : EnvEffect
	{
		private float t;
		// private Shape shape;

        public EnvEffect2(float x, float y, float w, float h)
            : base(x, y, w, h)
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

		public override void draw(Canvas canvas)
		{
            base.draw(canvas);

            // Временные переменные.
            float x;
            float y;
            float s;
            //Graphics gr = shape.graphics;

            //gr.clear();
            //gr.lineStyle();
            //gr.beginFill(c2);
            //gr.drawRect(0.0, 0.0, 640.0, 400.0);
            //gr.endFill();

            ColorTransform colorTransform = new ColorTransform(c1);

            Texture2D circleTex = utils.getImage(Res.IMG_EFFECT_CIRCLE);
            DrawMatrix m = new DrawMatrix();
            m.tx = utils.unscale(-0.5f * circleTex.Width);
            m.ty = utils.unscale(-0.5f * circleTex.Height);

            x = 40.0f;
            y = 40.0f;
            // r = 22.5f + 12.5f * Math.Sin(t);
            s = (float)(0.642857 + 0.3571428 * Math.Sin(t));
            m.scale(s, s);
            while (x < 640.0f)
            {
                while (y < 400.0f)
                {
                    //gr.beginFill(c1);
                    //gr.drawCircle(x, y, r);
                    //gr.endFill();
                    m.translate(x, y);
                    canvas.draw(Res.IMG_EFFECT_CIRCLE, m, colorTransform);
                    y += 80.0f;
                }
                y = 40.0f;
                x += 80.0f;
            }

            x = 80.0f;
            y = 80.0f;
            // r = 22.5 - 12.5 * Math.Sin(t);
            s = (float)(0.642857 - 0.3571428 * Math.Sin(t));
            m.scale(s, s);
            while (x < 640.0f)
            {
                while (y < 400.0f)
                {
                    //gr.beginFill(c1);
                    //gr.drawCircle(x, y, r);
                    //gr.endFill();
                    m.translate(x, y);
                    canvas.draw(Res.IMG_EFFECT_CIRCLE, m, colorTransform);
                    y += 80.0f;
                }
                y = 80.0f;
                x += 80.0f;
            }
			
            //canvas.draw(shape);            
		}		
	}

}
