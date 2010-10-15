using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

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

    public class RootController : ViewController
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
            transitionTime = Constants.UNDEFINED;
            previousView = null;
            transitionDelay = TRANSITION_DEFAULT_DELAY;            
        }

        public void processUpdate()
        {
            if (suspended)
                return;

            try
            {
                currentController.calculateTimeDelta();
                currentController.update();
            }
            catch (Exception e)
            {
                System.Console.WriteLine("Error on update: " + e);
                // TODO: show alert
                throw e;
            }
        }

        public void processDraw()
        {
            try
            {
                if (currentController.activeViewId != Constants.UNDEFINED)
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
            catch (Exception e)
            {
                System.Console.WriteLine("Error on draw: " + e);
                // TODO: show alert
                throw e;
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
    }
}
