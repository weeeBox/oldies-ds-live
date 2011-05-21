using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using app.menu;
using DuckstazyLive.app.game.env;
using asap.graphics;

namespace DuckstazyLive.app
{
    public class EnvScreen : Screen
    {
        private Env env;

        protected EnvScreen(ScreenId screenId) : base(screenId)
        {
            env = new Env();
            AddChild(env);
        }        
    }
}
