using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuckstazyLive
{
    class Application
    {
        private static Application instance;

        private float width;
        private float height;

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

        public static Application Instance
        {
            get { return instance; }
        }
    }
}
