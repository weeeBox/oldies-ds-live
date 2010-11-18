﻿﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuckstazyLive.game
{
    public class GameState
    {
        /*** Грейды ***/
        
        public int hell;
        public int norm;        

        /*** Уровень ***/
        public int level;        

        public GameState()
        {
            reset();
        }

        // Все вернуть как сначала.
        public void reset()
        {            
            norm = 0;
            hell = 0;         

            level = 0;            
        }

        // присвоить
        public void assign(GameState state)
        {            
            norm = state.norm;
            hell = state.hell;
        
            level = state.level;            
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
