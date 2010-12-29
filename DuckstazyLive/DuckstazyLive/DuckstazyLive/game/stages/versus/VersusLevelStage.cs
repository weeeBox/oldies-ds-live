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
        private int[] collected;
        protected VersusLevel level;

        private VersusProgress progress;

        public VersusLevelStage(VersusLevel level, float levelTime)
        {
            this.level = level;
            progress = new VersusProgress(levelTime);

            media = level.stageMedia;
            pills = level.pills;
            particles = level.pills.ps;
            heroes = level.heroes;

            collected = new int[Application.sharedInputMgr.getPlayersCount()];
        }

        public override void start()
        {
            for (int i = 0; i < collected.Length; ++i)
            {
                collected[i] = 0;
            }            
            
            base.start();

            setState(State.PLAYING);
            progress.start();
        }

        protected void stop()
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
                    stop();
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
            int heroIndex = hero.getPlayerIndex();
            Debug.Assert(heroIndex >= 0 && heroIndex < collected.Length);
            collected[heroIndex]++;            
        }

        public int getPillCollected(int playerIndex)
        {
            Debug.Assert(playerIndex >= 0 && playerIndex < collected.Length);
            return collected[playerIndex];
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
