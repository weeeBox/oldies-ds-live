using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace Framework.core
{
    public enum ButtonAction
    {
        None,
        OK,
        Back,
        Up,
        Down,
        Left,
        Right
    };

    public struct ButtonEvent
    {
        private const Keys NO_KEY = (Keys) (-1);
        private const Buttons NO_BUTTON = (Buttons) (-1);

        public ButtonAction action;
        public Keys key;
        public Buttons button;        
        public int playerIndex;

        public ButtonEvent(Keys key)
        {            
            this.key = key;
            action = InputManager.getInstance().getAction(key);
            button = NO_BUTTON;
            playerIndex = 0;
        }

        public ButtonEvent(int playerIndex, Buttons button)
        {
            this.playerIndex = playerIndex;
            this.button = button;
            action = InputManager.getInstance().getAction(button);
            key = NO_KEY;
        }

        public bool isGamepadEvent()
        {
            return button != NO_BUTTON;
        }

        public bool isKeyboardEvent()
        {
            return key != NO_KEY;
        }
    }

    public interface InputListener
    {
        // return true, if event processed, otherwise - false
        bool buttonPressed(ref ButtonEvent e);
        bool buttonReleased(ref ButtonEvent e);        
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

        private static InputManager instance;

        private GamePadState[] currentGamepadStates;
        private KeyboardState currentKeyboardState;
        private List<InputListener> inputListeners;

        private Dictionary<Keys, Buttons>[] buttonsMappings;
        private Dictionary<Keys, ButtonAction> keyActionMapping;
        private Dictionary<Buttons, ButtonAction> buttonActionMapping;

        private GamePadDeadZone deadZone;

        public InputManager(int playersCount)
        {
            instance = this;

            initButtonsMapping(playersCount);
            initActionMapping();

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

        public static InputManager getInstance()
        {
            return instance;
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

        public ButtonEvent makeButtonEvent(Keys key)
        {
            return new ButtonEvent(key);
        }

        public ButtonEvent makeButtonEvent(int playerIndex, Buttons button)
        {
            Debug.Assert(playerIndex >= 0 && playerIndex < getPlayersCount());
            return new ButtonEvent(playerIndex, button);
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
                        ButtonEvent evt = makeButtonEvent(newKeys[i]);
                        fireKeyPressed(ref evt);
                    }
                }
                for (int i = 0; i < oldKeys.Length; ++i)
                {
                    if (!newKeys.Contains(oldKeys[i]))
                    {
                        ButtonEvent evt = makeButtonEvent(oldKeys[i]);
                        fireKeyReleased(ref evt);
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

        private void fireKeyPressed(ref ButtonEvent evt)
        {
            foreach (InputListener l in inputListeners)
            {
                l.buttonPressed(ref evt);
            }
        }

        private void fireKeyReleased(ref ButtonEvent evt)
        {
            foreach (InputListener l in inputListeners)
            {
                l.buttonReleased(ref evt);
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

        public ButtonAction getAction(Keys key)
        {
            if (keyActionMapping.ContainsKey(key))
                return keyActionMapping[key];

            return ButtonAction.None;
        }

        public ButtonAction getAction(Buttons button)
        {
            if (buttonActionMapping.ContainsKey(button))
                return buttonActionMapping[button];

            return ButtonAction.None;
        }

        private void initActionMapping()
        {
            // keys
            keyActionMapping = new Dictionary<Keys, ButtonAction>();
            keyActionMapping.Add(Keys.Enter, ButtonAction.OK);
            keyActionMapping.Add(Keys.Escape, ButtonAction.Back);
            keyActionMapping.Add(Keys.Up, ButtonAction.Up);
            keyActionMapping.Add(Keys.Down, ButtonAction.Down);
            keyActionMapping.Add(Keys.Left, ButtonAction.Left);
            keyActionMapping.Add(Keys.Right, ButtonAction.Right);

            // buttons
            buttonActionMapping = new Dictionary<Buttons, ButtonAction>();
            buttonActionMapping.Add(Buttons.A, ButtonAction.OK);            
            buttonActionMapping.Add(Buttons.B, ButtonAction.Back);
            buttonActionMapping.Add(Buttons.Start, ButtonAction.OK);
            buttonActionMapping.Add(Buttons.Back, ButtonAction.Back);
            buttonActionMapping.Add(Buttons.DPadDown, ButtonAction.Down);
            buttonActionMapping.Add(Buttons.DPadUp, ButtonAction.Up);
            buttonActionMapping.Add(Buttons.DPadLeft, ButtonAction.Left);
            buttonActionMapping.Add(Buttons.DPadRight, ButtonAction.Right);
            buttonActionMapping.Add(Buttons.LeftThumbstickDown, ButtonAction.Down);
            buttonActionMapping.Add(Buttons.LeftThumbstickUp, ButtonAction.Up);
            buttonActionMapping.Add(Buttons.LeftThumbstickLeft, ButtonAction.Left);
            buttonActionMapping.Add(Buttons.LeftThumbstickRight, ButtonAction.Right);
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
            dic.Add(Keys.PageDown, Buttons.RightShoulder);
            dic.Add(Keys.PageUp, Buttons.LeftShoulder);            
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
