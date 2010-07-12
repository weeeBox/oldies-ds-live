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
            Console.WriteLine("Load resources...");

            resources = new IDisposable[Res.RESOURCES_COUNT];
            resources[Res.IMG_DUCK] = content.Load<Texture2D>("duck");
            resources[Res.IMG_GRASS] = content.Load<Texture2D>("grass");
            resources[Res.EFFECT_WAVE] = content.Load<Effect>("shaders\\WaveEffect");
            resources[Res.EFFECT_RADIAL] = content.Load<Effect>("shaders\\RadialSkyShader");
            resources[Res.IMG_SKY_CLOUD_1] = content.Load<Texture2D>("cloud_1");
            resources[Res.IMG_SKY_CLOUD_2] = content.Load<Texture2D>("cloud_2");
            resources[Res.IMG_SKY_CLOUD_3] = content.Load<Texture2D>("cloud_3");
            resources[Res.IMG_BUBBLE] = content.Load<Texture2D>("bubble");
        }       

        public static Texture2D GetTexture(int id)
        {            
            return (Texture2D)instance.resources[id];
        }

        public static Effect GetEffect(int id)
        {
            return (Effect)instance.resources[id];
        }
    }
}
