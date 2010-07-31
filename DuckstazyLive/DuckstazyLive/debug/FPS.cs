using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckstazyLive.core;
using DuckstazyLive.graphics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using DuckstazyLive.framework.core;

namespace DuckstazyLive.debug
{
    class FPS : Timer
    {        
        private float framesPerSecond;
        private int framesCount;
        
        private Vector2 position;

        public FPS(float refreshDelay, float x, float y) : base(refreshDelay)
        {            
            position.X = x;
            position.Y = y;
            StartTimer();
        }

        public override void Update(float dt)
        {            
            framesPerSecond = framesCount / Delay;            
            framesCount = 0;            
        }

        public void Draw(RenderContext context)
        {
            framesCount++;

            SpriteFont font = Resources.GetSpriteFont(Res.FONT_REGULAR);

            SpriteBatch batch = context.SpriteBatch;
            batch.Begin();
            batch.DrawString(font, "FPS: " + framesPerSecond, position, Color.White);
            batch.End();
        }     
    }
}
