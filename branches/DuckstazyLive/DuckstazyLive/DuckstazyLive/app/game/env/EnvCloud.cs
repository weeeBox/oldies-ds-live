using System;
using asap.util;
using asap.visual;
using asap.graphics;

namespace DuckstazyLive.app.game.env
{
    public class EnvCloud : Image
    {        
        public float counter;        

        public EnvCloud(GameTexture texture) : base(texture)
        {
        }

        public void init(float x)
        {
            this.x = x;
            this.y = RandomHelper.rnd_float(40, 90);            
            counter = RandomHelper.rnd();
            SetTexture(texture);
        }        

        public void Update(float dt, float power)
        {
            x -= (float)((0.75 + 0.25 * Math.Sin(counter * 6.2832)) * (30.0f + power * 200.0f) * dt);            
            counter += (0.1f + 0.9f * power) * dt;
            if (counter >= 1.0f)
                counter -= (int)(counter);

            scaleX = 0.9f + 0.1f * (float)Math.Sin(counter * 6.28);
            scaleY = 0.95f + 0.05f * (float)Math.Sin(counter * 6.28 + 3.14);
        }
    };
}
