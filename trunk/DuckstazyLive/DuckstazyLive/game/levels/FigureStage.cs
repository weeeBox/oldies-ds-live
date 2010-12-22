using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckstazyLive.game.levels.generator;
using DuckstazyLive.app;
using System.Diagnostics;

namespace DuckstazyLive.game.levels
{
    class FigurePattern
    {
        private byte[] pattern;
        private int width;
        private int height;

        public float vx, vy;

        public FigurePattern(byte[] pattern, int width, int height)
        {
            this.pattern = pattern;
            this.width = width;
            this.height = height;
        }        

        public byte at(int x, int y)
        {
            int index = y * width + x;
            Debug.Assert(index >= 0 && index < pattern.Length);
            return pattern[index];
        }

        public int getWidth()
        {
            return width;
        }

        public int getHeight()
        {
            return height;
        }
    }

    public class FigureStage : LevelStage
    {
        private Generator gen;
        private float elapsedTime;

        private FigurePattern duckFigure = new FigurePattern(new byte[] 
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
        }, 
        14, 11);        

        private Dictionary<int, Setuper> setuperLookup;

        public FigureStage() : base(TYPE_BONUS)
        {
            goalTime = 60.0f;
            setuperLookup = new Dictionary<int, Setuper>();
            PowerSetuper power1 = new PowerSetuper(0.0f, PowerSetuper.POWER1);
            PowerSetuper power2 = new PowerSetuper(0.0f, PowerSetuper.POWER2);
            PowerSetuper power3 = new PowerSetuper(0.0f, PowerSetuper.POWER3);
            Setuper missle = new MissleStarter();
            power1.userCallback = figureUserCallback;
            power2.userCallback = figureUserCallback;
            power3.userCallback = figureUserCallback;
            missle.userCallback = figureUserCallback;

            setuperLookup.Add(1, power1);
            setuperLookup.Add(2, power2);
            setuperLookup.Add(3, power3);
            setuperLookup.Add(4, missle);
        }

        public override void start()
        {
            base.start();

            elapsedTime = 0;

            gen = new Generator();
            gen.regen = false;
            gen.speed = 50.0f;

            int pillWidth = utils.textureWidth(Res.IMG_PILL_1P);
            int pillHeight = utils.textureHeight(Res.IMG_PILL_1P);

            int duckWidth = duckFigure.getWidth();
            int duckHeight = duckFigure.getHeight();

            float sx = 640.0f;
            float sy = 0.5f * (400.0f - duckWidth * pillHeight);
                        
            for (int i = 0; i < duckWidth; ++i)            
            {
                float px = sx + i * pillWidth;
                for (int j = 0; j < duckHeight; ++j)                
                {                    
                    int pillId = duckFigure.at(i, j);
                    if (setuperLookup.ContainsKey(pillId))
                    {
                        Setuper setuper = setuperLookup[pillId];
                        
                        float py = sy + j * pillHeight;
                        gen.map.Add(new Placer(setuper, px, py));                        
                    }            
                }                
            }

            gen.regen = false;            
            gen.start();
        }

        public override void update(float dt)
        {
            elapsedTime += dt;

            duckFigure.vx = -100.0f;
            duckFigure.vy = (float)(10 * Math.Sin(3.14 * elapsedTime)); 

            base.update(dt);
            gen.update(dt);
        }
        
        public void figureUserCallback(Pill pill, String msg, float dt)
        {
            if (null == msg)
            {
                pill.x += duckFigure.vx * dt;
                pill.y += duckFigure.vy * dt;                
                
                if (pill.x < 0 && pill.isAlive())
                {
                    pill.kill();
                    level.pills.ps.startAcid(pill.x, pill.y);
                }             
            }
        }
    }
}
