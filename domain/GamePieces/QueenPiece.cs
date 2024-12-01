using System;
using _2PlayerGames;

public class QueenPiece : ChessPiece
{
    public QueenPiece(int x, int y, Player player) : base(x, y, player) { 
        this._pieceType = "Queen";
    }

    public override bool isValidMove(int newXPosition, int newYPosition)
    {
        // Queens can move horizontally, vertically, or diagonally
        int xDistance = Math.Abs(newXPosition - _xPosition);
        int yDistance = Math.Abs(newYPosition - _yPosition);

        return (_xPosition == newXPosition || _yPosition == newYPosition || xDistance == yDistance);
    }
}
