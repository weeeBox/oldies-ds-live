using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using asap.util;
using app;
using asap.graphics;
using asap.visual;

namespace DuckstazyLive.app.game
{
    public class Particles : DisplayObject
    {
        // направляющие частицы
        public const int ACID = 0;
        // пузырьчатые частицы летят вверх
        public const int BUBBLE = 1;
        // пляма
        public const int WARNING = 2;
        public const int STAR = 3;
        public const int RING = 4;

        public const int poolSize = 100;

        private const float FALL_GRAVITY = 300.0f;
        private const float FLOATUP_GRAVITY = -150.0f;

        // Частицы FX
        public int imgFXAcid;
        public int imgFXBubble;
        public int imgFXWarning;
        public int imgFXStar;
        public int imgFXOut;
        public int imgFXIn;

        private Particle[] pool;
        private int parts;

        private Image image;

        public Particles()
        {
            int i = 0;

            pool = new Particle[poolSize];
            for (; i < poolSize; ++i)
                pool[i] = new Particle();

            imgFXAcid = Res.IMG_FX_ACID;
            imgFXBubble = Res.IMG_FX_BUBBLE;
            imgFXWarning = Res.IMG_FX_WARNING;
            imgFXStar = Res.IMG_FX_STAR;

            imgFXOut = Res.IMG_FX_OUT;
            imgFXIn = Res.IMG_FX_IN;

            image = new Image();
            image.alignX = image.alignY = ALIGN_CENTER;
        }

        public void clear()
        {
            foreach (Particle p in pool)
                p.t = 0.0f;

            parts = 0;
        }

        public override void Update(float dt)
        {
            int i = 0;
            int parts_process = parts;

            foreach (Particle p in pool)
            {
                if (i == parts_process)
                    break;

                if (p.t > 0.0f)
                {
                    switch (p.type)
                    {
                        case STAR:
                        {
                            p.a += 1.5708f * dt;

                            p.vy += FALL_GRAVITY * dt;
                            p.x += p.vx * dt;
                            p.y += p.vy * dt;

                            float yMax = height - 0.5f * p.Height;
                            if (p.y > yMax)
                            {
                                p.y = yMax;
                                p.vy = -0.5f * p.vy;
                                p.vx = 0.7f * p.vx;
                            }

                            if (p.t < 0.5f)
                            {
                                p.s = p.t * 2.0f;
                                p.col.MulA = p.alpha * p.s;
                            }
                            break;
                        }
                        case ACID:
                        {
                            p.vy += FALL_GRAVITY * dt;
                            p.x += p.vx * dt;
                            p.y += p.vy * dt;

                            float yMax = height - 0.5f * p.Height;
                            if (p.y > yMax)
                            {
                                p.y = yMax;
                                p.vy = -0.5f * p.vy;
                                p.vx = 0.7f * p.vx;
                            }

                            if (p.t < 0.5f)
                            {
                                p.s = p.t * 2.0f;
                                p.col.MulA = p.alpha * p.s;
                            }

                            p.a = (float)(Math.Atan2(p.vy, p.vx) - 1.57f);

                            break;
                        }

                        case BUBBLE:
                        {                        
                            p.vy += FLOATUP_GRAVITY * dt;
                            p.vx += (float)(300.0f * Math.Sin((p.p1 + p.t) * 6.2831f) * dt);
                            p.x += p.vx * dt;
                            p.y += p.vy * dt;                            
                            float xMax = width + 0.5f * p.Width;
                            if (p.x >= xMax)
                                p.x -= xMax;
                            if (p.x <= -0.5f * p.Width)
                                p.x += xMax;

                            if (p.t < 1.0f)
                                p.s = p.t;

                            break;
                        }
                        case WARNING:
                            p.a = 8.0f * 3.14f * (p.p1 - p.t);
                            p.s = (float)(Math.Cos(3.14f * 0.5f * (1.0f - p.t / p.p1)));
                            p.col.MulA = p.alpha * p.s;

                            break;

                        case RING:
                            if (p.p2 > 0.0f)
                                p.s = p.p2 * (1.0f - p.t / p.p1);
                            else
                                p.s = -p.p2 * p.t / p.p1;

                            p.col.MulA = (float)(p.alpha * Math.Sin(3.14 * p.t / p.p1));

                            break;
                    }

                    p.t -= dt;
                    if (p.t <= 0.0f)
                        --parts;

                    ++i;
                }
            }
        }

