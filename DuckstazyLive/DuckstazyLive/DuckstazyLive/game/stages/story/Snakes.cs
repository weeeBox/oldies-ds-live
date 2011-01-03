using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using DuckstazyLive.game.levels;
using Framework.core;
using DuckstazyLive.game.levels.fx;
using DuckstazyLive.game.stages.fx;

namespace DuckstazyLive.game.stages.story
{
    public class Snakes : PillCollectLevelStage
    {
        private struct HintInfo
        {
            public float angle;
            public uint color;
            public float x, y;
            public float timeout;

            public HintInfo(float x, float y, float angle, uint color, float timeout)
            {
                this.x = x;
                this.y = y;
                this.angle = angle;
                this.color = color;
                this.timeout = timeout;
            }
        }

        private Snake[] snakes =
        {
            // #1
            new Snake(new byte[] 
            {
                Snake.SCULL,
                Snake.POWER1,
                Snake.POWER1,
                Snake.POWER1,                
            },
            new Point[]
            {
                new Point(640, 50),
                new Point(0, 50),
                new Point(0, 380),
                new Point(640, 380),
                new Point(640, 50),
            }, 7, 200.0f, 0.3f, 2.0f),

            // #2
            new Snake(new byte[] 
            {                
                Snake.POWER2,                
            },
            new Point[]
            {
                new Point(0, 50),
                new Point(640, 50),                
            }, 20, 200.0f, 0.3f, 2.0f),

            // #3
            new Snake(new byte[] 
            {                
                Snake.POWER3,
            },
            new Point[]
            {
                new Point(680, 130),
                new Point(0, 130),                
            }, 19, 200.0f, 0.3f, 4.0f),

            // #4
            new Snake(new byte[] 
            {   
                Snake.SCULL,                
                Snake.POWER1,
                Snake.POWER2,
                Snake.POWER3,                
            },
            new Point[]
            {
                new Point(680, 210),
                new Point(0, 210),                
            }, 10, 200.0f, 0.3f, 4.0f)
        };

        private HintInfo[] hints =
        {
            new HintInfo(640, 300, 3.14f, 0xfff7a0e1, 4.0f),
            new HintInfo(680, 50, 3.14f * 0.5f, 0xfff7a0e1, 12.0f),
            new HintInfo(0, 130, 3.14f * 1.5f, 0xfff7a0e1, 12.0f),
            new HintInfo(680, 210, 3.14f * 0.5f, 0xfff7a0e1, 12.0f),
        };

        private int currentSnakeIndex;
        private int currentHintIndex;
        private float hintCounter;

        private HintArrow hintArrow;

        public Snakes() : base(0, 90)
        {
            hintArrow = new HintArrow(media);

            int collectableCount = 0;
            for (int snakeIndex = 0; snakeIndex < getSnakesCount(); ++snakeIndex)
            {
                collectableCount += snakes[snakeIndex].getCollectablePillsCount();
            }
            numPills = collectableCount;
        }

        public override void start()
        {
            base.start();

            currentSnakeIndex = -1;
            currentHintIndex = 0;
            hintCounter = 0.0f;
            startX = 0;

            startNextSnake();
            showHint(0);
        }

        public override void update(float dt)
        {
            base.update(dt);

            if (isPlaying())
            {
                Snake snake = getCurrentSnake();
                snake.update(dt);
                if (snake.isDone())
                {
                    startNextSnake();
                }
            }

            if (isHintVisible())
            {
                if (hintArrow.visible)
                {
                    hintCounter += dt;
                    if (hintCounter > hints[currentHintIndex].timeout)
                    {
                        hintArrow.visible = false;                        
                    }
                }
                else if (hintArrow.visibleCounter <= 0.0f)
                {
                    currentHintIndex++;
                    if (currentHintIndex < hints.Length)
                    {
                        showHint(currentHintIndex);
                        hintCounter = 0.0f;
                    }
                }

                hintArrow.update(dt);
            }            
        }

        public override void draw1(Canvas canvas)
        {
            if (isHintVisible())
            {            
                hintArrow.draw(canvas);
            }
        }

        private void startNextSnake()
        {            
            if (currentSnakeIndex < getSnakesCount() - 1)
            {             
                currentSnakeIndex++;
                snakes[currentSnakeIndex].start();
            }
        }

        private void showHint(int hintIndex)
        {
            Debug.Assert(hintIndex >= 0 && hintIndex < hints.Length);            

            float hx = hints[hintIndex].x;
            float hy = hints[hintIndex].y;
            float ha = hints[hintIndex].angle;
            uint hc = hints[hintIndex].color;

            hintArrow.place(hx, hy, ha, hc, true);
            hintArrow.visibleCounter = 0.0f;
            hintArrow.visible = true;
        }

        private bool isHintVisible()
        {
            return currentHintIndex < hints.Length;
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
