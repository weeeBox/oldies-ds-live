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

        public int getTotalPills()
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
    }

    public class FigureStage : LevelStage
    {
        private Generator gen;
        private float elapsedTime;
        private float figureAppearTime;

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
        private int numPills;
        private int totalPills;

        private Dictionary<int, Setuper> setuperLookup;

        public FigureStage() : base(TYPE_BONUS)
        {
            duckFigure.vx = -10.0f; // -18.0f;
            goalTime = (640 + duckFigure.getColsCount() * duckFigure.cellWidth) / Math.Abs(duckFigure.vx);

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
            figureAppearTime = 0;
            
            gen = new Generator();
            gen.regen = false;
            gen.speed = 8.0f;

            numVisibleLines = 0;
            totalPills = numPills = duckFigure.getTotalPills();
                        
            int duckWidth = duckFigure.getColsCount();
            int duckHeight = duckFigure.getRowsCount();

            duckFigure.x = 640.0f;
            duckFigure.y = 0.5f * (400.0f - duckWidth * Pill.DEFAULT_HEIGHT);            

            gen.regen = false;            
            gen.start();
        }

        public override void update(float dt)
        {
            elapsedTime += dt;
            
            duckFigure.vy = (float)(10 * Math.Sin(3.14 * elapsedTime)); 
            duckFigure.x += duckFigure.vx * dt;
            duckFigure.y += duckFigure.vy * dt;

            float dx = 640 - duckFigure.x;
            int newNumVisibleLines = (int)(dx / duckFigure.cellWidth);            
            if (newNumVisibleLines > numVisibleLines && newNumVisibleLines <= duckFigure.getColsCount())
            {
                if (numVisibleLines == 1)
                    figureAppearTime = elapsedTime;

                float px = duckFigure.x + numVisibleLines * duckFigure.cellWidth;
                for (int j = 0; j < duckFigure.getRowsCount(); ++j)
                {
                    int pillId = duckFigure.at(numVisibleLines, j);
                    if (setuperLookup.ContainsKey(pillId))
                    {
                        Setuper setuper = setuperLookup[pillId];

                        float py = duckFigure.y + j * duckFigure.cellHeight;
                        gen.map.Add(new Placer(setuper, px, py));
                    }
                }                
                numVisibleLines = newNumVisibleLines;
            }

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
            else if ("dead" == msg)
            {
                if (pill.type == Pill.POWER)
                {
                    numPills--;
                    Debug.WriteLine(numPills + "/" + totalPills);
                    if (numPills == 0)
                    {
                        if (collected == totalPills)
                        {
                            Debug.WriteLine("You win");
                        }
                        else
                        {
                            Debug.WriteLine("You've lost");
                        }
                    }
                }
            }
        }
    }
}
