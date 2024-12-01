using System;
using _2PlayerGames;

namespace _2PlayerGames
{
    public abstract class ChessPiece : IChessPiece
    {
        protected int _xPosition;
        protected int _yPosition;
        protected Player _player;
        protected bool _movedFromInitialPosition;
        protected string _pieceType;

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

        public bool MovedFromInitialPosition
        {
            get { return _movedFromInitialPosition; }
            set { _movedFromInitialPosition = value; }
        }

        public string PieceType { get => _pieceType; }

        public ChessPiece(int x, int y, Player player)
        {
            this._xPosition = x;
            this._yPosition = y;
            this._player = player;
            this._movedFromInitialPosition = false;
            this._pieceType = "";
        }



        public void updatePosition(int newXPosition, int newYPosition)
        {
                this._xPosition = newXPosition;
                this._yPosition = newYPosition;
                this._movedFromInitialPosition = true;
        }

        public abstract bool isValidMove(int newXPosition, int newYPosition);

        public String GetPieceType()
        {
            return this._pieceType;
        }
    }
}
