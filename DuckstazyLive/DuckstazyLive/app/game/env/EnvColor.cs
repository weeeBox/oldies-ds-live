using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuckstazyLive.app.game.env
{
    public class EnvColor
    {
        public uint bg;
        public uint text;

        public EnvColor(uint sky, uint floatText)
        {
            bg = sky;
            text = floatText;
        }

        public void lerp(float x, EnvColor c1, EnvColor c2)
        {
            bg = ColorUtils.lerpColor(c1.bg, c2.bg, x);
            text = ColorUtils.lerpColor(c1.text, c2.text, x);
        }

    }
}
