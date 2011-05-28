using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckstazyLive.app;
using asap.util;
using DuckstazyLive.app.game.stage;
using asap.graphics;
using asap.visual;

namespace DuckstazyLive.game.levels.fx
{
    public class HintArrow : BaseElement
    {
        private GameTexture img;        
        private float t;        
        public float visibleCounter;
        public bool visible;

        public HintArrow(StageMedia stageMedia)
        {
            img = stageMedia.imgHintArrow;
            this.width = img.GetWidth();
            this.height = img.GetHeight();
            t = 0.0f;
            alignX = ALIGN_CENTER;
        }

        public void place(float x, float y, float rotation, uint color, bool visible)
        {
            t = 0.0f;
            this.x = x;
            this.y = y;
            this.rotation = rotation;
            this.visible = visible;
            ColorUtils.ctSetRGB(ref ctForm, color);

            
            if (visible) visibleCounter = 1.0f;
            else visibleCounter = 0.0f;            
        }

        public override void Draw(Graphics g)
        {
            if (visibleCounter > 0.0f)
            {
                float f = (float)(Math.Sin(t * 6.28));
                float r;
                float sy;
                float sx;                

                if (f < 0)
                {
                    r = 0.0f;
                    sy = 0.6f + (f + 1) * 0.4f;
                    sx = 1.0f - f * 0.25f;
                }
                else
                {
                    r = f * 15;
                    sy = sx = 1.0f;
                }
                                
                translateY = -94.5f - r;
                rotationCenterY = r;
                scaleX = sx;
                scaleY = sy;
                ctForm.MulA = visibleCounter;

                PreDraw(g);
                g.DrawImage(img, 0, 0);
                PostDraw(g);          
            }            
        }

        public override void Update(float dt)
        {
            t += dt;
            if (t > 1.0) t -= (int)(t);

            if (visible)
            {
                if (visibleCounter < 1.0)
                {
                    visibleCounter += dt;
                    if (visibleCounter > 1.0f) visibleCounter = 1.0f;
                }
            }
            else
            {
                if (visibleCounter > 0.0f)
                {
                    visibleCounter -= dt;
                    if (visibleCounter < 0.0f) visibleCounter = 0.0f;
                }
            }
        }        
    }
}
