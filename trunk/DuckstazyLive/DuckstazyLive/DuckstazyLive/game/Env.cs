using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckstazyLive.game.env;
using DuckstazyLive.app;
using System.Diagnostics;
using Framework.core;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Framework.visual;

namespace DuckstazyLive.game
{
    public class Env
    {
        private int[] imgClouds;
        private int imgStar;

        private CustomGeomerty geomSkyDay;
        private CustomGeomerty geomSkyNight;
        private CustomGeomerty geomSkyBlanc;
        private CustomGeomerty geomGround;
        private CustomGeomerty geomGroundEffect;
        private CustomGeomerty geomGroundBlanc;        

        private int sndPower;
        private int sndTex2;
        private int music;

        private SoundChannel channel;
        private SoundTransform musicTrans;
        private float musicAttack;
        public float x;
        public float y;

        private const float ENV_TIMEOUT = 0.5f;
        private const float ENV_FADE_DELAY = 0.25f;
        private const float ENV_FADE_TIMEOUT = ENV_TIMEOUT + ENV_FADE_DELAY;
        private const float ENV_SPEED = 1.0f / ENV_FADE_DELAY;
        private float envElapsedTime;

        private float hitFade;
        public ColorTransform blackFade;
        public ColorTransform whiteFade;

        private float blanc;
        private CustomGeomerty geomBlanc;

        // Состояние окружения
        private float power;

        // Время для нормального состояния. Циклит для смены ДЕНЬ/НОЧЬ
        public bool day;
        // Время для глюков. Циклит палитру.
        private float time;

        // Счётчик для эффекта с травой
        private float grassCounter;

        private Text dammitText;
        private Color[] dammitColors;

        // Облака
        private EnvCloud[] clouds;
        private EnvStar[] nightSky;

        private EnvColor norm;

        private static EnvColor[] hell = 
        {
			new EnvColor(0xFF0000, 0xFFFF00), 
			new EnvColor(0xFFFF00, 0xFF0000), 
			new EnvColor(0x00FF00, 0x0000FF),
			new EnvColor(0x00FFFF, 0xFFFF00),
			new EnvColor(0x0000FF, 0xFFFF00),
			new EnvColor(0xFF00FF, 0xFFFF00),
			new EnvColor(0xFF0000, 0xFFFF00)
        };

        private const int hellCount = 7;

        // Текущие цвета.
        public EnvColor colors;

        public uint colGrass;
        public uint colGround;
        public uint colProgress;

        public ColorTransform ctGrass;
        public ColorTransform ctProgress;

        // эффекты
        private EnvEffect[] effects;
        private EnvEffect curEffect;        
                
        public Env()
        {
            // shape = new Shape();
            norm = new EnvColor(0x3FB5F2, 0x000000);

            // Текущие цвета
            colors = new EnvColor(0, 0);
            effects = new EnvEffect[] 
			{ 
			    new EnvEffect1(), 
			    new EnvEffect2(), 
			    new EnvEffect3(), 
			    new EnvEffect4() 
			};
            
            geomBlanc = utils.createSolidRect(0, 0, Constants.SCREEN_WIDTH, Constants.SCREEN_HEIGHT, Color.White, false);            

            // Инициализируем
            initGrass();
            initDay();
            initNight();
            initHitFade();

            curEffect = effects[3];//effects[int(Math.random()*effects.length)];

            resetColorTransform();

            sndPower = Res.SND_ENV_POWER;            
            sndTex2 = Res.SONG_ENV_TEX;
            
            music = sndTex2;            
        }

        private void resetColorTransform()
        {
            colGrass = 0xff00ff00;
            colGround = 0xff371d06;
            colProgress = 0xff5d310c;
            ctGrass = ColorTransform.NONE;
            utils.ARGB2ColorTransform(colGrass, ref ctGrass);
            ctProgress = ColorTransform.NONE;
            utils.ARGB2ColorTransform(colProgress, ref ctProgress);
        }

        public void reset()
        {
            hitFade = 0.0f;

            resetColorTransform();

            // Инициализируем траву
            grassCounter = 0.0f;

            // Инициализируем переменные окружения
            power = 0.0f;
            day = true;
            updateNorm();
            time = 0.0f;

            musicTrans = new SoundTransform(0);
        }

        public void playMusic()
        {
            musicTrans.Volume = 0;
            channel = Application.sharedSoundMgr.playSound(music, true, musicTrans);
            musicAttack = 0.0f;
        }

