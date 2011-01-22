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
            MenuView view = new MenuView(this);
            addViewWithId(view, VIEW_MENU);
        }

        public void newGame(GameMode mode)
        {
            StoryGameController storyController = (StoryGameController)Application.sharedRootController.getChild(DuckstazyRootController.CHILD_STORY);
            storyController.setGameMode(mode);
            deactivate();
            Application.sharedRootController.activateChild(DuckstazyRootController.CHILD_STORY);
        }

        public void versusGame()
        {            
            deactivate();
            Application.sharedRootController.activateChild(DuckstazyRootController.CHILD_VERSUS);

            VersusController versusController = (VersusController)Application.sharedRootController.getChild(DuckstazyRootController.CHILD_VERSUS);
            versusController.selectStage();
        }

        public override void activate()
        {
            base.activate();

            MenuView view = (MenuView)getView(VIEW_MENU);
            showView(VIEW_MENU);           
        }        
    }
}
