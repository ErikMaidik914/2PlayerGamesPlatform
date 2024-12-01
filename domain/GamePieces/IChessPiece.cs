using System;
using _2PlayerGames;


namespace _2PlayerGames
{
	public interface IChessPiece : IPiece
	{
		void updatePosition(int newXPosition, int newYPosition);
		bool isValidMove(int newXPosition, int newYPosition);
		bool MovedFromInitialPosition { get; set; }

		String GetPieceType();
	}
}