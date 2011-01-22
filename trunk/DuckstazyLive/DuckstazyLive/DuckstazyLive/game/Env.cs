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
        private DrawMatrix MAT = new DrawMatrix(true);

        private int[] imgClouds;
        private int imgStar;

        private CustomGeomerty geomSkyDay;
        private CustomGeomerty geomSkyNight;
        private CustomGeomerty geomGround;
        private CustomGeomerty geomGroundEffect;
        private CustomGeomerty geomBlanc;

        private int sndPower;
        private int sndTex2;
        private int music;

        private SoundChannel channel;
        private SoundTransform musicTrans;
        private float musicAttack;
        public float x;
        public float y;

        public float blanc;

        // Состояние окружения
        private float power;

        // Время для нормального состояния. Циклит для смены ДЕНЬ/НОЧЬ
        public bool day;
        // Время для глюков. Циклит палитру.
        private float time;

        // Счётчик для эффекта с травой
        private float grassCounter;

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
            ctGrass = new ColorTransform();
            utils.ARGB2ColorTransform(colGrass, ctGrass);
            ctProgress = new ColorTransform();
            utils.ARGB2ColorTransform(colProgress, ctProgress);
        }

        public void reset()
        {
            blanc = 0.0f;

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

            utils.ARGB2ColorTransform(colGrass, ctGrass);
            utils.ARGB2ColorTransform(colProgress, ctProgress);
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
                    blanc = 1.0f;
                    musicTrans.Volume = 1;
                    channel.SoundTransform = musicTrans;
                    Application.sharedSoundMgr.playSound(sndPower);
                }
                else if (power >= 0.5f && newPower < 0.5f)
                {
                    blanc = 1.0f;
                    colGrass = 0xff00ff00;
                    colGround = 0xff371d06;
                    colProgress = 0xff5d310c;
                    utils.ARGB2ColorTransform(colGrass, ctGrass);
                    utils.ARGB2ColorTransform(colProgress, ctProgress);
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
                    foreach (EnvCloud c in clouds)
                        c.update(dt, power);
                else
                    foreach (EnvStar s in nightSky)
                        s.update(dt, power);
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

        }

        public void draw1(Canvas canvas)
        {
            if (power < 0.5f)
            {
                if (day)
                {                    
                    canvas.drawGeometry(geomSkyDay);
                    drawSky(canvas);
                }
                else
                {
                    canvas.drawGeometry(geomSkyNight);
                    drawNight(canvas);
                }
            }
            else
            {
                curEffect.draw(canvas);         
            }
        }

        public void drawNight(Canvas canvas)
        {
            // Временные переменные.
            float x;

            // Рисуем ОБЛАКА
            foreach (EnvStar c in nightSky)
            {
                x = c.t;
                MAT.identity();
                MAT.translate(-7.0f, -7.0f);
                MAT.rotate(c.a);
                MAT.scale(0.75f + 0.25f * (float)Math.Sin(x * 6.28), 0.75f + 0.25f * (float)Math.Sin(x * 6.28));
                MAT.translate(c.x, c.y);

                canvas.draw(imgStar, MAT, c.color);
            }
        }

        public void drawSky(Canvas canvas)
        {
            // Временные переменные.
            float x;
            // Matrix MAT = new Matrix();
            Texture2D img;
            int imageId;

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
            }
        }

        public void draw2(Canvas canvas)
        {            
            ///**** ТРАВА ****/
            Color color = Color.White;
            color.R = (byte)(color.R * ctGrass.redMultiplier);
            color.G = (byte)(color.G * ctGrass.greenMultiplier);
            color.B = (byte)(color.B * ctGrass.blueMultiplier);

            if (power < 0.5f)
            {                
                canvas.drawGeometry(geomGround);

                Texture2D tex = Application.sharedResourceMgr.getTexture(Res.IMG_GRASS1);
                Rectangle src = new Rectangle(0, 0, tex.Width, tex.Height);
                Rectangle dst = new Rectangle(0, (int)(geomGround.y - tex.Height), Constants.SCREEN_WIDTH, tex.Height);

                AppGraphics.SetColor(color);
                AppGraphics.DrawImageTiled(tex, ref src, ref dst);
                AppGraphics.SetColor(Color.White);
            }
            else
            {
                geomGroundEffect.colorize(utils.makeColor(colGround));
                canvas.drawGeometry(geomGroundEffect);
                                
                Texture2D tex = Application.sharedResourceMgr.getTexture(Res.IMG_GRASS2);
                Rectangle src = new Rectangle(0, 0, tex.Width, tex.Height);
                Rectangle dst = new Rectangle(0, (int)(geomGroundEffect.y - tex.Height), Constants.SCREEN_WIDTH, tex.Height);

                AppGraphics.SetColor(color);
                AppGraphics.DrawImageTiled(tex, ref src, ref dst);
                AppGraphics.SetColor(Color.White);
            }
        }

        public void beat()
        {
            grassCounter = 1.0f;
        }

        public void updateBlanc(float dt)
        {
            if (blanc > 0.0f)
            {
                blanc -= 0.5f * dt;
                Color blancColor = Color.White * blanc;
                geomBlanc.colorize(blancColor);
            }
        }

        public void drawBlanc(Canvas canvas)
        {
            if (blanc > 0.0f)
            {
                canvas.drawGeometry(geomBlanc);
            }
        }
    }

}
