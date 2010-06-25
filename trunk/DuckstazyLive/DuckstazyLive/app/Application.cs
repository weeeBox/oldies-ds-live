using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace DuckstazyLive
{
    class Application
    {
        private static Application instance;

        private float width;
        private float height;
        private GraphicsDevice graphicsDevice;
        private SpriteBatch spriteBatch;

        public Application(float width, float height)
        {
            instance = this;

            this.width = width;
            this.height = height;
        }

        public float Width
        {
            get { return width; }
        }

        public float Height
        {
            get { return height; }
        }

        public GraphicsDevice GraphicsDevice
        {
            get { return graphicsDevice; }
            set { graphicsDevice = value; }
        }

        public SpriteBatch SpriteBatch
        {
            get { return spriteBatch; }
            set { spriteBatch = value; }
        }

        public static Application Instance
        {
            get { return instance; }
        }
    }
}
