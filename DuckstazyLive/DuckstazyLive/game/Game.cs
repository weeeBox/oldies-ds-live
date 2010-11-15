﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.core;
using Microsoft.Xna.Framework.Input;
using DuckstazyLive.app;
using Framework.visual;
using Microsoft.Xna.Framework;

namespace DuckstazyLive.game
{
    public class Game : BaseElement
    {
        public const int MENU = 0;
        public const int LEVEL = 1;
        public const int LOOSE = 2;

        public static Game instance;

        private int state;

        // Состояние текущее и сохранение состояния перед уровнем
        public GameState gameState;
        public GameState gameSave;

        // Уровень.
        public Level level;

        private Canvas canvas;

        private DeathView deathView;
        
        public bool mute;
        public bool inGame;

        public int maxScores;
        public bool maxScoresFinish;
        public int lastScores;
        public bool lastScoresFinish;

        public Game()
        {
            instance = this;
            // Игровые состояния
            gameState = new GameState();
            gameSave = new GameState();

            canvas = new Canvas(FrameworkConstants.SCREEN_WIDTH, FrameworkConstants.SCREEN_HEIGHT);

            // Уровень            
            // level = new SingleLevel(gameState);            
            level = new MultiplayerLevel(gameState);

            state = MENU;
            inGame = false;
            mute = false;

            maxScores = 0;
            maxScoresFinish = false;
            lastScores = 0;
            lastScoresFinish = false;

            level.env.blanc = 1;            
        }

        public override void update(float dt)
        {            
        	//Number dt = device.update();
        	Env env = level.env;            
						
			switch(state)
			{
			    case MENU:
				    env.update(dt, 0.0f);
				    level.progress.update(dt, 0.0f);
				    break;
			    case LEVEL:
				    level.update(dt);
				    break;                
                case LOOSE:
                    deathView.update(dt);
                    break;
			}
			
			// gui.update(dt);
			env.updateBlanc(dt);            
        }

        public override void draw()
        {            
            Env env = level.env;            
            switch (state)
            {
                case MENU:
                    env.draw1(canvas);
                    env.draw2(canvas);
                    level.progress.draw(canvas);
                    break;
                case LEVEL:
                    drawLevel();                    
                    break;
                case LOOSE:
                    deathView.draw(canvas);
                    break;
            }            

            if (env.blanc > 0.0f)
                env.drawBlanc(canvas);            
        }

        private void drawLevel()
        {            
            level.draw(canvas);            
        }        

        private CustomGeomerty geomTitleSafe;

        private void drawTitleSafe()
        {
            if (geomTitleSafe == null)
            {
                float w = Constants.TITLE_SAFE_X * Constants.SCREEN_WIDTH;
                float h = Constants.TITLE_SAFE_Y * Constants.SCREEN_HEIGHT;
                float x = 0.5f * (Constants.SCREEN_WIDTH - w);
                float y = 0.5f * (Constants.SCREEN_HEIGHT - h);

                geomTitleSafe = utils.createRect(x, y, w, h, Color.Red, false);
            }
            canvas.drawGeometry(geomTitleSafe);
        }

        private void setState(int newState)
        {
            state = newState;
        }

        private void newGame()
        {
            gameSave.reset();
            startLevel();
        }

        public void loose()
        {
            setState(LOOSE);
            deathView = new DeathView();
        }

        public void startLevel()
        {
            // levelMenu.go(gui);
            gameState.assign(gameSave);
            level.start();
            setState(LEVEL);
            inGame = true;
        }

        public void save()
        {
            gameSave.assign(gameState);
        }

        public void buttonPressed(ref ButtonEvent e)
        {
            if (state == LEVEL)
            {
                level.buttonPressed(ref e);
            }
            else if (state == LOOSE)
            {
                if (e.button == Buttons.A || e.button == Buttons.Start)
                {
                    deathView = null;
                    GC.Collect();
                    newGame();
                }
            }
        }

        public void buttonReleased(ref ButtonEvent e)
        {
            if (state == LEVEL)
                level.buttonReleased(ref e);
        }

        public void keyPressed(Keys key)
        {
            InputManager im = Application.sharedInputMgr;

            for (int playerIndex = 0; playerIndex < im.getPlayersCount(); ++playerIndex)
            {
                if (!im.isPlayerActive(playerIndex))
                    continue;

                if (!im.hasMappedButton(key, playerIndex))
                    continue;

                Buttons button = Application.sharedInputMgr.getMappedButton(key, playerIndex);
                ButtonEvent buttonEvent = im.makeButtonEvent(playerIndex, button);                
                buttonPressed(ref buttonEvent);
            }
        }

        public void keyReleased(Keys key)
        {
            InputManager im = Application.sharedInputMgr;

            for (int playerIndex = 0; playerIndex < im.getPlayersCount(); ++playerIndex)
            {
                if (!im.isPlayerActive(playerIndex))
                    continue;

                if (!im.hasMappedButton(key, playerIndex))
                    continue;

                Buttons button = Application.sharedInputMgr.getMappedButton(key, playerIndex);
                ButtonEvent buttonEvent = im.makeButtonEvent(playerIndex, button);                
                buttonReleased(ref buttonEvent);
            }
        }        

        public void changeMute()
        {
            //mute = !mute;
            //if (mute)
            //    SoundMixer.soundTransform = new SoundTransform(0.0f);
            //else
            //    SoundMixer.soundTransform = new SoundTransform(1.0f);
            //mainMenu.refreshVol(this);
            throw new NotImplementedException();
        }

        public void clickNewGame()
        {
            if (inGame)
            {
                level.setPause(false);
                // levelMenu.go(gui);
            }
            else
            {                
                newGame();
            }
        }

        public void goPause()
        {
            level.setPause(true);            
        }

        public void goNextLevel()
        {
            level.nextLevel();
        }

        public void goCredits()
        {
            //if (inGame)
            //{
            //    updateResults();
            //    state = MENU;
            //    inGame = false;
            //    if (gui.current != mainMenu)
            //        gui.current = mainMenu;
            //    mainMenu.refreshInGame(this);
            //    level.env.blanc = 1.0f;
            //    level.progress.end();
            //}
            //else descScreen.go(1);
            throw new NotImplementedException();
        }

        public void updateResults()
        {
            //bool finish = false;
            //if (level.stage != null)
            //{
            //    finish = gameState.level >= level.stagesCount - 1 && level.stage.win;
            //}
            //if (gameState.scores >= maxScores)
            //{
            //    if (gameState.scores == maxScores && finish)
            //        maxScoresFinish = true;
            //    else
            //        maxScoresFinish = finish;

            //    maxScores = gameState.scores;
            //}            
            throw new NotImplementedException();
        }
    }
}
