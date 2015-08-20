using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Framework.core;
using System.Diagnostics;
using Microsoft.Xna.Framework.Input;

namespace Framework.visual
{
    public interface ButtonDelegate
    {
        void onButtonPressed(int id, int playerIndex);
        void onButtonFocused(int id);
        void onButtonDefocused(int id);
    }

    public abstract class AbstractButton : BaseElementContainer
    {        
        protected const int BUTTON_NORMAL = 0;        
        protected const int BUTTON_FOCUSED = 1;

        protected int state;

        public ButtonDelegate buttonDelegate;
        protected int buttonID;               
        
        public AbstractButton(int n, float x, float y, int width, int height) : base(x, y, width, height)
        {
            setFocusable(true);

            buttonID = n;            
            setState(BUTTON_NORMAL);
        }
       
        public override bool isAcceptingInput()
        {
            return base.isAcceptingInput() && state == BUTTON_FOCUSED;
        }

        public void setState(int state)
        {
            Debug.Assert(state == BUTTON_NORMAL || state == BUTTON_FOCUSED);
            this.state = state;            
        }        

        public override bool buttonPressed(ref ButtonEvent e)
        {
            if (e.button == Buttons.A)
            {                
                Debug.Assert(isFocused());
                Debug.Assert(state == BUTTON_FOCUSED);
                if (buttonDelegate != null)
                    buttonDelegate.onButtonPressed(buttonID, e.playerIndex);
                return true;
            }
            return false;
        }        
        
        protected override void focusLost()
        {
            if (state == BUTTON_FOCUSED)
            {                
                setState(BUTTON_NORMAL);
                if (buttonDelegate != null)
                    buttonDelegate.onButtonDefocused(buttonID);                
            }            
        }

        protected override void focusGained()
        {
            if (state == BUTTON_NORMAL)
            {
                setState(BUTTON_FOCUSED);
                if (buttonDelegate != null)
                    buttonDelegate.onButtonFocused(buttonID);
            }
        }
    }
}