﻿﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuckstazyLive.game
{
    public class GameState
    {
        /*** Грейды ***/
        public int def; // коэффициент урона
        public int maxHP; // максимальное здоровье
        public int hell;
        public int norm;

        /*** Герой ***/
        public int health;

        /*** Уровень ***/
        public int level;

        public int scores;

        public GameState()
        {
            reset();
        }


        // Все вернуть как сначала.
        public void reset()
        {
            def = 0;
            maxHP = 25;
            norm = 0;
            hell = 0;

            health = maxHP;

            level = 0;

            scores = 0;
        }

        // присвоить
        public void assign(GameState state)
        {
            def = state.def;
            maxHP = state.maxHP;
            norm = state.norm;
            hell = state.hell;

            health = state.health;

            level = state.level;

            scores = state.scores;
        }

        public int calcHellScores(int id)
        {
            int i = 1;

            switch (id)
            {
                case 0: i = 5; break;
                case 1: i = 10; break;
                case 2: i = 25; break;
                case 3: i = 50; break;
                case 4: i = 100; break;
                case 5: i = 150; break;
            }
            return i;
        }

    }
}
