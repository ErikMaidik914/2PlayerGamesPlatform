using System;
using _2PlayerGames;


namespace _2PlayerGames
{
    public class RookPiece : ChessPiece
    {
        public RookPiece(int x, int y, Player player) : base(x, y, player) { 
            this._pieceType = "Rook";
        }

        public override bool isValidMove(int newXPosition, int newYPosition) => newXPosition == this._xPosition || newYPosition == _yPosition;
    }
}
