using System;
using _2PlayerGames;
using Newtonsoft.Json;

namespace _2PlayerGames
{
	public class DartsBolt: IPiece
	{
		private int _xPosition;
		private int _yPosition;
		private Player _player;

		[JsonProperty]
		public int XPosition
		{
			get { return _xPosition; }
			set { _xPosition = value; }
		}

        [JsonProperty]
        public int YPosition
		{
			get { return _yPosition; }
			set { _yPosition = value; }
		}

        [JsonProperty]
        public Player Player
		{
			get { return _player; }
			set { _player = value; }
		}

		public DartsBolt(int xPosition, int yPosition, Player player)
		{
			this._xPosition = xPosition;
			this._yPosition = yPosition;
			this._player = player;
		}
	}
}