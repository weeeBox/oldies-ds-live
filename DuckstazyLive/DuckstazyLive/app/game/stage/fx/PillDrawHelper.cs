using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using asap.visual;
using asap.graphics;

namespace DuckstazyLive.app.game.stage.fx
{
    public class PillDrawHelper : BaseElement
    {
        private PillsMedia media;

        public PillDrawHelper(PillsMedia media, float width, float height) : base(width, height)
        {
            this.media = media;

            //imgSmile1 = createImage(media.imgSmile1, 9.9f, 19.1f);            
            //imgSmile2 = createImage(media.imgSmile2, 4.1f, 18.1f);            
            //imgSmile3 = createImage(media.imgSmile3, 5.9f, 18.0f);
            //imgEyes1 = createImage(media.imgEyes1, 8.25f, 10.35f);            
            //imgEyes2 = createImage(media.imgEyes2, 5.5f, 9.9f);            
        }        

        public override void Draw(Graphics g)
        {            
        }

        private void drawEmoDefault(Graphics g, float alpha, float hx, float hy)
        {
            g.DrawImage(media.imgSmile1, 9.9f, 19.1f, alpha);
            g.DrawImage(media.imgEyes1, 8.25f + hx, 10.35f + hy, alpha);            
        }

        public void drawEmoHappy(Graphics g, float alpha, float angle, float hx, float hy)
        {            
            rotation = angle;

            PreDraw(g);            

            if (alpha < 1.0f) drawEmoDefault(g, 1.0f - alpha, hx, hy);
            
            g.DrawImage(media.imgSmile3, 5.9f, 18.0f, alpha);
            g.DrawImage(media.imgEyes2, 5.5f, 9.9f, alpha);

            PostDraw(g);

            rotation = 0.0f;
        }

        public void drawEmoShake(Graphics g, float alpha, float off, float hx, float hy)
        {
            PreDraw(g);

            if (alpha < 1.0f) drawEmoDefault(g, 1.0f - alpha, hx, hy);            

            g.DrawImage(media.imgSmile3, 5.9f, 18.0f, alpha);
            g.DrawImage(media.imgEyes2, 5.5f, 9.9f + off, alpha);

            PostDraw(g);
        }

        public void drawEmoSmile(Graphics g, float alpha, float hx, float hy)
        {
            PreDraw(g);

            if (alpha < 1.0f) drawEmoDefault(g, 1.0f - alpha, hx, hy);         

            g.DrawImage(media.imgSmile2, 4.1f, 18.1f, alpha);
            g.DrawImage(media.imgEyes1, 8.25f, 10.35f, alpha);

            PostDraw(g);
        }

        public void drawNid(Graphics g, float hx, float hy)
        {
            PreDraw(g);

            g.DrawImage(media.imgSmile1, 9.9f, 19.1f);
            g.DrawImage(media.imgEyes1, 8.25f + hx, 10.35f + hy);            

            PostDraw(g);            
        }
    }
}
