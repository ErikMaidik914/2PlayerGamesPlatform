using _2PlayerGames.domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2PlayerGames.domain.DatabaseObjects
{
    public class PlayerQueue
    {
        private Player _player;
        private Games _gameType;
        private int _eloRating;
        private ChessModes? _chessMode;
        private int? _obstructionWidth;
        private int? _obstructionHeight;

        public Player Player { get => _player; set => _player = value; }
        public Games GameType { get => _gameType; set => _gameType = value; }
        public int EloRating { get => _eloRating; set => _eloRating = value; }
        public ChessModes? ChessMode { get => _chessMode; set => _chessMode = value; }
        public int? ObstractionWidth { get => _obstructionWidth; set => _obstructionWidth = value; }
        public int? ObstractionHeight { get => _obstructionHeight; set => _obstructionHeight = value; }

        public PlayerQueue(Player player, Games gameType, int elo, ChessModes? chessMode, int? obstrustionWidth, int? obstructionHeigth) {
            this._player = player;
            this._gameType = gameType;
            this._eloRating = elo;
            this._chessMode = chessMode;
            this._obstructionWidth = obstrustionWidth;
            this._obstructionHeight = obstructionHeigth;
        }

        public PlayerQueue() {
            this._player = new Player();
            this._gameType = new Games();
            this._eloRating = 0;
            this._chessMode = null;
            this._obstructionWidth = null;
            this._obstructionHeight = null;
        }
    }
}
