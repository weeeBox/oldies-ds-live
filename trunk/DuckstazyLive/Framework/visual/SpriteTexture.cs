using Microsoft.Xna.Framework;

namespace Framework.visual
{
    public interface SpriteTexture
    {
        int Width { get; }
        int Height { get; }
                
        void draw(float x, float y);
        void draw(float x, float y, float opacity);
        void draw(float x, float y, ref Color color);
        void draw(ref Vector2 position, ref Color color, float rotation, ref Vector2 origin, ref Vector2 scale, ref Vector2 flip);

        void drawPart(ref Rectangle rectangle, float x, float y);
        void drawTiled(ref Rectangle src, ref Rectangle dst);        
    }
}
