using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.core;
using Microsoft.Xna.Framework;

namespace DuckstazyLive.app
{
    class DuckstazyRootController : RootController
    {        
        public const int CHILD_START = 0;
        public const int CHILD_MENU = 1;
        public const int CHILD_GAME = 2;        

        public DuckstazyRootController(ViewController p)
            : base(p)
        {
            StartupController startupController = new StartupController(this);
            addChildWithId(startupController, CHILD_START);

            viewTransition = Transition.TRANSITION_NONE;             
        }

        public override void activate()
        {
            base.activate();
            activateChild(CHILD_START);
        }        

        public override void processDraw()
        {
            base.processDraw();

#if WINDOWS
            float safeWidth = 0.8f * Constants.SCREEN_WIDTH;
            float safeHeight = 0.8f * Constants.SCREEN_HEIGHT;
            float safeX = 0.5f * (Constants.SCREEN_WIDTH - safeWidth);
            float safeY = 0.5f * (Constants.SCREEN_HEIGHT - safeHeight);
            // AppGraphics.DrawRect(safeX, safeY, safeWidth, safeHeight, Color.Red);
#endif
        }

        public override void onChildDeactivated(int n)
        {
            base.onChildDeactivated(n);
            
            switch (n)
            {
                case CHILD_START:
                    MenuController menu = new MenuController(this);
                    addChildWithId(menu, CHILD_MENU);

                    GameController gameController = new GameController(this);
                    addChildWithId(gameController, CHILD_GAME);

                    activateChild(CHILD_MENU);
                    break;

                case CHILD_MENU:
                    viewTransition = Transition.TRANSITION_NONE;
                    activateChild(CHILD_GAME);
                    break;

                case CHILD_GAME:
                    // re-activate game
                    activateChild(CHILD_MENU);                    
                    break;
            }
        }        
    }
}
