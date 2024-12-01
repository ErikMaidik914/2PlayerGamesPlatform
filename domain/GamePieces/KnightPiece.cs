using System;
using _2PlayerGames;

public class KnightPiece : ChessPiece
{
    public KnightPiece(int x, int y, Player player) : base(x, y, player) {
        this._pieceType = "Knight";
    }

    public override bool isValidMove(int newXPosition, int newYPosition)
    {
        // L-shape pattern: 2 squares in one direction and 1 square perpendicular to that direction.
        int xDistance = Math.Abs(newXPosition - _xPosition);
        int yDistance = Math.Abs(newYPosition - _yPosition);

        return (xDistance == 1 && yDistance == 2) || (xDistance == 2 && yDistance == 1);
    }
}
