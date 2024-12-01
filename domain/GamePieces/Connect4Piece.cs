using System;
using _2PlayerGames;

namespace _2PlayerGames
{
	public class Connect4Piece : IPiece
	{
		private int _xPosition;
		private int _yPosition;
		private Player _player;

		public int XPosition
		{
            get { return _xPosition; }
            set { _xPosition = value; }
        }

		public int YPosition
		{
            get { return _yPosition; }
            set { _yPosition = value; }
        }

		public Player Player
        {
            get => _player;
            set => _player = value;
        }

        public Connect4Piece(int x, int y, Player player)
		{
			this._xPosition = x;	
			this._yPosition = y;	
			this._player = player;
		}
	}
}
