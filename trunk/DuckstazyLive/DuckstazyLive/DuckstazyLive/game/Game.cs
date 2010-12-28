﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.core;
using Microsoft.Xna.Framework.Input;
using DuckstazyLive.app;
using Framework.visual;
using Microsoft.Xna.Framework;
using Framework.utils;
using System.Diagnostics;

namespace DuckstazyLive.game
{
    public class Game : GameView
    {
        public static Game instance;
        private GameMode gameMode;

        // Состояние текущее и сохранение состояния перед уровнем
        public GameState gameState;
        public GameState gameSave;

        // Уровень.
        public Level level;

        public Game(GameController controller) : base(controller)
        {
            instance = this;          

            // Игровые состояния
            gameState = new GameState();
            gameSave = new GameState();            
        }

        //////////////////////////////////////////////////////////////////////////////////////////////
        // game
        //////////////////////////////////////////////////////////////////////////////////////////////

        public void newGame(GameMode mode)
        {
            this.gameMode = mode;

            gameSave.reset();
            level = createLevel(mode);            
            startLevel();
        }

        private Level createLevel(GameMode mode)
        {
            switch (mode)
            {
                case GameMode.SINGLE:
                    return new SingleLevel(gameState);

                case GameMode.COOP:
                    return new CoopLevel(gameState);

                case GameMode.VERSUS:
                    throw new NotImplementedException();
            }

            Debug.Assert(false, "Wrong mode: " + mode);
            return null;
        }

        public void startLevel()
        {            
            gameState.assign(gameSave);            
            level.start();            
        }

        public void restartLevel()
        {
            level.restart();
        }

        public void nextLevel()
        {
            // level.nextLevel();
            throw new NotImplementedException();
        }

        public void win()
        {         
            throw new NotImplementedException();
        }

        public void loose(string message)
        {
            getController().showLooseScreen(message);
        }

        public void death()
        {            
            getController().showDeathView();
        }

        public void pause()
        {
            level.onPause();
            getController().showPause();            
        }        

        public void save()
        {
            gameSave.assign(gameState);
        }

        //////////////////////////////////////////////////////////////////////////////////////////////
        // Life cicle
        //////////////////////////////////////////////////////////////////////////////////////////////

        public override void update(float dt)
        {        	
        	Env env = level.env;			
			level.update(dt);			
			// gui.update(dt);
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
