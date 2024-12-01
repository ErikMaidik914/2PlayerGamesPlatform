using System;
using _2PlayerGames;


namespace _2PlayerGames
{
    public interface IPiece
    {
        int XPosition { get; set; }
        int YPosition { get; set; }
        Player? Player { get; set; }
    }
}
