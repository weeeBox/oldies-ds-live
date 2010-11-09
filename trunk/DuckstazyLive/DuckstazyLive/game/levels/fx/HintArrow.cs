using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckstazyLive.app;

namespace DuckstazyLive.game.levels.fx
{
	public class HintArrow
	{
		private int img;
		public float x;
		public float y;
		public float angle;
		public ColorTransform color;
		private float t;
		public bool visible;
		public float visibleCounter;
		
		public HintArrow(StageMedia stageMedia)
		{
			img = stageMedia.imgHintArrow;
			color = new ColorTransform();
			t = 0.0f;
		}
		
		public void place(float _x, float _y, float _angle, uint _color, bool _visible)
		{
			t = 0.0f;
			x = _x;
			y = _y;
			angle = _angle;
			utils.ctSetRGB(color, _color);
		
			visible = _visible;
			if(_visible) visibleCounter = 1.0f;
			else visibleCounter = 0.0f;
		}

		public void draw(Canvas canvas)
		{
            if (visibleCounter > 0.0f)
            {
			    float f = (float)(Math.Sin(t*6.28));
			    float r;
			    float sy;
			    float sx;
			    DrawMatrix mat;		
			
				if(f<0)
				{
					r = 0.0f;
					sy = 0.6f + (f+1)*0.4f;
					sx = 1.0f - f*0.25f;
				}
				else
				{
					r = f*15;
					sy = sx = 1.0f;
				}
				
				mat = new DrawMatrix();
                mat.tx = -28;
                mat.ty = -63 - r;
				mat.scale(sx, sy);
				mat.rotate(angle);
				mat.translate(x, y);
				
				color.alphaMultiplier = visibleCounter;
				
				canvas.draw(img, mat, color);
			}
		}
		
		public void update(float dt)
		{
			t+=dt;
			if(t>1.0) t-=(int)(t);
			
			if(visible)
			{
				if(visibleCounter<1.0)
				{
					visibleCounter+=dt;
					if(visibleCounter>1.0f) visibleCounter = 1.0f;
				}
			}
			else
			{
				if(visibleCounter>0.0f)
				{
					visibleCounter-=dt;
					if(visibleCounter<0.0f) visibleCounter = 0.0f;
				}
			}
		}
	}
}
