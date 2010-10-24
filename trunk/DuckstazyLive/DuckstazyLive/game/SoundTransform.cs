using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuckstazyLive.game
{
    public class SoundTransform
    {
        public float pan;
        public float volume;

        public SoundTransform() : this(1.0f, 0.0f)
        {

        }

        public SoundTransform(float volume) : this(volume, 0.0f)
        {            
        }

        public SoundTransform(float volume, float pan)
        {
            this.pan = pan;
            this.volume = volume;
        }
    }
}
