using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.core;
using Framework.visual;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace DuckstazyLive.app
{
    public class MenuController : ViewController
    {
        const int VIEW_MENU = 0;       

        public MenuController(ViewController p) : base(p)
        {
            MenuView view = new MenuView();
            
            addViewWithId(view, VIEW_MENU);            
        }

        private Image createButtonImage(int strokeId, float rotationDelay)
        {
            // button rotating part
            Image baseImage = new Image(Application.sharedResourceMgr.getTexture(Res.IMG_BUTTON_BASE));
            baseImage.toParentCenter();

            // rotating animation
            baseImage.turnTimelineSupportWithMaxKeyFrames(1);
            BaseElement.KeyFrame frame1 = new BaseElement.KeyFrame(baseImage.x, baseImage.y, Color.White, 1.0f, 1.0f, 360.0f, rotationDelay);
            baseImage.addKeyFrame(frame1);
            baseImage.setTimelineLoopType(BaseElement.Timeline.REPLAY);
            baseImage.playTimeline();

            // button stroke part            
            Image buttonImage = new Image(Application.sharedResourceMgr.getTexture((int)strokeId));
            buttonImage.addChild(baseImage);            

            return buttonImage;
        }

        public override void activate()
        {
            base.activate();

            MenuView view = (MenuView)getView(VIEW_MENU);
            showView(VIEW_MENU);            
        }
    }
}
