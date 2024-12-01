using _2PlayerGames.domain.Enums;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using _2PlayerGames.repository;

namespace _2PlayerGames.domain.GameObjects
{
    [Serializable]
    public class ChessGame : IGame
    {
        private IBoard _board;
        private GameState _gameState;
        private int _currentPlayer;

        public ChessGame(Player player1, Player player2, ChessModes mode, int startPlayer)
        {
            this._board = new ChessBoard([player1, player2], mode);
            this._currentPlayer = startPlayer;
            this._gameState = new GameState(player1, player2, GameStore.Games["Chess"], startPlayer);
            saveGame();
        }

        public ChessGame(GameState unifinishedGameState)
        {
            _board = new ChessBoard();
            this._gameState = unifinishedGameState;
            this.loadGame();
        }

        [JsonIgnore]
        public GameState GameState { get => _gameState; set => _gameState = value; }

        [JsonIgnore]
        public Player CurrentPlayer { get => this._gameState.Players[this._currentPlayer]; }

        [JsonIgnore]
        public IBoard Board { get => _board; set => _board = value; }

        [JsonProperty]
        private ChessBoard JsonBoard { get => (ChessBoard) this._board; set => this._board = value; }

        [JsonProperty]
        public int CurrentPlayerIndex { get => this._currentPlayer; set => _currentPlayer = value; }
        public string saveGame()
        {
            string jsonString = JsonConvert.SerializeObject(this, Formatting.Indented);
            this._gameState.StateJson = jsonString;
            return jsonString;
        }

        public void loadGame()
        {
            string json = this._gameState.StateJson;
            JObject obj = JsonConvert.DeserializeObject<JObject>(json);
            this.getFromJObject(obj);
        }
        private void getFromJObject(JObject obj)
        {
            this._board.getFromJToken(obj["JsonBoard"]);
            this._currentPlayer = obj["CurrentPlayerIndex"].ToObject<int>();
        }

    }
}
