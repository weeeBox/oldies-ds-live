using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.core
{
    public class Constants
    {
        public const int UNDEFINED = -1;

        public const int SCREEN_WIDTH = 1280;
        public const int SCREEN_HEIGHT = 720;

        public const int PILL_RADIUS = 15;

        public const float WORLD_VIEW_HEIGHT_RATIO = 0.83f;

        public const int WORLD_VIEW_X = 0;
        public const int WORLD_VIEW_Y = 0;
        public const int WORLD_VIEW_WIDTH = SCREEN_WIDTH;
        public const int WORLD_VIEW_HEIGHT = (int)(WORLD_VIEW_HEIGHT_RATIO * SCREEN_HEIGHT);       

        public const int GROUND_X = 0;
        public const int GROUND_Y = WORLD_VIEW_HEIGHT;

        public const int GROUND_WIDTH = SCREEN_WIDTH;
        public const int GROUND_HEIGHT = SCREEN_HEIGHT - WORLD_VIEW_HEIGHT;
    }
}
