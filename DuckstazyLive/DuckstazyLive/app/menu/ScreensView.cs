using System;

using System.Collections.Generic;


using app;
using asap.ui;
using asap.core;
using asap.graphics;

namespace app.menu
{
    /** 
     * Provide stack of screens and view of active/visible screens
     */
    public class ScreensView : UiComponent, TickListener
    {
        private const int MAX_SCREENS_COUNT = 5;
        
        private Screen[] screensStack;
        
        private int screenIndex;
        
        private bool drawPrevScreen;
        
        public ScreensView() 
        {
            screensStack = new Screen[MAX_SCREENS_COUNT];
            screenIndex = 0;
        }
        
        public virtual void ClearScreens()
        {
            for (int i = 0; i < (screensStack.Length); i++) 
            {
                screensStack[i] = null;
            }
            screenIndex = 0;
        }
        
        public virtual void Tick(float delta)
        {
            if ((GetActiveScreen()) != null) 
            {
                GetActiveScreen().Tick(delta);
            } 
        }
        
        public virtual void StartScreen(Screen screen)
        {
            screensStack[screenIndex] = screen;
            Application.sharedScreenMgr.SetRoot(screen);
            Application.Instance.SetMainView(this);
        }
        
        public virtual void StartNextScreen(Screen screen)
        {
            System.Diagnostics.Debug.Assert((screenIndex) < ((MAX_SCREENS_COUNT) - 1));
            (screenIndex)++;
            StartScreen(screen);
        }
        
        public virtual void BackScreen()
        {
            System.Diagnostics.Debug.Assert((screenIndex) > 0);
            screensStack[screenIndex] = null;
            (screenIndex)--;
            Application.sharedScreenMgr.SetRoot(GetActiveScreen());
            Application.Instance.SetMainView(this);
            GetActiveScreen().OnScreenBack();
        }
        
        public virtual void BackToScreen(ScreenId screenId)
        {
            while (((screenIndex) > 0) && ((GetActiveScreen().GetId()) != screenId))
                BackScreen();
        }
        
        public virtual bool IsScreenInStack(ScreenId screenId)
        {
            for (int i = screenIndex; i >= 0; i--) 
            {
                if ((screensStack[i].GetId()) == screenId)
                    return true;
                
            }
            return false;
        }
        
        public override void Draw(Graphics g)
        {
            System.Diagnostics.Debug.Assert((GetActiveScreen()) != null);
            if ((drawPrevScreen) && ((screenIndex) > 0))
                screensStack[((screenIndex) - 1)].Draw(g);

            Screen screen = GetActiveScreen();
            GetActiveScreen().Draw(g);
        }
        
        public float GetHeight()
        {
            System.Diagnostics.Debug.Assert((GetActiveScreen()) != null);
            return GetActiveScreen().Height;
        }
        
        public float GetWidth()
        {
            System.Diagnostics.Debug.Assert((GetActiveScreen()) != null);
            return GetActiveScreen().Width;
        }
        
        public virtual void SetDrawPrevScreen(bool value)
        {
            drawPrevScreen = value;
        }
        
        public virtual Screen GetActiveScreen()
        {
            return screensStack[screenIndex];
        }        
    }    
}