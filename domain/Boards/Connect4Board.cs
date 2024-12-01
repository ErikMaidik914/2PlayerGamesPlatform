using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Windows.Media.Animation;
using _2PlayerGames;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace _2PlayerGames
{
    public class Connect4Board : IBoard
    {
        private List<IPiece> _connect4Pieces;
        private int _width;
        private int _height;

        [JsonIgnore]
        public List<IPiece> Board { get => _connect4Pieces; set => _connect4Pieces = value; }

        [JsonProperty]
        private List<Connect4Piece> JsonBoard {
            get
            {
                List<Connect4Piece> connect4Pieces = new List<Connect4Piece>();
                foreach (IPiece bolt in this._connect4Pieces)
                {
                    connect4Pieces.Add((Connect4Piece)bolt);
                }
                return connect4Pieces;
            }
        }

        public int Width { get => _width; set => _width = value; }
        public int Height { get => _height; set => _height = value; }

        public Connect4Board()
        {
            this._connect4Pieces = new List<IPiece>();
            this._width = 8;
            this._height = 8;
        }

        public IPiece? GetPiece(int xPosition, int yPosition)
        {
            IPiece piece =  this._connect4Pieces.FirstOrDefault(piece => piece.XPosition == xPosition && piece.YPosition == yPosition);
            if(piece == null)
                return new Connect4Piece(xPosition, yPosition, Player.Null());
            return piece;
        }

        public void addPiece(IPiece piece)
        {
            this._connect4Pieces.Add(piece);
        }

        public bool isFull() => this._width * this._height == this._connect4Pieces.Count;

        public void removePiece(int xPosition, int yPosition)
        {
            this._connect4Pieces.Remove(this.GetPiece(xPosition, yPosition));
        }

        public void getFromJToken(JToken obj)
        {
            List<IPiece> list = new List<IPiece>();
            foreach (JToken token in obj["JsonBoard"])
            {
                Connect4Piece connect4Piece = token.ToObject<Connect4Piece>();
                list.Add(connect4Piece);
            }
            this.Board = list;
        }
    }
}

