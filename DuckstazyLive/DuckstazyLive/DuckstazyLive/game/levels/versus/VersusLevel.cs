using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckstazyLive.game.levels;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using DuckstazyLive.game.stages.versus;
using DuckstazyLive.app;
using Framework.visual;
using Framework.core;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using DuckstazyLive.game.stages;

namespace DuckstazyLive.game
{
    public class CollectedText : BaseElement
    {        
        private int heroIndex;
        private Font font;

        public CollectedText(int heroIndex)
        {
            this.heroIndex = heroIndex;

            font = Application.sharedResourceMgr.getFont(Res.FNT_HUD_DIGITS);
            width = font.stringWidth("000") - 4;
            height = font.fontHeight();
        }

        public override void draw()
        {
            preDraw();

            Hero hero = GameElements.Heroes[heroIndex];
            int collected = hero.gameState.getScores();

            float dx = width / 6.0f;            

            int units = collected % 10;
            int tens = (collected / 10) % 10;
            int hundreds = collected / 100;
            drawDigit(hundreds, drawX + dx, drawY, true);
            drawDigit(tens, drawX + 3.0f * dx, drawY, collected < 100);
            drawDigit(units, drawX + 5.0f * dx, drawY, collected < 10);
            
            postDraw();
        }

        private void drawDigit(int digit, float dx, float dy, bool useTransparency)
        {
            if (useTransparency && digit == 0)
            {
                AppGraphics.SetColor(Color.White * 0.5f);
                font.drawString("0", dx, dy, TextAlign.TOP | TextAlign.HCENTER);
                AppGraphics.SetColor(Color.White);                
            }
            else
            {
                font.drawString(digit.ToString(), dx, dy, TextAlign.TOP | TextAlign.HCENTER);
            }
        }
    }

    public class VersusLevelHud : Hud
    {
        protected CollectedText[] collectedTexts;        

        public VersusLevelHud(Level level) : base(level)
        {
            collectedTexts = new CollectedText[2];            
            CollectedText text1 = new CollectedText(0);            
            CollectedText text2 = new CollectedText(1);
            text1.setAlign(ALIGN_MIN, ALIGN_CENTER);
            text2.setAlign(ALIGN_MAX, ALIGN_CENTER);
            text1.parentAlignY = text2.parentAlignY = ALIGN_CENTER;
            text1.x = healthBars[0].x + healthBars[0].width + 5;
            text2.x = healthBars[1].x - 5;

            addChild(text1);
            addChild(text2);
        }

        protected override HealthBar[] createBars()
        {
            HealthBar bar1 = new HealthBar(Res.IMG_UI_HEALTH_EMO_BASE);
            HealthBar bar2 = new HealthBar(Res.IMG_UI_HEALTH_EMO_BASE2);            
            bar1.x = 0;
            bar2.x = width - bar2.width;
            bar1.alignY = bar2.alignY = bar1.parentAlignY = bar2.parentAlignY = ALIGN_CENTER;

            return new HealthBar[] { bar1, bar2 };
        }

        public override void update(float power, float dt)
        {
            base.update(power, dt);
            VersusLevelStage stage = getStage();            
            clock.setRemainingTime(stage.getRemainingTime());
        }

        public override void onStart()
        {
            base.onStart();
            
            clock.alignX = clock.alignY = clock.parentAlignX = clock.parentAlignY = ALIGN_CENTER;
            clock.show();
        }

        public VersusLevel getLevel()
        {
            return (VersusLevel)level;
        }

        public VersusLevelStage getStage()
        {
            return getLevel().getStage();
        }
    }

    public class VersusLevel : Level
    {
        private enum VersusStages
        {
            DoubleFrog,            
            TripleFrog,
            AirAttack,
            Duckfight
        }

        private struct StageInfo
        {
            public VersusStages stage;
            public String name;

            public StageInfo(VersusStages stage, String name)
            {
                this.stage = stage;
                this.name = name;
            }
        }

        private static StageInfo[] stagesInfo =
        {
            new StageInfo(VersusStages.DoubleFrog, "Double Frog"),
            new StageInfo(VersusStages.TripleFrog, "Triple Frog"),
            new StageInfo(VersusStages.AirAttack, "Air Attack"),
            new StageInfo(VersusStages.Duckfight, "Duck Fight"),
        };

        private const int STATE_START = 0;
        private const int STATE_PLAYING = 1;
        private const int STATE_END = 2;

