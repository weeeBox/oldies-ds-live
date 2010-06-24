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
            resources = new IDisposable[Res.RESOURCES_COUNT];
            resources[Res.IMG_DUCK] = content.Load<Texture2D>("duck");
            resources[Res.IMG_GRASS] = content.Load<Texture2D>("grass");
        }       

        public static Texture2D GetTexture(int id)
        {            
            return (Texture2D)instance.resources[id];
        }
    }
}