        private void initGrass()
        {
            float groundX = 0;
            float groundY = Constants.ENV_HEIGHT;
            float groundWidth = Constants.GROUND_WIDTH;
            float groundHeight = Constants.GROUND_HEIGHT;

            geomGround = utils.createGradient(groundX, groundY, groundWidth, groundHeight, utils.makeColor(0x371d06), utils.makeColor(0x5d310c), false);
            geomGroundBlanc = utils.createSolidRect(groundX, groundY, groundWidth, groundHeight, Color.Black, false);
            geomGroundEffect = utils.createSolidRect(groundX, groundY, groundWidth, groundHeight, Color.White, false);
        }

        private void initDay()
        {            
            geomSkyDay = utils.createGradient(0, 0, Constants.SCREEN_WIDTH, Constants.SCREEN_HEIGHT, utils.makeColor(0x3FB5F2), utils.makeColor(0xDDF2FF), false);

            imgClouds = new int[] { Res.IMG_CLOUD_1, Res.IMG_CLOUD_2, Res.IMG_CLOUD_3 };

            clouds = new EnvCloud[] { new EnvCloud(), new EnvCloud(), new EnvCloud(), new EnvCloud(), new EnvCloud() };
            foreach (EnvCloud it in clouds)
            {
                it.init(x);
                x += utils.scale(128.0f + utils.rnd() * 22.0f);
            }
        }

        private void initNight()
        {
            geomSkyNight = utils.createSolidRect(0, 0, Constants.SCREEN_WIDTH, Constants.SCREEN_HEIGHT, utils.makeColor(0x111133), false);

            int starsCount = 30;
            int i = starsCount - 1;

            imgStar = Res.IMG_STAR;
            nightSky = new EnvStar[starsCount];

            while (i >= 0)
            {
                nightSky[i] = new EnvStar();
                --i;
            }

        }

        private void initHitFade()
        {
            whiteFade = new ColorTransform(0xffffff);
            blackFade = new ColorTransform(0x000000);
            whiteFade.overlayColor = blackFade.overlayColor = true;
            geomSkyBlanc = GeometryFactory.createSolidRect(0, 0, Constants.SCREEN_WIDTH, Constants.SCREEN_HEIGHT, Color.White);

            Font badFont = Application.sharedResourceMgr.getFont(Res.FNT_BAD);
            dammitText = new Text(badFont);
            dammitText.setString("");
            dammitText.scaleX = dammitText.scaleY = 2.95f;
            dammitText.setAlign(Image.ALIGN_CENTER, Image.ALIGN_MIN);
            dammitText.rotationCenterY = -dammitText.height / 2;

            dammitColors = new Color[] { Color.Red, Color.Green, Color.Blue };
        }

        public void updateNorm()
        {
            if (day)
            {
                norm.bg = 0x3FB5F2;
                norm.text = 0x000000;
            }
            else
            {
                norm.bg = 0x111133;
                norm.text = 0xFFFFFF;
            }
        }

        private void updateColors()
        {            
            int c;
            float x = time;
            float p2 = power * power;

            c = (int)x; x -= c;            
            colors.lerp(x, hell[c], hell[c + 1]);            

            colGrass = 0xff000000 | utils.lerpColor(utils.multColorScalar(0x177705, 1.0f - p2), colors.bg, p2 * grassCounter);
            colGround = 0xff000000 | utils.lerpColor(0x371d06, utils.multColorScalar(colors.bg, grassCounter * power), p2);
            colProgress = 0xff000000 | utils.lerpColor(0x5d310c, colors.bg, p2);

            utils.ARGB2ColorTransform(colGrass, ref ctGrass);
            utils.ARGB2ColorTransform(colProgress, ref ctProgress);
        }

