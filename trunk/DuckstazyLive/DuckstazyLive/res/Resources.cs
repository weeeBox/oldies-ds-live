using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DuckstazyLive
{
    public class Resources
    {
        private IDisposable[] resources;
        private static Resources instance = new Resources();

        private Resources()
        {             
        }

        public static Resources Instance
        {
            get { return instance; }
        }

        public void Init(ContentManager content)
        {
            resources = new IDisposable[1];
            resources[0] = content.Load<Texture2D>("duck");
        }       

        public static Texture2D GetTexture(int id)
        {            
            return (Texture2D)instance.resources[id];
        }
    }
}
