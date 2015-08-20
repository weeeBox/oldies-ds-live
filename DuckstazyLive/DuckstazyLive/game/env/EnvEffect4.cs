using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using DuckstazyLive.app;
using System.Diagnostics;
using Framework.visual;

namespace DuckstazyLive.game.env
{
    public struct ScaleFrame
    {
        public float duration;
        public float scale;

        public ScaleFrame(float scale, float duration)
        {
            this.scale = scale;
            this.duration = duration;
        }
    }

    public class EnvEffect4 : EnvEffect
    {
        private static ScaleFrame[] scaleSequence = 
        {
            new ScaleFrame(0.8f, 0.1f),
            new ScaleFrame(1.0f, 0.1f),
            new ScaleFrame(0.5f, 0.1f),
            new ScaleFrame(0.8f, 0.1f),
            new ScaleFrame(0.6f, 0.1f),
            new ScaleFrame(0.9f, 0.1f),
            new ScaleFrame(0.8f, 0.1f),
            new ScaleFrame(1.0f, 0.1f),
            new ScaleFrame(0.3f, 0.1f),
            new ScaleFrame(0.8f, 0.1f),
            new ScaleFrame(0.5f, 0.1f),
            new ScaleFrame(0.9f, 0.1f),
            new ScaleFrame(0.8f, 0.1f),
            new ScaleFrame(1.0f, 0.1f),
            new ScaleFrame(0.8f, 0.1f),
            new ScaleFrame(0.8f, 0.1f),
            new ScaleFrame(0.5f, 0.1f),
            new ScaleFrame(0.9f, 0.1f),
            new ScaleFrame(0.8f, 0.1f),
            new ScaleFrame(1.5f, 0.1f),
            new ScaleFrame(0.9f, 0.1f),
            new ScaleFrame(0.8f, 0.1f),
            new ScaleFrame(0.5f, 0.1f),
            new ScaleFrame(0.9f, 0.1f),
        };

        private int scaleFrameIndex;
        private float scale;
        private float dScale;
        private float scaleFrameRemainingTime;

        private float t;        

        public EnvEffect4()
        {            
            t = 0.0f;
            scale = 1.0f;
        }

        public override void update(float dt)
        {
            t += dt * 1.256f * (power - 0.5f);
            if (t > 6.28f)
                t -= 6.28f;

            scaleFrameRemainingTime -= dt;
            if (scaleFrameRemainingTime <= 0)
            {
                scaleFrameIndex = (scaleFrameIndex + 1) % scaleSequence.Length;
                scaleFrameRemainingTime = scaleSequence[scaleFrameIndex].duration;
                dScale = (scaleSequence[scaleFrameIndex].scale - scale) / scaleFrameRemainingTime;
            }

            scale += dScale * dt;            
        }

        public override void draw(Canvas canvas)
        {
            base.draw(canvas);           

            // Временные переменные.
            float c = 0.0f;
            float a = t;
            float a2 = t + 0.314f;

            float transX = 0.5f * Constants.ENV_WIDTH;
            float transY = 0.5f * Constants.ENV_HEIGHT;

            int rayImg = Res.IMG_EFFECT_RAY;
            SpriteTexture rayTex = utils.getTexture(rayImg);

            DrawMatrix m = DrawMatrix.Instance;
            m.tx = -rayTex.Width;
            m.ty = -0.5f * rayTex.Height;
            m.translate(transX, transY);

            ColorTransform colorTransform = new ColorTransform(c1);

            while (c < 6.28f)
            {
                //gr.beginFill(c1);
                //gr.moveTo(320.0 + 512.0 * Math.Cos(a), 200.0 + 512.0 * Math.Sin(a));
                //gr.lineTo(320.0, 200.0);
                //gr.lineTo(320.0 + 512.0 * Math.Cos(a2), 200.0 + 512.0 * Math.Sin(a2));
                //gr.endFill();

                m.rotate(a);
                canvas.draw(rayImg, m, colorTransform);

                a += 0.628f;
                a2 += 0.628f;
                c += 0.628f;
            }

            int circleImage = Res.IMG_EFFECT_CIRCLE;
            SpriteTexture circleTex = utils.getTexture(circleImage);
            m.identity();
            m.translate(transX, transY);
            m.tx = -0.5f * circleTex.Width;
            m.ty = -0.5f * circleTex.Height;
            m.scale(scale, scale);
            canvas.draw(circleImage, m, colorTransform);
        }

    }

}
