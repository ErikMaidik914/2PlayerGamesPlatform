using System;
using _2PlayerGames;

namespace _2PlayerGames
{
	public class KingPiece : ChessPiece
	{
		public KingPiece(int x, int y, Player player) : base(x, y, player) { 
			this._pieceType = "King";
		}

		public override bool isValidMove(int newXPosition, int newYPosition)
		{
			// Kings can move one square in any direction
			int xDistance = Math.Abs(newXPosition - _xPosition);
			int yDistance = Math.Abs(newYPosition - _yPosition);

			return xDistance <= 1 && yDistance <= 1;
		}
	}
}
