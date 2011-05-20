using System;

using System.Collections.Generic;


using asap.ui;
using asap.core;
using asap.app;
using asap.graphics;

namespace app.menu
{
    public class Screen : UiComponent, KeyListener, TickListener, TimerSource
    {
        public ScreenId id;        
        
        private TimerController timerController;
        
        private Button backButton;
        
        private ButtonListener backListener;
        
        private int backCode;
        
        public Screen(ScreenId id) : this(id, Application.Width, Application.Height)
        {
        }

        public Screen(ScreenId id, float width, float height) : base(width, height)
        {
            this.id = id;            
            timerController = new TimerController();
        }
        
        public virtual void Tick(float delta)
        {
            timerController.Tick(delta);
            Update(delta);
        }
        
        public virtual TimerController GetTimerController()
        {
            return timerController;
        }
        
        public virtual ScreenId GetId()
        {
            return id;
        }        
        
        protected UiComponent CreateTitleSafeContainer()
        {
            UiComponent container = new UiComponent(Application.Width - 2 * Application.TitleSafeBorderWidth, Application.Height - 2 * Application.TitleSafeBorderHeight);
            AddChild(container);
            AttachCenter(container);
            return container;
        }

        public virtual void OnScreenBack()
        {
        }
        
        public virtual void SetBackButton(Button backButton)
        {
            this.backButton = backButton;
        }
        
        public virtual void SetBackListener(ButtonListener listener, int code)
        {
            backListener = listener;
            backCode = code;
        }
        
        public virtual bool KeyPressed(KeyEvent evt)
        {
            if (evt.action == KeyAction.BACK) 
            {
                if ((backButton) != null) 
                {
                    backButton.Click();
                    return true;
                } 
                else if ((backListener) != null) 
                {
                    backListener.ButtonPressed(backCode);
                    return true;
                } 
            } 
            return false;
        }
        
        public virtual bool KeyReleased(KeyEvent evt)
        {
            return false;
        }
 
        public override FocusTraversalPolicy GetFocusTraversalPolicy()
        {
            if (focusTraversalPolicy != null)
                return focusTraversalPolicy;

            return InputManager.GetDefaultFocusTraversalPolicy();
        }
        
        public override HashSet<KeyCode> GetNextFocusKeyCodes()
        {
            if (nextFocusKeyCodes != null)
                return nextFocusKeyCodes;
        
            return InputManager.GetDefaultNextFocusKeyCodes();
    }
    
        public override HashSet<KeyCode> GetPrevFocusKeyCodes()
        {
            if (prevFocusKeyCodes != null)
                return prevFocusKeyCodes;
    
            return InputManager.GetDefaultPrevFocusKeyCodes();
        }
    }    
}