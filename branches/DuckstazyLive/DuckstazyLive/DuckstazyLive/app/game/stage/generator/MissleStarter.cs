using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckstazyLive.app.game;

namespace DuckstazyLive.game.levels.generator
{
    public class MissleStarter : Setuper
    {        
        public MissleStarter() 
        {     
        }

        public override Pill start(float x, float y, Pill pill)
        {
            pill.user = userCallback;
            pill.startMissle(x, y, 0);
            return pill;
        }

    }
}
