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

        private int width;
        private int height;
        private GraphicsDevice graphicsDevice;
        private SpriteBatch spriteBatch;

        public Application(int width, int height)
        {
            instance = this;

            this.width = width;
            this.height = height;
        }

        public int Width
        {
            get { return width; }
        }

        public int Height
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
