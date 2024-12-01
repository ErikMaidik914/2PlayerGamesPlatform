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
    internal class ObstructionGame : IGame
    {
        private IBoard _board;
        private GameState _gameState;
        private int _currentPlayer;

        public ObstructionGame(Player player1, Player player2, int width, int heigth)
        {
            this._board = new ObstructionBoard(width, heigth);
            this._currentPlayer = 0;
            this._gameState = new GameState(player1, player2, GameStore.Games["Obstruction"]);
        }

        public ObstructionGame(GameState unifinishedGameState)
        {
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
        public ObstructionBoard JsonBoard { get => (ObstructionBoard)this._board; set => this._board = value; }

        [JsonProperty]
        public int CurrentPlayerIndex => this._currentPlayer;
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
