using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using asap.util;

namespace DuckstazyLive.app.game.env
{
    public class EnvStar
    {
        public float x;
        public float y;
        public float vx;
        public float vy;
        public float t;
        public float a;
        public ColorTransform color = ColorTransform.NONE;        

        public void update(float dt, float power)
        {
            float delta = dt * power;

            x += vx * delta;
            y += vy * delta;            

            t += 5.0f * power * dt;
            if (t >= 1.0f) t -= (int)t;

            delta = 1.0f - y / Constants.ENV_HEIGHT_UNSCALE;
            color.MulA = (float)Math.Sqrt(delta);//*(0.5-power)*2.0f;
        }
    }
}
