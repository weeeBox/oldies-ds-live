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
        public enum State
        {
            SLEEPING,
            APPEARING,
            FLOATING,
            DISSAPEARING,
        }

        private const int PILLS_IN_LINE_COUNT = 15;
        private static byte[] JUMPER_PATTERNS =
        {
            0, 1, 0, 0, 1, 1, 0, 0, 1, 0, 1, 0, 0, 0, 0,
            1, 0, 1, 0, 1, 1, 1, 0, 0, 0, 1, 0, 1, 0, 0,
            1, 0, 0, 1, 1, 0, 0, 1, 0, 0, 1, 1, 0, 0, 1,
            0, 1, 0, 0, 1, 0, 0, 1, 1, 0, 0, 1, 1, 0, 0,
            0, 1, 1, 0, 1, 1, 0, 1, 0, 0, 1, 0, 0, 1, 1,
            1, 0, 1, 0, 1, 0, 0, 1, 0, 1, 1, 0, 0, 1, 0,            
        };                

        private float stateElapsedTime;        

        public float baseY; // y-координата центра колебаний
        private float baseYTop; // высшая точка (hero.awakePower = 1.0f)
        private float baseYBottom; // низшая точка (hero.awakePower = 0);
        private float baseYTarget; // y-координата центра колбеаний, к которой необходимо переместиться
        private float floatSpeed; // скорость "всплытия" полосы по мере увеличения hero awakePower

        private float omega; // угловая скорость
        private float k; // волновое число
        private float amplitude; // амплитуда синусоидальных колебаний
        private double phase; // фаза колебаний        

        private int pillsCount; // количество активных pill'сов
        public State state;

        public Pill[] pills;
        public PillLine(int lineIndex)
        {
            Rect bounds = Level.levelBounds;

            float lineWidth = 0.8f * bounds.Width;
            float lineHeight = 0.1f * bounds.Height;            
            
            state = State.SLEEPING;
            int jumperPatternIndex = -1;

            // init base cords
            switch (lineIndex)
            {
                case 0:
                    {
                        baseYTop = bounds.Height - Level.sharedHero.JUMP_HEIGHT_MAX;
                        baseYBottom = bounds.Height - Level.sharedHero.JUMP_HEIGHT_MIN;                        
                        state = State.APPEARING;
                    }
                    break;

                case 1:
                    {
                        baseYTop = bounds.Height - 0.5f * (Level.sharedHero.JUMP_HEIGHT_MAX + lineHeight);
                        baseYBottom = bounds.Height - Level.sharedHero.JUMP_HEIGHT_MIN;
                        phase = MathHelper.Pi;
                        jumperPatternIndex = new Random().Next() % (JUMPER_PATTERNS.Length / PILLS_IN_LINE_COUNT);
                    }
                    break;

                case 2:
                    {
                        baseYTop = baseYBottom = bounds.Height - lineHeight;
                        jumperPatternIndex = new Random().Next() % (JUMPER_PATTERNS.Length / PILLS_IN_LINE_COUNT);
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
            float nextPillX = bounds.X + 0.5f * (bounds.Width - lineWidth);
            pills = new Pill[PILLS_IN_LINE_COUNT];
            float stepX = lineWidth / (float)(pills.Length - 1);            
            for (int pillIndex = 0; pillIndex < pills.Length; ++pillIndex)
            {
                pills[pillIndex] = Level.Pool.allocatePill(nextPillX, 0);
                nextPillX += stepX;
            }

            if (jumperPatternIndex != -1)
            {
                for (int pillIndex = 0; pillIndex < pills.Length; ++pillIndex)
                {
                    pills[pillIndex].jumper = JUMPER_PATTERNS[PILLS_IN_LINE_COUNT * jumperPatternIndex + pillIndex] != 0;
                }
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
                    if (pill.lifeTime < getSpawnTimeout())                        
                        continue; // wait a little before respawning

                    if (Level.sharedHero.collides(pill))                    
                        continue; // don't let hero eat a pill again                    

                    if (state == State.DISSAPEARING)
                        continue; // do not respawn on dissapear
                    
                    pill.active = true;
                    pill.lifeTime = 0.0f; // reset life time to break wave synchronization
                }
                                
                pill.y = baseY + amplitude * (float)(Math.Sin(omega * pill.lifeTime - k * pill.x + phase));

                if (Level.sharedHero.collides(pill))
                {
                    Level.sharedHero.onPillCollide(pill);
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
 
        public void setState(State state)
        {
            this.state = state;
            stateElapsedTime = 0;
        }

        private float getSpawnTimeout()
        {
            return MathHelper.Lerp(Constants.PILL_RESPAWN_TIMEOUT_MAX, Constants.PILL_RESPAWN_TIMEOUT_MIN, Level.sharedHero.power);
        }
    }

    public class Level1 : Level,  HeroListener
    {
        private const int LINES_COUNT = 3;        

        private PillLine[] pillsLines;        
        private float elapsedTime;

        public Level1(Hero hero) : base(hero)
        {
        }

        public override void init()
        {
            pillsLines = new PillLine[LINES_COUNT];
            for (int lineIndex = 0; lineIndex < pillsLines.Length; ++lineIndex)
            {                
                pillsLines[lineIndex] = new PillLine(lineIndex);                
            }
            sharedHero.addListener(this);            
        }

        protected override void updateLevel(float dt)
        {
            elapsedTime += dt;
            for (int lineIndex = 0; lineIndex < pillsLines.Length; ++lineIndex)
            {
                pillsLines[lineIndex].update(dt);
            }            

            if (pillsLines[0].baseY < 0.6f * Level.levelBounds.Height && pillsLines[1].state == PillLine.State.SLEEPING)
            {
                pillsLines[1].setState(PillLine.State.APPEARING);
            }
            else if (pillsLines[1].baseY < 0.7f * Level.levelBounds.Height && pillsLines[2].state == PillLine.State.SLEEPING)
            {
                pillsLines[2].setState(PillLine.State.APPEARING);
            }
        }       

        protected override void drawLevel()
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
