using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckstazyLive.app;
using Microsoft.Xna.Framework.Graphics;
using Framework.core;

namespace DuckstazyLive.game.env
{
    public class EnvEffect1 : EnvEffect
    {
        private float t;
        private int lineId;
        
        public EnvEffect1()
        {        
            t = 0.0f;
            lineId = Res.IMG_EFFECT_LINE;
        }

        public override void update(float dt)
        {
            int lineHeight = getLineHeight();
            t += 2 * dt * lineHeight * (power - 0.5f);
            if (t > lineHeight)
                t -= lineHeight;
        }

        public override void draw(Canvas canvas)
        {
            base.draw(canvas);

            // Временные переменные.            
            //Graphics gr = shape.graphics;

            //gr.clear();
            //gr.lineStyle();
            //gr.beginFill(c1);
            //gr.drawRect(0.0, 0.0, 640.0, 400.0);
            //gr.endFill();

            ColorTransform trans = new ColorTransform(c1);
            trans.alphaMultiplier = 0.7f;
            DrawMatrix m = DrawMatrix.Instance;

            int lineHeight = getLineHeight();
            float x = Constants.SAFE_OFFSET_X;
            float y = t - lineHeight;
            float maxY = Constants.ENV_HEIGHT;
            while (y < maxY)
            {
                //gr.beginFill(c2);
                //gr.moveTo(0.0, x);
                //gr.lineTo(640.0, x + 40.0);
                //gr.lineTo(640.0, x + 80.0);
                //gr.lineTo(0.0, x + 40.0);
                //gr.endFill();

                m.translate(x, y);
                canvas.draw(Res.IMG_EFFECT_LINE, m, trans);

                y += lineHeight;
            }

            //canvas.draw(shape);            
        }

        private int getLineHeight()
        {
            return utils.textureHeight(lineId);
        }
    }
}
