using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Framework.core
{
    public class SoundMgr
    {
        private Song song; // Channel 0
        private SoundEffectInstance[] sounds = new SoundEffectInstance[16];
        private int[] soundIds = new int[16];
        private float[] volumes = { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };

        private bool isPlaying(int channel)
        {
            if (channel == 0)
            {
                return MediaPlayer.State == MediaState.Playing;
            }
            else
            {
                if (sounds[channel] == null)
                    return false;
                else
                    return sounds[channel].State == SoundState.Playing;
            }
        }

        public void playLowPrioritySound(int soundId, int channel, bool looped)
        {
            playSound(soundId, channel, looped);
        }

        public void playSound(int sid)
        {
            playSound(sid, false);
        }

        public void playSound(int sid, bool looped)
        {
            object audio = Application.sharedResourceMgr.getResource(sid);
            if (audio is Song)
            {
                playSound(sid, 0, looped);
            }
            else
            {
                for (int i = 1; i < soundIds.Length; i++)
                {
                    if (!isPlaying(i))
                    {
                        playSound(sid, i, looped);
                        break;
                    }
                }
            }
        }

        public void playSound(int sid, int channel, bool looped)
        {
            object audio = Application.sharedResourceMgr.getResource(sid);
            if (audio is Song)
            {
                Song s = (Song)audio;

                if (MediaPlayer.IsRepeating && looped && song == s)
                    return;

                stopChannel(0);

                song = s;
                MediaPlayer.IsRepeating = looped;
                //MediaPlayer.Volume = volumes[0];
                MediaPlayer.Play(s);
                if (volumes[0] < 0.5f)
                    MediaPlayer.Pause();

                soundIds[0] = sid;
            }
            else
            {
                if (soundIds[channel] == sid && sounds[channel] != null && sounds[channel].IsLooped && looped)
                    return;

                stopChannel(channel);

                SoundEffect se = (SoundEffect)audio;
                SoundEffectInstance si = se.CreateInstance();
                si.Volume = volumes[channel];
                si.IsLooped = looped;
                sounds[channel] = si;
                soundIds[channel] = sid;
                si.Play();
            }
        }

        private void stopChannel(int channel)
        {
            if (channel == 0)
            {
                MediaPlayer.Stop();
                soundIds[0] = -1;
            }
            else
            {
                if (sounds[channel] != null)
                {
                    sounds[channel].Stop();
                    sounds[channel] = null;
                    soundIds[channel] = -1;
                }
            }
        }

        public void stopSound(int soundId)
        {
            for (int i = 0; i < soundIds.Length; i++)
            {
                if (soundIds[i] == soundId)
                    stopChannel(i);
            }
        }

        public void stopAllSounds()
        {
            for (int i = 0; i < soundIds.Length; i++)
            {
                stopChannel(i);
            }
        }

        public void setVolume(float v, int channel)
        {
            volumes[channel] = v;
            if (channel == 0)
            {
                if (v > 0.5f)
                {
                    if (MediaPlayer.State == MediaState.Paused)
                        MediaPlayer.Resume();
                }
                else
                {
                    if (MediaPlayer.State == MediaState.Playing)
                        MediaPlayer.Pause();
                }
            }
            else
            {
                if (sounds[channel] != null)
                {
                    sounds[channel].Volume = v;
                }
            }
        }
    }
}
