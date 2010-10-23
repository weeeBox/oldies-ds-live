using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuckstazyLive.game
{
    public class Particles
    {
        // направл€ющие частицы
        public const int ACID = 0;
        // пузырьчатые частицы лет€т вверх
        public const int BUBBLE = 1;
        // пл€ма
        public const int WARNING = 2;
        public const int STAR = 3;
        public const int RING = 4;

        public const int poolSize = 100;

        //[Embed(source = "gfx/fx_acid.png")]
        //private Class rFXAcidImg;

        //[Embed(source = "gfx/fx_bub.png")]
        //private Class rFXBubbleImg;

        //[Embed(source = "gfx/fx_w.png")]
        //private Class rFXWarningImg;

        //[Embed(source = "gfx/fx_star.png")]
        //private Class rFXStarImg;

        //[Embed(source = "gfx/fx_star2.png")]
        //private Class rFXStar2Img;

        //[Embed(source = "gfx/fx_out.png")]
        //private Class rFXOutImg;

        //[Embed(source = "gfx/fx_in.png")]
        //private Class rFXInImg;

        // „астицы FX
        public Texture2D imgFXAcid;
        public Texture2D imgFXBubble;
        public Texture2D imgFXWarning;
        public Texture2D imgFXStar;
        public Texture2D imgFXStar2;
        public Texture2D imgFXOut;
        public Texture2D imgFXIn;


        private Array pool;
        private int parts;

        private const Matrix mat = new Matrix();

        public Particles()
        {
            int i;

            pool = new Array(poolSize);
            for (; i < poolSize; ++i)
                pool[i] = new Particle();

            //imgFXAcid = (new rFXAcidImg()).bitmapData;
            //imgFXBubble = (new rFXBubbleImg()).bitmapData;
            //imgFXWarning = (new rFXWarningImg()).bitmapData;
            //imgFXStar = (new rFXStarImg()).bitmapData;
            //imgFXStar2 = (new rFXStar2Img()).bitmapData;
            //imgFXOut = (new rFXOutImg()).bitmapData;
            //imgFXIn = (new rFXInImg()).bitmapData;

        }

        public void clear()
        {
            foreach (Particle p in pool)
                p.t = 0.0;

            parts = 0;
        }

        public void update(float dt)
        {
            int i = 0;
            int parts_process = parts;

            foreach (Particle p in pool)
            {
                if (i == parts_process)
                    break;

                if (p.t > 0.0)
                {
                    switch (p.type)
                    {
                        case STAR:
                            p.a += 1.5708 * dt;

                            p.vy += 200.0 * dt;
                            p.x += p.vx * dt;
                            p.y += p.vy * dt;

                            if (p.y + 3.0 > 400.0)
                            {
                                p.y = 397.0;
                                p.vy = -0.5 * p.vy;
                                p.vx = 0.7 * p.vx;
                            }

                            if (p.t < 0.5)
                            {
                                p.s = p.t * 2.0;
                                p.col.alphaMultiplier = p.alpha * p.s;
                            }


                            break;
                        case ACID:

                            p.vy += 200.0 * dt;
                            p.x += p.vx * dt;
                            p.y += p.vy * dt;

                            if (p.y + 3.0 > 400.0)
                            {
                                p.y = 397.0;
                                p.vy = -0.5 * p.vy;
                                p.vx = 0.7 * p.vx;
                            }

                            if (p.t < 0.5)
                            {
                                p.s = p.t * 2.0;
                                p.col.alphaMultiplier = p.alpha * p.s;
                            }

                            p.a = Math.atan2(p.vy, p.vx) - 1.57;

                            break;

                        case BUBBLE:
                            p.vy -= 100.0 * dt;
                            p.vx += 200.0 * Math.sin((p.p1 + p.t) * 6.2831) * dt;
                            p.x += p.vx * dt;
                            p.y += p.vy * dt;
                            if (p.x >= 644.0)
                                p.x -= 643.0;
                            if (p.y <= -4.0)
                                p.y += 647.0;

                            if (p.t < 1.0)
                                p.s = p.t;

                            break;
                        case WARNING:
                            p.a = 8.0 * 3.14 * (p.p1 - p.t);
                            p.s = Math.cos(3.14 * 0.5 * (1.0 - p.t / p.p1));
                            p.col.alphaMultiplier = p.alpha * p.s;

                            break;

                        case RING:
                            if (p.p2 > 0.0)
                                p.s = p.p2 * (1.0 - p.t / p.p1);
                            else
                                p.s = -p.p2 * p.t / p.p1;

                            p.col.alphaMultiplier = p.alpha * Math.sin(3.14 * p.t / p.p1);

                            break;
                    }

                    p.t -= dt;
                    if (p.t <= 0.0)
                        --parts;

                    ++i;
                }
            }
        }

        public void draw(bool canvas)
        {
            int i = 0;
            float a;
            float s;

            foreach (Particle p in pool)
            {
                if (i == parts)
                    break;

                if (p.t > 0.0)
                {
                    a = p.a;
                    s = p.s;

                    mat.identity();

                    mat.tx = p.px;
                    mat.ty = p.py;
                    if (a != 0.0)
                        mat.rotate(a);

                    mat.scale(s, s);
                    mat.translate(p.x, p.y);

                    canvas.draw(p.img, mat, p.col, null, null, true);
                    //p.draw(canvas);
                    ++i;
                }
            }
        }

        private void setCT(C color, int argb)
        {
            color.alphaMultiplier = 0.0039216 * ((argb >> 24) & 0xFF);
            color.redMultiplier = 0.0039216 * ((argb >> 16) & 0xFF);
            color.greenMultiplier = 0.0039216 * ((argb >> 8) & 0xFF);
            color.blueMultiplier = 0.0039216 * (argb & 0xFF);
        }

        private Particle findDead()
        {
            foreach (Particle p in pool)
            {
                if (p.t <= 0.0)
                    return p;
            }

            return null;
        }

        public void startAcid(float x, float y, int color)
        {
            int i = 5;
            float a = Math.random() * 6.28; ;
            float speed;
            Particle p;

            while (i > 0)
            {
                p = findDead();
                if (p != null)
                {
                    speed = 10.0 + Math.random() * 190.0;

                    p.t = 0.2 + Math.random() * 0.5;
                    p.vx = Math.cos(a);
                    p.vy = Math.sin(a);
                    p.x = 9.0 * p.vx + x;
                    p.y = 9.0 * p.vy + y;
                    /*if(p.y>=397)
                    {
                        p.y = 379;
                        p.vy = -Math.abs(p.vy);
                    }*/
                    p.vx *= speed;
                    p.vy *= speed;
                    p.type = ACID;
                    setCT(p.col, color);
                    p.alpha = p.col.alphaMultiplier;
                    p.px = -3.0;
                    p.py = -7.0;
                    p.s = 1.0;
                    p.a = Math.atan2(p.vy, p.vx) - 1.57;
                    p.img = imgFXAcid;

                    a += 1.0 + Math.random() * 0.5;
                    ++parts;
                }
                else break;
                --i;
            }
        }

        public void startStepBubble(float x, float y)
        {
            Particle p;

            p = findDead();
            if (p != null)
            {
                p.p1 = Math.random();
                p.p2 = 0.5 + Math.random();
                p.t = 0.5 + Math.random();
                p.vx = -10.0 + Math.random() * 20.0;
                p.vy = -Math.random() * 100.0;

                p.col.alphaMultiplier = 1.0;
                p.col.redMultiplier = 0.5;
                p.col.greenMultiplier = 0.2 + 0.1 * Math.random();
                p.col.blueMultiplier = 0.0;
                p.alpha = p.col.alphaMultiplier;

                p.x = x;
                p.y = y;
                p.px = -4.0;
                p.py = -4.0;
                p.s = 1.0;
                p.a = 0.0;
                p.img = imgFXBubble;
                p.type = BUBBLE;

                ++parts;
            }
        }

        public void startBubble(float x, float y, int color)
        {
            Particle p;

            p = findDead();
            if (p != null)
            {
                p.p1 = Math.random();
                p.p2 = 0.5 + Math.random();
                p.t = 0.5 + Math.random();
                p.vx = -10.0 + Math.random() * 20.0;
                p.vy = -Math.random() * 100.0;
                //p.col = utils.ARGB2ColorTransform(color);
                setCT(p.col, color);
                p.alpha = p.col.alphaMultiplier;
                p.x = x;
                p.y = y;
                p.px = -4.0;
                p.py = -4.0;
                p.s = 1.0;
                p.a = 0.0;
                p.img = imgFXBubble;
                p.type = BUBBLE;

                ++parts;
            }
        }

        public void startStarToxic(float x, float y, float vx, float vy, int id)
        {
            Particle p;

            uint c1;
            uint c2;

            p = findDead();
            if (p != null)
            {

                switch (id)
                {
                    case 0:
                        c1 = 0xff000000;
                        c2 = 0xffffffff;
                        break;
                    case 1:
                        c1 = 0xffffff00;
                        c2 = 0xffff7f00;
                        break;
                }

                p.t = 0.2 + Math.random() * 0.5;
                p.vx = vx;
                p.vy = vy;
                p.x = x;
                p.y = y;
                p.type = STAR;
                if (Math.random() >= 0.5)
                    setCT(p.col, c1);
                else
                    setCT(p.col, c2);
                p.alpha = p.col.alphaMultiplier;
                p.px = -7.0;
                p.py = -7.0;
                p.s = 1.0;
                p.a = Math.random() * 3.14;
                p.img = imgFXStar;

                ++parts;
            }
        }

        public void startStarPower(float x, float y, float vx, float vy, int id)
        {
            Particle p;
            uint c1;
            uint c2;

            switch (id)
            {
                case 0:
                    c1 = 0xffd5fdfd;
                    c2 = 0xff00c0ff;
                    break;
                case 1:
                    c1 = 0xffffff00;
                    c2 = 0xffff7f00;
                    break;
                case 2:
                    c1 = 0xfffea7f9;
                    c2 = 0xffff006c;
                    break;
            }

            p = findDead();
            if (p != null)
            {
                p.t = 0.2 + Math.random() * 0.5;
                p.vx = vx;
                p.vy = vy;
                p.x = x;
                p.y = y;
                p.type = STAR;

                setCT(p.col, utils.lerpColor(c1, c2, Math.random()));
                p.alpha = p.col.alphaMultiplier;

                p.px = -7.0;
                p.py = -7.0;
                p.s = 1.0;
                p.a = Math.random() * 3.14;
                p.img = imgFXStar;

                ++parts;
            }
        }

        public void startWarning(float x, float y, float warning, float r, float g, float b)
        {
            Particle p;

            p = findDead();
            if (p != null)
            {
                p.p1 = warning;
                //p.p2 = utils.rnd_float(0.5, 1.5);
                p.t = warning;
                //p.vx = 0.0;
                //p.vy = 0.0;
                p.col.alphaMultiplier = 1.0;
                p.col.redMultiplier = r;
                p.col.greenMultiplier = g;
                p.col.blueMultiplier = b;
                p.alpha = 1.0;

                p.x = x;
                p.y = y;
                p.px = -15.0;
                p.py = -13.0;
                p.s = 0.0;
                p.a = 0.0;
                p.img = imgFXWarning;
                p.type = WARNING;

                ++parts;
            }
        }

        public void startRing(float x, float y, float radius, float speed, float start, int color)
        {
            Particle p;

            p = findDead();
            if (p != null)
            {
                p.p1 = speed;
                p.p2 = radius;
                p.t = start;
                setCT(p.col, color);
                p.alpha = p.col.alphaMultiplier;
                p.x = x;
                p.y = y;
                p.px = -32.0;
                p.py = -32.0;
                p.s = 0.0;
                p.a = 0.0;
                if (radius > 0.0)
                    p.img = imgFXOut;
                else
                    p.img = imgFXIn;
                p.type = RING;

                ++parts;
            }
        }

        public void explStarsPower(float x, float y, int id)
        {
            int i = 10;
            float a = Math.random() * 6.28;
            float speed;
            Particle p;
            uint c1;
            uint c2;

            switch (id)
            {
                case 0:
                    c1 = 0xffd5fdfd;
                    c2 = 0xff00c0ff;
                    break;
                case 1:
                    c1 = 0xffffff00;
                    c2 = 0xffff7f00;
                    break;
                case 2:
                    c1 = 0xfffea7f9;
                    c2 = 0xffff006c;
                    break;
            }
            while (i > 0)
            {
                p = findDead();
                if (p != null)
                {
                    speed = 10.0 + Math.random() * 90.0;

                    p.t = 0.2 + Math.random() * 0.5;
                    p.vx = Math.cos(a) * speed;
                    p.vy = Math.sin(a) * speed;
                    p.x = x;
                    p.y = y;
                    p.type = STAR;

                    setCT(p.col, utils.lerpColor(c1, c2, Math.random()));
                    p.alpha = p.col.alphaMultiplier;

                    p.px = -7.0;
                    p.py = -7.0;
                    p.s = 1.0;
                    p.a = Math.random() * 3.14;
                    p.img = imgFXStar;

                    a += 1.0 + Math.random() * 0.5;
                    ++parts;
                }
                else break;
                --i;
            }
        }

        public void explStarsToxic(float x, float y, int id, bool damage)
        {
            int i = 10;
            float a = Math.random() * 6.28;
            float speed;
            Particle p;

            uint c1;
            uint c2;
            bool c;


            if (damage) i = 30;

            switch (id)
            {
                case 0:
                    c1 = 0xff000000;
                    c2 = 0xffffffff;
                    break;
                case 1:
                    c1 = 0xffffff00;
                    c2 = 0xffff7f00;
                    break;
            }

            while (i > 0)
            {
                p = findDead();
                if (p != null)
                {
                    speed = 10.0 + Math.random() * 90.0;
                    p.t = 0.2 + Math.random() * 0.5;
                    if (damage)
                    {
                        speed *= 3.0;
                        //p.t*=1.5;
                    }
                    p.vx = Math.cos(a) * speed;
                    p.vy = Math.sin(a) * speed;
                    p.x = x;
                    p.y = y;
                    p.type = STAR;
                    if (c)
                        setCT(p.col, c1);
                    else
                        setCT(p.col, c2);
                    p.alpha = p.col.alphaMultiplier;
                    p.px = -7.0;
                    p.py = -7.0;
                    p.s = 1.0;
                    p.a = Math.random() * 3.14;
                    p.img = imgFXStar;

                    c = !c;
                    a += 1.0 + Math.random() * 0.5; ;

                    ++parts;
                }
                else break;
                --i;
            }
        }

        public void explStarsSleep(float x, float y)
        {
            int i = 10;
            float a = Math.random() * 6.28;
            float speed;
            Particle p;

            uint c1 = 0xff2e0678;
            uint c2 = 0xffb066cf;
            bool c;

            while (i > 0)
            {
                p = findDead();
                if (p != null)
                {
                    speed = 10.0 + Math.random() * 90.0;


                    p.t = 0.2 + Math.random() * 0.5;
                    p.vx = Math.cos(a) * speed;
                    p.vy = Math.sin(a) * speed;
                    p.x = x;
                    p.y = y;
                    p.type = STAR;
                    if (c) setCT(p.col, c1);
                    else setCT(p.col, c2);
                    p.alpha = p.col.alphaMultiplier;
                    p.px = -7.0;
                    p.py = -7.0;
                    p.s = 1.0;
                    p.a = Math.random() * 3.14;
                    p.img = imgFXStar;

                    c = !c;
                    a += 1.0 + Math.random() * 0.5;

                    ++parts;
                }
                else break;
                --i;
            }
        }

        public void explHeal(float x, float y)
        {
            int i = 10;
            Particle p;
            //var col:ColorTransform = utils.ARGB2ColorTransform(0xffff0000);

            while (i > 0)
            {
                p = findDead();
                if (p != null)
                {
                    p.p1 = Math.random();
                    p.p2 = 0.5 + Math.random();
                    p.t = 0.5 + Math.random();
                    p.vx = -10.0 + Math.random() * 20.0;
                    p.vy = -Math.random() * 100.0;

                    setCT(p.col, 0xffff0000);
                    p.alpha = p.col.alphaMultiplier;

                    p.x = x + Math.random() * 54.0;
                    p.y = y + Math.random() * 40.0;
                    p.px = -4.0;
                    p.py = -4.0;
                    p.s = 1.0;
                    p.a = 0.0;
                    p.img = imgFXBubble;
                    p.type = BUBBLE;

                    ++parts;
                }
                else break;
                --i;
            }
        }

    }

}