        private BaseElement[] combos;
        private int comboIndex;
        private float comboCounter;
        private Color[] comboColors;
        private static Color[] comboColors1 = 
        {
            utils.makeColor(0xfff200), utils.makeColor(0xf26522), Color.Red
        };
        private static Color[] comboColors2 = 
        {
            utils.makeColor(0xec008c), utils.makeColor(0xed1c24), Color.Blue
        };
        private static Color[] comboBackColors = 
        {
            utils.makeColor(0xe1e1e1), Color.White
        };

        public VersusLevel(GameController controller, int stageIndex) : base(controller)
        {            
            this.stageIndex = stageIndex;
            
            GameElements.initHeroes(2);
            GameElements.reset();

            initCombos();
        }        

        private void initCombos()
        {
            Image comboX2 = new Image(Application.sharedResourceMgr.getTexture(Res.IMG_COMBO_X2));
            comboX2.setAlign(ALIGN_CENTER, ALIGN_MAX);
            Image comboX3 = new Image(Application.sharedResourceMgr.getTexture(Res.IMG_COMBO_X3));
            comboX3.setAlign(ALIGN_CENTER, ALIGN_MAX);
            Image comboX4 = new Image(Application.sharedResourceMgr.getTexture(Res.IMG_COMBO_X4));
            comboX4.setAlign(ALIGN_CENTER, ALIGN_MAX);
            Image comboX5 = new Image(Application.sharedResourceMgr.getTexture(Res.IMG_COMBO_X5));
            comboX5.setAlign(ALIGN_CENTER, ALIGN_MAX);
            Image comboX6 = new Image(Application.sharedResourceMgr.getTexture(Res.IMG_COMBO_X6));
            comboX6.setAlign(ALIGN_CENTER, ALIGN_MAX);

            Text comboX7 = createComboText("YEAH!!!");
            Text comboX8 = createComboText("FUCK\nYEAH!!!");
            Text comboX9 = createComboText("DAMN\nYOU’RE\nGOOD!!!");           

            combos = new BaseElement[]
            {
                comboX2, comboX3, comboX4, comboX5, comboX6, comboX7, comboX8, comboX9
            };
        }

        private Text createComboText(String text)
        {
            Font font = Application.sharedResourceMgr.getFont(Res.FNT_COMBO);
            Text comboText = new Text(font);
            comboText.setString(text);
            comboText.setAlign(ALIGN_CENTER, ALIGN_CENTER);
            comboText.x = utils.scale(320);
            comboText.y = utils.scale(200) - Constants.TITLE_SAFE_TOP_Y;
            return comboText;
        }

        public override void start()
        {
            getHeroes().clear();
            base.start();
            startLevelState(STATE_START);

            Heroes heroes = getHeroes();
            heroes[0].user = heroes[1].user = victimCallback;
            comboIndex = Constants.UNDEFINED;            
        }

        public override void onEnd()
        {
            base.onEnd();
            getPills().finish();
            getHeroes().buttonsReset();
        }

        protected override void startLevelState(int levelState)
        {
            base.startLevelState(levelState);

            switch (levelState)
            {
                case STATE_START:                
                case STATE_PLAYING:                 
                    break;
                case STATE_END:
                {
                    onEnd();
                    break;
                }                    
                default:
                    Debug.Assert(false, "Bad level state: " + levelState);
                    break;
            }
        }

        protected override LevelStage createStage(int stageIndex)
        {
            Debug.Assert(stageIndex >= 0 && stageIndex < stagesInfo.Length);
            VersusStages stage = stagesInfo[stageIndex].stage;            

            switch (stage)
            {
                case VersusStages.DoubleFrog:
                    return new DoubleFrogVs(this);

                case VersusStages.AirAttack:
                    return new AirAttackVs(this);

                case VersusStages.TripleFrog:
                    return new TripleFrog(this);

                case VersusStages.Duckfight:
                    return new Duckfight(this);

                default:
                    Debug.Assert(false, "Bad stage: " + stage);
                    break;
            }

            return null;
        }

        protected override Hud createHud()
        {
            return new VersusLevelHud(this);
        }

        public override void update(float dt)
        {
            base.update(dt);

            levelStateElapsed += dt;
            switch (levelState)
            {
                case STATE_START:
                {
                    startLevelState(STATE_PLAYING);
                    break;
                }
                
                case STATE_PLAYING:
                {
                    updateCombo(dt);


                    Heroes heroes = getHeroes();
                    if (heroes[1].isDead())
                    {
                        onWin(0);
                    }
                    else if (heroes[0].isDead())
                    {
                        onWin(1);
                    }
                    else if (getStage().isEnded())
                    {
                        int collected0 = getStage().getPillCollected(0);
                        int collected1 = getStage().getPillCollected(1);

                        if (collected0 > collected1)
                        {
                            onWin(0);
                        }
                        else if (collected1 > collected0)
                        {
                            onWin(1);
                        }
                        else
                        {
                            onDraw();
                        }
                    }
                    break;
                }                
            }            
        }

