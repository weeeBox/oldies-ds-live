using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.core
{
    public class SoundTransform
    {
        public static readonly SoundTransform NONE = new SoundTransform();

        public float pan;
        public float volume;

        public SoundTransform()
            : this(1.0f, 0.0f)
        {

        }

        public SoundTransform(float volume)
            : this(volume, 0.0f)
        {
        }

        public SoundTransform(float volume, float pan)
        {
            this.pan = pan;
            this.volume = volume;
        }
    }
}
