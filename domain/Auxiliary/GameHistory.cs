using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2PlayerGames.domain.Auxiliary
{
    public class GameHistory
    {
        private Player[] _players;
        private Games _gameType;
        private Player _winnerPlayer;

        public Player[] Players { get => _players; set => _players = value; }
        public Games GameType { get => _gameType; }
        public Player Winner { get => _winnerPlayer; set => _winnerPlayer = value; }

        public GameHistory(Player player1, Player player2, Games gameType, int winnerIndex)
        {
            this._players = new Player[2];
            this._players[0] = player1;
            this._players[1] = player2;
            this._gameType = gameType;
            this._winnerPlayer = this._players[winnerIndex];
        }

        public GameHistory(Player player1, Player player2, Games gameType, Player winnerPlayer)
        {
            this._players = new Player[2];
            this._players[0] = player1;
            this._players[1] = player2;
            this._gameType = gameType;
            this._winnerPlayer = winnerPlayer;
        }
    }
}
