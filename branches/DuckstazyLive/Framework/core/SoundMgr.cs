using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Framework.core
{
    //public class SoundMgr
    //{
    //    private Song song; // Channel 0
    //    private SoundEffectInstance[] sounds = new SoundEffectInstance[16];
    //    private int[] soundIds = new int[16];        

    //    private bool isPlaying(int channel)
    //    {
    //        if (channel == 0)
    //        {
    //            return MediaPlayer.State == MediaState.Playing;
    //        }
    //        else
    //        {
    //            if (sounds[channel] == null)
    //                return false;
    //            else
    //                return sounds[channel].State == SoundState.Playing;
    //        }
    //    }        

    //    public SoundChannel playSound(int sid)
    //    {
    //        return playSound(sid, SoundTransform.NONE);
    //    }

    //    public SoundChannel playSound(int sid, SoundTransform soundTransform)
    //    {
    //        return playSound(sid, false, soundTransform);
    //    }

    //    public SoundChannel playSound(int sid, bool looped, SoundTransform soundTransform)
    //    {
    //        object audio = Application.sharedResourceMgr.getResource(sid);
    //        if (audio is Song)
    //        {
    //            return playSound(sid, 0, looped, soundTransform);
    //        }
    //        else
    //        {
    //            for (int i = 1; i < soundIds.Length; i++)
    //            {
    //                if (!isPlaying(i))
    //                {
    //                    return playSound(sid, i, looped, soundTransform);                        
    //                }
    //            }
    //        }
    //        return null;
    //    }

    //    private SoundChannel playSound(int sid, int channel, bool looped, SoundTransform soundTransform)
    //    {
    //        object audio = Application.sharedResourceMgr.getResource(sid);
    //        if (audio is Song)
    //        {
    //            Song s = (Song)audio;

    //            stopChannel(0);

    //            song = s;

    //            SoundChannel soundChannel = new SongSoundChannel();
    //            soundChannel.SoundTransform = soundTransform;

    //            MediaPlayer.IsRepeating = looped;                
    //            MediaPlayer.Play(s);                

    //            soundIds[0] = sid;
    //            return soundChannel;
    //        }
    //        else
    //        {
    //            if (soundIds[channel] == sid && sounds[channel] != null && sounds[channel].IsLooped && looped)
    //                return null;

    //            stopChannel(channel);

    //            SoundEffect se = (SoundEffect)audio;
    //            SoundEffectInstance si = se.CreateInstance();                
    //            sounds[channel] = si;
    //            soundIds[channel] = sid;

    //            SoundChannel soundChannel = new EffectSoundChannel(si);
    //            soundChannel.SoundTransform = soundTransform;

    //            si.IsLooped = looped;
    //            si.Play();

    //            return soundChannel;
    //        }
    //    }

    //    private void stopChannel(int channel)
    //    {
    //        if (channel == 0)
    //        {
    //            MediaPlayer.Stop();
    //            soundIds[0] = -1;
    //        }
    //        else
    //        {
    //            if (sounds[channel] != null)
    //            {
    //                sounds[channel].Stop();
    //                sounds[channel].Dispose();
    //                sounds[channel] = null;
    //                soundIds[channel] = -1;
    //            }
    //        }
    //    }

    //    public void stopSound(int soundId)
    //    {
    //        for (int i = 0; i < soundIds.Length; i++)
    //        {
    //            if (soundIds[i] == soundId)
    //                stopChannel(i);
    //        }
    //    }

    //    public void stopAllSounds()
    //    {
    //        for (int i = 0; i < soundIds.Length; i++)
    //        {
    //            stopChannel(i);
    //        }
    //    }        
    //}
}
