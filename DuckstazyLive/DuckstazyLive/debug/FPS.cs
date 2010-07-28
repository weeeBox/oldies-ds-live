using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckstazyLive.core;
using DuckstazyLive.graphics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace DuckstazyLive.debug
{
    class FPS : GameObject
    {
        private float refreshDelay;
        private float elapsedTime;
        private float framesPerSecond;
        private int framesCount;
        
        private Vector2 position;

        public FPS(float refreshDelay, float x, float y)
        {
            this.refreshDelay = refreshDelay;
            position.X = x;
            position.Y = y;
        }

        public void Update(float dt)
        {
            elapsedTime += dt;
            if (elapsedTime > refreshDelay)
            {
                framesPerSecond = framesCount / refreshDelay;
                elapsedTime = 0;
                framesCount = 0;
            }
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
