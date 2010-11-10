using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework.Input;

namespace Framework.core
{
    public enum Transition
    {
        TRANSITION_NONE,
        TRANSITION_SLIDE_HORIZONTAL_RIGHT,
        TRANSITION_SLIDE_HORIZONTAL_LEFT,
        TRANSITION_SLIDE_VERTICAL,
        TRANSITION_FADE_OUT_BLACK,
        TRANSITION_FADE_OUT_WHITE,
        TRANSITIONS_COUNT
    }

    public class RootController : ViewController, InputListener
    {
        public const float TRANSITION_DEFAULT_DELAY = 0.4f;

        public ViewController currentController;
        public Transition viewTransition;
        float transitionTime; // CFAbsoluteTime
        View previousView;
        public float transitionDelay;
        bool suspended;

        public RootController(ViewController p) : base(p)
        {
            viewTransition = Transition.TRANSITION_NONE;
            transitionTime = FrameworkConstants.UNDEFINED;
            previousView = null;
            transitionDelay = TRANSITION_DEFAULT_DELAY;            
        }

        public void processUpdate()
        {
            if (suspended)
                return;
            
            currentController.calculateTimeDelta();
            currentController.update();            
        }

        public void processDraw()
        {            
            if (currentController.activeViewId != FrameworkConstants.UNDEFINED)
            {
                if (transitionTime < 0)
                {
                    currentController.activeView().draw();
                }
                else
                {
                    drawViewTransition();

                    if (currentController.lastTime > transitionTime)
                    {
                        transitionTime = -1;// DateTime.MinValue; //UNDEFINED;
                    }
                }
            }
        }

        public void setViewTransition(Transition transition)
        {
            // TODO
        }

        public void setViewTransitionDelay(float delay)
        {
            // TODO
        }

        public void drawViewTransition()
        {
            // TODO
        }

        public void onControllerActivated(ViewController c)
        {
            setCurrentController(c);
        }

        public void onControllerDeactivated(ViewController c)
        {
            setCurrentController(null);
        }

        public void onControllerPaused(ViewController c)
        {
            setCurrentController(null);
        }

        public void onControllerUnpaused(ViewController c)
        {
            setCurrentController(c);
        }

        public void onControllerViewShow(View v)
        {
            if (viewTransition != Transition.TRANSITION_NONE && previousView != null)
            {
                currentController.calculateTimeDelta();
                transitionTime = currentController.lastTime + transitionDelay;//DateTimeHelper.toTimeSpan(transitionDelay);
            }
        }

        public void onControllerViewHide(View v)
        {
            previousView = v;
        }

        public void setCurrentController(ViewController c)
        {
            currentController = c;
            if (c != null)
            {
                float fps = Application.sharedAppSettings.getValue(ApplicationSettings.APP_SETTING_FPS);
                currentController.idealDelta = 1.0f / fps;// new TimeSpan((long)(1.0 / fps * 10000));//(TimeType)1.0 / fps;
            }
        }

        public void suspend()
        {
            Debug.Assert(!suspended);
            suspended = true;
        }

        public void resume()
        {
            Debug.Assert(suspended);
            suspended = false;
        }

        public override void buttonPressed(ref ButtonEvent e)
        {
            if (currentController != null)
            {
                currentController.buttonPressed(ref e);
            }
        }

        public override void buttonReleased(ref ButtonEvent e)
        {
            if (currentController != null)
            {
                currentController.buttonReleased(ref e);
            }
        }

        public override void keyPressed(Keys key)
        {
            if (currentController != null)
            {
                currentController.keyPressed(key);
            }
        }

        public override void keyReleased(Keys key)
        {
            if (currentController != null)
            {
                currentController.keyReleased(key);
            }
        }
    }
}
