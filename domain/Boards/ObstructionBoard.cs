using System;
using System.Collections.Generic;
using System.Linq;
using _2PlayerGames;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace _2PlayerGames
{
    public class ObstructionBoard : IBoard
    {
        private List<IPiece> _obstractionPieces;
        private int _width;
        private int _height;


        [JsonIgnore]
        public List<IPiece> Board { get => _obstractionPieces; set => _obstractionPieces = value; }

        [JsonProperty]
        public List<ObstructionPiece> JsonBoard
        {
            get
            {
                List<ObstructionPiece> obstructionPieces = new List<ObstructionPiece>();
                foreach (IPiece piece in this._obstractionPieces)
                {
                    obstructionPieces.Add((ObstructionPiece)piece);
                }
                return obstructionPieces;
            }
        }
        public int Width { get => _width; set => _width = value; }
        public int Height { get => _height; set => _height = value; }


        public ObstructionBoard(int width, int height)
        {
            this._obstractionPieces = new List<IPiece>();
            this._width = width;
            this._height = height;
        }
        public IPiece? GetPiece(int xPosition, int yPosition)
        {
            return this._obstractionPieces.FirstOrDefault(piece => piece.XPosition == xPosition && piece.YPosition == yPosition);
        }
        public void addPiece(IPiece piece)
        {
            this._obstractionPieces.Add(piece);
        }
        public bool isFull() => this._width * this._height == this._obstractionPieces.Count;

        public void getFromJObject(JObject obj)
        {
            throw new NotImplementedException();
        }

        public void getFromJToken(JToken obj)
        {
            List<IPiece> list = new List<IPiece>();
            foreach (JToken token in obj["JsonBoard"])
            {
                ObstructionPiece obstruction = token.ToObject<ObstructionPiece>();
                list.Add(obstruction);
            }
            this.Board = list;
            this.Width = obj["Width"].ToObject<int>();
            this.Height = obj["Height"].ToObject<int>();
        }
    }
}