using System;
using asap.core;
using asap.graphics;
using asap.ui;
using asap.resources;
using asap.app;
using Microsoft.Xna.Framework;

namespace app.menu
{
    public class Button : UiComponent, KeyListener, PointerListener, Focusable
    {
        private bool focused = false;
        
        private bool isPressed;               
        
        public int code;
        
        public ButtonListener listener;
        
        public String text;
        
        public GameTexture image;        
        
        public Button(int code, ButtonListener listener)
        {
            this.code = code;
            this.listener = listener;            
        }        
        
        public virtual void SetText(String text)
        {
            this.text = text;
        }
        
        public virtual void SetCode(int code)
        {
            this.code = code;
        }     
        
        public virtual void PointerPressed(int x, int y, int fingerId)
        {
            if (IsEnabled()) 
            {
                isPressed = true;                
            } 
        }
        
        public virtual void PointerReleased(int x, int y, int fingerId)
        {
            if (IsEnabled()) 
            {
                if (isPressed) 
                {
                    isPressed = false;
                    Click();
                } 
            } 
        }
        
        public virtual void PointerDragged(int x, int y, int fingerId)
        {
        }
        
        public virtual void PointerEntered(int x, int y, int fingerId)
        {
            if (IsEnabled())
                isPressed = true;
            
        }
        
        public virtual void PointerExited(int x, int y, int fingerId)
        {
            if (IsEnabled())
                isPressed = false;
            
        }
        
        public virtual bool IsPressedState()
        {
            return (focused) || (isPressed);
        }        
                
        public virtual void Click()
        {
            if (listener != null)
            {
                listener.ButtonPressed(code);
            }
        }        

        public virtual bool KeyPressed(KeyEvent evt)
        {
            if (evt.action == KeyAction.OK) 
            {
                Click();
                return true;
            } 
            return false;
        }

        public virtual bool KeyReleased(KeyEvent evt)
        {
            return false;
        }

        public bool CanAcceptFocus()
        {
            return IsEnabled() & IsVisible();
        }

        public bool IsFocused()
        {
            return focused;
        }

        public virtual void FocusGained()
        {
            focused = true;
        }

        public virtual void FocusLost()
        {
            focused = false;
        }        
    }    
}