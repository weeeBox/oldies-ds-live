using Microsoft.Xna.Framework.Graphics;

namespace DuckstazyLive.env
{
    public class Ground
    {
        private static Color GROUND_UPPER_COLOR = new Color(55, 29, 6);
        private static Color GROUND_LOWER_COLOR = new Color(93, 49, 12);
                
        private GradientRect ground;
        private Grass grass;

        public Ground(float x, float y, float width, float height)
        {
            ground = new GradientRect(x, y, width, height, GROUND_UPPER_COLOR, GROUND_LOWER_COLOR);
            grass = new Grass();
        }

        public void Draw(BasicEffect effect)
        {
            ground.Draw(effect);
            grass.Draw();
        }
    }
}
