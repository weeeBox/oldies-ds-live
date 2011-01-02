using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace DuckstazyLive.game.stages.story
{
    class Snake
    {
        public const byte POWER1 = 0;
        public const byte POWER2 = 1;
        public const byte POWER3 = 2;
        public const byte SCULL = 3;
        public const byte BANNED = 4;
        public const byte SLEEP = 5;
        public const byte MATRIX = 6;

        private byte[] pattern;
        private Point[] nodes;
        private int segmentsCount;

        private float genCounter;
        private int generatedCount;        

        public Snake(byte[] pattern, Point[] nodes, int segmentsCount)
        {
            this.pattern = pattern;
            this.nodes = nodes;
            this.segmentsCount = segmentsCount;
        }

        public void reset()
        {
            genCounter = 0.0f;
            generatedCount = 0;
        }

        public void update(float dt)
        {
            if (generatedCount < getPillsCount())
            {
                genCounter += dt;
                if (genCounter > 1.0f)
                {
                    genCounter = 0.0f;
                    startPill(generatedCount);
                    generatedCount++;
                }
            }
        }

        private void startPill(int pillIndex)
        {
            Pill pill = Pills.instance.findDead();
            if (pill == null)
                return;

            pill.init();

            int pillType = getPillType(pillIndex);
            float px = 100;
            float py = 100;

            switch (pillType)
            {
                case POWER1:
                case POWER2:
                case POWER3:
                    pill.startPower(px, py, pillType - POWER1, true);
                    break;
                case SCULL:
                    pill.startMissle(px, py, Pill.TOXIC_SKULL);
                    break;
                case BANNED:
                    pill.startMissle(px, py, Pill.TOXIC_FORBID);
                    break;
                case SLEEP:
                    pill.startSleep(px, py);
                    break;
                case MATRIX:
                    throw new NotImplementedException();                    
            }
        }

        private int getPillType(int pillIndex)
        {
            Debug.Assert(pillIndex >= 0 && pillIndex < getPillsCount());
            int patternIndex = pillIndex % pattern.Length;

            return pattern[patternIndex];
        }

        public int getPillsCount()
        {
            return segmentsCount * pattern.Length;
        }
    }

    public class Snakes : StoryLevelStage
    {
        private Snake[] snakes =
        {
            new Snake(new byte[] 
            {
                Snake.POWER1,
                Snake.POWER1,
                Snake.POWER1,
                Snake.SCULL,
            },
            new Point[]
            {
                new Point(0, 0),
                new Point(640, 0),
                new Point(640, 400)
            },10)
        };

        private int currentSnakeIndex;       

        public Snakes()
        {

        }

        public override void start()
        {
            base.start();

            currentSnakeIndex = 0;            
        }

        public override void update(float dt)
        {
            base.update(dt);

            Snake snake = getCurrentSnake();
            snake.update(dt);
        }

        protected override void startProgress()
        {
            progress.start(0, 0);
        }

        private void startSnake(int snakeIndex)
        {

        }

        private Snake getCurrentSnake()
        {
            return snakes[currentSnakeIndex];
        }

        private int getSnakesCount()
        {
            return snakes.Length;
        }
    }
}
