using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DuckstazyLive.env;
using DuckstazyLive.graphics;
using DuckstazyLive.env.sky;

namespace DuckstazyLive
{
    public class Background
    {        
        private static Color SKY_UPPER_COLOR = new Color(63, 181, 242);
        private static Color SKY_LOWER_COLOR = new Color(221, 242, 255);

        private static Color GROUND_UPPER_COLOR = new Color(55, 29, 6);
        private static Color GROUND_LOWER_COLOR = new Color(93, 49, 12);
        
        private AbstractSky sky;
        private Ground ground;
        
        public Background(float groundHeight)
        {
            float screenWidth = App.Width;
            float screenHeight = App.Height;
            float skyWidth = screenWidth;
            float skyHeight = screenHeight - groundHeight;            
            sky = new NormalSky(skyWidth, skyHeight, SKY_UPPER_COLOR, SKY_LOWER_COLOR);
            ground = new Ground(0, skyHeight, screenWidth, groundHeight);
        }

        public void Update(GameTime gameTime)
        {
            sky.Update(gameTime);
        }

        public void DrawSky(RenderContext context)
        {
            sky.Draw(context);
        }             

        public void DrawGround(RenderContext context)
        {
            ground.Draw(context);
        }

        private Application App
        {
            get { return Application.Instance; }
        }
    }
}
