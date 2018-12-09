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

        static public string song = "";

        static public System.Windows.Media.MediaPlayer s_Ready = new System.Windows.Media.MediaPlayer();
        static public System.Windows.Media.MediaPlayer s_Go = new System.Windows.Media.MediaPlayer();
        static public System.Windows.Media.MediaPlayer s_Tet1 = new System.Windows.Media.MediaPlayer();
        static public System.Windows.Media.MediaPlayer s_Tet2 = new System.Windows.Media.MediaPlayer();
        static public System.Windows.Media.MediaPlayer s_Tet3 = new System.Windows.Media.MediaPlayer();
        static public System.Windows.Media.MediaPlayer s_Tet4 = new System.Windows.Media.MediaPlayer();
        static public System.Windows.Media.MediaPlayer s_Tet5 = new System.Windows.Media.MediaPlayer();
        static public System.Windows.Media.MediaPlayer s_Tet6 = new System.Windows.Media.MediaPlayer();
        static public System.Windows.Media.MediaPlayer s_Tet7 = new System.Windows.Media.MediaPlayer();
        static public System.Windows.Media.MediaPlayer s_PreRot = new System.Windows.Media.MediaPlayer();
        static public System.Windows.Media.MediaPlayer s_Contact = new System.Windows.Media.MediaPlayer();
        static public System.Windows.Media.MediaPlayer s_Lock = new System.Windows.Media.MediaPlayer();
        static public System.Windows.Media.MediaPlayer s_Clear = new System.Windows.Media.MediaPlayer();
        static public System.Windows.Media.MediaPlayer s_Impact = new System.Windows.Media.MediaPlayer();
        static public System.Windows.Media.MediaPlayer s_Grade = new System.Windows.Media.MediaPlayer();
        static public System.Windows.Media.MediaPlayer s_Hold = new System.Windows.Media.MediaPlayer();
        static public System.Windows.Media.MediaPlayer s_GameClear = new System.Windows.Media.MediaPlayer();
        static public System.Windows.Media.MediaPlayer s_Cool = new System.Windows.Media.MediaPlayer();
        static public System.Windows.Media.MediaPlayer s_Regret = new System.Windows.Media.MediaPlayer();
        static public System.Windows.Media.MediaPlayer s_Section = new System.Windows.Media.MediaPlayer();
        static public System.Windows.Media.MediaPlayer s_Tetris = new System.Windows.Media.MediaPlayer();
        static public System.Windows.Media.MediaPlayer s_Combo = new System.Windows.Media.MediaPlayer();
        static public System.Windows.Media.MediaPlayer s_Medal = new System.Windows.Media.MediaPlayer();
        static public System.Windows.Media.MediaPlayer s_Bell = new System.Windows.Media.MediaPlayer();

        //static MMDeviceEnumerator numer = new MMDeviceEnumerator();
        //static MMDevice endpoint = numer.GetDefaultAudioEndpoint(DataFlow.All, Role.Communications);

        static public int musVol = 7;
        static public int sfxVol = 7;

        static public bool muted = false;

        static Audio()
        {
            addSound(s_Ready, @"\Res\Audio\SE\SEP_ready.wav");
            addSound(s_Go, @"/Res/Audio/SE/SEP_go.wav");
            addSound(s_Tet1, @"/Res/Audio/SE/SEB_mino1.wav");
            addSound(s_Tet2, @"/Res/Audio/SE/SEB_mino2.wav");
            addSound(s_Tet3, @"/Res/Audio/SE/SEB_mino3.wav");
            addSound(s_Tet4, @"/Res/Audio/SE/SEB_mino4.wav");
            addSound(s_Tet5, @"/Res/Audio/SE/SEB_mino5.wav");
            addSound(s_Tet6, @"/Res/Audio/SE/SEB_mino6.wav");
            addSound(s_Tet7, @"/Res/Audio/SE/SEB_mino7.wav");
            addSound(s_PreRot, @"/Res/Audio/SE/SEB_prerotate.wav");
            addSound(s_Contact, @"/Res/Audio/SE/SEB_fixa.wav");
            addSound(s_Lock, @"/Res/Audio/SE/SEB_instal.wav");
            addSound(s_Clear, @"/Res/Audio/SE/SEB_disappear.wav");
            addSound(s_Impact, @"/Res/Audio/SE/SEB_fall.wav");
            addSound(s_Grade, @"/Res/Audio/SE/SEP_levelchange.wav");
            addSound(s_Hold, @"/Res/Audio/SE/SEB_prehold.wav");
            addSound(s_GameClear, @"/Res/Audio/SE/SEP_gameclear.wav");
            addSound(s_Cool, @"/Res/Audio/SE/SEP_cool.wav");
            addSound(s_Regret, @"/Res/Audio/SE/SEI_vs_select.wav");
            addSound(s_Section, @"/Res/Audio/SE/SEP_lankup.wav");
            addSound(s_Combo, @"/Res/Audio/SE/SEP_combo.wav");
            addSound(s_Tetris, @"/Res/Audio/SE/SEP_tetris.wav");
            addSound(s_Medal, @"/Res/Audio/SE/SEP_platinum.wav");
            addSound(s_Bell, @"/Res/Audio/SE/bell.wav");
        }

        static public void addSound(System.Windows.Media.MediaPlayer plr, string uri)
        {
            plr.IsMuted = true;
            plr.Open(new Uri(Environment.CurrentDirectory + uri));
        }

        static public void playSound(System.Windows.Media.MediaPlayer snd)
        {
            snd.IsMuted = false;
            snd.Volume = (double)sfxVol/10;
            snd.Position = new TimeSpan(0);
            snd.Play();
        }

        static public void playMusic(string s)
        {
            try
            {
                song = s;
                musicStream = new NAudio.Vorbis.VorbisWaveReader(@"Res\Audio\" + song + ".ogg");
                LoopStream loop = new LoopStream(musicStream);
                songPlayer.Init(loop);
                //SimpleAudioVolume test = 
                //AudioMeterInformation test = endpoint.AudioMeterInformation;
                songPlayer.Volume = (float)musVol/10;
                if(!muted)
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

        static public void setMusicVolume(int vol)
        {
            musVol = vol;
            songPlayer.Volume = (float)musVol / 10;
        }
        static public void setSFXVolume(int vol)
        {
            sfxVol = vol;
        }
    }
}
