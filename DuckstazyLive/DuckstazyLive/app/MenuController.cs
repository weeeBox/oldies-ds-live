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

            CustomGeomerty sky = GeometryFactory.createGradient(Constants.WORLD_VIEW_X, Constants.WORLD_VIEW_Y, Constants.WORLD_VIEW_WIDTH, Constants.WORLD_VIEW_HEIGHT, new Color(63, 181, 242), new Color(217, 240, 254));
            CustomGeomerty ground = GeometryFactory.createGradient(Constants.GROUND_X, Constants.GROUND_Y, Constants.GROUND_WIDTH, Constants.GROUND_HEIGHT, new Color(55, 29, 0), new Color(92, 48, 11));
            Image grass = new Image(Application.sharedResourceMgr.getTexture((int)DuckstazyResource.IMG_GRASS));
            grass.setAlign(BaseElement.ALIGN_MIN, BaseElement.ALIGN_MAX);            
            view.addChild(sky);
            view.addChild(ground);
            ground.addChild(grass);

            Image titleBack = new Image(Application.sharedResourceMgr.getTexture((int)DuckstazyResource.IMG_MENU_TITLE_BACK));            
            Image title = new Image(Application.sharedResourceMgr.getTexture((int)DuckstazyResource.IMG_MENU_TITLE));
            titleBack.toParentCenter();
            title.toParentCenter();
            view.addChild(titleBack);
            view.addChild(title);            

            Image buttonUpImage = createButtonImage(DuckstazyResource.IMG_BUTTON_STROKE_DEFAULT, 5.0f);            
            Image buttonDownImage = createButtonImage(DuckstazyResource.IMG_BUTTON_STROKE_FOCUSED, 2.5f);
            Image buttonFocusedImage = createButtonImage(DuckstazyResource.IMG_BUTTON_STROKE_FOCUSED, 2.5f);
            Button button = new Button(buttonUpImage, buttonDownImage, buttonFocusedImage, 0);
            
            view.addChild(button);
            addViewWithId(view, VIEW_MENU);
        }

        private Image createButtonImage(DuckstazyResource strokeId, float rotationDelay)
        {
            // button rotating part
            Image baseImage = new Image(Application.sharedResourceMgr.getTexture((int)DuckstazyResource.IMG_BUTTON_BASE));
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

            // deactivate();
        }
    }
}
