using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using DuckstazyLive.app;
using System.Diagnostics;
using Framework.visual;
using Framework.core;

namespace DuckstazyLive.game
{
    public class LevelStage
    {
        // кол-во времени для прохождения
        public float goalTime;

        // обратить флаг в true, если прошли уровень.
        public bool win;

        // уровень
        protected Level level;
        protected Pills pills;
        protected Particles particles;
        
        protected Heroes heroes;
        protected Env env;
        
        public const int TYPE_PUMP = 0; // накачай утку
        public const int TYPE_BONUS = 1; // бонус уровень (собирай данное время)
        public const int TYPE_DUCKTRIP = 2; // трип
        protected int type;

        protected float pumpProg; // прогресс накачки 0->1 после power==1
        protected float pumpVel; // скорость накачки
        public int collected;

        protected float startX;
        protected bool heroStarted;

        public StageMedia media;        

        protected bool end;
        protected int endImg;
        protected float endCounter;

        public LevelStage(int _type)
        {
            level = Level.instance;
            media = level.stageMedia;
            pills = level.pills;
            particles = level.pills.ps;
            heroes = level.heroes;            
            env = level.env;

            if (_type == TYPE_PUMP)
            {
                goalTime = 2.0f;
                pumpVel = 1.0f;
            }           

            type = _type;
        }

        public virtual void start()
        {
            win = false;            

            pumpProg = 0.0f;
            collected = 0;

            startX = utils.rnd() * (640 - 54);
            heroStarted = false;

            end = false;
        }

        public virtual void onWin()
        {
        }

        public virtual void draw1(Canvas canvas)
        {

        }

        public virtual void draw2(Canvas canvas)
        {
            
        }

        public virtual void update(float dt)
        {
            float t;
            int i;
            String str;

            if (!heroStarted)
            {
                heroStarted = true;
                if (heroes.getHeroesCount() > 1)
                {
                    heroes.startHeroes();
                }
                else
                {
                    heroes.start(startX);
                }
            }

            if (!win)
            {

                if (type == TYPE_PUMP)
                {
                    level.progress.updateProgress(level.power + pumpProg);
                    if (level.power >= 1.0f)
                    {
                        pumpProg += dt * pumpVel;
                        if (pumpProg > 1.0f)
                            pumpProg = 1.0f;
                    }

                    str = ((int)(level.progress.getCompletePercent() * 100)).ToString() + "%";
                    if (level.infoText != str) level.infoText = str;
                }
                else if (type == TYPE_BONUS)
                {
                    level.progress.updateProgress(pumpProg);
                    if (pumpProg < goalTime)
                    {
                        pumpProg += dt;
                        if (pumpProg > goalTime)
                            pumpProg = goalTime;
                    }

                    t = (1.0f - level.progress.getCompletePercent()) * goalTime;
                    i = (int)(t / 60);
                    if (i < 10) str = "0" + i.ToString() + ":";
                    else str = i.ToString() + ":";
                    i = ((int)t) % 60;
                    if (i < 10) str += "0" + i.ToString();
                    else str += i.ToString();

                    if (level.infoText != str) level.infoText = str;
                }
                else if (type == TYPE_DUCKTRIP)
                {
                    level.progress.updateProgress(collected);
                    str = collected.ToString() + " OF " + ((int)goalTime).ToString();
                    if (level.infoText != str) level.infoText = str;
                }

                if (level.progress.isCompleted())
                {
                    win = true;
                    level.infoText = "";
                    this.onWin();
                    end = true;
                    endImg = media.imgStageEnd;
                    endCounter = 0.0f;
                }
                else if (!end && !heroes.hasAliveHero())
                {
                    level.infoText = "";
                    end = true;
                    endImg = media.imgTheEnd;
                    endCounter = 0.0f;
                }
            }

            if (end)
                endCounter += dt;            
        }
    }
}
