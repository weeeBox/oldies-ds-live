using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using DuckstazyLive.graphics;
using Microsoft.Xna.Framework;

namespace DuckstazyLive.env.sky
{
    public class NormalSky : AbstractSky
    {
        private static readonly int CLOUDS_COUNT = 5;       
        private CloudsManager cloudsManager;

        public NormalSky(float width, float height, Color upperColor, Color lowerColor) : base(width, height, upperColor, lowerColor)
        {
            Rectangle bounds = new Rectangle(0, 0, (int)width, (int)(0.25f * height));
            cloudsManager = new CloudsManager(CLOUDS_COUNT, bounds);            
        }

        public override void Update(GameTime gameTime)
        {            
            cloudsManager.Update(gameTime);            
        }

        public override void Draw(RenderContext context)
        {
            base.Draw(context);      
            
            context.SpriteBatch.Begin();            
            cloudsManager.Draw(context.SpriteBatch);
            context.SpriteBatch.End();
        }        
    }
}
