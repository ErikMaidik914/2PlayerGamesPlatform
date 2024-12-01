using System;
using _2PlayerGames;
using _2PlayerGames.domain.DatabaseObjects;

public class BishopPiece: ChessPiece
{
    public BishopPiece(int x, int y, Player player): 
        base(x, y, player) { 
        this._pieceType = "Bishop";
    }

    public override bool isValidMove(int newXPosition, int newYPosition)
    {
        return Math.Abs(newXPosition - _xPosition) == Math.Abs(newYPosition - _yPosition);
    }

}