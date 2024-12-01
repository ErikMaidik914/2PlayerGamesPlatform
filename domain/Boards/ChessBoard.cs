using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using _2PlayerGames;
using _2PlayerGames.domain.DatabaseObjects;
using _2PlayerGames.domain.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace _2PlayerGames
{
    public class ChessBoard : IBoard
    {
        private List<IPiece> _chessPieces;
        private int _width;
        private int _height;
        private int[] _timers;

        [JsonIgnore]
        public List<IPiece> Board { get => _chessPieces; set => _chessPieces = value; }

        [JsonProperty]
        private List<ChessPiece> JsonBoard {
            get {
                List<ChessPiece> chessPieces = new List<ChessPiece>();
                foreach (IPiece piece in this._chessPieces)
                {
                    chessPieces.Add((ChessPiece)piece);
                }
                return chessPieces;
            }
        }
        public int Width { get => _width; set => _width = value; }
        public int Height { get => _height; set => _height = value; }

        public int[] Timers { get => _timers; set => _timers = value; }

        public ChessBoard(Player[] players, ChessModes mode)
        {
            this._chessPieces = ChessBoard.InitializeBoard(players);
            this._width = 8;
            this._height = 8;
            this._timers = new int[2];
            switch (mode) {
                case ChessModes.RAPID:
                    this._timers[0] = 600;
                    this._timers[1] = 600;
                    break;
                case ChessModes.BLITZ:
                    this._timers[0] = 300;
                    this._timers[1] = 300;
                    break;
                case ChessModes.BULLET:
                    this._timers[0] = 60;
                    this._timers[1] = 60;
                    break;
            }
        }

        public ChessBoard()
        {
            this._chessPieces = new List<IPiece>();
            this._width = 8;
            this._height = 8;
            this._timers = new int[2];
        }

        private static List<IPiece> InitializeBoard(Player[] players)
        {
            List<IPiece> chessPieces = new List<IPiece>();
            chessPieces.Add(new RookPiece(0, 0, players[0]));
            chessPieces.Add(new KnightPiece(1, 0, players[0]));
            chessPieces.Add(new BishopPiece(2, 0, players[0]));
            chessPieces.Add(new QueenPiece(3, 0, players[0]));
            chessPieces.Add(new KingPiece(4, 0, players[0]));
            chessPieces.Add(new BishopPiece(5, 0, players[0]));
            chessPieces.Add(new KnightPiece(6, 0, players[0]));
            chessPieces.Add(new RookPiece(7, 0, players[0]));

            for (int i = 0; i < 8; i++)
            {
                chessPieces.Add(new PawnPiece(i, 1, players[0]));
            }

            chessPieces.Add(new RookPiece(0, 7, players[1]));
            chessPieces.Add(new KnightPiece(1, 7, players[1]));
            chessPieces.Add(new BishopPiece(2, 7, players[1]));
            chessPieces.Add(new QueenPiece(3, 7, players[1]));
            chessPieces.Add(new KingPiece(4, 7, players[1]));
            chessPieces.Add(new BishopPiece(5, 7, players[1]));
            chessPieces.Add(new KnightPiece(6, 7, players[1]));
            chessPieces.Add(new RookPiece(7, 7, players[1]));

            for (int i = 0; i < 8; i++)
            {
                chessPieces.Add(new PawnPiece(i, 6, players[1]));
            }

            return chessPieces;
        }

        public IPiece? GetPiece(int xPosition, int yPosition)
        {
            return _chessPieces.FirstOrDefault(piece => piece.XPosition == xPosition && piece.YPosition == yPosition);
        }

        public void addPiece(IPiece piece)
        {
            this._chessPieces.Add(piece);
        }

        public bool removePiece(int xPosition, int yPosition) { 
            int numberOfPiecesRemoved = _chessPieces.RemoveAll(piece => piece.XPosition == xPosition && piece.YPosition == yPosition);
            return numberOfPiecesRemoved > 0;
        }

        public void getFromJToken(JToken obj)
        {
            List<IPiece> list = new List<IPiece>();
            foreach (JToken token in obj["JsonBoard"])
            {
                string type = token["PieceType"].ToString();
                switch (type)
                {
                    case "Pawn":
                        list.Add(token.ToObject<PawnPiece>());
                        break;
                    case "Rook":
                        list.Add(token.ToObject<RookPiece>());
                        break;
                    case "Knight":
                        list.Add(token.ToObject<KnightPiece>());
                        break;
                    case "Bishop":
                        list.Add(token.ToObject<BishopPiece>());
                        break;
                    case "Queen":
                        list.Add(token.ToObject<QueenPiece>());
                        break;
                    case "King":
                        list.Add(token.ToObject<KingPiece>());
                        break;
                }
            }
            this.Board = list;
            this.Timers = obj["Timers"].ToObject<int[]>();
        }
    }
}