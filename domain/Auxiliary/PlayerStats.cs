using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2PlayerGames.domain.Auxiliary
{
    public class PlayerStats
    {
        private Player _player;
        private int _trophies;
        private int _averageElo;
        private string _rank;
        private Games? _favoriteGame;

        public Player Player { get => _player; set => _player = value; }
        public int Trophies { get => _trophies; set => _trophies = value; }
        public int AverageElo { get => _averageElo; set => _averageElo = value; }
        public string Rank { get => _rank; set => _rank = value; }
        public Games FavoriteGame { get => _favoriteGame; set => _favoriteGame = value; }
        public PlayerStats(Player player, int trophies, int averageElo, string rank, Games favoriteGame)
        {
            this._player = player;
            this._trophies = trophies;
            this._averageElo = averageElo;
            _rank = rank;
            _favoriteGame = favoriteGame;
        }

        public PlayerStats(Player player)
        {
            this._player = player;
            this._trophies = 0;
            this._averageElo = 0;
            _rank = "";
            _favoriteGame = null;
        }


    }
}
