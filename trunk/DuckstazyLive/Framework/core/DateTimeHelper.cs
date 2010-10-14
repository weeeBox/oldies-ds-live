using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.core
{
    public class DateTimeHelper
    {
        private static float gameTime = 0;

        public static float getGameTime()
        {
            return gameTime;
        }

        public static void advanceTime(float val)
        {
            gameTime += val;
        }        
    }
}
