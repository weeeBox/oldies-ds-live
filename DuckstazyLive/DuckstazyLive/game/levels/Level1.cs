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

namespace DuckstazyLive.game.levels
{
    public sealed class PillLine
    {
        private float elapsedTime;
        private Rectangle bounds;       

        private float lineBaseY; // y-координата центра колебаний

        private float omega; // угловая скорость
        private float k; // волновое число
        private float amplitude; // амплитуда синусоидальных колебаний

        public Pill[] pills;
        public PillLine(Rectangle bounds, int pillsCount)
        {
            this.bounds = bounds;
            float stepX = bounds.Width / (float)(pillsCount - 1);
            float nextPillX = bounds.X;
            lineBaseY = bounds.Y + 0.5f * bounds.Height;
            pills = new Pill[pillsCount];
            for (int pillIndex = 0; pillIndex < pillsCount; ++pillIndex)
            {               
                pills[pillIndex] = PillsManager.Pool.allocatePill(nextPillX, lineBaseY);
                nextPillX += stepX;
            }

            k = (float)(MathHelper.TwoPi / bounds.Width);
            amplitude = 0.5f * bounds.Height;
            omega = (float)MathHelper.Pi;
        }

        public void update(float dt)
        {
            elapsedTime += dt;
            Hero hero = PillsManager.Hero;
            for (int pillIndex = 0; pillIndex < pills.Length; ++pillIndex)
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
                                
                pill.y = lineBaseY + amplitude * (float)(Math.Sin(omega * pill.lifeTime - k * pill.x));

                if (CollisionHelper.collidesRectVsCircle(hero.x, hero.y, hero.width, hero.height, pill.x, pill.y, Constants.PILL_RADIUS))
                    pill.active = false;
            }
        }

        public void draw()
        {
            for (int pillIndex = 0; pillIndex < pills.Length; ++pillIndex)
            {
                Pill pill = pills[pillIndex];
                if (pill.active)
                {
                    pill.draw();
                }                
            }
        }
    }

    public class Level1 : PillsManager
    {
        private const int LINES_COUNT = 3;
        private const int PILLS_IN_LINE_COUNT = 15;

        private PillLine[] pillsLines;

        public Level1()
        {            
        }

        public override void init()
        {
            int lineWidth = (int)(0.8f * Bounds.Width);
            int lineHeight = Constants.PILL_RADIUS;
            int lineX = (int)(0.5f * (Bounds.Width - lineWidth));
            int lineY = (int)(0.8f * Bounds.Height);
            Rectangle pillsBounds = new Rectangle(lineX, lineY, lineWidth, lineHeight);

            pillsLines = new PillLine[LINES_COUNT];
            for (int lineIndex = 0; lineIndex < pillsLines.Length; ++lineIndex)
            {
                pillsLines[lineIndex] = new PillLine(pillsBounds, PILLS_IN_LINE_COUNT);
                pillsBounds.Y -= 100;
            }
        }

        protected override void updatePills(float dt)
        {            
            float k = MathHelper.TwoPi / Bounds.Width;
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
    }
}