        private void updateCombo(float dt)
        {
            if (comboIndex != Constants.UNDEFINED)
            {
                BaseElement comboElement = combos[comboIndex];
                comboElement.update(dt);                
                if (!comboElement.isTimelinePlaying())
                {
                    comboIndex = Constants.UNDEFINED;
                }
                else if (comboIndex < 5)
                {
                    comboElement.y -= 100 * dt;
                }
                else
                {
                    comboCounter += dt;

                    int colorIndex = ((int)(comboCounter / 0.025f)) % comboColors.Length;
                    float dammitAlpha = comboElement.color.A / 255.0f;
                    comboElement.color.R = (byte)(comboColors[colorIndex].R * dammitAlpha);
                    comboElement.color.G = (byte)(comboColors[colorIndex].G * dammitAlpha);
                    comboElement.color.B = (byte)(comboColors[colorIndex].B * dammitAlpha);
                }

            }
        }

        public override void draw1()
        {
            base.draw1();

            if (comboIndex > 4)
            {
                BaseElement comboElement = combos[comboIndex];

                float tx = Constants.SAFE_OFFSET_X;
                float ty = Constants.SAFE_OFFSET_Y;
                int colorIndex = ((int)(comboCounter / 0.025f)) % comboBackColors.Length;
                float alpha = comboElement.color.A / 255.0f;
                Color color = comboBackColors[colorIndex] * alpha;
                AppGraphics.FillRect(-tx, -ty, Constants.SCREEN_WIDTH, Constants.SCREEN_HEIGHT, color);

                comboElement.draw();
            }
        }

        public override void draw2()
        {
            base.draw2();
            if (comboIndex >= 0 && comboIndex <= 4)
                combos[comboIndex].draw();
        }
 
        protected virtual void onWin(int playerIndex)
        {            
            startLevelState(STATE_END);
            getController().showWinner(playerIndex);
        }

        protected virtual void onDraw()
        {            
            startLevelState(STATE_END);
            getController().showDraw();
        }

        public static int getStagesCount()
        {
            return stagesInfo.Length;
        }
        
        public void victimCallback(Hero hero, HeroMessage message)
        {
            if (message == HeroMessage.ATTACKER)
            {
                comboIndex = Constants.UNDEFINED;
                comboCounter = 0.0f;
                int combo = hero.combo;
                if (combo >= 2 && combo <= 9)
                {
                    comboIndex = combo - 2;
                    BaseElement comboElement = combos[comboIndex];                                        
                    if (combo < 7)
                    {                        
                        comboElement.x = utils.scale(hero.x + Hero.duck_w);
                        comboElement.y = utils.scale(hero.y) - 50;
                        comboElement.color = hero.getPlayerIndex() == 0 ? utils.makeColor(0xfff799) : utils.makeColor(0xf49ac1);
                        comboElement.scaleX = comboElement.scaleY = 0.1f;
                        comboElement.turnTimelineSupportWithMaxKeyFrames(3);
                        comboElement.addKeyFrame(new KeyFrame(comboElement.x, comboElement.y, comboElement.color, 1.2f, 1.2f, 0.0f, 0.2f));
                        comboElement.addKeyFrame(new KeyFrame(comboElement.x, comboElement.y, comboElement.color, 1.0f, 1.0f, 0.0f, 0.05f));
                        comboElement.addKeyFrame(new KeyFrame(comboElement.x, comboElement.y, comboElement.color * 0.0f, 1.0f, 1.0f, 0.0f, 0.6f));                        
                    }
                    else
                    {
                        comboElement.color = Color.White;
                        comboColors = hero.getPlayerIndex() == 0 ? comboColors1 : comboColors2;
                        comboElement.turnTimelineSupportWithMaxKeyFrames(2);                        
                        comboElement.addKeyFrame(new KeyFrame(comboElement.x, comboElement.y, comboElement.color, 1.0f, 1.0f, 0.0f, 1.0f));
                        comboElement.addKeyFrame(new KeyFrame(comboElement.x, comboElement.y, comboElement.color * 0.0f, 1.0f, 1.0f, 0.0f, 0.25f));
                    }
                    comboElement.playTimeline();
                }               
            }
        }

        public static String getStageName(int stageIndex)
        {
            Debug.Assert(stageIndex >= 0 && stageIndex < getStagesCount());
            return stagesInfo[stageIndex].name;
        }

        protected VersusController getController()
        {
            return (VersusController)controller;
        }

        public VersusLevelStage getStage()
        {
            return (VersusLevelStage)stage;
        }           
    }
}
