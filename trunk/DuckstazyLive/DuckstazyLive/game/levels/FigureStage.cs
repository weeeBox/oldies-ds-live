using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckstazyLive.game.levels.generator;
using DuckstazyLive.app;

namespace DuckstazyLive.game.levels
{
    public class FigureStage : LevelStage
    {
        private Generator gen;

        private const int duckWidth = 14;
        private const int duckHeight = 11;
        private static byte[] duckPattern = 
        {
            0,0,2,2,2,2,0,0,0,0,0,0,0,0,
            0,0,2,4,2,2,0,0,0,0,0,0,0,0,
            0,0,2,4,2,2,0,0,0,0,0,0,0,0,
            3,3,2,2,2,2,0,2,2,2,2,2,2,0,
            0,0,0,0,2,2,0,2,2,2,2,2,2,0,
            0,0,0,0,2,2,0,2,4,4,4,4,2,0,
            0,0,0,0,2,2,0,2,2,2,2,2,2,0,
            0,0,0,0,2,2,0,2,4,4,4,4,2,0,
            0,0,0,0,2,2,2,2,2,2,2,2,2,2,
            0,0,0,0,2,2,2,2,2,2,2,2,2,0,
            0,0,0,0,0,0,0,0,3,0,3,0,0,0,
        };

        private Dictionary<int, Setuper> setuperLookup;

        public FigureStage() : base(TYPE_BONUS)
        {
            goalTime = 60.0f;
            setuperLookup = new Dictionary<int, Setuper>();
            setuperLookup.Add(1 ,new PowerSetuper(0.0f, PowerSetuper.POWER1));
            setuperLookup.Add(2, new PowerSetuper(0.0f, PowerSetuper.POWER2));
            setuperLookup.Add(3, new PowerSetuper(0.0f, PowerSetuper.POWER3));
            setuperLookup.Add(4, new MissleStarter());
        }

        public override void start()
        {
            base.start();            

            gen = new Generator();
            gen.regen = false;
            gen.speed = 8.0f;

            int pillWidth = utils.textureWidth(Res.IMG_PILL_1P);
            int pillHeight = utils.textureHeight(Res.IMG_PILL_1P);

            float sx = 0.5f * (640.0f - duckWidth * pillWidth);
            float sy = 0.5f * (400.0f - duckHeight * pillHeight);

            int index = 0;
            for (int j = 0; j < duckHeight; ++j)            
            {                
                float py = sy + j * pillHeight;
                for (int i = 0; i < duckWidth; ++i)    
                {                    
                    int pillId = duckPattern[index];
                    if (setuperLookup.ContainsKey(pillId))
                    {
                        Setuper setuper = setuperLookup[pillId];

                        float px = sx + i * pillWidth;           
                        gen.map.Add(new Placer(setuper, px, py));                        
                    }
                    ++index;
                }
            }

            gen.regen = false;
            gen.start();
        }

        public override void update(float dt)
        {
            base.update(dt);
            gen.update(dt);
        }
    }
}
