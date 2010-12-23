using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using DuckstazyLive.app;

namespace DuckstazyLive.game
{
    public class LevelProgress
    {
        private float progress;
        private float progressMax;
        private float percent;

        private bool play;
        private bool full;        

        public LevelProgress()
        {
            end();
        }

        public void start(float progressTime)
        {
            Debug.Assert(progressTime > 0);

            progress = 0.0f;
            progressMax = progressTime;
            play = true;
            full = false;
        }

        public void end()
        {
            percent = 0.0f;
            progress = 0.0f;
            progressMax = 0.0f;
            play = false;
            full = false;
        }       

        public void updateProgress(float newProgress)
        {
            if (play)
            {
                if (!full)
                {
                    progress = newProgress;
                    percent = progress / progressMax;
                    if (progress >= progressMax)
                    {
                        progress = progressMax;
                        percent = 1.0f;
                        full = true;
                    }
                }
            }
        }        

        public float getCompletePercent()
        {
            return percent;
        }

        public float getCurrentProgress()
        {
            return progress;
        }

        public float getMaxProgress()
        {
            return progressMax;
        }

        public bool isCompleted()
        {
            return full;
        }

        public bool isPlaying()
        {
            return play;
        }

        public void stop()
        {
            play = false;
        }
    }
}
