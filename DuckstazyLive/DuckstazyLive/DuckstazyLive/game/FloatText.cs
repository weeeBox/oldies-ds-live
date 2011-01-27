using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Framework.core;
using Framework.visual;
using DuckstazyLive.app;

namespace DuckstazyLive.game
{
    public class FloatText : BaseElement
    {
        public float t;
        public string text;
        public ColorTransform ct;

        public FloatText()
        {
            t = 0.0f;
            ct = ColorTransform.NONE;
            setAlign(ALIGN_CENTER, ALIGN_MIN);
        }

        public override void draw()
        {
            preDraw();

            Font font = Application.sharedResourceMgr.getFont(Res.FNT_FLOAT);
            font.drawString(text, utils.scale(drawX), utils.scale(drawY));

            postDraw();
        }
    }
}
