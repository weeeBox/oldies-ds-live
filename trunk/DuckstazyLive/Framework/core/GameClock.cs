using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.core
{
    public class GameClock
    {
        private static float elapsedTime;

        public static void update(float dt)
        {
            elapsedTime += dt;
        }

        public static float ElapsedTime
        {
            get { return elapsedTime; }
        }

        public static int ElapsedTimeMs
        {
            get { return (int)(elapsedTime * 1000); }
        }
    }
}
