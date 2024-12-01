using System;
using System.IO;
using System.Text.Json;
using _2PlayerGames;
using _2PlayerGames.repository;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace _2PlayerGames
{

    [Serializable]
    public class DartsGame : IGame
    {
        private IBoard _board;
        private int _currentPlayer;
        private GameState _gameState;



        [JsonIgnore]
        public GameState GameState { get => _gameState; set => _gameState = value; }

        [JsonIgnore]
        public Player CurrentPlayer { get => this._gameState.Players[this._currentPlayer]; }

        [JsonIgnore]
        public IBoard Board { get => _board; set => _board = (DartsBoard) value; }

        [JsonProperty]
        private DartsBoard JsonBoard { get => (DartsBoard) this._board; set => this._board = value; }

        [JsonProperty]
        public int CurrentPlayerIndex { get => this._currentPlayer; }


        public DartsGame(Player player1, Player player2)
        {
            this._board = new DartsBoard();
            this._currentPlayer = 0;
            //I shouldn't have to create a new game state here, it should be passed in from the database
            //Maybe we can implement a repo for the game types
            //Or a static class that holds the game types, thus could it the game type object
            //TODO
            this._gameState = new GameState(player1, player2, GameStore.Games["Darts"]);
            saveGame();
        }

        public DartsGame(GameState unifinishedGameState)
        {
            this._gameState = unifinishedGameState;
            this._board = new DartsBoard();
            this.loadGame();
        }

        public string saveGame()
        {
            string jsonString = JsonConvert.SerializeObject(this, Formatting.Indented);
            this._gameState.StateJson = jsonString;
            return jsonString;
        }

        public void loadGame() { 
            string json = this._gameState.StateJson;
            JObject obj = JsonConvert.DeserializeObject<JObject>(json);
            this.getFromJObject(obj);
        }

        //public void switchPlayers() { 
        //    this._currentPlayer = (this._currentPlayer + 1) % 2;
        //}

        //public void loadGame()
        //{
        //    string filepath = this._gameState.Filepath;
        //    string jsonString = File.ReadAllText(filepath);
        //    DartsGame game = JsonSerializer.Deserialize<DartsGame>(jsonString);
        //    this._board = game.Board;
        //    this._currentPlayer = game._currentPlayer;
        //    this._gameState = game._gameState;
        //    this._playerScores = game._playerScores;
        //}

        private void getFromJObject(JObject obj)
        {
            this._board.getFromJToken(obj["JsonBoard"]);
            this._currentPlayer = obj["CurrentPlayerIndex"].ToObject<int>();
        }
    }
}