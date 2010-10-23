﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuckstazyLive.game
{
    public class Level
	{
		public static Level instance;
		
		private const String HARVEST_TEXTString = "HARVESTING";
		private const String NEXT_LEVEL_TEXT_BEGIN = "WARP IN ";
		private const String NEXT_LEVEL_TEXT_END = " SEC...";
		 
        //[Embed(source="gfx/hp1.png")]
        //private var rHPImg1:Class;
        
        //[Embed(source="gfx/hp2.png")]
        //private var rHPImg2:Class;
        
        //[Embed(source="gfx/hp3.png")]
        //private var rHPImg3:Class;
        
        //[Embed(source="gfx/score.png")]
        //private var rScoreImg:Class;
        
        //[Embed(source="sfx/start.mp3")]
        //private var rStartSnd:Class;
           
        //public var imgHP1:BitmapData;
        //private var imgHP2:BitmapData;
        //public var imgHP3:BitmapData;
        //private var imgScore:BitmapData;
        private float hpPulse;
        private float hpCounter;
        //private var hpText:TextField;
        //private var scoreText:TextField;
		private int scoreOld;
		private float scoreCounter;
		
		//public var infoText:TextField;
		
		//public var sndStart:Sound;
        
		public Game game;
		public Hero hero;
		public Pills pills;
		public Env env;
		private Particles ps;
		
		public LevelProgress progress;
		
		public float power;
		private float powerUp;
		
		// Состояние уровня
		public GameState state;
		
		// Конец Уровня
		public bool finish;
		private float finishCounter;
		
		
		private Array stages; // Уровни
		public LevelStage stage; // текущий уровень
		public StageMedia stageMedia;
		public int stagesCount;
		
		// магазин
		//private var shop:Shop;
		//public var upgradeMenu:UpgradeMenu;
		
		// инфа
		public GameInfo info;
		
		// мен.
		//public var gui:SUISystem;
		//public var levelMenu:LevelMenu;
		public bool pause;
		//public var imgPause:BitmapData;
		
		private float nextLevelCounter;
		private int harvestProcess;
		private int nextLevelCountdown;
		
		public function Level(gameState:GameState)
		{
			//var tf:TextFormat = new TextFormat("_default", 28, 0xffffffff);
			//var shadow:DropShadowFilter = new DropShadowFilter(0.0, 0.0, 0, 1, 4, 4, 4, 1);
			
			instance = this;
			
			//var o:*;
			
			state = gameState;
			
			info = new GameInfo();
			ps = new Particles();
			env = new Env();
			hero = new Hero();
			pills = new Pills(hero, ps, this);
			progress = new LevelProgress();
			
			hero.particles = ps;
			hero.state = state;
			hero.env = env;		
			progress.env = env;
					
			hero.init();
			
			stageMedia = new StageMedia();
            //stages = new Array();
            //stages.push([Harvesting, null]);
            //stages.push([PartyTime, [30, 0]]);
            //stages.push([Bubbles, [0.05, 0]]);
            //stages.push([DoubleFrog, null]);
            //stages.push([PartyTime, [60, 1]]);
            //stages.push([BetweenCatsStage, null]);
            //stages.push([Bubbles, [0.04, 1]]);
            //stages.push([AirAttack, null]);
            //stages.push([PartyTime, [120, 2]]);
            //stages.push([Trains, null]);
            //stages.push([Bubbles, [0.03, 2]]);			
			
			stagesCount = stages.length;

			stage = null;
			
			finish = false;
			
            //imgHP1 = (new rHPImg1()).bitmapData;
            //imgHP2 = (new rHPImg2()).bitmapData;
            //imgHP3 = (new rHPImg3()).bitmapData;
			
            //imgScore = (new rScoreImg()).bitmapData;
			
            //sndStart = new rStartSnd();
			
			hpCounter = 0.0;
			hpPulse = 0.0;
			
            //hpText = new TextField();
            //hpText.defaultTextFormat = tf;
            //hpText.embedFonts = true;
            //hpText.cacheAsBitmap = true;
            //hpText.autoSize = TextFieldAutoSize.LEFT;
            //hpText.filters = [shadow];
		
            //scoreText = new TextField();
            //scoreText.defaultTextFormat = tf;
            //scoreText.embedFonts = true;
            //scoreText.cacheAsBitmap = true;
            //scoreText.autoSize = TextFieldAutoSize.LEFT;
            //scoreText.filters = [shadow];
			
            //infoText = new TextField();
            //infoText.defaultTextFormat = tf;
            //infoText.embedFonts = true;
            //infoText.cacheAsBitmap = true;
            //infoText.autoSize = TextFieldAutoSize.LEFT;
            //infoText.filters = [shadow];	
			
			scoreOld = 0;
			scoreCounter = 0.0;
			
			game = Game.instance;
			//imgPause = game.imgBG;
		}
		
		public void start()
		{
			Class stageClass = stages[state.level][0];
			Array stageParams = stages[state.level][1];
			
			env.blanc = 1.0;
			power = 0.0;
			powerUp = 0.0;
			
			if(stageParams!=null)
				stage = new stageClass(stageParams);
			else
				stage = new stageClass();
			
			//power = 0.0;
			//powerUp = 0.0;
			ps.clear();
			pills.clear();
			info.reset();

			progress.start(stage.goalTime);
			hero.init();
			game.save();
			
			finish = false;
			pause = false;
			
			state.health = state.maxHP;
			syncScores();
			enterLevel();
		}
		
		public void drawUI(BitmapData canvas)
		{
            //var mat:Matrix = new Matrix(1.0, 0.0, 0.0, 1.0, -25.0, -23.0);
            //var col:ColorTransform = new ColorTransform(1.0, 1.0, 1.0, power);
            //var sc:float = 1.0 + 0.3*hpPulse;
            //var str:String;
			
            //mat.scale(sc, sc);
            //mat.translate(22.0, 410+18);//463.0);
            //canvas.draw(imgHP1, mat, null, null, null, true);
            //canvas.draw(imgHP2, mat, col, null, null, true);
            //canvas.draw(imgHP3, mat, null, null, null, true);
			
            //mat.identity();
            //mat.tx = -24.0;
            //mat.ty = -24.0;
            //sc = 1.0+0.3*scoreCounter;
            //mat.scale(sc, sc);
            //mat.translate(620.0, 410+18);//463.0);
            //canvas.draw(imgScore, mat, null, null, null, true);
			
            //mat.identity();
			
            //mat.tx = 40.0;
            //mat.ty = 410;//445.0;
            //str = state.health.toString()+"/"+state.maxHP.toString();
            //if(hpText.text != str) hpText.text = str;
            //canvas.draw(hpText, mat);
			
            //mat.tx = 600.0 - scoreText.textWidth;
            //canvas.draw(scoreText, mat);
		}
		
		public void draw(BitmapData canvas)
		{
			if(pause)
			{
				canvas.draw(imgPause);
			}
			else
			{
				env.draw1(canvas);
				
				//if(!room)
				stage.draw1(canvas);
				
				info.drawFT(canvas);
				pills.draw(canvas);
				
				if(state.health>0)
					hero.draw(canvas);
				
				ps.draw(canvas);
				env.draw2(canvas);
				progress.draw(canvas);
				drawUI(canvas);
				stage.draw2(canvas);
			}
		}
		
		public void enterLevel()
		{
			gui.setCurrent(levelMenu);
			levelMenu.setState(0);
			env.blanc = 1.0;

			stage.start();
			sndStart.play();
		}
		
		public void update(float dt)
		{
			float power_drain = 0.0;
			int i;
				
			if(!pause)
			{			
				if(stage!=null)
				{
					stage.update(dt);
					if(stage.win && !finish)
					{
						winLevel();
					}
				}
				
				if(state.health<=0)
				{
					if(!finish)
					{
						finish = true;		
						state.health = 0;
						env.blanc = 1.0;
						progress.play = false;
						
						levelMenu.setState(2);
					}
				}
				else
				{
					if(finish)
					{
						if(pills.harvestCount>0)
							updateHarvesting(dt);
						else
						{
							if(nextLevelCountdown>0)
							{
								nextLevelCounter+=dt;
								if(nextLevelCounter>1)
								{
									nextLevelCounter--;
									nextLevelCountdown--;
									infoText.text = NEXT_LEVEL_TEXT_BEGIN+
													nextLevelCountdown.toString()+
													NEXT_LEVEL_TEXT_END;
								}
							}
							else
								nextLevel();
						}
					}
				}
	
				if(hero.sleep) power_drain = 0.3;
				if(powerUp<power)
				{
					power-=dt*power_drain;
					if(power<0.0) power = 0.0;
				}
				else
				{
					power+=dt*0.05;
					if(power>powerUp) power = powerUp;
				}
				
				if(state.health>0) hero.update(dt, power);
				
				pills.update(dt, power);
				
				env.x = hero.x;
				env.y = hero.y;
				env.update(dt, power);
				
				progress.update(dt, power);
	
				ps.update(dt);
				
				if(hpPulse>0.0) { hpPulse-=4.0*dt; if(hpPulse<0.0) hpPulse = 0.0; }
				hpCounter+=4.0*dt;
				if(power<0.33) {
					if(hpCounter>4.0) { hpCounter-=4.0; hpPulse = 1.0; }
				}
				else if(power<0.66) {
					if(hpCounter>2.0) { hpCounter-=2.0; hpPulse = 1.0; }
				}
				else {
					if(hpCounter>1.0) {	hpCounter-=1.0; hpPulse = 1.0; }
				}
	
				if(power>=0.5) info.setRGB(env.colors.bg);
				else {
					if(env.day) info.setRGB(0x000000);
					else info.setRGB(0xffffff);
				}
				info.update(power, dt);
				
				if(state.scores>scoreOld)
				{
					scoreCounter+=30.0*dt;
					if(scoreCounter>1.0)
					{
						i = (state.scores-scoreOld)/5;
						if(i==0)
						{
							scoreOld = state.scores;
							scoreCounter = 0.0;
						}
						else
						{
							scoreOld+=i;
							scoreCounter -= int(scoreCounter);
						}
								
						scoreText.text = scoreOld.toString();
					}
				}
				// КОНЕЦ ОБНОВЛЕНИЯ УРОВНЯ
			}
		}

		public void gainPower(float gained)
		{
			powerUp+=gained;
			if(powerUp>1.0)
				powerUp = 1.0;
		}
		
		public void gainSleep()
		{
			powerUp = 0.0;
		}
		
		public void keyDown(uint code)
		{
			if(pause)
			{
				if(code ==0x1B)// ESC
				{
					if(gui.current==game.mainMenu)
						game.clickNewGame();
					else if(gui.current==upgradeMenu)
						closeUpgradeMenu();
				}
				else if(code==0x0D && gui.current==upgradeMenu && upgradeMenu.btnBuy.enabled)
				{
					upgradeMenu.buy();
				}
			}
			else
			{
				if(finish)
				{
					if(state.health>0)
					{
						//if(code==0x0D || code==0x1B) // ENTER or ESC
							//nextLevel();
						//else
						hero.keyDown(code);
					}
					else
					{
						if(code==0x0D) // ENTER
							game.startLevel();
						else if(code==0x1B)// ESC
							game.goPause();
						else if(code==0x20 && levelMenu.btnSubmit.enabled)// SPACE
							levelMenu.submit();
						
					}
				}
				else
				{
					hero.keyDown(code);
					if(code==0x1B)// ESC
						game.goPause();
					else if(code==0x0D)
						goUpgradeMenu();
					/*else if(code==0x57)
						nextLevel();
					else if(code==0x44)
						hero.doToxicDamage(320, 200, 20, 0);
					else if(code==0x50)
						powerUp = power = 1;*/
				}
			
				
			}
		}
		
		public void nextLevel()
		{
			if(state.level>=stagesCount-1)
			{
				game.goCredits();
				game.submitHighScores();
			}
			else
			{
				state.level++;
				start();
			}
		}
		
		public void setPause(bool value)
		{
			if(value)
			{
				draw(imgPause);
				imgPause.applyFilter(imgPause, new Rect(0.0, 0.0, 640.0, 480.0), new Point(), new BlurFilter(16.0, 16.0, 2)); 
				pause = true;
				hero.keysReset();
			}
			else
			{
				pause = false;
			}
			env.blanc = 1.0;
		}
		
		public void switchEvnPower()
		{
			if(power>=0.5)
			{
				powerUp = power = 0.49;
			}
			else
			{
				powerUp = power = 0.5;
			}
		}
		
		public void keyUp(uint code)
		{
			if(!pause)
				hero.keyUp(code);
		}

		public void initShopMenu(UpgradeMenu _shopMenu)
		{
			upgradeMenu = _shopMenu;
			upgradeMenu.pillsMedia = pills.media;
			upgradeMenu.gameInfo = info;
		}
		
		public void goUpgradeMenu()
		{
			if(!pause)
			{
				upgradeMenu.go();
				setPause(true);
			}
		}
		
		public void closeUpgradeMenu()
		{
			if(pause)
			{
				levelMenu.go(gui);
				setPause(false);
			}
		}
		
		// Синхронизировать очки, тоесть указать oldScore=state.scores, обновить надпись.
		public void syncScores()
		{
			scoreOld = state.scores;
			scoreText.text = scoreOld.toString();
		}
		
		private void winLevel()
		{
			pills.finish();
			nextLevelCountdown = 3;
			harvestProcess = 2;
			infoText.text = HARVEST_TEXT+"...";
			nextLevelCounter = 0;
			finish = true;
			env.blanc = 1.0;
			levelMenu.setState(1);
		}

		private void updateHarvesting(float dt)
		{
			String str = "";
			int i;
			
			pills.harvest(dt);
			if(pills.harvestCount>0)
			{
				nextLevelCounter+=dt;
				if(nextLevelCounter>=1)
				{
					nextLevelCounter--;
					harvestProcess++;
					if(harvestProcess>2)
						harvestProcess = 0;
					i = harvestProcess;
					while(i>0)
					{
						str+=".";
						--i;
					}
					infoText.text = HARVEST_TEXT+str;
				}
			}
			else
			{
				nextLevelCounter = 0;
				infoText.text = NEXT_LEVEL_TEXT_BEGIN+
								nextLevelCountdown.toString()+
								NEXT_LEVEL_TEXT_END;
			}
		}
		
		public void resetPower(float newPower)
		{
			power = powerUp = newPower;
		}
	}
}
