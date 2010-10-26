﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.core;
using Microsoft.Xna.Framework.Input;

namespace DuckstazyLive.game
{
    public class Game : InputListener
    {
        public const int MENU = 0;
        public const int LEVEL = 1;

        public static Game instance;

        private int state;

        // Состояние текущее и сохранение состояния перед уровнем
        public GameState gameState;
        public GameState gameSave;

        // Уровень.
        public Level level;

        private Canvas canvas = new Canvas();

        //private Menu menu;

        //// Вывод
        //private BitmapData canvas;
        ////private bool flipped;

        //public BitmapData back;
        //public Bitmap backBitmap;

        //public BitmapData imgBG;

        //private BitmapData front;
        //public Bitmap frontBitmap;

        // Ресурсы
        // private Resources resourceManager;

        // ГУИ
        //public SUISystem gui;
        //public UIMedia uiMedia;
        //public GameMenu mainMenu;
        //public LevelMenu levelMenu;
        //public DescScreen descScreen;
        //private UpgradeMenu shopMenu;
        //public ScoresTable scoresTable;
        public bool hires;
        public bool mute;
        public bool inGame;

        public int maxScores;
        public bool maxScoresFinish;
        public int lastScores;
        public bool lastScoresFinish;

        // public Stage stage;

        // private ekDevice device;

        public Game()
        {
            instance = this;
            // device = ekDevice.instance;
            // device.callbackFPS = updateFPS;
            // device.listener = this;

            // stage = device.stage;
            // stage.frameRate = 75;
            //device.quality = 0;

            // Таймер всему голова, вначале его создаем
            //timer = new GameTimer();

            // Игровые состояния
            gameState = new GameState();
            gameSave = new GameState();

            // Грузим и кешируем ресурсы
            // resourceManager = new Resources();


            // Поверхности для вывода
            //back = new BitmapData(640, 480, false, 0);
            //backBitmap = new Bitmap(back);//, PixelSnapping.NEVER, false);
            //canvas = back;
            //imgBG = new BitmapData(640, 480, false);

            // Уровень
            level = new Level(gameState);

            // debugInfoTgl = true;

            state = MENU;
            inGame = false;
            mute = false;
            hires = true;

            maxScores = 0;
            maxScoresFinish = false;
            lastScores = 0;
            lastScoresFinish = false;

            // stage.addChildAt(backBitmap as DisplayObject, 0);

            //debugInfoText = new TextField();
            //debugInfoText.defaultTextFormat = new TextFormat("_mini", 15, 0xffffff);
            //debugInfoText.embedFonts = true;
            //debugInfoText.cacheAsBitmap = true;

            // ГУИ
            //gui = new SUISystem();
            //level.gui = gui;
            //gui.listen(stage);

            //uiMedia = new UIMedia();

            //mainMenu = new GameMenu(this, gui);
            //levelMenu = new LevelMenu(this);
            //descScreen = new DescScreen(gui);
            //shopMenu = new UpgradeMenu(gui, level);
            //mainMenu.shop = shopMenu;

            //level.initShopMenu(shopMenu);
            //level.levelMenu = levelMenu;

            //scoresTable = new ScoresTable();

            //mainMenu.go();

            level.env.blanc = 1;            
        }

        public void update(float dt)
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
			}
			
			// gui.update(dt);
			env.updateBlanc(dt);			
        }

        public void draw()
        {            
            Env env = level.env;

            //**RENDER**//
            //backBitmap.visible = false;
            //canvas.lock();

            //if(gui.current!=scoresTable)
            //{
            switch (state)
            {
                case MENU:
                    env.draw1(canvas);
                    env.draw2(canvas);
                    level.progress.draw(canvas);
                    break;
                case LEVEL:
                    level.draw(canvas);
                    break;
            }
            //}

            // gui.draw(canvas);

            if (env.blanc > 0.0f)
                env.drawBlanc(canvas);

            //if(debugInfoTgl)
            //    canvas.draw(debugInfoText);

            //canvas.unlock();
            //backBitmap.visible = true;
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

        public void buttonPressed(ButtonEvent e)
        {

        }

        public void buttonReleased(ButtonEvent e)
        {

        }

        public void keyPressed(Keys key)
        {           
            if (state == LEVEL)
                level.keyDown(key);             
        }

        public void keyReleased(Keys key)
        {
            if (state == LEVEL)
                level.keyUp(key);
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
