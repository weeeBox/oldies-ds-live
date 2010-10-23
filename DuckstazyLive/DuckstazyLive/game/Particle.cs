using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuckstazyLive.game
{
	public class Particle
	{
		public float x;
		public float y;
		public float vx;
		public float vy;
		
		public float a;
		public float va;
		
		public float t;
		
		public int type;
		public float p1;
		public float p2;
		
		public Texture2D img;
		public float px;
		public float py;
		public float s;
		
		public float alpha;
		public ColorTransform col;
		

		public Particle()
		{
			t = 0.0;
			col = new ColorTransform();
		}

		/*public void draw(bool canvas)
		{
			Matrix mat = new Matrix(1.0, 0.0, 0.0, 1.0, px, py);
			
			if(a!=0.0)
				mat.rotate(a);
				
			mat.scale(s, s);
			mat.translate(x, y);

			canvas.draw(img, mat, col, null, null, true);
		}*/

	}

}
