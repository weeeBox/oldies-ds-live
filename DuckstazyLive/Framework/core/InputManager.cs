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
        public int playerIndex;

        public ButtonEvent(int playerIndex, Buttons button, GamePadThumbSticks thumbSticks, GamePadTriggers triggers)
        {
            this.playerIndex = playerIndex;
            this.button = button;
            this.thumbSticks = thumbSticks;
            this.triggers = triggers;
        }
    }

    public interface InputListener
    {
        void buttonPressed(ref ButtonEvent e);
        void buttonReleased(ref ButtonEvent e);    
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

        private GamePadState[] currentGamepadStates;
        private KeyboardState currentKeyboardState;
        private List<InputListener> inputListeners;

        private Dictionary<Keys, Buttons>[] buttonsMappings;

        private GamePadDeadZone deadZone;

        public InputManager(int playersCount)
        {
            initButtonsMapping(playersCount);

            currentGamepadStates = new GamePadState[playersCount];
            deadZone = GamePadDeadZone.Circular;

            for (int i = 0; i < playersCount; ++i)
            {
                PlayerIndex player = getPlayer(i);
                currentGamepadStates[i] = GamePad.GetState(player, deadZone);
            }
             
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
            for (int playerIndex = 0; playerIndex < getPlayersCount(); ++playerIndex)
            {
                updateGamepad(playerIndex);
            }
        }

        private void updateGamepad(int playerIndex)
        {
            GamePadState oldState = currentGamepadStates[playerIndex];
            currentGamepadStates[playerIndex] = GamePad.GetState(getPlayer(playerIndex), deadZone);

            if (inputListeners.Count > 0)
            {
                for (int buttonIndex = 0; buttonIndex < CHECK_BUTTONS.Length; ++buttonIndex)
                {
                    Buttons button = CHECK_BUTTONS[buttonIndex];
                    if (isButtonDown(button, ref oldState, ref currentGamepadStates[playerIndex]))
                    {
                        ButtonEvent e = makeButtonEvent(playerIndex, button);
                        fireButtonPressed(ref e);
                    }
                    else if (isButtonUp(button, ref oldState, ref currentGamepadStates[playerIndex]))
                    {
                        ButtonEvent e = makeButtonEvent(playerIndex, button);
                        fireButtonReleased(ref e);
                    }
                }
            }
        }

        public ButtonEvent makeButtonEvent(int playerIndex, Buttons button)
        {
            Debug.Assert(playerIndex >= 0 && playerIndex < getPlayersCount());
            return new ButtonEvent(playerIndex, button, currentGamepadStates[playerIndex].ThumbSticks, currentGamepadStates[playerIndex].Triggers);
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
                l.buttonPressed(ref e);
            }
        }

        private void fireButtonReleased(ref ButtonEvent e)
        {
            foreach (InputListener l in inputListeners)
            {
                l.buttonReleased(ref e);
            }
        }

        public GamePadThumbSticks ThumbSticks()
        {
            return ThumbSticks(0);
        }

        public GamePadThumbSticks ThumbSticks(int playerIndex)
        {
            Debug.Assert(playerIndex >= 0 && playerIndex < getPlayersCount());
            return currentGamepadStates[playerIndex].ThumbSticks;
        }

        public GamePadTriggers Triggers()
        {
            return Triggers(0);
        }

        public GamePadTriggers Triggers(int playerIndex)
        {
            Debug.Assert(playerIndex >= 0 && playerIndex < getPlayersCount());
            return currentGamepadStates[playerIndex].Triggers; 
        }

        public bool isKeyPressed(Keys key)
        {
            return currentKeyboardState.IsKeyDown(key);
        }

        

        public bool hasMappedButton(Keys key, int playerIndex)
        {
            Dictionary<Keys, Buttons> mapping = getButtonsMapping(playerIndex);
            return mapping.ContainsKey(key);
        }

        public Buttons getMappedButton(Keys key, int playerIndex)
        {
            Dictionary<Keys, Buttons> mapping = getButtonsMapping(playerIndex); 
            return mapping[key];
        }

        public int getPlayersCount()
        {
            return currentGamepadStates.Length;
        }

        public bool isPlayerActive(int playerIndex)
        {
#if WINDOWS
            return true;
#else
            Debug.Assert(playerIndex >= 0 && playerIndex < getPlayersCount());
            return currentGamepadStates[playerIndex].IsConnected;
#endif
        }

        private int getPlayerIndex(PlayerIndex player)
        {
            return (int) player;
        }

        private PlayerIndex getPlayer(int playerIndex)
        {
            Debug.Assert(playerIndex >= 0 && playerIndex < getPlayersCount());
            return (PlayerIndex) playerIndex;
        }

        private void initButtonsMapping(int playersCount)
        {
            Debug.Assert(playersCount > 0 && playersCount <= 2);
            buttonsMappings = new Dictionary<Keys, Buttons>[2];

            Dictionary<Keys, Buttons> dic = new Dictionary<Keys, Buttons>();
            dic.Add(Keys.Up, Buttons.A);
            dic.Add(Keys.Down, Buttons.DPadDown);
            dic.Add(Keys.Left, Buttons.DPadLeft);
            dic.Add(Keys.Right, Buttons.DPadRight);
            buttonsMappings[0] = dic;

            dic = new Dictionary<Keys, Buttons>();
            dic.Add(Keys.W, Buttons.A);
            dic.Add(Keys.S, Buttons.DPadDown);
            dic.Add(Keys.A, Buttons.DPadLeft);
            dic.Add(Keys.D, Buttons.DPadRight);
            buttonsMappings[1] = dic;
        }

        private Dictionary<Keys, Buttons> getButtonsMapping(int playerIndex)
        {
            Debug.Assert(playerIndex >= 0 && playerIndex < buttonsMappings.Length);
            return buttonsMappings[playerIndex];
        }
    }
}
