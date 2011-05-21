using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using asap.visual;
using asap.util;
using asap.graphics;
using app;

namespace DuckstazyLive.app.game.env
{
    public class DaySky : Sky
    {
        private const int CLOUDS_COUNT = 5;

        private EnvCloud[] clouds;
        private GameTexture[] cloudTextures;        

        public DaySky(float width, float height)
            : base(width, height, ColorUtils.MakeColor(0x3FB5F2), ColorUtils.MakeColor(0xDDF2FF))        
        {
            cloudTextures = new GameTexture[]
            {
                Application.sharedResourceMgr.GetTexture(Res.IMG_CLOUD_1),
                Application.sharedResourceMgr.GetTexture(Res.IMG_CLOUD_2),
                Application.sharedResourceMgr.GetTexture(Res.IMG_CLOUD_3)
            };            

            float x = 0.0f;
            clouds = new EnvCloud[CLOUDS_COUNT];
            for (int i = 0; i < clouds.Length; ++i)
            {
                int texIndex = RandomHelper.rnd_int(cloudTextures.Length);
                EnvCloud cloud = new EnvCloud(cloudTextures[texIndex]);
                cloud.init(x);                
                clouds[i] = cloud;
                AddChild(cloud);

                x += 288.0f + RandomHelper.rnd() * 33.0f;
            }           
        }

        protected override void Update(float delta, float power)
        {
            foreach (EnvCloud cloud in clouds)
            {
                cloud.update(delta, power);

                float border = cloud.width;
                if (cloud.x <= -border)
                {
                    int texIndex = RandomHelper.rnd_int(cloudTextures.Length);
                    cloud.SetTexture(cloudTextures[texIndex]);

                    cloud.x += width + border;
                    cloud.y = 60.0f + RandomHelper.rnd() * 135.0f;                    
                }
            }
        }
    }
}
