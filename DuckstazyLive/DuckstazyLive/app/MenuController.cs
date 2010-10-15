using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.core;
using Framework.visual;
using Microsoft.Xna.Framework.Graphics;

namespace DuckstazyLive.app
{
    public class MenuController : ViewController
    {
        const int VIEW_MENU = 0;       

        public MenuController(ViewController p) : base(p)
        {
            MenuView view = new MenuView();

            Image buttonUpImage = createButtonImage(DuckstazyResource.IMG_BUTTON_STROKE_DEFAULT, 5.0f);            
            Image buttonDownImage = createButtonImage(DuckstazyResource.IMG_BUTTON_STROKE_FOCUSED, 2.5f);
            Image buttonFocusedImage = createButtonImage(DuckstazyResource.IMG_BUTTON_STROKE_FOCUSED, 2.5f);
            Button button = new Button(buttonUpImage, buttonDownImage, buttonFocusedImage, 0);
            
            view.addChildWithId(button, 0);
            addViewWithId(view, VIEW_MENU);
        }

        private Image createButtonImage(DuckstazyResource strokeId, float rotationDelay)
        {
            Image buttonImage = new Image(Application.sharedResourceMgr.getTexture((int)strokeId));
            Image baseImage = new Image(Application.sharedResourceMgr.getTexture((int)DuckstazyResource.IMG_BUTTON_BASE));

            buttonImage.addChild(baseImage);
            baseImage.parentAnchor = BaseElement.ANCHOR_CENTER;
            baseImage.anchor = BaseElement.ANCHOR_CENTER;

            baseImage.turnTimelineSupportWithMaxKeyFrames(1);
            BaseElement.KeyFrame frame1 = new BaseElement.KeyFrame(buttonImage.x, buttonImage.y, Color.White, 1.0f, 1.0f, 360.0f, rotationDelay);
            baseImage.addKeyFrame(frame1);
            baseImage.setTimelineLoopType(BaseElement.Timeline.REPLAY);
            baseImage.playTimeline();

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
