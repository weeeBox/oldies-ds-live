using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using DuckstazyLive.app;
using Framework.visual;

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
            t += dt * 6.28f * (power - 0.5f);
            if (t > 6.28f)
                t -= 6.28f;
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

            SpriteTexture circleTex = utils.getTexture(Res.IMG_EFFECT_CIRCLE);
            float cw = circleTex.Width;
            float ch = circleTex.Height;
            float cw2 = 0.5f * cw;
            float ch2 = 0.5f * ch;

            float availWidth = Constants.ENV_WIDTH - cw2;
            float availHeight = Constants.ENV_HEIGHT - ch2;

            int circlesHor = (int)((availWidth - 2 * cw) / cw);
            int circlesVer = (int)(availHeight / ch);

            float offsetX = 0.5f * (availWidth - circlesHor * cw);
            float offsetY = 0.5f * (availHeight - circlesVer * ch);

            DrawMatrix m = DrawMatrix.Instance;
            m.tx = -0.5f * circleTex.Width;
            m.ty = -0.5f * circleTex.Height;

            x = offsetX + cw2;
            y = offsetY + ch2;
            // r = 22.5f + 12.5f * Math.Sin(t);
            s = (float)(0.642857 + 0.3571428 * Math.Sin(t));
            m.scale(s, s);
            for (int i = 0; i < circlesHor; ++i)
            {
                for (int j = 0; j < circlesVer; ++j)
                {
                    //gr.beginFill(c1);
                    //gr.drawCircle(x, y, r);
                    //gr.endFill();
                    m.translate(x, y);
                    canvas.draw(Res.IMG_EFFECT_CIRCLE, m, colorTransform);
                    y += ch;
                }
                y = offsetY + ch2;
                x += cw;
            }

            x = offsetX + cw;
            y = offsetY + ch;
            // r = 22.5 - 12.5 * Math.Sin(t);
            s = (float)(0.642857 - 0.3571428 * Math.Sin(t));
            m.scale(s, s);
            for (int i = 0; i < circlesHor; ++i)
            {
                for (int j = 0; j < circlesVer; ++j)
                {
                    //gr.beginFill(c1);
                    //gr.drawCircle(x, y, r);
                    //gr.endFill();
                    m.translate(x, y);
                    canvas.draw(Res.IMG_EFFECT_CIRCLE, m, colorTransform);
                    y += ch;
                }
                y = offsetY + ch;
                x += cw;
            }

            //canvas.draw(shape);            
        }
    }

}