        public override void Draw(Graphics g)
        {
            PreDraw(g);

            int i = 0;

            foreach (Particle p in pool)
            {
                if (i == parts)
                    break;

                if (p.t > 0.0f)
                {
                    image.SetTexture(p.img);
                    image.rotation = p.a;
                    image.scaleX = image.scaleY = p.s;
                    image.x = p.x;
                    image.y = p.y;
                    image.ctForm = p.col;
                    image.Draw(g);

                    //mat.tx = p.px;
                    //mat.ty = p.py;                    
                }
            }

            PostDraw(g);
        }

        private void setCT(ref ColorTransform color, uint argb)
        {
            color.MulA = 0.0039216f * ((argb >> 24) & 0xFF);
            color.MulR = 0.0039216f * ((argb >> 16) & 0xFF);
            color.MulG = 0.0039216f * ((argb >> 8) & 0xFF);
            color.MulB = 0.0039216f * (argb & 0xFF);
        }

        private Particle findDead()
        {
            foreach (Particle p in pool)
            {
                if (p.t <= 0.0f)
                    return p;
            }

            return null;
        }

        public void startAcid(float x, float y)
        {
            startAcid(x, y, 0xff000000);
        }

        public void startAcid(float x, float y, uint color)
        {
            int i = 5;
            float a = RandomHelper.rnd() * 6.28f;
            float speed;
            Particle p;

            while (i > 0)
            {
                p = findDead();
                if (p != null)
                {
                    speed = 15.0f + RandomHelper.rnd() * 285;

                    p.t = 0.2f + RandomHelper.rnd() * 0.5f;
                    p.vx = (float)Math.Cos(a);
                    p.vy = (float)Math.Sin(a);
                    p.x = 13.5f * p.vx + x;
                    p.y = 13.5f * p.vy + y;
                    /*if(p.y>=397)
                    {
                        p.y = 379;
                        p.vy = -Math.abs(p.vy);
                    }*/
                    p.vx *= speed;
                    p.vy *= speed;
                    p.type = ACID;
                    setCT(ref p.col, color);
                    p.alpha = p.col.MulA;
                    p.px = -3.0f;
                    p.py = -7.0f;
                    p.s = 1.0f;
                    p.a = (float)(Math.Atan2(p.vy, p.vx) - 1.57f);
                    p.img = GetTexture(imgFXAcid);

                    a += 1.0f + RandomHelper.rnd() * 0.5f;
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
                p.p1 = RandomHelper.rnd();
                p.p2 = 0.5f + RandomHelper.rnd();
                p.t = 0.5f + RandomHelper.rnd();
                p.vx = -15.0f + RandomHelper.rnd() * 30.0f;
                p.vy = -RandomHelper.rnd() * 150;

                p.col.MulA = 1.0f;
                p.col.MulR = 0.5f;
                p.col.MulG = 0.2f + 0.1f * RandomHelper.rnd();
                p.col.MulB = 0.0f;
                p.alpha = p.col.MulA;

                p.x = x;
                p.y = y;
                p.px = -4.0f;
                p.py = -4.0f;
                p.s = 1.0f;
                p.a = 0.0f;
                p.img = GetTexture(imgFXBubble);
                p.type = BUBBLE;

                ++parts;
            }
        }

        public void startBubble(float x, float y, uint color)
        {
            Particle p;

            p = findDead();
            if (p != null)
            {
                p.p1 = RandomHelper.rnd();
                p.p2 = 0.5f + RandomHelper.rnd();
                p.t = 0.5f + RandomHelper.rnd();
                p.vx = -15.0f + RandomHelper.rnd() * 30.0f;
                p.vy = -RandomHelper.rnd() * 150.0f;
                //p.col = utils.ARGB2ColorTransform(color);
                setCT(ref p.col, color);
                p.alpha = p.col.MulA;
                p.x = x;
                p.y = y;
                p.px = -4.0f;
                p.py = -4.0f;
                p.s = 1.0f;
                p.a = 0.0f;
                p.img = GetTexture(imgFXBubble);
                p.type = BUBBLE;

                ++parts;
            }
        }

        public void startCrapBubble(float x, float y, float vx, float vy)
        {
            Particle p;

            p = findDead();
            if (p != null)
            {
                p.p1 = RandomHelper.rnd();
                p.p2 = 0.5f + RandomHelper.rnd();
                p.t = 0.5f + RandomHelper.rnd();
                p.vx = vx;
                p.vy = vy;
                //p.col = utils.ARGB2ColorTransform(color);
                setCT(ref p.col, 0x406618);
                p.alpha = p.col.MulA;
                p.x = x;
                p.y = y;
                p.px = -4.0f;
                p.py = -4.0f;
                p.s = 1.0f;
                p.a = 0.0f;
                p.img = GetTexture(imgFXBubble);
                p.type = ACID;

                ++parts;
            }
        }

        public void startStarToxic(float x, float y, float vx, float vy, int id)
        {
            Particle p;

            uint c1 = 0;
            uint c2 = 0;

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

                p.t = 0.2f + RandomHelper.rnd() * 0.5f;
                p.vx = vx;
                p.vy = vy;
                p.x = x;
                p.y = y;
                p.type = STAR;
                if (RandomHelper.rnd() >= 0.5f)
                    setCT(ref p.col, c1);
                else
                    setCT(ref p.col, c2);
                p.alpha = p.col.MulA;
                p.px = -7.0f;
                p.py = -7.0f;
                p.s = 1.0f;
                p.a = RandomHelper.rnd() * 3.14f;
                p.img = GetTexture(imgFXStar);

                ++parts;
            }
        }

