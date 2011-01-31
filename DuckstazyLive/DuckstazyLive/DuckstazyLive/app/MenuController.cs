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
        }

        public void newGame(StoryGameMode mode)
        {
            StoryController storyController = (StoryController)Application.sharedRootController.getChild(DuckstazyRootController.CHILD_STORY);
            storyController.setGameMode(mode);
            deactivate();
            Application.sharedRootController.activateChild(DuckstazyRootController.CHILD_STORY);
        }

        public void versusGame()
        {
            VersusController versusController = (VersusController)Application.sharedRootController.getChild(DuckstazyRootController.CHILD_VERSUS);
            versusController.selectStage();
            deactivate();
            Application.sharedRootController.activateChild(DuckstazyRootController.CHILD_VERSUS);            
        }

        public override void activate()
        {
            base.activate();

            MenuView menuView = new MenuView(this);
            showView(menuView);
        }        
    }
}
