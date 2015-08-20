using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using System.Diagnostics;
using Microsoft.Xna.Framework.Media;

namespace Framework.core
{
    public abstract class SoundChannel
    {
        protected SoundTransform soundTransform;
        public abstract SoundTransform SoundTransform
        {
            get;
            set;
        }

        public virtual float LeftPeak
        {
            get { return 1.0f; }
        }

        public virtual float RightPeak
        {
            get { return 1.0f; }
        }
    }

    public class EffectSoundChannel : SoundChannel
    {
        private SoundEffectInstance instance;

        public EffectSoundChannel(SoundEffectInstance instance)
        {
            this.instance = instance;
        }

        public override SoundTransform SoundTransform
        {
            get { return soundTransform; }
            set
            {
                Debug.Assert(value != null);

                soundTransform = value;
                instance.Volume = soundTransform.Volume;
                instance.Pan = soundTransform.Pan;
            }
        }
    }

    public class SongSoundChannel : SoundChannel
    {
        public override SoundTransform SoundTransform
        {
            get { return soundTransform; }
            set
            {
                Debug.Assert(value != null);

                soundTransform = value;
                MediaPlayer.Volume = soundTransform.Volume;
            }
        }
    }
}
