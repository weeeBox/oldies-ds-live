using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace DuckstazyLive.framework.graphics
{
    public class Font
    {
        private SpriteFont font;
        private Vector2 drawPosition;

        public Font(SpriteFont font)
        {
            this.font = font;
        }

        public void Draw(GameGraphics g, String str, float x, float y)
        {
            Draw(g, str, x, y, 0);
        }

        public void Draw(GameGraphics g, String str, float x, float y, GraphicsAnchor anchor)
        {
            if ((anchor & GraphicsAnchor.RIGHT) != 0)
            {
                x -= font.MeasureString(str).X;
            }
            else if ((anchor & GraphicsAnchor.HCENTER) != 0)
            {
                x -= font.MeasureString(str).X * 0.5f;
            }

            if ((anchor & GraphicsAnchor.BOTTOM) != 0)
            {
                y -= font.MeasureString(str).Y;
            }
            else if ((anchor & GraphicsAnchor.VCENTER) != 0)
            {
                y -= font.MeasureString(str).Y * 0.5f;
            }

            drawPosition.X = x;
            drawPosition.Y = y;

            SpriteBatch batch = g.GetSpriteBatch();
            batch.DrawString(font, str, drawPosition, Color.White);            
        }
    }
}
