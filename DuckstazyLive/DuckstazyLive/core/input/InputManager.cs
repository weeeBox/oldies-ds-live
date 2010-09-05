using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using DuckstazyLive.framework.core;
using System.Diagnostics;
using System.Collections.Generic;

namespace DuckstazyLive.core.input
{
    public class InputManager : Timer
    {
        private static Buttons[] supportedGamePadButtons = new Buttons[]
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

        private GamePadState[] gamePadStates;
        private GamePadState[] oldGamePadStates;
        private static PlayerIndex[] players = new PlayerIndex[] 
        { 
            PlayerIndex.One, 
            PlayerIndex.Two, 
            PlayerIndex.Three, 
            PlayerIndex.Four 
        };

        private List<InputListener> listeners;
        private static InputManager instance;

        public InputManager(int playersCount)
        {
            Debug.Assert(instance == null, "InputManager already initialized");
            instance = this;

            gamePadStates = new GamePadState[playersCount];
            oldGamePadStates = new GamePadState[playersCount];
            for (int playerIndex = 0; playerIndex < getPlayersCount(); playerIndex++)
            {
                gamePadStates[playerIndex] = oldGamePadStates[playerIndex] = GamePad.GetState(players[playerIndex]);
            }            
            listeners = new List<InputListener>(16);
        }        

        public static InputManager getInstance()
        {
            Debug.Assert(instance != null, "Input manager not initialized");
            return instance;
        }        

        private void Update(ref GamePadState state, ref GamePadState oldState, ref PlayerIndex player)
        {
            if (state.IsConnected && !oldState.IsConnected)
            {
                FirePlayerConnected(ref player);
            }
            else if (!state.IsConnected && oldState.IsConnected)
            {
                FirePlayerDisconnected(ref player);
            }

            foreach (Buttons button in supportedGamePadButtons)
            {
                if (oldState.IsButtonUp(button))
                {
                    if (state.IsButtonDown(button))
                    {
                        FireButtonDown(button, ref state, ref player);
                    }
                }
                else
                {
                    if (state.IsButtonUp(button))
                    {
                        FireButtonUp(button, ref state, ref player);
                    }
                }
            } 
        }        

        //public float LeftThumbStickX
        //{
        //    get { return gamePadState.ThumbSticks.Left.X; }
        //}

        //public float LeftThumbStickY
        //{
        //    get { return gamePadState.ThumbSticks.Left.Y; }
        //}

        //public float RightThumbStickX
        //{
        //    get { return gamePadState.ThumbSticks.Right.X; }
        //}

        //public float RightThumbStickY
        //{
        //    get { return gamePadState.ThumbSticks.Right.Y; }
        //}       

        public void AddInputListener(InputListener l)
        {
            if (!listeners.Contains(l))
                listeners.Add(l);
        }

        public void RemoveInputListener(InputListener l)
        {
            listeners.Remove(l);
        }

        private void FireButtonDown(Buttons button, ref GamePadState state, ref PlayerIndex player)
        {
            InputEvent e = new InputEvent(button, player, state);            
            foreach (InputListener l in listeners)
            {
                l.buttonPressed(e);
            }
        }

        private void FireButtonUp(Buttons button, ref GamePadState state, ref PlayerIndex player)
        {
            InputEvent e = new InputEvent(button, player, state);            
            foreach (InputListener l in listeners)
            {
                l.buttonReleased(e);
            }
        }

        private void FirePlayerConnected(ref PlayerIndex player)
        {
            foreach (InputListener l in listeners)
            {
                l.playerConnected(player);
            }
        }

        private void FirePlayerDisconnected(ref PlayerIndex player)
        {
            foreach (InputListener l in listeners)
            {
                l.playerDisconnected(player);
            }
        }        

        private int getPlayersCount()
        {
            return gamePadStates.Length;
        }

        /************************************************************************/
        /* Timers stuff                                                         */
        /************************************************************************/

        protected override void tickTimer(float dt)
        {
            for (int playerIndex = 0; playerIndex < getPlayersCount(); playerIndex++)
            {
                gamePadStates[playerIndex] = GamePad.GetState(players[playerIndex]);
                Update(ref gamePadStates[playerIndex], ref oldGamePadStates[playerIndex], ref players[playerIndex]);
                oldGamePadStates[playerIndex] = gamePadStates[playerIndex];
            }              
        }
    }
}
