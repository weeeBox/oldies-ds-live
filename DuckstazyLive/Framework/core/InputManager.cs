using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace Framework.core
{
    public struct ButtonEvent
    {        
        public Buttons button;
        public GamePadThumbSticks thumbSticks;
        public GamePadTriggers triggers;

        public ButtonEvent(Buttons button, GamePadThumbSticks thumbSticks, GamePadTriggers triggers)
        {
            this.button = button;
            this.thumbSticks = thumbSticks;
            this.triggers = triggers;
        }
    }

    public interface InputDelegate
    {
        void buttonPressed(ButtonEvent e);
        void buttonReleased(ButtonEvent e);
    }

    public class InputManager
    {        
        private static Buttons[] CHECK_BUTTONS = 
        {
            Buttons.DPadUp,
            Buttons.DPadDown,
            Buttons.DPadLeft,
            Buttons.DPadRight,
            Buttons.Start,
            Buttons.Back,
            Buttons.LeftStick,
            Buttons.RightStick,
            Buttons.LeftShoulder,
            Buttons.RightShoulder,
            Buttons.BigButton,
            Buttons.A,
            Buttons.B,
            Buttons.X,
            Buttons.Y,
            Buttons.LeftThumbstickLeft,
            Buttons.RightTrigger,
            Buttons.LeftTrigger,
            Buttons.RightThumbstickUp,
            Buttons.RightThumbstickDown,
            Buttons.RightThumbstickRight,
            Buttons.RightThumbstickLeft,
            Buttons.LeftThumbstickUp,
            Buttons.LeftThumbstickDown,
            Buttons.LeftThumbstickRight,
        };

        private GamePadState currentState;
        private InputDelegate inputDelegate;       

        public InputManager()
        {            
            currentState = GamePad.GetState(PlayerIndex.One);
        }

        public void update()
        {
            GamePadState oldState = currentState;
            currentState = GamePad.GetState(PlayerIndex.One);

            if (inputDelegate != null)
            {
                for (int buttonIndex = 0; buttonIndex < CHECK_BUTTONS.Length; ++buttonIndex)
                {
                    Buttons button = CHECK_BUTTONS[buttonIndex];
                    if (isButtonDown(button, ref oldState, ref currentState))
                    {
                        inputDelegate.buttonPressed(new ButtonEvent(button, currentState.ThumbSticks, currentState.Triggers));
                    }
                    else if (isButtonUp(button, ref oldState, ref currentState))
                    {
                        inputDelegate.buttonReleased(new ButtonEvent(button, currentState.ThumbSticks, currentState.Triggers));
                    }
                }
            }
        }

        private bool isButtonDown(Buttons button, ref GamePadState oldState, ref GamePadState newState)
        {
            return newState.IsButtonDown(button) && oldState.IsButtonUp(button);
        }

        private bool isButtonUp(Buttons button, ref GamePadState oldState, ref GamePadState newState)
        {
            return newState.IsButtonUp(button) && oldState.IsButtonDown(button);
        }

        public InputDelegate InputDelegate
        {
            get { return inputDelegate; }
            set { inputDelegate = value; }
        }

        public GamePadThumbSticks ThumbSticks
        {
            get { return currentState.ThumbSticks; }
        }

        public GamePadTriggers Triggers
        {
            get { return currentState.Triggers; }
        }
    }
}
