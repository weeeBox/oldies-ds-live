using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Framework.core
{
    public class SoundTransform
    {
        public static readonly SoundTransform NONE = new SoundTransform();

        private float pan;
        private float volume;

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

        public float Volume
        {
            get { return volume; }
            set 
            {
                Debug.Assert(volume >= 0 && volume < 1.0f);
                volume = value;
            }
        }

        public float Pan
        {
            get { return pan; }
            set 
            {
                Debug.Assert(pan >= -1 && pan <= 1);
                pan = value;
            }
        }
    }
}
