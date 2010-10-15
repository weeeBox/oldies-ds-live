using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.core;

namespace DuckstazyLive.app
{
    class DuckstazyRootController : RootController
    {
        public enum Childs
        {
            CHILD_START,
            CHILD_MENU,
            CHILD_GAME
        }

        public DuckstazyRootController(ViewController p)
            : base(p)
        {
            StartupController startupController = new StartupController(this);
            addChildWithId(startupController, (int)Childs.CHILD_START);

            viewTransition = Transition.TRANSITION_NONE;            
        }

        public override void activate()
        {
            base.activate();

            activateChild((int)Childs.CHILD_START);
        }

        public override void onChildDeactivated(int n)
        {
            base.onChildDeactivated(n);

            Childs c = (Childs)n;
            switch (c)
            {
                case Childs.CHILD_START:
                    MenuController menu = new MenuController(this);
                    addChildWithId(menu, (int)Childs.CHILD_MENU);

                    GameController gameController = new GameController(this);
                    addChildWithId(gameController, (int)Childs.CHILD_GAME);

                    activateChild((int)Childs.CHILD_MENU);
                    break;

                case Childs.CHILD_MENU:
                    viewTransition = Transition.TRANSITION_NONE;
                    activateChild((int)Childs.CHILD_GAME);
                    break;

                case Childs.CHILD_GAME:
                    // re-activate game
                    activateChild((int)Childs.CHILD_MENU);                    
                    break;
            }
        }        
    }
}
