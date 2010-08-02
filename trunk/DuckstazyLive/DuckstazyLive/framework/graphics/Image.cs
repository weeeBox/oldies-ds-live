using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace DuckstazyLive.framework.graphics
{
    public class Image : IDisposable
    {
#region Fields

        Texture2D texture;
        int width;
        int height;
        float rotation;
        Vector2 drawPosition;
        Vector2 origin;
        Vector2 scale;
        Color color;
        SpriteEffects effects;
#endregion
#region Constuctors
        public Image(Texture2D texture)
        {
            this.texture = texture;
            width = texture.Width;
            height = texture.Height;
            ResetColor();
            ResetOrigin();
            ResetScale();
        }
#endregion
#region Drawing
        public void Draw(GameGraphics g, float x, float y)
        {
            Draw(g, x, y, 0);
        }

        public void Draw(GameGraphics g, float x, float y, GraphicsAnchor anchor)
        {
            if ((anchor & GraphicsAnchor.RIGHT) != 0)
            {
                x -= Width;
            }
            else if ((anchor & GraphicsAnchor.HCENTER) != 0)
            {
                x -= Width >> 1;
            }

            if ((anchor & GraphicsAnchor.BOTTOM) != 0)
            {
                y -= Height;
            }
            else if ((anchor & GraphicsAnchor.VCENTER) != 0)
            {
                y -= Height >> 1;
            }

            drawPosition.X = x;
            drawPosition.Y = y;
            SpriteBatch batch = g.GetSpriteBatch();
            batch.Draw(texture, drawPosition, null, color, rotation, origin, scale, effects, 0);
        }

        public void DrawTiled(GameGraphics g, float x, float y, float width, float height)
        {
            Rectangle source = new Rectangle(0, 0, (int)width, (int)height);
            Vector2 position = new Vector2(x, y);

            SpriteBatch batch = g.GetSpriteBatch();
            g.GraphicsDevice.SamplerStates[0].AddressU = TextureAddressMode.Wrap;
            g.GraphicsDevice.SamplerStates[0].AddressV = TextureAddressMode.Wrap;
            batch.Draw(texture, position, source, Color.White);
        }
#endregion
#region Methods
        public void SetColor(Color c)
        {
            color = c;
        }

        public void SetColor(byte r, byte g, byte b)
        {
            SetColor(r, g, b, 255);
        }        

        public void SetColor(byte r, byte g, byte b, byte a)
        {
            color.A = a;
            color.R = r;
            color.G = g;
            color.B = b;
        }

        public void ResetColor()
        {
            color = Color.White;
        }

        public void SetOrigin(float x, float y)
        {
            origin.X = x;
            origin.Y = y;
        }

        public void SetOriginToCenter()
        {
            SetOrigin(0.5f * Width, 0.5f * Height);
        }

        public void ResetOrigin()
        {
            origin = Vector2.Zero;
        }

        public void Scale(float sx, float sy)
        {
            scale.X = sx;
            scale.Y = sy;
        }

        public void SetScale(float k)
        {
            Scale(k, k);
        }

        public void ResetScale()
        {
            Scale(1.0f, 1.0f);
        }

        public void FlipHorizontal()
        {            
            effects |= SpriteEffects.FlipHorizontally;            
        }

        public void FlipVectical()
        {            
            effects |= SpriteEffects.FlipVertically;            
        }

        public void ResetFlips()
        {
            effects = SpriteEffects.None;
        }

        public void RotateDegrees(float degrees)
        {
            Rotate(MathHelper.ToRadians(degrees));
        }

        public void Rotate(float radians)
        {
            rotation = radians;
        }
#endregion
#region Properties
        public int Width
        {
            get { return width; }
        }

        public int Height
        {
            get { return height; }
        }
#endregion
#region Dispose
        public void Dispose()
        {
            texture.Dispose();
        }
#endregion
    }
}
