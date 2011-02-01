using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.visual;
using Framework.core;
using DuckstazyLive.app;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace DuckstazyLive.game
{
    public class HeroGameState
    {        
        public int maxHP;

        public int health;        
        public int scores;
        public int pillsCollected;
        public int toxicCollected;
        public int sleepCollected;

        public int pillsCollectedHud;
        private float pillsAddCounter;

        public HeroGameState()
        {            
            reset();
        }
        
        public void reset()
        {            
            maxHP = 3;
            health = maxHP;            
            scores = 0;
            pillsCollected = 0;
            toxicCollected = 0;
            sleepCollected = 0;
        }       

        public void update(float dt)
        {
            int pillsToAdd = pillsCollected - pillsCollectedHud;
            if (pillsToAdd != 0)
            {
                pillsAddCounter += dt;
                if (pillsAddCounter > 0.05f)
                {
                    pillsAddCounter = 0.0f;
                    if (Math.Sign(pillsToAdd) > 0)
                    {
                        pillsCollectedHud++;
                    }
                    else
                    {
                        pillsCollectedHud--;
                    }
                }
            }
        }

        public void addPills(int pills)
        {
            Debug.Assert(pillsCollected + pills >= 0);
            pillsCollected += pills;
            pillsAddCounter = 0;
        }
    }
}
