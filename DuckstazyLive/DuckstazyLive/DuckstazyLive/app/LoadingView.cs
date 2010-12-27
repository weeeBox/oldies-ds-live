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
    public class LoadingView : View
    {        
        private Color linesColor;
        private float offset;

        private StartupController controller;

        public LoadingView(StartupController controller)
        {
            this.controller = controller;
            linesColor = utils.makeColor(0xd7d7d7);            
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
            AppGraphics.FillRect(0, 0, Constants.SCREEN_WIDTH, Constants.SCREEN_HEIGHT, Color.White);

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

            Font font = Application.sharedResourceMgr.getFont(Res.FNT_BIG);
            if (font != null)
            {
                font.drawString("LOADING " + controller.getPercentLoaded() + "%...", 0.5f * (Constants.TITLE_SAFE_LEFT_X + Constants.TITLE_SAFE_RIGHT_X), Constants.TITLE_SAFE_BOTTOM_Y, TextAlign.HCENTER | TextAlign.BOTTOM);
            }
        }
    }
}
