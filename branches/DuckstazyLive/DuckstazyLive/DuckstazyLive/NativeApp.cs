using asap.app;
using asap.core;
using asap.graphics;
using asap.resources;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace app
{
    public class NativeApp : AppImpl, GamePadListener, KeyboardListener, TouchListener
    {
        private Application application;        
        private Graphics appGraphics;
        private NativeInput input;
        private bool running;

        public NativeApp(int width, int height, ContentManager content)
        {
            new ResFactory(content);            

            appGraphics = new Graphics(width, height);
            application = new Application(width, height);
            input = new NativeInput();
            input.AddGamePadListener(this);
            input.AddKeyboardListener(this);
            input.AddTouchListener(this);
            application.SetImpl(this);            
            running = true;

            application.Start();
        }        
        
        public void Stop()
        {
            running = false;
        }                

        public void Tick(float deltaTime)
        {
            input.Tick();
            application.Tick(deltaTime);
        }

        public void Draw(GraphicsDevice device)
        {
            appGraphics.Begin(device);
            application.Draw(appGraphics);
            appGraphics.End();
        }

        public void PointerPressed(int x, int y)
        {
            application.PointerPressed(x, y, 0);
        }

        public void PointerDragged(int x, int y)
        {
            application.PointerDragged(x, y, 0);
        }

        public void PointerReleased(int x, int y)
        {
            application.PointerReleased(x, y, 0);
        }

        public void ButtonPressed(ButtonEventArg e)
        {
            KeyCode code = InputHelper.GetKeyCode(e.button);
            KeyAction action = InputHelper.GetKeyAction(e.button);
            application.KeyPressed(new KeyEvent(e.playerIndex, code, action));
        }

        public void ButtonReleased(ButtonEventArg e)
        {           
            KeyCode code = InputHelper.GetKeyCode(e.button);
            KeyAction action = InputHelper.GetKeyAction(e.button);
            application.KeyReleased(new KeyEvent(e.playerIndex, code, action));
        }

        public void GamePadConnected(int playerIndex)
        {
            application.GamePadConnected(playerIndex);
        }

        public void GamePadDisconnected(int playerIndex)
        {
            application.GamePadDisconnected(playerIndex);
        }

        public void KeyPressed(Keys key)
        {
            int playerIndex = InputHelper.GetPlayerIndex(key);
            KeyCode code = InputHelper.GetKeyCode(key);
            KeyAction action = InputHelper.GetKeyAction(key);
            application.KeyPressed(new KeyEvent(playerIndex, code, action));
        }

        public void KeyReleased(Keys key)
        {
            int playerIndex = InputHelper.GetPlayerIndex(key);
            KeyCode code = InputHelper.GetKeyCode(key);
            KeyAction action = InputHelper.GetKeyAction(key);
            application.KeyReleased(new KeyEvent(playerIndex, code, action));
        }

        public bool isRunning()
        {
            return running;
        }

        public void Dispose()
        {
            ResFactory.GetInstance().Dispose();
        }
    }
}
