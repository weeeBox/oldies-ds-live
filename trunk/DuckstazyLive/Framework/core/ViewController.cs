using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework.Input;

namespace Framework.core
{
    public enum ControllerState
    {
        CONTROLLER_DEACTIVE,
        CONTROLLER_ACTIVE,
        CONTROLLER_PAUSED
    }

    public abstract class ViewController
    {
        private const int DEFAULT_VIEWS_CAPACITY = 10;
        private const int DEFAULT_CHILDS_CAPACITY = 10;

        ControllerState controllerState;
        public int activeViewId;
        View[] views;

        int activeChildId;
        ViewController[] childs;
        public ViewController parent;
        int pausedViewId;

        public float delta;
        public float idealDelta;
        public float lastTime;

        int frames;
        float accumDt = 0;// TimeType accumDt
        float frameRate;

        public ViewController(ViewController p)
        {
            controllerState = ControllerState.CONTROLLER_DEACTIVE;
            views = new View[DEFAULT_VIEWS_CAPACITY];
            childs = new ViewController[DEFAULT_CHILDS_CAPACITY];
            activeViewId = FrameworkConstants.UNDEFINED;
            activeChildId = FrameworkConstants.UNDEFINED;
            pausedViewId = FrameworkConstants.UNDEFINED;
            parent = p;
            lastTime = -1;// DateTime.MinValue;// FrameworkConstants.UNDEFINED;
        }

        public virtual void activate()
        {
            Debug.Assert(controllerState == ControllerState.CONTROLLER_DEACTIVE);
            controllerState = ControllerState.CONTROLLER_ACTIVE;
            Application.sharedRootController.onControllerActivated(this);
        }

        public virtual void deactivate()
        {
            Debug.Assert(controllerState == ControllerState.CONTROLLER_PAUSED || controllerState == ControllerState.CONTROLLER_ACTIVE);
            controllerState = ControllerState.CONTROLLER_DEACTIVE;

            if (activeViewId != FrameworkConstants.UNDEFINED)
            {
                hideActiveView();
            }

            // notify parent controller
            //ASSERT_MSG(([parent activeChild] == self || parent == nil), @"Trying to deactivate child which is not marked as active by it's parent"); 
            Application.sharedRootController.onControllerDeactivated(this);
            parent.onChildDeactivated(parent.activeChildId);
        }

        public void pause()
        {
            Debug.Assert(controllerState == ControllerState.CONTROLLER_ACTIVE);
            controllerState = ControllerState.CONTROLLER_PAUSED;
            Application.sharedRootController.onControllerPaused(this);

            if (activeViewId != FrameworkConstants.UNDEFINED)
            {
                pausedViewId = activeViewId;
                hideActiveView();
            }
        }

        public void unpause()
        {
            Debug.Assert(controllerState == ControllerState.CONTROLLER_PAUSED);
            controllerState = ControllerState.CONTROLLER_ACTIVE;            

            if (activeChildId != FrameworkConstants.UNDEFINED)
            {
                activeChildId = FrameworkConstants.UNDEFINED;
            }

            Application.sharedRootController.onControllerUnpaused(this);

            if (pausedViewId != FrameworkConstants.UNDEFINED)
            {
                showView(pausedViewId);
            }
        }

        public virtual void update()
        {
            if (activeViewId == FrameworkConstants.UNDEFINED)
                return;

            View v = activeView();
            v.update(delta);
        }

        public void addViewWithId(View v, int n)
        {
            Debug.Assert(views[n] == null);
            views[n] = v;
        }

        public void deleteView(int n)
        {
            Debug.Assert(views[n] != null);
            views[n] = null;
        }

        public void showView(int n)
        {
            Debug.Assert(controllerState == ControllerState.CONTROLLER_ACTIVE);
            Debug.Assert(views[n] != null);

            // check that we don't activate already active view
            if (activeViewId != FrameworkConstants.UNDEFINED)
            {
                Debug.Assert(views[n] != views[activeViewId]); 
                hideActiveView();
            }

            activeViewId = n;
            View v = views[n];
            Application.sharedRootController.onControllerViewShow(v);
            v.onShow();
        }

        public View activeView()
        {
            Debug.Assert(activeViewId != FrameworkConstants.UNDEFINED);
            Debug.Assert(views[activeViewId] != null);
            View v = views[activeViewId];
            Debug.Assert(v != null);
            return v;
        }

        public View getView(int n)
        {
            return views[n];
        }

        public void addChildWithId(ViewController c, int n)
        {
            Debug.Assert(childs[n] == null);
            childs[n] = c;
        }

        public void deleteChild(int n)
        {
            Debug.Assert(childs[n] != null);
            childs[n] = null;
        }

        public void deactivateActiveChild()
        {
            ViewController prevC = childs[activeChildId];
            prevC.deactivate();
            activeChildId = FrameworkConstants.UNDEFINED;
        }

        public void activateChild(int n)
        {
            Debug.Assert(controllerState == ControllerState.CONTROLLER_ACTIVE);
            Debug.Assert(childs[n] != null);

            // check that we don't activate already active child
            if (activeChildId != FrameworkConstants.UNDEFINED)
            {
                Debug.Assert(childs[n] != childs[activeChildId]); 
                deactivateActiveChild();
            }

            pause();

            activeChildId = n;
            ViewController c = childs[n];
            c.activate();
        }

        public ViewController activeChild()
        {
            Debug.Assert(activeChildId != FrameworkConstants.UNDEFINED);
            Debug.Assert(childs[activeChildId] != null);
            ViewController c = childs[activeChildId];
            Debug.Assert(c != null);
            return c;
        }

        public ViewController getChild(int n)
        {
            return childs[n];
        }

        public virtual void onChildDeactivated(int n)
        {
            unpause();
        }

        public void calculateTimeDelta()
        {
            float time = GameClock.ElapsedTime;

            if (lastTime >= 0)
            {
                delta = time - lastTime;
            }
            else
            {
                delta = 0;
            }

            lastTime = time;
        }

        public void calculateFps()
        {
            frames++;
            accumDt += delta;

            if (accumDt > 0.1f)
            {
                frameRate = frames / accumDt;
                frames = 0;
                accumDt = 0;
            }
        }

        private void hideActiveView()
        {
            View prevV = views[activeViewId];
            Application.sharedRootController.onControllerViewHide(prevV);
            if (prevV != null)
                prevV.onHide();
            activeViewId = FrameworkConstants.UNDEFINED;
        }

        public virtual bool buttonPressed(ref ButtonEvent e)
        {
            if (activeViewId != FrameworkConstants.UNDEFINED)
            {
                return activeView().buttonPressed(ref e);
            }
            return false;
        }

        public virtual bool buttonReleased(ref ButtonEvent e)
        {
            if (activeViewId != FrameworkConstants.UNDEFINED)
            {
                return activeView().buttonReleased(ref e);
            }
            return false;
        }

        public virtual bool keyPressed(Keys key)
        {
            if (activeViewId != FrameworkConstants.UNDEFINED)
            {
                return activeView().keyPressed(key);
            }
            return false;
        }

        public virtual bool keyReleased(Keys key)
        {
            if (activeViewId != FrameworkConstants.UNDEFINED)
            {
                return activeView().keyReleased(key);
            }
            return false;
        }
    }
}
