using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckstazyLive.core;
using DuckstazyLive.graphics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using DuckstazyLive.framework.core;
using DuckstazyLive.framework.graphics;

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

        public void Draw(GameGraphics g)
        {
            framesCount++;

            Font font = Resources.GetSpriteFont(Res.FONT_REGULAR);            
            font.Draw(g, "FPS: " + framesPerSecond, position.X, position.Y);            
        }     
    }
}
