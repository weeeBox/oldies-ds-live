using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using DuckstazyLive.app;
using DuckstazyLive.game.levels;
using System.Diagnostics;
using Framework.core;
using Framework.visual;
using Microsoft.Xna.Framework.Graphics;

namespace DuckstazyLive.game
{
	public class Level
	{
		public static Level instance;
		
		private const String HARVEST_TEXT = "HARVESTING";
		private const String NEXT_LEVEL_TEXT_BEGIN = "WARP IN ";
		private const String NEXT_LEVEL_TEXT_END = " SEC...";		 
           
        public int imgHP1;        
        private int imgScore;
        private float hpPulse;
        private float hpCounter;
        
		private int scoreOld;
        private string scoreText;
		private float scoreCounter;
		
		public string infoText;
		
		public int sndStart;
        
		public Game game;
		public Heroes heroes;
		public Pills pills;
		public Env env;
		protected Particles ps;
		
		public LevelProgress progress;
		
		public float power;
		protected float powerUp;

        // Состояние уровня
		public GameState state;

        // Конец Уровня
		public bool finish;
		private float finishCounter;

        protected List<LevelStages> stages; // Уровни
        public LevelStage stage; // текущий уровень
		public StageMedia stageMedia;
		public int stagesCount;

        // инфа
		public GameInfo info;		
		
		public bool pause;
		public int imgPause;
		
		protected float nextLevelCounter;
		protected int harvestProcess;
		protected int nextLevelCountdown;
		
		public Level(GameState gameState)
		{			
			instance = this;
            state = gameState;

            game = Game.instance;			
			
			info = new GameInfo();
			ps = new Particles();
			env = new Env(game);

            initHero();

            progress = new LevelProgress();
            progress.env = env;
			
			stageMedia = new StageMedia();
			stages = new List<LevelStages>();            

			stage = null;			
			finish = false;

            imgHP1 = Res.IMG_UI_HP;
            imgScore = Res.IMG_UI_SCORE;
			sndStart = Res.SND_LEVEL_START;
			
			hpCounter = 0.0f;
			hpPulse = 0.0f;
			
			scoreOld = 0;
			scoreCounter = 0.0f;			
		}

        protected virtual void initHero()
        {
            heroes = new Heroes();
            Hero heroInstance = new Hero(heroes, 0);
            heroInstance.state = state;
            heroes.addHero(heroInstance);

            pills = new Pills(heroes, ps, this);
            heroes.particles = ps;            
            heroes.env = env;
            heroes.init();
        }
		
		public void start()
		{
            env.blanc = 1.0f;
            power = 0.0f;
            powerUp = 0.0f;
            
            stage = LevelStageFactory.createStage(stages[state.level]);			
			
			ps.clear();
			pills.clear();
			info.reset();

			progress.start(stage.goalTime);
			heroes.init();
			game.save();
			
			finish = false;
			pause = false;
			
			state.health = state.maxHP;
			syncScores();
			enterLevel();
		}
		
		public void drawUI(Canvas canvas)
		{
            DrawMatrix mat = new DrawMatrix();            
            float sc = 1.0f + 0.3f * hpPulse;            

            mat.tx = -25.0f;
            mat.ty = -23.0f;
            mat.scale(sc, sc);
            mat.translate(22.0f, 410 + 18);//463.0f);
            canvas.draw(imgHP1, mat);            

            mat.identity();
            mat.tx = -24.0f;
            mat.ty = -24.0f;
            sc = 1.0f + 0.3f * scoreCounter;
            mat.scale(sc, sc);
            mat.translate(620.0f, 410 + 18);//463.0f);
            canvas.draw(imgScore, mat);

            mat.identity();

            mat.translate(40.0f, 410.0f);//445.0f;
            String str = state.health.ToString() + "/" + state.maxHP.ToString();            
            canvas.draw(Res.FNT_BIG, str, mat);

            Font font = Application.sharedResourceMgr.getFont(Res.FNT_BIG);
            mat.translate(600.0f - font.stringWidth(scoreText), 410.0f);
            canvas.draw(Res.FNT_BIG, scoreText, mat);
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
                levelPreDraw();
                stage.draw1(canvas);

                info.drawFT(canvas);
                pills.draw(canvas);

                if (state.health > 0)
                    heroes.draw(canvas);

                ps.draw(canvas);
                levelPostDraw();

                env.draw2(canvas);

                levelPreDraw();
                progress.draw(canvas);
                drawUI(canvas);
                stage.draw2(canvas);
                levelPostDraw();
            }            
		}

        private void levelPreDraw()
        {
            float tx = Constants.SAFE_OFFSET_X;
            float ty = Constants.SAFE_OFFSET_Y;
            AppGraphics.PushMatrix();
            AppGraphics.Translate(tx, ty, 0);
        }

        private void levelPostDraw()
        {
            AppGraphics.PopMatrix();
        }
		
		public void enterLevel()
		{            
            env.blanc = 1.0f;

            stage.start();         
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

						game.loose();
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
									infoText = NEXT_LEVEL_TEXT_BEGIN+
													nextLevelCountdown.ToString()+
													NEXT_LEVEL_TEXT_END;
								}
							}
							else
								nextLevel();
						}
					}
				}
	
				if(heroes[0].sleep) power_drain = 0.3f;
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
				
				if(state.health>0) heroes.update(dt, power);
				
				pills.update(dt, power);
				
				env.x = heroes[0].x;
				env.y = heroes[0].y;
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
								
						scoreText = scoreOld.ToString();
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

        public void buttonPressed(ref ButtonEvent e)
        {
            if (pause)
            {               
                throw new NotImplementedException();                
            }
            else
            {
                if (finish)
                {
                    if (state.health > 0)
                    {
                        //if(code==0x0D || code==0x1B) // ENTER or ESC
                        //nextLevel();
                        //else
                        heroes.buttonPressed(ref e);
                    }
                    else
                    {
                        if (e.button == Buttons.A || e.button == Buttons.Start) // ENTER
                            game.startLevel();
                        else if (e.button == Buttons.Back)// ESC
                            game.goPause();
                    }
                }
                else
                {
                    heroes.buttonPressed(ref e);
                    if (e.button == Buttons.Back)// ESC
                        game.goPause();
                    else if (e.button == Buttons.LeftShoulder)
                        nextLevel();
                    //else if (key == Keys.End)
                    //    powerUp = power = 1.0f;
                    //else if (key == Keys.Delete)
                    //    state.health = 0;
                    /*else if(code==0x44)
                        hero.doToxicDamage(320, 200, 20, 0);
                    else if(code==0x50)
                        powerUp = power = 1;*/
                }
            }
        }

        public void buttonReleased(ref ButtonEvent e)
        {
            if (!pause)
                heroes.buttonReleased(ref e);
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

        // Синхронизировать очки, тоесть указать oldScore=state.scores, обновить надпись.
		public void syncScores()
		{
			scoreOld = state.scores;
			scoreText = scoreOld.ToString();
		}
		
		private void winLevel()
		{
			pills.finish();
			nextLevelCountdown = 3;
			harvestProcess = 2;
			infoText = HARVEST_TEXT+"...";
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
					infoText = HARVEST_TEXT+str;
				}
			}
			else
			{
				nextLevelCounter = 0;
				infoText = NEXT_LEVEL_TEXT_BEGIN+
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
