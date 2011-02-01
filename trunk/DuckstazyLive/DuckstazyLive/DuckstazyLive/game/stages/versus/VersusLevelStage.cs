using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Framework.core;

namespace DuckstazyLive.game.stages.versus
{
    public class VersusLevelStage : LevelStage
    {
        protected enum State
        {
            PLAYING, // играем
            ENDED, // завершили игру
            LOOSE // оба игрока проиграли
        }

        private State state;
        protected VersusLevel level;

        private VersusProgress progress;

        public VersusLevelStage(VersusLevel level, float levelTime)
        {
            this.level = level;
            progress = new VersusProgress(levelTime);

            media = level.stageMedia;            
        }

        public override void onStart()
        {            
            base.onStart();

            setState(State.PLAYING);
            progress.start();
        }

        protected void stopPlaying()
        {            
            onStop();
        }

        public override void update(float dt)
        {
            base.update(dt);

            if (isPlaying())
            {
                updateProgress(dt);
                
                if (progress.isTimeUp())
                {
                    stopPlaying();
                }
            }
        }

        protected override void updateProgress(float dt)
        {
            progress.update(dt);
        }

        public override bool isPlaying()
        {
            return state == State.PLAYING;
        }

        public bool isEnded()
        {
            return state == State.ENDED;
        }

        public override void collectPill(Hero hero, Pill pill)
        {            
            if (pill.isPower())
            {
                hero.addPills(pill.scores);
            }            
        }

        public int getPillCollected(int playerIndex)
        {
            return getHero(playerIndex).pillsCollected;
        }

        protected void setState(State state)
        {
            this.state = state;
        }

        protected virtual void onStop()
        {
            setState(State.ENDED);
        }

        public float getRemainingTime()
        {
            return progress.getLevelTime() - progress.getElapsedTime();
        }
    }
}
