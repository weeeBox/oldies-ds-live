using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using DuckstazyLive.game.levels;

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

        private float moveSpeed = 100.0f;

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
                if (genCounter > 0.5f)
                {
                    genCounter = 0.0f;
                    startPill(generatedCount);
                    generatedCount++;
                }
            }
        }

        private void startPill(int pillIndex)
        {
            Pill pill = getPills().findDead();
            if (pill == null)
                return;

            pill.user = snakeCallback;

            int pillType = getPillType(pillIndex);

            Point startPoint = getNode(0);
            float px = startPoint.X;
            float py = startPoint.Y;           

            switch (pillType)
            {
                case POWER1:
                case POWER2:
                case POWER3:
                    pill.startPower(px, py, pillType - POWER1, false);
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

            pill.t1 = 0;
            pill.t2 = 0;
            setPillTargetNode(pill, 1);

            getPills().actives++;
        }

        private void setPillTargetNode(Pill pill, int nodeIndex)
        {
            Debug.Assert(nodeIndex > 0 && nodeIndex < getNodesCount());

            Point target = getNode(nodeIndex);

            Vector2 distance = new Vector2(target.X - pill.x, target.Y - pill.y);
            float travelTime = distance.Length() / moveSpeed;
            
            pill.vx = distance.X / travelTime;
            pill.vy = distance.Y / travelTime;

            pill.t1 = nodeIndex;
            pill.t2 = travelTime;
        }

        private int getPillTargetNode(Pill pill)
        {
            return (int)(pill.t1);
        }

        public void snakeCallback(Pill pill, String msg, float dt)
        {
            if (msg == null && pill.isAlive())
            {                
                if (pill.t2 > dt)
                {
                    pill.x += pill.vx * dt;
                    pill.y += pill.vy * dt;
                    pill.t2 -= dt;                    
                }
                else
                {
                    int targetNode = getPillTargetNode(pill);
                    Point target = getNode(targetNode);
                    pill.x = target.X;
                    pill.y = target.Y;

                    if (targetNode < getNodesCount() - 1)
                    {                        
                        setPillTargetNode(pill, targetNode + 1);
                    }
                    else
                    {
                        pill.kill();
                        pill.t2 = 0.0f;
                        getParticles().startAcid(pill.x, pill.y);
                    }                    
                }
            }
        }        

        private int getPillType(int pillIndex)
        {
            Debug.Assert(pillIndex >= 0 && pillIndex < getPillsCount());
            int patternIndex = pillIndex % pattern.Length;

            return pattern[patternIndex];
        }

        private int getNodesCount()
        {
            return nodes.Length;
        }

        private Point getNode(int nodeIndex)
        {
            Debug.Assert(nodeIndex >= 0 && nodeIndex < getNodesCount());
            return nodes[nodeIndex];
        }

        public int getPillsCount()
        {
            return segmentsCount * pattern.Length;
        }

        private Pills getPills()
        {
            return GameMgr.getInstance().getPills();
        }

        private Particles getParticles()
        {
            return GameMgr.getInstance().getParticles();
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
                new Point(0, 100),
                new Point(320, 100),
                new Point(320, 400),
                new Point(500, 400),
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
