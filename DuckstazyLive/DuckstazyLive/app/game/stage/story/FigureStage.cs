using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckstazyLive.game.levels.generator;
using DuckstazyLive.app;
using System.Diagnostics;
using DuckstazyLive.app.game;

namespace DuckstazyLive.game.levels
{
    public class FigurePattern
    {
        private byte[] pattern;
        private int cols;
        private int rows;

        public float vx, vy;
        public float x, y;

        public float cellWidth, cellHeight;

        public FigurePattern(byte[] pattern, int cols, int rows)
        {
            this.pattern = pattern;
            this.cols = cols;
            this.rows = rows;

            cellWidth = 1.6f * Pill.DEFAULT_WIDTH;
            cellHeight = 1.5f * Pill.DEFAULT_HEIGHT;
        }        

        public byte at(int x, int y)
        {
            int index = y * cols + x;
            Debug.Assert(index >= 0 && index < pattern.Length);
            return pattern[index];
        }

        public int getColsCount()
        {
            return cols;
        }

        public int getRowsCount()
        {
            return rows;
        }

        public int getPowerPillsCount()
        {
            int total = 0;
            for (int i = 0; i < pattern.Length; ++i)
            {
                byte pill = pattern[i];
                if (pill == 1 || pill == 2 || pill == 3)
                    total++;
            }

            return total;
        }

        public int getTotalPillsCount()
        {
            int total = 0;
            for (int i = 0; i < pattern.Length; ++i)
            {
                byte pill = pattern[i];
                if (pill != 0)
                    total++;
            }

            return total;
        }
    }

    public class FigurePlacer : Placer
    {
        private int row;
        private int col;

        private FigurePattern pattern;

        public FigurePlacer(Setuper setuper, FigurePattern pattern, int row, int col)
            : base(setuper, pattern.cellWidth * col, pattern.cellHeight * row)
        {
            this.pattern = pattern;
            this.row = row;
            this.col = col;
        }

        protected override Pill start(Pill pill)
        {
            float x = pattern.x + pattern.cellWidth * col;
            float y = pattern.y + pattern.cellHeight * row;
            return setuper.start(x, y, pill);
        }
    }

    public class FigureStage : PillCollectLevelStage
    {
        private Generator gen;
        private float elapsedTime;
        private float figureAppearTime;

        private float additionVx;        

        private FigurePattern duckFigure = new FigurePattern(new byte[] 
        {
            0,0,2,2,2,2,0,0,0,0,0,0,0,0,
            0,0,2,4,2,2,0,0,0,0,0,0,0,0,
            0,0,2,4,2,2,0,0,0,0,0,0,0,0,
            3,3,2,2,2,2,0,2,2,2,2,2,2,0,
            0,0,0,0,2,2,0,2,4,4,4,4,2,0,
            0,0,0,0,2,2,0,2,2,2,2,2,2,0,
            0,0,0,0,2,2,0,2,2,2,2,2,2,0,
            0,0,0,0,2,2,0,2,4,4,4,4,2,2,
            0,0,0,0,2,2,2,2,2,2,2,2,2,2,
            0,0,0,0,2,2,2,2,2,2,2,2,2,0,
            0,0,0,0,0,0,0,0,3,0,3,0,0,0,
        }, 
        14, 11);

        private int numVisibleLines;        
        private int totalPills;

        private Dictionary<int, Setuper> setuperLookup;

        public FigureStage() : base(0)
        {
            duckFigure.vx = -18.0f;
            numPills = duckFigure.getPowerPillsCount();

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

            day = false;
        }

        public override void onStart()
        {
            base.onStart();

            elapsedTime = 0;
            figureAppearTime = 0;
            
            gen = new Generator();
            gen.regen = false;
            gen.speed = 10.0f;

            numVisibleLines = 0;
            totalPills = duckFigure.getTotalPillsCount();
                        
            int duckWidth = duckFigure.getColsCount();
            int duckHeight = duckFigure.getRowsCount();

            duckFigure.x = 640.0f;
            duckFigure.y = 0.5f * (400.0f - duckWidth * Pill.DEFAULT_HEIGHT);            

            gen.regen = false;            
            gen.start();

            additionVx = 0;            
        }

        public override void Update(float dt)
        {
            elapsedTime += dt;                       

            if (hasHeroOnTheGround())
            {
                additionVx -= 20 * dt;
                if (additionVx < -40.0f)
                    additionVx = -40.0f;
            }
            else
            {
                additionVx += 10 * dt;
                if (additionVx > 0)
                    additionVx = 0;
            }

            float fvx = getFigureVx();
            float fvy = duckFigure.vy;

            duckFigure.vy = (float)(10 * (1 + 2 * level.power) * Math.Sin(3.14 * elapsedTime)); 
            duckFigure.x += fvx * dt;
            duckFigure.y += fvy * dt;            

            float genTimeout = 1.0f / gen.speed;

            float dx = 640 - duckFigure.x;
            int newNumVisibleLines = (int)(dx / duckFigure.cellWidth);            
            if (newNumVisibleLines > numVisibleLines && newNumVisibleLines <= duckFigure.getColsCount())
            {
                if (numVisibleLines == 1)
                    figureAppearTime = elapsedTime;
                                
                for (int j = 0; j < duckFigure.getRowsCount(); ++j)
                {
                    int pillId = duckFigure.at(numVisibleLines, j);
                    if (setuperLookup.ContainsKey(pillId))
                    {
                        Setuper setuper = setuperLookup[pillId];                       
                        gen.map.Add(new FigurePlacer(setuper, duckFigure, j, numVisibleLines));
                    }
                }                
                numVisibleLines = newNumVisibleLines;
            }

            base.Update(dt);
            gen.Update(dt);
        }
        
        public void figureUserCallback(Pill pill, String msg, float dt)
        {
            if (null == msg)
            {
                pill.x += getFigureVx() * dt;
                pill.y += duckFigure.vy * dt;                
                
                if (pill.x < 0 && pill.isAlive())
                {
                    pill.kill();
                    getParticles().startAcid(pill.x, pill.y);
                }             
            }
            else if ("dead" == msg)
            {
                totalPills--;
                if (totalPills == 0 && !progress.isProgressComplete())
                {
                    loose();
                }
            }
        }     
   
        private float getFigureVx()
        {
            return duckFigure.vx + additionVx;
        }

        private bool hasHeroOnTheGround()
        {
            Heroes heroes = getHeroes();
            for (int i = 0; i < heroes.getHeroesCount(); ++i)
            {
                Hero h = heroes[i];           
                if (!h.isFlying())
                    return true;
            }

            return false;
        }
    }
}
