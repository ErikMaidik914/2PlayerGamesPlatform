using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Enumeration;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using _2PlayerGames.repository;
using System.Windows.Media;
using _2PlayerGames.domain.Auxiliary;



namespace _2PlayerGames.service
{

    internal class InGameService
    {
        private MediaPlayer m_mediaPlayer;


        public void Play(Sounds sound)
        {
            string filename = sound.Path;

            m_mediaPlayer = new MediaPlayer();
            m_mediaPlayer.Open(new Uri(filename));
            m_mediaPlayer.Play();
        }

        public void SetVolume(float volume)
        {
            m_mediaPlayer.Volume = volume;
        }


        public PlayerStats GetStats(Player player)
        {
            return StatsRepository.GetProfileStats(player);
        }
    }
}


