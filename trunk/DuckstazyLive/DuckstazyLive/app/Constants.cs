using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.core;

namespace DuckstazyLive.app
{
    public class Constants : FrameworkConstants
    {
        public const int PILL_RADIUS = 15;
        public const int PILLS_POOL_SIZE = 50;

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
