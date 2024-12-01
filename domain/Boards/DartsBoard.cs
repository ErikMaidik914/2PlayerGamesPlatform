using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using _2PlayerGames;
using Newtonsoft.Json.Linq;
using System.Runtime.InteropServices;

namespace _2PlayerGames {
    public class DartsBoard : IBoard
    {
        /// <summary>
        /// David, stiu ca mi-ai zis ca ii redundant sa tinem piesele in memorie la darts, dar am nevoie de ele pentru a putea implementa IBoard, ca IGame vrea sa
        /// returneze un IBoard
        /// </summary>
        private int _width;
        private int _height;
        private int _centerX;
        private int _centerY;
        private int _boardRadius;
        private int _bullseyeRadius;
        private List<IPiece> _dartBolts;
        private int[] _playerScores;
        private int _count;

        public static Dictionary<int, int> DartScores = DartsBoard.initializeDartScores();

        [JsonIgnore]
        public List<IPiece> Board { get => _dartBolts; set => _dartBolts = value; }

        [JsonProperty]
        private List<DartsBolt> JsonBoard
        {
            get
            {
                List<DartsBolt> dartsBolts = new List<DartsBolt>();
                foreach (IPiece bolt in this._dartBolts)
                {
                    dartsBolts.Add((DartsBolt)bolt);
                }
                return dartsBolts;
            }

        }

        public int Width { get => _width; set => _width = value; }
        public int Height { get => _height; set => _height = value; }
        public int[] Scores { get => _playerScores; set => _playerScores = value; }
        public int Count { get => _count; set => _count = value; }

        public int CenterX { get => _centerX; }

        public int CenterY { get => _centerY; }

        public int BoardRadius { get => _boardRadius; }

        public int BullseyeRadius { get => _bullseyeRadius; }

        public DartsBoard()
        {
            this._width = 500;
            this._height = 500;
            this._boardRadius = Math.Min(this._width, this._width) / 2;
            this._centerX = this._width / 2;
            this._centerY = this._height / 2;
            this._bullseyeRadius = this._boardRadius / 10;
            this._dartBolts = new List<IPiece>();
            this._playerScores = new int[2];
            this._playerScores[0] = 301;
            this._playerScores[1] = 301;
            this._count = 0;
        }

        private static Dictionary<int, int> initializeDartScores()
        {
            Dictionary<int, int> dartScores = new Dictionary<int, int>();
            dartScores.Add(0, 20);
            dartScores.Add(1, 1);
            dartScores.Add(2, 18);
            dartScores.Add(3, 4);
            dartScores.Add(4, 13);
            dartScores.Add(5, 6);
            dartScores.Add(6, 10);
            dartScores.Add(7, 15);
            dartScores.Add(8, 2);
            dartScores.Add(9, 17);
            dartScores.Add(10, 3);
            dartScores.Add(11, 19);
            dartScores.Add(12, 7);
            dartScores.Add(13, 16);
            dartScores.Add(14, 8);
            dartScores.Add(15, 11);
            dartScores.Add(16, 14);
            dartScores.Add(17, 9);
            dartScores.Add(18, 12);
            dartScores.Add(19, 5);
            return dartScores;
        }

        //public bool resetBoard()
        //{
        //    bool resetNeeded = this._dartBolts.Count == 3;
        //    if (resetNeeded)
        //    {
        //        this._dartBolts.Clear();
        //    }
        //    return resetNeeded;
        //}

        public void addPiece(IPiece bolt)
        {
            this._dartBolts.Add(bolt);
            
        }

        //this will very likely never be called, but it's here for the sake of the interface
        public IPiece? GetPiece(int xPosition, int yPosition)
        {
            return this._dartBolts.FirstOrDefault(bolt => bolt.XPosition == xPosition && bolt.YPosition == yPosition);
        }

        public void getFromJToken(JToken obj)
        {
            DartsBoard dartsBoard = obj.ToObject<DartsBoard>();
            foreach (JToken token in obj["JsonBoard"]) { 
                DartsBolt dart = token.ToObject<DartsBolt>();
                dartsBoard.Board.Add(dart);
            }
            this.Board = dartsBoard.Board;
            this.Scores = dartsBoard.Scores;
            this.Count = dartsBoard.Count;
            dartsBoard = null;
        }
    }
}