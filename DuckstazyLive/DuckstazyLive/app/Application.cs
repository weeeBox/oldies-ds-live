using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using DuckstazyLive.graphics;

namespace DuckstazyLive
{
    class Application
    {
        private static Application instance;

        private int width;
        private int height;
        private GraphicsDevice graphicsDevice;
        private SpriteBatch spriteBatch;
        private Camera camera;

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

        public Camera Camera
        {
            get { return camera; }
            set { camera = value; }
        }

        public static Application Instance
        {
            get { return instance; }
        }
    }
}
