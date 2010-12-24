using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.core;
using System.Diagnostics;
using Microsoft.Xna.Framework.Input;
using DuckstazyLive.game;
using Microsoft.Xna.Framework;

namespace DuckstazyLive.app
{
    public class IntroView : View
    {
        private const int STATE_INTRO = 0;
        private const int STATE_MENU = 1;
        private const int STATES_COUNT = 2;
        
        private StartupController controller;
        private int state;
        private float stateElapsedTime;

        private Env env;
        private Canvas canvas;

        public IntroView(StartupController controller)
        {
            this.controller = controller;
            env = Env.getIntance();
            env.day = true;

            canvas = new Canvas(width, height);
        }

        private void startState(int state)
        {
            this.state = state;
            switch (state)
            {
                case STATE_MENU:
                    env.blanc = 1.0f;
                    break;
            }
        }

        private void startNextState()
        {
            state++;
            stateElapsedTime = 0.0f;

            if (state == STATES_COUNT)
                controller.deactivate();
        }

        public override void update(float delta)
        {
            base.update(delta);
            env.update(delta, 0.0f);

            stateElapsedTime += delta;
            switch (state)
            {
                case STATE_INTRO:
                    if (stateElapsedTime > 3.0f)
                        startNextState();
                    break;               
            }
        }
                
        public override void draw()
        {
            switch(state)
            {
                case STATE_INTRO:
                    drawIntro();
                    break;
                case STATE_MENU:
                    drawMenu();
                    break;
                default:
                    Debug.Assert(false, "Wrong state: " + state);
                    break;
            }
        }

        private void drawIntro()
        {
            AppGraphics.FillRect(0, 0, Constants.SCREEN_WIDTH, Constants.SCREEN_HEIGHT, Color.White);
        }

        private void drawMenu()
        {
            env.draw1(canvas);
            env.draw2(canvas);
        }

        public override bool buttonPressed(ref ButtonEvent evt)
        {
            switch (evt.button)
            {
                case Buttons.A:
                case Buttons.Start:
                    startNextState();
                    return true;
            }

            return false;
        }
    }
}
