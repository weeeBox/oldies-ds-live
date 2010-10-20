using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;
using Framework.core;
using DuckstazyLive.app;
using Microsoft.Xna.Framework;
using DuckstazyLive.game.utils;
using Framework.utils;

namespace DuckstazyLive.game.levels
{
    public sealed class PillLine
    {
        private const int PILLS_IN_LINE_COUNT = 15;        

        private float elapsedTime;        

        private float baseY; // y-координата центра колебаний
        private float baseYTop; // высшая точка (hero.power = 1.0f)
        private float baseYBottom; // низшая точка (hero.power = 0);
        private float baseYTarget; // y-координата центра колбеаний, к которой необходимо переместиться
        private float floatSpeed; // скорость "всплытия" полосы по мере увеличения hero power

        private float omega; // угловая скорость
        private float k; // волновое число
        private float amplitude; // амплитуда синусоидальных колебаний

        private int pillsCount; // количество активных pill'сов

        public Pill[] pills;
        public PillLine(ref Rect parentBounds, int lineIndex)
        {        
            float lineWidth = 0.8f * parentBounds.Width;
            float lineHeight = 0.09f * parentBounds.Height;
            float lineX = 0.5f * (parentBounds.Width - lineWidth);
            float lineY = 0.8f * parentBounds.Height;

            // init base cords
            switch (lineIndex)
            {
                case 0:
                    {
                        baseYTop = parentBounds.Y + 0.5f * lineHeight;
                        baseYBottom = parentBounds.Y + lineY;
                    }
                    break;

                case 1:
                    {
                        baseYTop = parentBounds.Y + 0.5f * parentBounds.Height;
                        baseYBottom = parentBounds.Y + lineY;
                    }
                    break;

                case 2:
                    {
                        Texture2D grass = Application.sharedResourceMgr.getTexture(Res.IMG_GRASS);
                        baseYTop = baseYBottom = parentBounds.Height - (0.5f * lineHeight + grass.Height);
                    }
                    break;

                default:
                    Debug.Assert(false, "Line index to hi: " + lineIndex);
                    break;
            }
            baseY = baseYTarget = baseYBottom;
            floatSpeed = (baseYTop - baseYBottom) / 10.0f;
        
            // init pills
            pillsCount = 0;
            float nextPillX = lineX;
            pills = new Pill[PILLS_IN_LINE_COUNT];
            float stepX = lineWidth / (float)(pills.Length - 1);
            for (int pillIndex = 0; pillIndex < pills.Length; ++pillIndex)
            {
                pills[pillIndex] = PillsManager.Pool.allocatePill(nextPillX, 0);
                nextPillX += stepX;
            }

            // initWavering
            k = (float)(MathHelper.TwoPi / (0.5f * lineWidth));
            amplitude = 0.5f * lineHeight - Constants.PILL_RADIUS;
            omega = (float)MathHelper.Pi;
        }

        public void update(float dt)
        {
            elapsedTime += dt;

            baseY += floatSpeed * dt;
            if (baseY < baseYTarget)
                baseY = baseYTarget;

            pillsCount = Math.Min((int)(elapsedTime / Constants.PILL_SPAWN_TIMEOUT), pills.Length);

            Hero hero = PillsManager.Hero;
            for (int pillIndex = 0; pillIndex < pillsCount; ++pillIndex)
            {
                Pill pill = pills[pillIndex];
                pill.lifeTime += dt;

                if (!pill.active)
                {
                    if (pill.lifeTime < Constants.PILL_SPAWN_TIMEOUT)                        
                        continue;
                    
                    pill.active = true;
                    pill.lifeTime = 0.0f;
                }
                                
                pill.y = baseY + amplitude * (float)(Math.Sin(omega * pill.lifeTime - k * pill.x));

                if (hero.collides(pill))
                {
                    pill.active = false;
                    pill.lifeTime = 0.0f;
                    hero.addPower(0.01f);
                }
            }
        }

        public void draw()
        {
            for (int pillIndex = 0; pillIndex < pillsCount; ++pillIndex)
            {
                Pill pill = pills[pillIndex];
                if (pill.active)
                {
                    pill.draw();
                }                
            }
        }

        public void setPower(float power)
        {
            baseYTarget = baseYTop + (baseYBottom - baseYTop) *  (1 - power);
        }
    }

    public class Level1 : PillsManager,  HeroListener
    {
        private const int LINES_COUNT = 3;        

        private PillLine[] pillsLines;
        private int activeLinesCount;

        public Level1()
        {            
        }

        public override void init()
        {
            Rect bounds = Bounds;
            pillsLines = new PillLine[LINES_COUNT];
            for (int lineIndex = 0; lineIndex < pillsLines.Length; ++lineIndex)
            {                
                pillsLines[lineIndex] = new PillLine(ref bounds, lineIndex);                
            }
            Hero.addListener(this);
        }

        protected override void updatePills(float dt)
        {
            activeLinesCount = getLinesCount(Hero.power);
            for (int lineIndex = 0; lineIndex < activeLinesCount; ++lineIndex)
            {
                pillsLines[lineIndex].update(dt);
            }            
        }

        private int getLinesCount(float power)
        {            
            if (power < 0.3f)
                return 1;
            else if (power < 0.6f)
                return 2;
            else
                return 3;
        }

        protected override void drawPills()
        {           
            for (int lineIndex = 0; lineIndex < activeLinesCount; ++lineIndex)
            {
                pillsLines[lineIndex].draw();
            }
        }

        #region HeroListener Members

        public void heroPowerChanged(float oldPower, float newPower)
        {
            for (int lineIndex = 0; lineIndex < activeLinesCount; ++lineIndex)
            {
                pillsLines[lineIndex].setPower(newPower);
            }            
        }

        #endregion
    }
}
