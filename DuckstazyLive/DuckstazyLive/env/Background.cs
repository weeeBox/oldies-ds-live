using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DuckstazyLive.env;
using DuckstazyLive.graphics;
using DuckstazyLive.env.sky;
using DuckstazyLive.framework.graphics;

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
            sky = new NightSky(skyWidth, skyHeight, new Color(17, 17, 51), new Color(17, 17, 51), 50, 50);
            ground = new Ground(0, skyHeight, screenWidth, groundHeight);
        }

        public void Update(float dt)
        {
            sky.Update(dt);
        }

        public void DrawSky(GameGraphics g)
        {
            sky.Draw(g);
        }             

        public void DrawGround(GameGraphics g)
        {
            ground.Draw(g);
        }

        private Application App
        {
            get { return Application.Instance; }
        }
    }
}
