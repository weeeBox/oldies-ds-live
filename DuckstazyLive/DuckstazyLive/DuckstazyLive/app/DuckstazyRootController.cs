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
        public const int CHILD_STORY = 2;
        public const int CHILD_VERSUS = 3;

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

        public override void onChildDeactivated(int n)
        {
            base.onChildDeactivated(n);
            
            switch (n)
            {
                case CHILD_START:
                    MenuController menu = new MenuController(this);
                    addChildWithId(menu, CHILD_MENU);

                    StoryController storyController = new StoryController(this);
                    addChildWithId(storyController, CHILD_STORY);

                    VersusController versusController = new VersusController(this);
                    addChildWithId(versusController, CHILD_VERSUS);

                    activateChild(CHILD_MENU);
                    break;

                case CHILD_STORY:
                case CHILD_VERSUS:
                    // re-activate game
                    activateChild(CHILD_MENU);                    
                    break;
            }
        }        
    }
}
