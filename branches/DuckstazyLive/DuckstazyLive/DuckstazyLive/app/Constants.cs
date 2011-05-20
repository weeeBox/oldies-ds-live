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
        public const float SCALE = 1.0f;
        public const float SCALE_INV = 1.0f / SCALE;       

        public const float TITLE_SAFE_SCALE_X = 0.8f;
        public const float TITLE_SAFE_SCALE_Y = 0.8f;

        public const float TITLE_SAFE_LEFT_X = 0.5f * (1 - TITLE_SAFE_SCALE_X) * SCREEN_WIDTH;
        public const float TITLE_SAFE_TOP_Y = 0.5f * (1 - TITLE_SAFE_SCALE_Y) * SCREEN_HEIGHT;
        public const float TITLE_SAFE_RIGHT_X = SCREEN_WIDTH - TITLE_SAFE_LEFT_X;
        public const float TITLE_SAFE_BOTTOM_Y = SCREEN_HEIGHT - TITLE_SAFE_TOP_Y;

        public const float TITLE_SAFE_AREA_WIDTH = TITLE_SAFE_RIGHT_X - TITLE_SAFE_LEFT_X;
        public const float TITLE_SAFE_AREA_HEIGHT = TITLE_SAFE_BOTTOM_Y - TITLE_SAFE_TOP_Y;

        public const float SCREEN_WIDTH_UNSCALE = SCREEN_WIDTH * SCALE_INV;
        public const float SCREEN_HEIGHT_UNSCALE = SCREEN_HEIGHT * SCALE_INV;

        public const float SAFE_OFFSET_X = 0.5f * (SCREEN_WIDTH - 640.0f * SCALE);
        public const float SAFE_OFFSET_Y = 0.5f * (1 - TITLE_SAFE_SCALE_Y) * 480.0f * SCALE;

        public const float SAFE_OFFSET_X_UNSCALE = SAFE_OFFSET_X * SCALE_INV;
        public const float SAFE_OFFSET_Y_UNSCALE = SAFE_OFFSET_Y * SCALE_INV;

        public const float ENV_WIDTH = SCREEN_WIDTH;
        public const float ENV_HEIGHT = SAFE_OFFSET_Y + 400.0f * SCALE;

        public const float ENV_WIDTH_UNSCALE = ENV_WIDTH * SCALE_INV;
        public const float ENV_HEIGHT_UNSCALE = ENV_HEIGHT * SCALE_INV;

        public const float GROUND_WIDTH = SCREEN_WIDTH;
        public const float GROUND_HEIGHT = SCREEN_HEIGHT - ENV_HEIGHT;

        public const float GROUND_WIDTH_UNSCALE = GROUND_WIDTH * SCALE_INV;
        public const float GROUND_HEIGHT_UNSCALE = GROUND_HEIGHT * SCALE_INV;
    }
}
