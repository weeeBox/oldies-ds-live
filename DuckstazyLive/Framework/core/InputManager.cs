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

    public interface InputListener
    {
        void buttonPressed(ButtonEvent e);
        void buttonReleased(ButtonEvent e);    
        void keyPressed(Keys key);
        void keyReleased(Keys key);
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

        private GamePadState currentGamepadState;
        private KeyboardState currentKeyboardState;
        private List<InputListener> inputListeners;

        public InputManager()
        {            
            currentGamepadState = GamePad.GetState(PlayerIndex.One);
            currentKeyboardState = Keyboard.GetState();
            inputListeners = new List<InputListener>();
        }

        public void update()
        {
            updateGamepad();
#if WINDOWS
            updateKeyboard();
#endif
        }

        private void updateGamepad()
        {
            GamePadState oldState = currentGamepadState;
            currentGamepadState = GamePad.GetState(PlayerIndex.One);

            if (inputListeners.Count > 0)
            {
                for (int buttonIndex = 0; buttonIndex < CHECK_BUTTONS.Length; ++buttonIndex)
                {
                    Buttons button = CHECK_BUTTONS[buttonIndex];
                    if (isButtonDown(button, ref oldState, ref currentGamepadState))
                    {
                        ButtonEvent e = new ButtonEvent(button, currentGamepadState.ThumbSticks, currentGamepadState.Triggers);
                        fireButtonPressed(ref e);
                    }
                    else if (isButtonUp(button, ref oldState, ref currentGamepadState))
                    {
                        ButtonEvent e = new ButtonEvent(button, currentGamepadState.ThumbSticks, currentGamepadState.Triggers);
                        fireButtonReleased(ref e);
                    }
                }
            }
        }

        private void updateKeyboard()
        {
            KeyboardState oldState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();

            if (inputListeners.Count > 0)
            {
                Keys[] oldKeys = oldState.GetPressedKeys();
                Keys[] newKeys = currentKeyboardState.GetPressedKeys();

                for (int i = 0; i < newKeys.Length; ++i)
                {
                    if (!oldKeys.Contains(newKeys[i]))
                    {
                        fireKeyPressed(newKeys[i]);
                    }
                }
                for (int i = 0; i < oldKeys.Length; ++i)
                {
                    if (!newKeys.Contains(oldKeys[i]))
                    {
                        fireKeyReleased(oldKeys[i]);
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

        public void addInputListener(InputListener listener)
        {
            inputListeners.Add(listener);
        }

        public void removeInputListener(InputListener listener)
        {
            inputListeners.Remove(listener);
        }

        private void fireKeyPressed(Keys key)
        {
            foreach (InputListener l in inputListeners)
            {
                l.keyPressed(key);
            }
        }

        private void fireKeyReleased(Keys key)
        {
            foreach (InputListener l in inputListeners)
            {
                l.keyReleased(key);
            }
        }

        private void fireButtonPressed(ref ButtonEvent e)
        {
            foreach (InputListener l in inputListeners)
            {
                l.buttonPressed(e);
            }
        }

        private void fireButtonReleased(ref ButtonEvent e)
        {
            foreach (InputListener l in inputListeners)
            {
                l.buttonReleased(e);
            }
        }

        public GamePadThumbSticks ThumbSticks
        {
            get { return currentGamepadState.ThumbSticks; }
        }

        public GamePadTriggers Triggers
        {
            get { return currentGamepadState.Triggers; }
        }

        public bool isKeyPressed(Keys key)
        {
            return currentKeyboardState.IsKeyDown(key);
        }
    }
}