        public void startStarPower(float x, float y, float vx, float vy, int id)
        {
            Particle p;
            uint c1 = 0;
            uint c2 = 0;

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
                p.t = 0.2f + RandomHelper.rnd() * 0.5f;
                p.vx = vx;
                p.vy = vy;
                p.x = x;
                p.y = y;
                p.type = STAR;

                setCT(ref p.col, ColorUtils.lerpColor(c1, c2, RandomHelper.rnd()));
                p.alpha = p.col.MulA;

                p.px = -7.0f;
                p.py = -7.0f;
                p.s = 1.0f;
                p.a = RandomHelper.rnd() * 3.14f;
                p.img = GetTexture(imgFXStar);

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
                //p.p2 = RandomHelper.rnd_float(0.5f, 1.5);
                p.t = warning;
                //p.vx = 0.0f;
                //p.vy = 0.0f;
                p.col.MulA = 1.0f;
                p.col.MulR = r;
                p.col.MulG = g;
                p.col.MulB = b;
                p.alpha = 1.0f;

                p.x = x;
                p.y = y;
                p.px = -15.0f;
                p.py = -13.0f;
                p.s = 0.0f;
                p.a = 0.0f;
                p.img = GetTexture(imgFXWarning);
                p.type = WARNING;

                ++parts;
            }
        }

        public void startRing(float x, float y, float radius, float speed, float start, uint color)
        {
            Particle p;

            p = findDead();
            if (p != null)
            {
                p.p1 = speed;
                p.p2 = radius;
                p.t = start;
                setCT(ref p.col, color);
                p.alpha = p.col.MulA;
                p.x = x;
                p.y = y;
                p.px = -32.0f;
                p.py = -32.0f;
                p.s = 0.0f;
                p.a = 0.0f;
                if (radius > 0.0f)
                    p.img = GetTexture(imgFXOut);
                else
                    p.img = GetTexture(imgFXIn);
                p.type = RING;

                ++parts;
            }
        }

