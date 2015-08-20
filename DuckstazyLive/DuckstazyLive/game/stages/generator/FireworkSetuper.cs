using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckstazyLive.game.levels.generator;
using DuckstazyLive.app;
using System.Diagnostics;

namespace DuckstazyLive.game.stages.generator
{
    public class FireworkSetuper : Setuper
    {
        public static int[] POWER1 = new int[] { Pill.POWER1 };
        public static int[] POWER2 = new int[] { Pill.POWER2 };
        public static int[] POWER3 = new int[] { Pill.POWER3 };
        public static int[] POWERS = new int[] { Pill.POWER1, Pill.POWER2, Pill.POWER3 };
        public static int[] TOXIC = new int[] { Pill.TOXIC };

        public int[] ids;
        private List<int> genQueue;
        private int queueIndex;

        public FireworkSetuper()
        {
            genQueue = new List<int>();
        }

        public void init(int[] powerIDs, int sleepCount, int totalCount)
        {
            Debug.Assert(sleepCount <= totalCount);

            // clean
            ids = powerIDs;
            queueIndex = 0;
            genQueue.Clear();

            // add sleeps
            for (int i = 0; i < sleepCount; ++i)
            {
                genQueue.Add(Pill.SLEEP);
            }

            // add powers
            int idsCount = ids.Length;
            for (int i = 0; i < totalCount; ++i)
            {
                int id = 0;
                if (idsCount > 0)
                {
                    if (idsCount > 1) id = ids[(int)(utils.rnd() * idsCount)];
                    else id = ids[0];
                }
                genQueue.Add(id);
            }

            // shuff
            int queueSize = genQueue.Count;
            for (int i = 0; i < queueSize; ++i)
            {
                int index = utils.rnd_int(queueSize);
                int temp = genQueue[i];
                genQueue[i] = genQueue[index];
                genQueue[index] = temp;
            }
        }

        public override Pill start(float x, float y, Pill pill)
        {
            Debug.Assert(queueIndex >= 0 && queueIndex < genQueue.Count);

            int id = genQueue[queueIndex++];
            switch (id)
            {
                case Pill.POWER1:
                case Pill.POWER2:
                case Pill.POWER3:
                    pill.startPower(x, y, id, false);
                    break;
                case Pill.SLEEP:
                    pill.startSleep(x, y);
                    break;
                case Pill.TOXIC:
                    pill.startMissle(x, y, Pill.TOXIC_SKULL);
                    break;
            }

            pill.user = userCallback;
            return pill;
        }

        public bool isDone()
        {
            return queueIndex == genQueue.Count;
        }
    }
}
