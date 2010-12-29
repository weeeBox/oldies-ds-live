using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckstazyLive.app;
using Framework.core;

namespace DuckstazyLive.game
{
    public class VersusGame : VersusView
    {
        // Уровень.
        public VersusLevel level;

        public VersusGame(VersusController controller) : base(controller)
        {
            level = new VersusLevel(this);
        }

        //////////////////////////////////////////////////////////////////////////////////////////////
        // game
        //////////////////////////////////////////////////////////////////////////////////////////////

        public void newGame(int levelIndex)
        {
            level.state.level = levelIndex;
            level.start();
        }        

        public void showDraw()
        {
            getController().showDraw();
        }

        public void showWinner(int playerIndex)
        {
            getController().showWinner(playerIndex);
        }

        public void restartLevel()
        {
            level.restart();
        }        

        public void pause()
        {
            level.onPause();
            getController().showPause();            
        }        

        //////////////////////////////////////////////////////////////////////////////////////////////
        // Life cicle
        //////////////////////////////////////////////////////////////////////////////////////////////

        public override void update(float dt)
        {        	
        	Env env = level.env;			
			level.update(dt);		
			env.updateBlanc(dt);            
        }

        public override void draw()
        {
            Canvas canvas = getCanvas();

            Env env = level.env;            
            level.draw(canvas);            

            if (env.blanc > 0.0f)
                env.drawBlanc(canvas);
        }

        //////////////////////////////////////////////////////////////////////////////////////////////
        // Events
        //////////////////////////////////////////////////////////////////////////////////////////////

        public override bool buttonPressed(ref ButtonEvent e)
        {
            if (e.isKeyboardEvent())
            {
                InputManager im = Application.sharedInputMgr;
                for (int playerIndex = 0; playerIndex < im.getPlayersCount(); ++playerIndex)
                {
                    if (im.hasMappedButton(e.key, playerIndex))
                    {
                        ButtonEvent newEvent = im.makeButtonEvent(playerIndex, im.getMappedButton(e.key, playerIndex));
                        return level.buttonPressed(ref newEvent);
                    }
                }
            }

            return level.buttonPressed(ref e);            
        }

        public override bool buttonReleased(ref ButtonEvent e)
        {
            if (e.isKeyboardEvent())
            {
                InputManager im = Application.sharedInputMgr;
                for (int playerIndex = 0; playerIndex < im.getPlayersCount(); ++playerIndex)
                {
                    if (im.hasMappedButton(e.key, playerIndex))
                    {
                        ButtonEvent newEvent = im.makeButtonEvent(playerIndex, im.getMappedButton(e.key, playerIndex));
                        return level.buttonReleased(ref newEvent);
                    }
                }
            }

            return level.buttonReleased(ref e);            
        }        
    }
}
