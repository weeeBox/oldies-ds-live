using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using DuckstazyLive.app;
using DuckstazyLive.game.levels;
using System.Diagnostics;
using Framework.core;

namespace DuckstazyLive.game
{
	public class Level
	{
		public static Level instance;
		
		private const String HARVEST_TEXT = "HARVESTING";
		private const String NEXT_LEVEL_TEXT_BEGIN = "WARP IN ";
		private const String NEXT_LEVEL_TEXT_END = " SEC...";
		 
        //[Embed(source="gfx/hp1.png")]
        //private Class rHPImg1;
        
        //[Embed(source="gfx/hp2.png")]
        //private Class rHPImg2;
        
        //[Embed(source="gfx/hp3.png")]
        //private Class rHPImg3;
        
        //[Embed(source="gfx/score.png")]
        //private Class rScoreImg;
        
        //[Embed(source="sfx/start.mp3")]
        //private Class rStartSnd;
           
        public int imgHP1;
        private int imgHP2;
        public int imgHP3;
        private int imgScore;
        private float hpPulse;
        private float hpCounter;
        private TextField hpText;
        private TextField scoreText;
		private int scoreOld;
		private float scoreCounter;
		
		public TextField infoText;
		
		public int sndStart;
        
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


        private List<LevelStages> stages; // Уровни
        public LevelStage stage; // текущий уровень
		public StageMedia stageMedia;
		public int stagesCount;

        // инфа
		public GameInfo info;		
		
		public bool pause;
		public int imgPause;
		
		private float nextLevelCounter;
		private int harvestProcess;
		private int nextLevelCountdown;
		
		public Level(GameState gameState)
		{			
			instance = this;		
			
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
			stages = new List<LevelStages>();
            stages.Add(LevelStages.Harvesting);
            stages.Add(LevelStages.PartyTime);
            stages.Add(LevelStages.Bubbles);
            stages.Add(LevelStages.DoubleFrog);
            stages.Add(LevelStages.PartyTime2);
            stages.Add(LevelStages.BetweenCatsStage);
            stages.Add(LevelStages.Bubbles2);
            stages.Add(LevelStages.AirAttack);
            stages.Add(LevelStages.PartyTime3);
            stages.Add(LevelStages.Trains);
            stages.Add(LevelStages.Bubbles3);           
			
			stagesCount = stages.Count;

			stage = null;
			
			finish = false;			
			
			sndStart = Res.SND_LEVEL_START;
			
			hpCounter = 0.0f;
			hpPulse = 0.0f;
			
            hpText = new TextField();
            //hpText.defaultTextFormat = tf;
            //hpText.embedFonts = true;
            //hpText.cacheAsBitmap = true;
            //hpText.autoSize = TextFieldAutoSize.LEFT;
            //hpText.filters = [shadow];
		
            scoreText = new TextField();
            //scoreText.defaultTextFormat = tf;
            //scoreText.embedFonts = true;
            //scoreText.cacheAsBitmap = true;
            //scoreText.autoSize = TextFieldAutoSize.LEFT;
            //scoreText.filters = [shadow];
			
            infoText = new TextField();
            //infoText.defaultTextFormat = tf;
            //infoText.embedFonts = true;
            //infoText.cacheAsBitmap = true;
            //infoText.autoSize = TextFieldAutoSize.LEFT;
            //infoText.filters = [shadow];	
			
			scoreOld = 0;
			scoreCounter = 0.0f;
			
			game = Game.instance;
			// imgPause = game.imgBG;
            Debug.WriteLine("Implement me: Level.Leve()");
		}
		
		public void start()
		{
            //Class stageClass = stages[state.level][0];
            //Array stageParams = stages[state.level][1];

            env.blanc = 1.0f;
            power = 0.0f;
            powerUp = 0.0f;

            //if (stageParams != null)
            //    stage = new stageClass(stageParams);
            //else
            //    stage = new stageClass();            
            stage = LevelStageFactory.createStage(stages[state.level]);
			
			//power = 0.0f;
			//powerUp = 0.0f;
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
		
		public void drawUI(bool canvas)
		{
            //Matrix mat = new Matrix(1.0f, 0.0f, 0.0f, 1.0f, -25.0f, -23.0f);
            //ColorTransform col = new ColorTransform(1.0f, 1.0f, 1.0f, power);
            //float sc = 1.0f + 0.3f*hpPulse;
            //String str;
			
            //mat.scale(sc, sc);
            //mat.translate(22.0f, 410+18);//463.0f);
            //canvas.draw(imgHP1, mat, null, null, null, true);
            //canvas.draw(imgHP2, mat, col, null, null, true);
            //canvas.draw(imgHP3, mat, null, null, null, true);
			
            //mat.identity();
            //mat.tx = -24.0f;
            //mat.ty = -24.0f;
            //sc = 1.0f+0.3f*scoreCounter;
            //mat.scale(sc, sc);
            //mat.translate(620.0f, 410+18);//463.0f);
            //canvas.draw(imgScore, mat, null, null, null, true);
			
            //mat.identity();
			
            //mat.tx = 40.0f;
            //mat.ty = 410;//445.0f;
            //str = state.health.toString()+"/"+state.maxHP.toString();
            //if(hpText.text != str) hpText.text = str;
            //canvas.draw(hpText, mat);
			
            //mat.tx = 600.0f - scoreText.textWidth;
            //canvas.draw(scoreText, mat);
            throw new NotImplementedException();
		}
		
		public void draw(Canvas canvas)
		{
            if (pause)
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

                if (state.health > 0)
                    hero.draw(canvas);

                ps.draw(canvas);
                env.draw2(canvas);
                progress.draw(canvas);
                // drawUI(canvas);
                stage.draw2(canvas);
            }            
		}
		