        public void update(float dt, float newPower)
        {
            // Временные переменные.
            float x;
            // int i;
            // SoundTransform st;

            if (newPower != power)
            {
                if (newPower >= 0.5f && power < 0.5f)
                {
                    day = !day;
                    updateNorm();
                    hitFade = 1.0f;
                    musicTrans.Volume = 1;
                    channel.SoundTransform = musicTrans;
                    Application.sharedSoundMgr.playSound(sndPower);
                }
                else if (power >= 0.5f && newPower < 0.5f)
                {
                    hitFade = 1.0f;
                    colGrass = 0xff00ff00;
                    colGround = 0xff371d06;
                    colProgress = 0xff5d310c;
                    utils.ARGB2ColorTransform(colGrass, ref ctGrass);
                    utils.ARGB2ColorTransform(colProgress, ref ctProgress);
                    curEffect = effects[(int)(utils.rnd() * effects.Length)];
                }

                power = newPower;

                if (power < 0.5f)
                {
                    musicTrans.Volume = power * 0.3f;
                    channel.SoundTransform = musicTrans;
                }
            }

            // Обноляем счётчик с травой
            if (grassCounter > 0)
            {
                grassCounter -= dt * 4.0f;
                if (grassCounter < 0.0f)
                    grassCounter = 0.0f;
            }

            if (power < 0.5f)
            {
                if (day)
                {
                    foreach (EnvCloud c in clouds)
                    {
                        c.update(dt, power);
                    }                    
                }
                else
                {
                    foreach (EnvStar s in nightSky)
                    {
                        s.update(dt, power);
                    }
                }
            }
            else
            {
                curEffect.power = power;                
                curEffect.c1 = colors.bg;
                curEffect.c2 = utils.multColorScalar(colors.bg, 0.5f);
                curEffect.update(dt);
                // Прокручиваем время день/ночь
                time += dt * 0.1f;
                while (time > hellCount - 1)
                    time -= hellCount - 1;

                x = (channel.LeftPeak + channel.RightPeak) * 0.5f;
                // Обновляем текущие цвета
                updateColors();

                musicAttack = musicAttack * 0.7f + x * 0.7f;

                curEffect.peak = musicAttack;
            }
                        
            updateHifFade(dt);
        }

        public void draw1(Canvas canvas)
        {
            if (power < 0.5f)
            {
                if (day)
                {                    
                    canvas.drawGeometry(geomSkyDay);
                    drawSkyBlanc(canvas);
                    drawSky(canvas);
                }
                else
                {
                    canvas.drawGeometry(geomSkyNight);
                    drawSkyBlanc(canvas);
                    drawNight(canvas);
                }
            }
            else
            {                
                curEffect.draw(canvas);
                drawSkyBlanc(canvas);                
            }

            if (dammitText.isTimelinePlaying())
                dammitText.draw();
        }

        public void drawSkyBlanc(Canvas canvas)
        {
            if (isHitFaded())
            {
                canvas.drawGeometry(geomSkyBlanc);
            }
        }

        public void drawNight(Canvas canvas)
        {
            // Временные переменные.
            float x;

            DrawMatrix MAT = DrawMatrix.ScaledInstance;

            // Рисуем звезды
            ColorTransform colorTrans = ColorTransform.NONE;
            foreach (EnvStar c in nightSky)
            {
                x = c.t;
                MAT.identity();
                MAT.translate(-7.0f, -7.0f);
                MAT.rotate(c.a);
                MAT.scale(0.75f + 0.25f * (float)Math.Sin(x * 6.28), 0.75f + 0.25f * (float)Math.Sin(x * 6.28));
                MAT.translate(c.x, c.y);
                                
                canvas.draw(imgStar, MAT, c.color);
                if (isHitFaded())
                {
                    canvas.draw(imgStar, MAT, blackFade);
                }
            }
        }

        public void drawSky(Canvas canvas)
        {
            // Временные переменные.
            float x;
            // Matrix MAT = new Matrix();
            SpriteTexture img;
            int imageId;

            DrawMatrix MAT = DrawMatrix.ScaledInstance;
            // Рисуем ОБЛАКА
            foreach (EnvCloud c in clouds)
            {
                x = c.counter;
                imageId = imgClouds[c.id];
                img = Application.sharedResourceMgr.getTexture(imageId);

                MAT.identity();
                MAT.tx = utils.unscale(-img.Width * 0.5f);
                MAT.ty = utils.unscale(-img.Height * 0.5f);
                MAT.scale(0.9f + 0.1f * (float)Math.Sin(x * 6.28), 0.95f + 0.05f * (float)Math.Sin(x * 6.28 + 3.14));
                MAT.translate(c.x, c.y);

                canvas.draw(imageId, MAT);
                if (isHitFaded())
                {
                    canvas.draw(imageId, MAT, blackFade);
                }
            }
        }

