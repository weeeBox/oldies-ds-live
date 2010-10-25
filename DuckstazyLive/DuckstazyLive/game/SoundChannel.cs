using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuckstazyLive.game
{
    public class SoundChannel
    {
        public SoundTransform soundTransform;
        public float leftPeak;
        public float rightPeak;

        public SoundChannel(SoundTransform trans, float lp, float rp)
        {
            this.soundTransform = trans;
            leftPeak = lp;
            rightPeak = rp;
        }
    }
}
