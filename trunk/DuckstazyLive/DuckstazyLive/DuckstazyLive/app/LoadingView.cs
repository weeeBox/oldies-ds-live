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
        private Text progressText;

        public LoadingView(StartupController controller)
        {
            this.controller = controller;
            linesColor = utils.makeColor(0xd7d7d7);
        }

        public override void update(float delta)
        {
            SpriteTexture line = Application.sharedResourceMgr.getTexture(Res.IMG_LOAD_LINE);

            offset += delta * 24;
            if (offset > line.Height)
                offset -= line.Height;

            if (progressText != null)
            {
                progressText.setString("" + controller.getPercentLoaded());
            }
            else if (Application.sharedResourceMgr.isResourceLoaded(Res.FNT_BIG))
            {
                Font font = Application.sharedResourceMgr.getFont(Res.FNT_BIG);
                BaseElementContainer container = new BaseElementContainer(0, 0);

                Text loadingText = new Text(font);
                loadingText.setString("LOADING");                

                progressText = new Text(font);
                progressText.setString("100");
                progressText.setAlign(TextAlign.HCENTER | TextAlign.TOP);                

                Text percentText = new Text(font);
                percentText.setString("%...");                

                container.addChild(loadingText);
                container.addChild(progressText);
                container.addChild(percentText);

                container.arrangeHorizontally(0, 0);
                container.resizeToFitItems();

                progressText.x += 0.5f * progressText.width;

                addChild(container);
                attachHor(container, AttachStyle.CENTER);
                container.y = Constants.TITLE_SAFE_BOTTOM_Y - container.height;                
            }
        }

        public override void draw()
        {
            preDraw();

            // back
            AppGraphics.FillRect(0, 0, Constants.SCREEN_WIDTH, Constants.SCREEN_HEIGHT, Color.White);

            // lines
            AppGraphics.SetColor(linesColor);

            SpriteTexture line = Application.sharedResourceMgr.getTexture(Res.IMG_LOAD_LINE);

            float x = Constants.SAFE_OFFSET_X;
            float y = -line.Height + offset;
            while (y < Constants.SCREEN_HEIGHT)
            {
                line.draw(x, y);
                y += line.Height;
            }

            AppGraphics.SetColor(Color.White);

            postDraw();
        }
    }
}
