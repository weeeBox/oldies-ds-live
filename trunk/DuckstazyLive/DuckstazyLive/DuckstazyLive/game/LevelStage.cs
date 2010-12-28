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
    public abstract class LevelStage
    {        
        public bool day;
                
        protected Pills pills;
        protected Particles particles;
        protected Heroes heroes;

        public int collected;

        protected float startX;
        protected bool heroStarted;

        public StageMedia media;        

        public LevelStage()
        {            
            day = true;
        }        

        public virtual void start()
        {            
            collected = 0;

            startX = utils.rnd() * (640 - 54);
            heroStarted = false;           

            setDay(day);            
        }

        public virtual void draw1(Canvas canvas)
        {
        }

        public virtual void draw2(Canvas canvas)
        {            
        }

        public virtual void drawUI(Canvas canvas)
        {            
        }

        public virtual void update(float dt)
        {
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
        }

        public void setDay(bool day)
        {
            Env.getIntance().day = day;        
        }        
    }
}
