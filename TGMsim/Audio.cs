using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.CoreAudioApi;

namespace TGMsim
{
    static class Audio
    {
        static NAudio.Wave.WaveOutEvent songPlayer = new NAudio.Wave.WaveOutEvent();
        static NAudio.Vorbis.VorbisWaveReader musicStream;

        static public float musVol = 0.3f;
        static public float sfxVol = 0.3f;

        static public void addSound(System.Windows.Media.MediaPlayer plr, string uri)
        {
            plr.IsMuted = true;
            plr.Open(new Uri(Environment.CurrentDirectory + uri));
        }

        static public void playSound(System.Windows.Media.MediaPlayer snd)
        {
            snd.IsMuted = false;
            snd.Volume = sfxVol;
            snd.Position = new TimeSpan(0);
            snd.Play();
        }

        static public void playMusic(string song)
        {
            try
            {
                musicStream = new NAudio.Vorbis.VorbisWaveReader(@"Res\Audio\" + song + ".ogg");
                LoopStream loop = new LoopStream(musicStream);
                songPlayer.Init(loop);
                songPlayer.Volume = musVol;
                //NAudio.CoreAudioApi.AudioMeterInformation test = new NAudio.CoreAudioApi.AudioMeterInformation(new  );
                //float vol = test.MasterPeakValue;
                songPlayer.Play();

            }
            catch (Exception)
            {
                //MessageBox.Show("The file \"" + song + ".ogg\" was not found!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                //throw;
            }
        }

        static public void stopMusic()
        {
            songPlayer.Stop();
            songPlayer.Dispose();
        }

        static public void setMusicVolume(float vol)
        {
            musVol = vol;
        }
        static public void setSFXVolume(float vol)
        {
            sfxVol = vol;
        }
    }
}
