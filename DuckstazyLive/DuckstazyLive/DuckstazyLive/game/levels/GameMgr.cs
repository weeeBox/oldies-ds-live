using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuckstazyLive.game.levels
{
    public class GameMgr
    {
        private Heroes heroes;
        private Pills pills;
        private Env env;
        private Particles ps;

        private static GameMgr instance;

        public static void createInstance()
        {
            instance = new GameMgr();
        }

        public static GameMgr getInstance()
        {
            return instance;
        }

        private GameMgr()
        {
            heroes = new Heroes();
            ps = new Particles();
            pills = new Pills(heroes, ps);
            env = Env.getIntance();
        }

        public void initHeroes(int heroesCount)
        {
            heroes.clearHeroes();
            for (int heroIndex = 0; heroIndex < heroesCount; ++heroIndex)
            {
                Hero hero = new Hero(heroes, heroIndex);
                heroes.addHero(hero);
            }
        }

        public void reset()
        {
            clear();
            heroes.init();
        }

        public void clear()
        {
            heroes.clear();
            ps.clear();
            pills.clear();           
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
