using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.core;
using Microsoft.Xna.Framework;
using DuckstazyLive.game.levels;

namespace DuckstazyLive.app
{
    class DuckstazyRootController : RootController
    {        
        public const int CHILD_START = 0;
        public const int CHILD_MENU = 1;
        public const int CHILD_STORY = 2;
        public const int CHILD_VERSUS = 3;

        private ControllerWarning controllerWarning;

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

            controllerWarning = new ControllerWarning();
        }

        public override void processDraw()
        {            
            base.processDraw();
            if (isControllerWarningActive())
                controllerWarning.draw();
        }

        public override void processUpdate()
        {
            if (!isControllerWarningActive())
                base.processUpdate();
        }

        public override void onChildDeactivated(int n)
        {
            base.onChildDeactivated(n);
            
            switch (n)
            {
                case CHILD_START:
                    GameMgr.createInstance();

                    MenuController menu = new MenuController(this);
                    addChildWithId(menu, CHILD_MENU);

                    StoryGameController storyController = new StoryGameController(this);
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

        public override bool buttonPressed(ref ButtonEvent e)
        {
            if (isControllerWarningActive())
                return controllerWarning.buttonPressed(ref e);

            return base.buttonPressed(ref e);
        }

        public override bool buttonReleased(ref ButtonEvent e)
        {
            if (isControllerWarningActive())
                return false;

            return base.buttonReleased(ref e);
        }

        public override void controllerConnected(int playerIndex)
        {
            if (playerIndex == 0)
            {
                controllerWarning.controllerConnected();
            }

            base.controllerConnected(playerIndex);
        }

        public override void controllerDisconnected(int playerIndex)
        {
            if (playerIndex == 0)
            {
                controllerWarning.start();
            }

            base.controllerDisconnected(playerIndex);
        }

        private bool isControllerWarningActive()
        {
            return controllerWarning.isShowed();
        }
    }
}
