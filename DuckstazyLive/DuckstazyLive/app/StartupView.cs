using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.core;
using Framework.visual;
using DuckstazyLive.game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DuckstazyLive.app
{
    public class StartupView : View
    {
        private CustomGeomerty back;
        private Color linesColor;
        private float offset;

        public StartupView()
        {
            back = GeometryFactory.createSolidRect(0, 0, Constants.SCREEN_WIDTH, Constants.SCREEN_HEIGHT, utils.makeColor(0x555555));
            linesColor = utils.makeColor(0x333333);            
        }

        public override void update(float delta)
        {
            Texture2D line = Application.sharedResourceMgr.getTexture(Res.IMG_LOAD_LINE);

            offset += delta * 24;
            if (offset > line.Height)
                offset -= line.Height;
        }

        public override void draw()
        {
            // back
            AppGraphics.DrawGeomerty(back);

            // lines
            AppGraphics.SetColor(linesColor);

            Texture2D line = Application.sharedResourceMgr.getTexture(Res.IMG_LOAD_LINE);

            float x = Constants.SAFE_OFFSET_X;
            float y = -line.Height + offset;
            while (y < Constants.SCREEN_HEIGHT)
            {                
                AppGraphics.DrawImage(line, x, y);                
                y += line.Height;
            }

            AppGraphics.SetColor(Color.White);
        }
    }
}
