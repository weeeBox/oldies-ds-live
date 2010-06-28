using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DuckstazyLive.graphics
{
    public class Camera
    {
        private Matrix world;
        private Matrix view;
        private Matrix projection;

        public Camera(Matrix world, Matrix view, Matrix projection)
        {
            this.world = world;
            this.view = view;
            this.projection = projection;
        }

        public Matrix View
        {
            get { return view; }
        }

        public Matrix Projection
        {
            get { return projection; }
        }

        public Matrix World
        {
            get { return world; }
        }        
    }
}
