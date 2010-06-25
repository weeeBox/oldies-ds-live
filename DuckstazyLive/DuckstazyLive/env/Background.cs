using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DuckstazyLive
{
    public class Background
    {        
        private static Color SKY_UPPER_COLOR = new Color(63, 181, 242);
        private static Color SKY_LOWER_COLOR = new Color(221, 242, 255);

        private static Color GROUND_UPPER_COLOR = new Color(55, 29, 6);
        private static Color GROUND_LOWER_COLOR = new Color(93, 49, 12);

        private GradientRect sky;
        private GradientRect ground;
        
        public Background(float groundHeight)
        {
            float screenWidth = App.Width;
            float screenHeight = App.Height;
            float skyHeight = screenHeight - groundHeight;
            sky = new GradientRect(0, 0, screenWidth, skyHeight, SKY_UPPER_COLOR, SKY_LOWER_COLOR);
            ground = new GradientRect(0, skyHeight, screenWidth, groundHeight, GROUND_UPPER_COLOR, GROUND_LOWER_COLOR);            
        }

        public void Draw(BasicEffect effect)
        {
            sky.Draw(effect);
            ground.Draw(effect);
        }

        public void fillGround(Color color)
        {
            ground.fillWith(color);
        }

        private Application App
        {
            get { return Application.Instance; }
        }
    }
}
