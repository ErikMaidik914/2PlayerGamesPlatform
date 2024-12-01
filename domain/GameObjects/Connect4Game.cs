using _2PlayerGames.domain.DatabaseObjects;
using _2PlayerGames.repository;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace _2PlayerGames.domain.GameObjects
{
    internal class Connect4Game : IGame
    {
        private IBoard _board;
        private GameState _gameState;
        private int _currentPlayer;

        public Connect4Game(Player player1, Player player2)
        {
            this._board = new Connect4Board();
            this._currentPlayer = 0;
            this._gameState = new GameState(player1, player2, GameStore.Games["Connect4"]);
            this.saveGame();
        }

        public Connect4Game(GameState unifinishedGameState)
        {
            this._board = new Connect4Board();
            this._gameState = unifinishedGameState;
            this.loadGame();
        }

        [JsonIgnore]
        public IBoard Board { get => _board; set => _board = value; }
        [JsonIgnore]
        public GameState GameState { get => _gameState; set => _gameState = value; }

        [JsonIgnore]
        public Player CurrentPlayer => this._gameState.Players[this._currentPlayer];

        [JsonProperty]
        public Connect4Board JsonBoard { get => (Connect4Board) this._board; set => this._board = value; }

        [JsonProperty]
        public int CurrentPlayerIndex { get => this._currentPlayer; set => this._currentPlayer = value; }

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
