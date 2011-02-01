using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.visual;
using Framework.core;
using DuckstazyLive.app;
using Microsoft.Xna.Framework;

namespace DuckstazyLive.game
{
    public class HeroGameState
    {        
        public int maxHP;

        public int health;        
        public int scores;        

        public HeroGameState()
        {            
            reset();
        }
        
        public void reset()
        {            
            maxHP = 3;
            health = maxHP;            
            scores = 0;
        }       
    }
}
