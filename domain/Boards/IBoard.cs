using System;
using System.Collections.Generic;
using _2PlayerGames;
using Newtonsoft.Json.Linq;

namespace _2PlayerGames
{
    public interface IBoard
    {
        List<IPiece> Board { get; set; }
        int Width { get; set; }
        int Height { get; set; }


        IPiece? GetPiece(int xPosition, int yPosition);
        void addPiece(IPiece piece);

        void getFromJToken(JToken obj);
    }
}

