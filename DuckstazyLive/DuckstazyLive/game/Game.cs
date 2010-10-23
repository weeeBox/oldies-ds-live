﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuckstazyLive.game
{
    public class Game
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

        //private Menu menu;

        //// Вывод
        //private BitmapData canvas;
        ////private Boolean flipped;

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
        public Boolean hires;
        public Boolean mute;
        public Boolean inGame;

        public int maxScores;
        public Boolean maxScoresFinish;
        public int lastScores;
        public Boolean lastScoresFinish;

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
            level.sndStart.play();
        }

        public void frame(Number dt)
        {
        	//Number dt = device.update();
        	Env env = level.env;
						
			switch(state)
			{
			case MENU:
				env.update(dt, 0.0);
				level.progress.update(dt, 0.0);
				break;
			case LEVEL:
				level.update(dt);
				break;
			}
			
			gui.update(dt);
			env.updateBlanc(dt);
			
			
			//**RENDER**//
			//backBitmap.visible = false;
			//canvas.lock();
			
			if(gui.current!=scoresTable)
			{
				switch(state)
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
			}
			
			// gui.draw(canvas);
			
			if(env.blanc>0.0)
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

        public void updateFPS(int fps)
        {
            //if (debugInfoTgl)
            //    debugInfoText.text = fps.toString();
        }

        public void keyDown(KeyboardEvent e)
        {
            uint code = e.keyCode;

            if (code == 0x70)
                debugInfoTgl = !debugInfoTgl;
            else
            {
                if (state == LEVEL)
                    level.keyDown(code);
                if (gui.current == descScreen && (code == 0x1B || code == 0x0D))
                    descScreen.back();
            }
        }

        public void keyUp(KeyboardEvent e)
        {
            uint code = e.keyCode;

            if (state == LEVEL)
                level.keyUp(code);
        }

        public void changeRes()
        {
            hires = !hires;
            if (hires)
                device.quality = 0;
            else
                device.quality = 2;

            mainMenu.refreshRes(this);
            level.env.blanc = 1.0;
        }

        public void changeMute()
        {
            mute = !mute;
            if (mute)
                SoundMixer.soundTransform = new SoundTransform(0.0);
            else
                SoundMixer.soundTransform = new SoundTransform(1.0);
            mainMenu.refreshVol(this);
        }

        public void clickNewGame()
        {
            if (inGame)
            {
                level.setPause(false);
                levelMenu.go(gui);
            }
            else
            {
                newGame();
            }
        }

        public void goPause()
        {
            level.setPause(true);
            mainMenu.go();
            mainMenu.refreshInGame(this);
        }

        public void goHelp()
        {
            descScreen.go(0);
        }

        public void goNextLevel()
        {
            level.nextLevel();
        }

        public void goCredits()
        {
            if (inGame)
            {
                updateResults();
                state = MENU;
                inGame = false;
                if (gui.current != mainMenu)
                    gui.current = mainMenu;
                mainMenu.refreshInGame(this);
                level.env.blanc = 1.0;
                level.progress.end();
            }
            else descScreen.go(1);
        }

        public void updateResults()
        {
            Boolean finish = false;
            if (level.stage != null)
            {
                finish = gameState.level >= level.stagesCount - 1 && level.stage.win;
            }
            if (gameState.scores >= maxScores)
            {
                if (gameState.scores == maxScores && finish)
                    maxScoresFinish = true;
                else
                    maxScoresFinish = finish;

                maxScores = gameState.scores;
            }            
        }
    }
}