        public void draw2(Canvas canvas)
        {            
            /**** ТРАВА ****/
            Color color = Color.White;
            color.R = (byte)(color.R * ctGrass.redMultiplier);
            color.G = (byte)(color.G * ctGrass.greenMultiplier);
            color.B = (byte)(color.B * ctGrass.blueMultiplier);
            int grassImageId;

            if (power < 0.5f)
            {
                canvas.drawGeometry(geomGround);
                grassImageId = Res.IMG_GRASS1;
                drawGrass(grassImageId, ref color);
            }
            else
            {
                geomGroundEffect.colorize(utils.makeColor(colGround));
                canvas.drawGeometry(geomGroundEffect);
                grassImageId = Res.IMG_GRASS2;
                drawGrass(grassImageId, ref color);
            }

            if (isHitFaded())
            {
                canvas.drawGeometry(geomGroundBlanc);
                Color colorBlanc = Color.Black;
                colorBlanc.R = (byte)(colorBlanc.R * blackFade.redMultiplier);
                colorBlanc.G = (byte)(colorBlanc.G * blackFade.greenMultiplier);
                colorBlanc.B = (byte)(colorBlanc.B * blackFade.blueMultiplier);
                colorBlanc.A = (byte)(colorBlanc.A * blackFade.alphaMultiplier);
                drawGrass(grassImageId, ref colorBlanc);
            }
        }       

        private void drawGrass(int imageId, ref Color color)
        {
            SpriteTexture tex = Application.sharedResourceMgr.getTexture(imageId);
            Rectangle src = new Rectangle(0, 0, tex.Width, tex.Height);
            Rectangle dst = new Rectangle(0, (int)(geomGroundEffect.y - tex.Height), Constants.SCREEN_WIDTH, tex.Height);

            AppGraphics.SetColor(color);
            tex.drawTiled(ref src, ref dst);
            AppGraphics.SetColor(Color.White);
        }

        public void beat()
        {
            grassCounter = 1.0f;
        }

        public void startHitFade(bool dead)
        {
            hitFade = 1.0f;
            envElapsedTime = 0.0f;

            if (dead) dammitText.setString("DEAD!!!");
            else dammitText.setString("DAMMIT!");

            dammitText.x = 0.5f * Constants.SCREEN_WIDTH;
            dammitText.y = Constants.ENV_HEIGHT;            
            dammitText.turnTimelineSupportWithMaxKeyFrames(1);
            dammitText.color = Color.White;
            dammitText.addKeyFrame(new BaseElement.KeyFrame(dammitText.x, -dammitText.height, Color.White * 0.0f, dammitText.scaleX, dammitText.scaleY, 0.0f, 2.5f * ENV_TIMEOUT));
            dammitText.playTimeline();
        }

        public bool isHitFaded()
        {
            return hitFade > 0.0f;
        }

        public void updateHifFade(float dt)
        {
            envElapsedTime += dt;

            if (isHitFaded())
            {
                proccessHitFade(hitFade);                               
                
                if (envElapsedTime < ENV_TIMEOUT)
                {                    
                }
                else if (envElapsedTime < ENV_FADE_TIMEOUT)
                {
                    hitFade -= ENV_SPEED * dt;
                    if (hitFade < 0.0f)
                        hitFade = 0.0f;
                }               
                else
                {
                    hitFade = 0.0f;
                }
            }

            if (dammitText.isTimelinePlaying())
            {
                dammitText.update(dt);
                int colorIndex = ((int)(envElapsedTime / 0.025f)) % dammitColors.Length;
                float dammitAlpha = dammitText.color.A / 255.0f;
                dammitText.color.R = (byte)(dammitColors[colorIndex].R * dammitAlpha);
                dammitText.color.G = (byte)(dammitColors[colorIndex].G * dammitAlpha);
                dammitText.color.B = (byte)(dammitColors[colorIndex].B * dammitAlpha);
            }
        }

        public void proccessHitFade(float blanc)
        {
            whiteFade.alphaMultiplier = blackFade.alphaMultiplier = blanc;

            Color skyBlanc = Color.White * blanc;
            Color groundBlanc = Color.Black * blanc;
            geomSkyBlanc.colorize(skyBlanc);
            geomGroundBlanc.colorize(groundBlanc);            
        }       

        public void startBlanc()
        {
            blanc = 1.0f;
        }

        public void updateBlanc(float dt)
        {
            if (blanc > 0)
            {
                blanc -= 0.5f * dt;
                if (blanc < 0.0f)
                {
                    blanc = 0;
                }
                else
                {
                    setBlanc(blanc);
                }
            }
        }

        public void setBlanc(float blanc)
        {
            this.blanc = blanc;
            Color color = Color.White * blanc;
            geomBlanc.colorize(color);
        }

        public void drawBlanc(Canvas canvas)
        {
            if (blanc > 0)
                canvas.drawGeometry(geomBlanc);
        }
    }
}
