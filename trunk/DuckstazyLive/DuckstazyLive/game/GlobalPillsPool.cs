using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace DuckstazyLive.game
{
    public class GlobalPillsPool
    {
        private Pill[] pills;
        private int pillsCount;

        public GlobalPillsPool(int initialSize)
        {
            pills = new Pill[initialSize];
        }

        public Pill allocatePill(float x, float y)
        {
            Debug.Assert(pillsCount < pills.Length); // todo add pool resize

            Pill pill = pills[pillsCount];
            if (pill == null)
            {
                pill = new Pill();
                pills[pillsCount] = pill;
            }
            pill.clear();
            pill.PoolIndex = pillsCount;
            pill.x = x;
            pill.y = y;
            pillsCount++;

            Debug.WriteLine("Pill allocated: " + (pillsCount - 1) + " total: " + pillsCount);
            return pill;
        }

        public void freePill(Pill pill)
        {
            int pillIndex = pill.PoolIndex;
            pills[pillIndex] = pills[pillsCount - 1];
            pills[pillIndex].PoolIndex = pillIndex;
            pills[pillsCount - 1] = pill;
            pillsCount--;
        }        
    }
}
