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
        public int viewsPointer;        
        View[] views;

        int activeChildId;
        ViewController[] childs;
        public ViewController parent;        

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
            viewsPointer = -1;
            activeChildId = FrameworkConstants.UNDEFINED;            
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

            clearViews();

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
        }

        public void unpause()
        {
            Debug.Assert(controllerState == ControllerState.CONTROLLER_PAUSED);
            controllerState = ControllerState.CONTROLLER_ACTIVE;            
           
            activeChildId = FrameworkConstants.UNDEFINED;
            Application.sharedRootController.onControllerUnpaused(this);           
        }

        public virtual void update()
        {
            for (int viewIndex = 0; viewIndex <= viewsPointer; ++viewIndex)
            {
                View v = views[viewIndex];
                if (v.isActive() || v.isUpdateInnactive())
                {
                    v.update(delta);
                }               
            }
        }

        public virtual void processDraw()
        {
            for (int viewIndex = 0; viewIndex <= viewsPointer; ++viewIndex)
            {
                View v = views[viewIndex];
                if (v.isActive() || v.isDrawInnactive())
                {
                    v.draw();
                }
            }             
        }

        public void showView(View v)
        {
            if (viewsPointer == -1)
            {
                viewsPointer = 0;                
            }
            else if (views[viewsPointer] != null)
            {
                views[viewsPointer].onHide();
            }
            views[viewsPointer] = v;            
            v.onShow();            
        }

        public void showNextView(View v)
        {
            Debug.Assert(viewsPointer < views.Length - 1);
            if (viewsPointer >= 0)
            {
                View view = views[viewsPointer];
                view.onOvertop();
            }
            viewsPointer++;
            showView(v);
        }

        public View getActiveView()
        {
            Debug.Assert(viewsPointer != -1);
            return views[viewsPointer];
        }

        public void hideView()
        {
            Debug.Assert(viewsPointer >= 0);            
            View view = views[viewsPointer];
            view.onHide();
            views[viewsPointer] = null;
            viewsPointer--;
            if (viewsPointer >= 0)
            {
                views[viewsPointer].onReveal();
            }
        }        

        public void clearViews()
        {
            for (int i = 0; i <= viewsPointer; ++i)
            {
                views[i] = null;
            }
            viewsPointer = 0;
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

        public int getActiveChildId()
        {
            return activeChildId;
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

        public virtual bool buttonPressed(ref ButtonEvent e)
        {
            if (viewsPointer != -1)
            {
                View topView = views[viewsPointer];
                topView.buttonPressed(ref e);                
            }
            return false;
        }

        public virtual bool buttonReleased(ref ButtonEvent e)
        {
            if (viewsPointer != -1)
            {
                View topView = views[viewsPointer];
                topView.buttonReleased(ref e);
            }
            return false;
        }
        
        public virtual void controllerConnected(int playerIndex)
        {

        }

        public virtual void controllerDisconnected(int playerIndex)
        {

        }
    }
}
