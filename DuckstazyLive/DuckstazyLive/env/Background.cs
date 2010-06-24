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
        
        private GraphicsDevice device;

        public Background(GraphicsDevice device, float groundHeight)
        {
            float screenWidth = device.Viewport.Width;
            float screenHeight = device.Viewport.Height;
            float skyHeight = screenHeight - groundHeight;
            sky = new GradientRect(device, 0, 0, screenWidth, skyHeight, SKY_UPPER_COLOR, SKY_LOWER_COLOR);
            ground = new GradientRect(device, 0, skyHeight, screenWidth, groundHeight, GROUND_UPPER_COLOR, GROUND_LOWER_COLOR);
                        
            this.device = device;
        }

        public void Draw(ref Matrix viewMatrix, ref Matrix projectionMatrix, ref Matrix worldMatrix)
        {
            sky.Draw(ref viewMatrix, ref projectionMatrix, ref worldMatrix);
            ground.Draw(ref viewMatrix, ref projectionMatrix, ref worldMatrix);
        }

        public void fillGround(Color color)
        {
            ground.fillWith(color);
        }
    }
}
