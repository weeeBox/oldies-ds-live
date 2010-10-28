using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.core;
using Microsoft.Xna.Framework;

namespace DuckstazyLive.app
{
    public class Constants : FrameworkConstants
    {
        public const float SCALE = 1.5f;
        public const float SCALE_INV = 1.0f / SCALE;

        public const int PILL_RADIUS = 15;
        public const int PILLS_POOL_SIZE = 50;

        public const float PILL_SPAWN_TIMEOUT = 0.25f;
        public const float PILL_RESPAWN_TIMEOUT_MIN = 0.5f;
        public const float PILL_RESPAWN_TIMEOUT_MAX = 1.5f;

        public static readonly float[] HERO_COLLISION_SUB_RECTS = 
        {
            0.202381f, 0.000000f, 0.296296f, 0.428571f,
            0.250000f, 0.297619f, 0.666667f, 0.619048f,
            0.000000f, 0.297619f, 0.157407f, 0.130952f,
            0.888889f, 0.654762f, 0.111111f, 0.142857f,
            0.537037f, 0.880952f, 0.240741f, 0.119048f,
        };

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
