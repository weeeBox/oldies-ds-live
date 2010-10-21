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
        enum State
        {
            SLEEPING,
            APPEARING,
            FLOATING,
            DISSAPEARING,
        }

        private const int PILLS_IN_LINE_COUNT = 15;        

        private float stateElapsedTime;        

        private float baseY; // y-координата центра колебаний
        private float baseYTop; // высшая точка (hero.awakePower = 1.0f)
        private float baseYBottom; // низшая точка (hero.awakePower = 0);
        private float baseYTarget; // y-координата центра колбеаний, к которой необходимо переместиться
        private float floatSpeed; // скорость "всплытия" полосы по мере увеличения hero awakePower

        private float omega; // угловая скорость
        private float k; // волновое число
        private float amplitude; // амплитуда синусоидальных колебаний
        private double phase; // фаза колебаний
        private float awakePower; // hero awakePower при которой начинают появляться pill'сы

        private int pillsCount; // количество активных pill'сов
        private State state;

        public Pill[] pills;
        public PillLine(ref Rect bounds, int lineIndex)
        {
            float lineWidth = bounds.Width;
            float lineHeight = 0.12f * bounds.Height;            
            
            state = State.SLEEPING;

            // init base cords
            switch (lineIndex)
            {
                case 0:
                    {
                        baseYTop = bounds.Y + 0.5f * lineHeight;
                        baseYBottom = bounds.Y + 0.9f * bounds.Height; ;
                        awakePower = 0;
                        state = State.APPEARING;
                    }
                    break;

                case 1:
                    {
                        baseYTop = bounds.Y + 0.5f * bounds.Height;
                        baseYBottom = bounds.Y + 0.9f * bounds.Height; ;
                        phase = MathHelper.Pi;
                        awakePower = 0.3f;
                    }
                    break;

                case 2:
                    {                        
                        baseYTop = baseYBottom = bounds.Y + bounds.Height - 0.5f * lineHeight;
                        awakePower = 0.6f;
                    }
                    break;

                default:
                    Debug.Assert(false, "Line index to hi: " + lineIndex);
                    break;
            }
            baseY = baseYTarget = baseYBottom;
            floatSpeed = (baseYTop - baseYBottom) / 20.0f;
        
            // init pills
            pillsCount = 0;
            float nextPillX = bounds.X;
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
            stateElapsedTime += dt;

            switch (state)
            {
                case State.SLEEPING:
                    break;

                case State.APPEARING:
                case State.FLOATING:
                    baseY += floatSpeed * dt;
                    if (baseY < baseYTarget)
                        baseY = baseYTarget;

                    if (state == State.APPEARING)
                    {
                        pillsCount = (int)(stateElapsedTime / Constants.PILL_SPAWN_TIMEOUT);
                        if (pillsCount > pills.Length)
                        {
                            pillsCount = pills.Length;
                            setState(State.FLOATING);
                        }
                    }
                    break;                    

                case State.DISSAPEARING:
                    throw new NotImplementedException(); // TODO: make'em dissapear                    

                default:
                    Debug.Assert(false, "Bad state: " + state);
                    break;
            }
            
            for (int pillIndex = 0; pillIndex < pills.Length; ++pillIndex)
            {
                Pill pill = pills[pillIndex];
                pill.lifeTime += dt;

                if (pillIndex >= pillsCount)
                {
                    continue;
                }

                if (!pill.active)
                {
                    if (pill.lifeTime < Constants.PILL_SPAWN_TIMEOUT)                        
                        continue; // wait a little before respawning

                    if (state == State.DISSAPEARING)
                        continue; // do not respawn on dissapear
                    
                    pill.active = true;
                    pill.lifeTime = 0.0f; // reset life time to break wave synchronization
                }
                                
                pill.y = baseY + amplitude * (float)(Math.Sin(omega * pill.lifeTime - k * pill.x + phase));

                if (PillsManager.sharedHero.collides(pill))
                {
                    PillsManager.sharedHero.eatPill(pill);
                    pill.active = false;
                    pill.lifeTime = 0.0f;                    
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

        public void heroPowerUpdated(float power)
        {
            switch (state)
            {
                case State.SLEEPING:
                    if (power >= awakePower)
                    {
                        setState(State.APPEARING);
                    }
                    break;

                case State.APPEARING:
                case State.FLOATING:
                    baseYTarget = baseYTop + (baseYBottom - baseYTop) * (1 - power);
                    break;

                case State.DISSAPEARING:
                    break;

                default:
                    Debug.Assert(false, "Bad state: " + state);
                    break;
            }            
        }       
 
        private void setState(State state)
        {
            this.state = state;
            stateElapsedTime = 0;
        }
    }

    public class Level1 : PillsManager,  HeroListener
    {
        private const int LINES_COUNT = 3;        

        private PillLine[] pillsLines;        
        private float elapsedTime;

        public Level1()
        {            
        }

        public override void init()
        {
            Rect lineBounds;
            lineBounds.Width = 0.8f * Bounds.Width;
            lineBounds.Height = 0.8f * Bounds.Height;
            lineBounds.X = 0.5f * (Bounds.Width - lineBounds.Width);
            lineBounds.Y = 0.5f * (Bounds.Height - lineBounds.Height);

            pillsLines = new PillLine[LINES_COUNT];
            for (int lineIndex = 0; lineIndex < pillsLines.Length; ++lineIndex)
            {                
                pillsLines[lineIndex] = new PillLine(ref lineBounds, lineIndex);                
            }
            sharedHero.addListener(this);            
        }

        protected override void updatePills(float dt)
        {
            elapsedTime += dt;
            sharedHero.power += 0.001f;
            if (sharedHero.power > 1)
            {
                sharedHero.power = 1;
            }
            else
            {
                heroPowerChanged(0, sharedHero.power);
            }

            for (int lineIndex = 0; lineIndex < pillsLines.Length; ++lineIndex)
            {
                pillsLines[lineIndex].update(dt);
            }            
        }       

        protected override void drawPills()
        {
            for (int lineIndex = 0; lineIndex < pillsLines.Length; ++lineIndex)
            {
                pillsLines[lineIndex].draw();
            }
        }

        #region HeroListener Members

        public void heroPowerChanged(float oldPower, float newPower)
        {            
            for (int lineIndex = 0; lineIndex < pillsLines.Length; ++lineIndex)
            {
                pillsLines[lineIndex].heroPowerUpdated(newPower);
            }
        }

        #endregion
    }
}
