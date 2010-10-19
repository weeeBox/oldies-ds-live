using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DuckstazyLive.game
{
    public class Pill
    {
        public float x, y, vx, vy;

        private int poolIndex;
        internal int PoolIndex
        {
            get { return poolIndex; }
            set { poolIndex = value; }
        }
        internal void clear()
        {            
            x = y = vx = vy = 0.0f;
        }
    }
}
