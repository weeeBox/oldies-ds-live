using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using DuckstazyLive.app;

namespace DuckstazyLive.env
{
    public sealed class Grass
    {
        private Color tiltColor = Color.Red;
        private bool hasTilt = true;

        public void Draw()
        {
            Texture2D grass = Resources.GetTexture(Res.IMG_GRASS);
            int width = grass.Width;
            int height = grass.Height;

            Rectangle source = new Rectangle(0, 0, App.Width, height);
            Vector2 position = new Vector2(0, App.Height - (Constants.GROUND_HEIGHT + height));

            SpriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None);
            GraphicsDevice.SamplerStates[0].AddressU = TextureAddressMode.Wrap;
            GraphicsDevice.SamplerStates[0].AddressV = TextureAddressMode.Wrap;
            SpriteBatch.Draw(grass, position, source, hasTilt ? tiltColor : Color.White);
            SpriteBatch.End();
        }

        private GraphicsDevice GraphicsDevice
        {
            get { return App.GraphicsDevice; }
        }

        private SpriteBatch SpriteBatch
        {
            get { return App.SpriteBatch; }
        }

        private Application App
        {
            get { return Application.Instance; }
        }
    }
}
