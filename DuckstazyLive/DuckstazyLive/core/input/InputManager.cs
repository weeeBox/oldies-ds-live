using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace DuckstazyLive.core.input
{
    public class InputManager
    {
        private KeyboardState keyboardState;
        private GamePadState gamePadState;
        private List<InputListener> listeners;

        private readonly Buttons[] gamePadButtons = new Buttons[]
        {
            Buttons.A,
            Buttons.B,
            Buttons.X,
            Buttons.Y,
            Buttons.Start,
            Buttons.Back,
            Buttons.LeftShoulder,
            Buttons.RightShoulder,
            Buttons.LeftStick,
            Buttons.RightStick,
            Buttons.BigButton,
            Buttons.DPadUp,
            Buttons.DPadDown,
            Buttons.DPadLeft,
            Buttons.DPadRight,
        };

        public InputManager()
        {
            keyboardState = Keyboard.GetState();
            gamePadState = GamePad.GetState(PlayerIndex.One);
            listeners = new List<InputListener>();
        }

        public void Update()        
        {
            UpdateKeyboard();
            UpdateGamePad();
        }

        public float LeftThumbStickX
        {
            get { return gamePadState.ThumbSticks.Left.X; }
        }

        public float LeftThumbStickY
        {
            get { return gamePadState.ThumbSticks.Left.Y; }
        }

        public float RightThumbStickX
        {
            get { return gamePadState.ThumbSticks.Right.X; }
        }

        public float RightThumbStickY
        {
            get { return gamePadState.ThumbSticks.Right.Y; }
        }

        private void UpdateKeyboard()
        {
            KeyboardState lastKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();           
        }

        private void UpdateGamePad()
        {
            GamePadState lastGamePadState = gamePadState;
            gamePadState = GamePad.GetState(PlayerIndex.One);

            // update buttons
            foreach (Buttons button in gamePadButtons)
            {
                if (lastGamePadState.IsButtonUp(button))
                {
                    if (gamePadState.IsButtonDown(button))
                    {
                        FireButtonDown(button);
                    }
                }
                else
                {
                    if (gamePadState.IsButtonUp(button))
                    {
                        FireButtonUp(button);
                    }
                }
            }                       
        }        

        public void AddInputListener(InputListener l)
        {
            if (!listeners.Contains(l))
                listeners.Add(l);
        }

        public void RemoveInputListener(InputListener l)
        {
            listeners.Remove(l);
        }

        private void FireButtonDown(Buttons button)
        {
            foreach (InputListener l in listeners)
            {
                l.ButtonDown(button);
            }
        }

        private void FireButtonUp(Buttons button)
        {
            foreach (InputListener l in listeners)
            {
                l.ButtonUp(button);
            }
        }
    }
}
