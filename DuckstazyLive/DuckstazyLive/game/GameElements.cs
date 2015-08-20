using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuckstazyLive.game
{
    public class GameElements
    {
        private static Heroes heroes;
        private static Pills pills;
        private static Env env;
        private static Particles ps;
        
        public static void init()
        {        
            heroes = new Heroes();
            ps = new Particles();
            pills = new Pills(heroes, ps);
            env = new Env();
        }

        public static void initHeroes(int heroesCount)
        {
            heroes.clearHeroes();
            for (int heroIndex = 0; heroIndex < heroesCount; ++heroIndex)
            {
                Hero hero = new Hero(heroes, heroIndex);
                heroes.addHero(hero);
            }
        }

        public static void reset()
        {
            clear();
            heroes.init();
        }

        public static void clear()
        {
            heroes.clear();
            ps.clear();
            pills.clear();           
        }

        public static Heroes Heroes
        {
            get { return heroes; }
        }

        public static Pills Pills
        {
            get { return pills; }
        }

        public static Env Env
        {
            get { return env; }
        }

        public static Particles Particles
        {
            get { return ps; }
        }
    }
}