		public void enterLevel()
		{
            // gui.setCurrent(levelMenu);
            // levelMenu.setState(0);
            env.blanc = 1.0f;

            stage.start();
            // sndStart.play();            
            Application.sharedSoundMgr.playSound(sndStart);
		}
		
		public void update(float dt)
		{
			float power_drain = 0.0f;
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
						env.blanc = 1.0f;
						progress.play = false;
						
						// levelMenu.setState(2);
                        throw new NotImplementedException();
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
													nextLevelCountdown.ToString()+
													NEXT_LEVEL_TEXT_END;
								}
							}
							else
								nextLevel();
						}
					}
				}
	
				if(hero.sleep) power_drain = 0.3f;
				if(powerUp<power)
				{
					power-=dt*power_drain;
					if(power<0.0f) power = 0.0f;
				}
				else
				{
					power+=dt*0.05f;
					if(power>powerUp) power = powerUp;
				}
				
				if(state.health>0) hero.update(dt, power);
				
				pills.update(dt, power);
				
				env.x = hero.x;
				env.y = hero.y;
				env.update(dt, power);
				
				progress.update(dt, power);
	
				ps.update(dt);
				
				if(hpPulse>0.0f) { hpPulse-=4.0f*dt; if(hpPulse<0.0f) hpPulse = 0.0f; }
				hpCounter+=4.0f*dt;
				if(power<0.33) {
					if(hpCounter>4.0f) { hpCounter-=4.0f; hpPulse = 1.0f; }
				}
				else if(power<0.66) {
					if(hpCounter>2.0f) { hpCounter-=2.0f; hpPulse = 1.0f; }
				}
				else {
					if(hpCounter>1.0f) {	hpCounter-=1.0f; hpPulse = 1.0f; }
				}
	
				if(power>=0.5) info.setRGB(env.colors.bg);
				else {
					if(env.day) info.setRGB(0x000000);
					else info.setRGB(0xffffff);
				}
				info.update(power, dt);
				
				if(state.scores>scoreOld)
				{
					scoreCounter+=30.0f*dt;
					if(scoreCounter>1.0f)
					{
						i = (state.scores-scoreOld)/5;
						if(i==0)
						{
							scoreOld = state.scores;
							scoreCounter = 0.0f;
						}
						else
						{
							scoreOld+=i;
							scoreCounter -= (int)scoreCounter;
						}
								
						scoreText.text = scoreOld.ToString();
					}
				}				
			}
		}

		public void gainPower(float gained)
		{
			powerUp+=gained;
			if(powerUp>1.0f)
				powerUp = 1.0f;
		}
		
		public void gainSleep()
		{
			powerUp = 0.0f;
		}
		
		public void keyDown(Keys key)
		{
			if(pause)
			{
				if(key == Keys.Escape)// ESC
				{
                    //if(gui.current==game.mainMenu)
                    //    game.clickNewGame();					
                    throw new NotImplementedException();
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
						hero.keyDown(key);
					}
					else
					{
						if(key==Keys.Enter) // ENTER
							game.startLevel();
						else if(key==Keys.Escape)// ESC
							game.goPause();						
					}
				}
				else
				{
					hero.keyDown(key);
					if(key==Keys.Escape)// ESC
						game.goPause();					
					else if(key==Keys.PageDown)
						nextLevel();
					/*else if(code==0x44)
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
                //draw(imgPause);
                //imgPause.applyFilter(imgPause, new Rect(0.0f, 0.0f, 640.0f, 480.0f), new Point(), new BlurFilter(16.0f, 16.0f, 2)); 
                //pause = true;
                //hero.keysReset();
                throw new NotImplementedException();
			}
			else
			{
				pause = false;
			}
			env.blanc = 1.0f;
		}
		
		public void switchEvnPower()
		{
			if(power>=0.5f)
			{
				powerUp = power = 0.49f;
			}
			else
			{
				powerUp = power = 0.5f;
			}
		}
		
		public void keyUp(Keys code)
		{
			if(!pause)
				hero.keyUp(code);
		}

        // Синхронизировать очки, тоесть указать oldScore=state.scores, обновить надпись.
		public void syncScores()
		{
			scoreOld = state.scores;
			scoreText.text = scoreOld.ToString();
		}
		
		private void winLevel()
		{
			pills.finish();
			nextLevelCountdown = 3;
			harvestProcess = 2;
			infoText.text = HARVEST_TEXT+"...";
			nextLevelCounter = 0;
			finish = true;
			env.blanc = 1.0f;			
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
								nextLevelCountdown.ToString()+
								NEXT_LEVEL_TEXT_END;
			}
		}
		
		public void resetPower(float newPower)
		{
			power = powerUp = newPower;
		}
	}

}
