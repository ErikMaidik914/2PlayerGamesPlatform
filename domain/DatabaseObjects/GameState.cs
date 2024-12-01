using System;
using _2PlayerGames;

namespace _2PlayerGames
{
    public class GameState
    {
        private Guid _id;
        private Player[] _players;
        private Games _gameType;
        private Player? _winnerPlayer;
        private string _stateJson;
        private int _turn;
        private int _timePlayed;
        
        public Guid Id { get => _id; set => _id = value; }
        public Player[] Players { get => _players; set => _players = value; }

        public Games GameType { get => _gameType; }
        public string StateJson { get => _stateJson; set => _stateJson = value; }

        public Player? Winner { get => _winnerPlayer; set => _winnerPlayer = value; }
        public int Turn { get => _turn; set => _turn = value; }

        public int TimePlayed { get => _timePlayed; set => _timePlayed = value; }

        public GameState(Player player1, Player player2, Games gameType)
        {
            this._id = Guid.NewGuid();
            this._players = new Player[2];
            this._players[0] = player1;
            this._players[1] = player2;
            this._gameType = gameType;
            this._winnerPlayer = null;
            this._turn = 0;
            this._timePlayed = 0;
            this._stateJson = "";
        }

        public GameState(Player player1, Player player2, Games gameType, int turn)
        {
            this._id = Guid.NewGuid();
            this._players = new Player[2];
            this._players[0] = player1;
            this._players[1] = player2;
            this._gameType = gameType;
            this._winnerPlayer = null;
            this._turn = turn;
            this._timePlayed = 0;
            this._stateJson = "";
        }

        public GameState() {
            this._id = Guid.Empty;
            this._players = new Player[2];
            this._players[0] = new Player();
            this._players[1] = new Player();
            this._gameType = new Games();
            this._winnerPlayer = null;
            this._turn = 0;
            this._timePlayed = 0;
            this._stateJson = "";
        }

        public GameState(Guid gameStateId, Player player1, Player player2, Games gameType, int turn, int timePlayed, Player? winner, string jsonString)
        {
            this._id = gameStateId;
            this._players = new Player[2];
            this._players[0] = player1;
            this._players[1] = player2;
            this._gameType = gameType;
            this._winnerPlayer = winner;
            this._turn = turn;
            this._timePlayed = timePlayed;
            this._stateJson = jsonString;
        }
    }
}