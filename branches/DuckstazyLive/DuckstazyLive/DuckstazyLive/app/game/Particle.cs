using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using asap.util;

namespace DuckstazyLive.app.game
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

        public int img;
        public float px;
        public float py;
        public float s;

        public float alpha;
        public ColorTransform col = ColorTransform.NONE;


        public Particle()
        {
            t = 0.0f;
        }

        /*public void draw(bool canvas)
        {
            Matrix mat = new Matrix(1.0f, 0.0f, 0.0f, 1.0f, px, py);
			
            if(a!=0.0f)
                mat.rotate(a);
				
            mat.scale(s, s);
            mat.translate(x, y);

            canvas.draw(img, mat, col, null, null, true);
        }*/

    }
}
