using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2PlayerGames.domain.DatabaseObjects
{
    public class GameStats
    {
        private Player _player;
        private Games _game;
        private int _eloRating;
        private int _highestElo;
        private int _totalMatches;
        private int _totalWins;
        private int _totalDraws;
        private int _totalPlayTime;
        private int _totalNumberOfTurn;

        public Player Player { get => _player; set => this._player = value; }
        public Games Game { get => _game; set => this._game = value; }
        public int EloRating { get => _eloRating; set => this._eloRating = value; }
        public int HighestElo { get => _highestElo; set => this._highestElo = value; }
        public int TotalMatches { get => _totalMatches; set => this._totalMatches = value; }
        public int TotalWins { get => _totalWins; set => this._totalWins = value; }
        public int TotalDraws { get => _totalDraws; set => this._totalDraws = value; }
        public int TotalPlayTime { get => _totalPlayTime; set => this._totalPlayTime = value; }
        public int TotalNumberOfTurn { get => _totalNumberOfTurn; set => this._totalNumberOfTurn = value; }

        public GameStats(Player player, Games game)
        {
            this._player = player;
            this._game = game;
            this._eloRating = 420;
            this._highestElo = 420;
            this._totalMatches = 0;
            this._totalWins = 0;
            this._totalDraws = 0;
            this._totalPlayTime = 0;
            this._totalNumberOfTurn = 0;
        }

        public GameStats(Player player, Games game, int eloRating, int highestElo, int totalMathces, int totalWins, int totalDraws, int totalPlayTime, int totalNumberOfTurn)
        {
            this._player = player;
            this._game = game;
            this._eloRating = eloRating;
            this._highestElo = highestElo;
            this._totalMatches = totalMathces;
            this._totalWins = totalWins;
            this._totalDraws = totalDraws;
            this._totalPlayTime = totalPlayTime;
            this._totalNumberOfTurn = totalNumberOfTurn;
        }
    }
}