        public void explStarsPower(float x, float y, int id)
        {
            int i = 10;
            float a = RandomHelper.rnd() * 6.28f;
            float speed;
            Particle p;
            uint c1 = 0;
            uint c2 = 0;

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
                    speed = 10.0f + RandomHelper.rnd() * 90.0f;

                    p.t = 0.2f + RandomHelper.rnd() * 0.5f;
                    p.vx = (float)Math.Cos(a) * speed;
                    p.vy = (float)Math.Sin(a) * speed;
                    p.x = x;
                    p.y = y;
                    p.type = STAR;

                    setCT(ref p.col, ColorUtils.lerpColor(c1, c2, RandomHelper.rnd()));
                    p.alpha = p.col.MulA;

                    p.px = -7.0f;
                    p.py = -7.0f;
                    p.s = 1.0f;
                    p.a = RandomHelper.rnd() * 3.14f;
                    p.img = GetTexture(imgFXStar);

                    a += 1.0f + RandomHelper.rnd() * 0.5f;
                    ++parts;
                }
                else break;
                --i;
            }
        }

        public void explStarsToxic(float x, float y, int id, bool damage)
        {
            int i = 10;
            float a = RandomHelper.rnd() * 6.28f;
            float speed;
            Particle p;

            uint c1 = 0;
            uint c2 = 0;
            bool c = false;


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
                    speed = 10.0f + RandomHelper.rnd() * 90.0f;
                    p.t = 0.2f + RandomHelper.rnd() * 0.5f;
                    if (damage)
                    {
                        speed *= 3.0f;
                        //p.t*=1.5;
                    }
                    p.vx = (float)Math.Cos(a) * speed;
                    p.vy = (float)Math.Sin(a) * speed;
                    p.x = x;
                    p.y = y;
                    p.type = STAR;
                    if (c)
                        setCT(ref p.col, c1);
                    else
                        setCT(ref p.col, c2);
                    p.alpha = p.col.MulA;
                    p.px = -7.0f;
                    p.py = -7.0f;
                    p.s = 1.0f;
                    p.a = RandomHelper.rnd() * 3.14f;
                    p.img = GetTexture(imgFXStar);

                    c = !c;
                    a += 1.0f + RandomHelper.rnd() * 0.5f; ;

                    ++parts;
                }
                else break;
                --i;
            }
        }

        public void explStarsSleep(float x, float y)
        {
            int i = 10;
            float a = RandomHelper.rnd() * 6.28f;
            float speed;
            Particle p;

            uint c1 = 0xff2e0678;
            uint c2 = 0xffb066cf;
            bool c = false;

            while (i > 0)
            {
                p = findDead();
                if (p != null)
                {
                    speed = 10.0f + RandomHelper.rnd() * 90.0f;


                    p.t = 0.2f + RandomHelper.rnd() * 0.5f;
                    p.vx = (float)Math.Cos(a) * speed;
                    p.vy = (float)Math.Sin(a) * speed;
                    p.x = x;
                    p.y = y;
                    p.type = STAR;
                    if (c) setCT(ref p.col, c1);
                    else setCT(ref p.col, c2);
                    p.alpha = p.col.MulA;
                    p.px = -7.0f;
                    p.py = -7.0f;
                    p.s = 1.0f;
                    p.a = RandomHelper.rnd() * 3.14f;
                    p.img = GetTexture(imgFXStar);

                    c = !c;
                    a += 1.0f + RandomHelper.rnd() * 0.5f;

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
                    p.p1 = RandomHelper.rnd();
                    p.p2 = 0.5f + RandomHelper.rnd();
                    p.t = 0.5f + RandomHelper.rnd();
                    p.vx = -10.0f + RandomHelper.rnd() * 20.0f;
                    p.vy = -RandomHelper.rnd() * 100.0f;

                    setCT(ref p.col, 0xffff0000);
                    p.alpha = p.col.MulA;

                    p.x = x + RandomHelper.rnd() * 54.0f;
                    p.y = y + RandomHelper.rnd() * 40.0f;
                    p.px = -4.0f;
                    p.py = -4.0f;
                    p.s = 1.0f;
                    p.a = 0.0f;
                    p.img = GetTexture(imgFXBubble);
                    p.type = BUBBLE;

                    ++parts;
                }
                else break;
                --i;
            }
        }

        private GameTexture GetTexture(int id)
        {
            return Application.sharedResourceMgr.GetTexture(id);
        }

    }
}
