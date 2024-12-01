using System;
using _2PlayerGames;
using Newtonsoft.Json.Linq;

namespace _2PlayerGames
{
    public interface IGame
    {
        string saveGame();
        void loadGame();
        IBoard Board { get; set; }
        GameState GameState { get; set; }
        Player CurrentPlayer { get; }
        int CurrentPlayerIndex { get; }
    }
}
