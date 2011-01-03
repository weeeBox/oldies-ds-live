using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuckstazyLive.game.levels
{
    public class Game
    {
        private Heroes heroes;
        private Pills pills;
        private Env env;
        private Particles ps;

        private Game instance;

        public void createInstance()
        {
            instance = new Game();
        }

        private Game()
        {
            heroes = new Heroes();
            ps = new Particles();
            pills = new Pills(heroes, ps);
        }

        public Heroes getHeroes()
        {
            return heroes;
        }

        public Pills getPills()
        {
            return pills;
        }

        public Env getEnv()
        {
            return env;
        }

        public Particles getParticles()
        {
            return ps;
        }
    }
}
