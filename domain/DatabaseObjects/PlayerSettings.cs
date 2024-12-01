using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing.IndexedProperties;
using System.Text;
using System.Threading.Tasks;

namespace _2PlayerGames.domain.DatabaseObjects
{
    public class PlayerSettings
    {
        private Player _player;
        private string _ip;
        private bool _isSoundOn;
        private bool _isMusicOn;
        private float _soundVolume;
        private float _musicVolume;

        public Player Player { get => _player; set => _player = value; }
        public bool IsSoundOn { get => _isSoundOn; set => _isSoundOn = value; }
        public bool IsMusicOn { get => _isMusicOn; set => _isMusicOn = value; }
        public float SoundVolume { get => _soundVolume; set => _soundVolume = value; }
        public float MusicVolume { get => _musicVolume; set => _musicVolume = value; }
        public string Ip { get => _ip; set => _ip = value; }

        public PlayerSettings(Player player, string ip, bool isSoundOn, bool isMusicOn, float soundVolume, float musicVolume)
        {
            this._player = player;
            this._ip = ip;
            this._isSoundOn = isSoundOn;
            this._isMusicOn = isMusicOn;
            this._soundVolume = soundVolume;
            this._musicVolume = musicVolume;
        }

        public PlayerSettings() { 
            this._player = new Player();
            this._ip = "";
            this._isSoundOn = true;
            this._isMusicOn = true;
            this._soundVolume = 1.0f;
            this._musicVolume = 1.0f;
        }
    }
}
