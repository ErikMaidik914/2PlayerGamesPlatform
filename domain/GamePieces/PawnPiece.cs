using System;
using _2PlayerGames;

public class PawnPiece : ChessPiece
{
	public PawnPiece(int x, int y, Player player) :
		base(x, y, player)
	{ 
		this._pieceType = "Pawn";
	}

	public override bool isValidMove(int newXPosition, int newYPosition)
	{
		//standard position
		if (newXPosition == _xPosition && Math.Abs(newYPosition - _yPosition ) == 1)
		{
			return true;
		}
		//2 squares if not moved
		else if (newXPosition == _xPosition && Math.Abs(newYPosition - _yPosition) ==  2 && !_movedFromInitialPosition)
		{
			return true;
		}
		// Pawns standard capture
		else if (Math.Abs(newXPosition - _xPosition) == 1 && (newYPosition == _yPosition +1 || newYPosition == _yPosition -1))
		{
			return true;
		}
		//TODO thing about EnPassant
		// Otherwise, the move is invalid
		// I think EnPassant should be the service's job
		else
		{
			return false;
			//throw exception
		}
	}

}