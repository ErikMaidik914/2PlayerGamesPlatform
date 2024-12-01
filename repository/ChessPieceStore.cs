using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2PlayerGames.repository
{
    internal class ChessPieceStore
    {
        private static Dictionary<string, string> blackStore = new Dictionary<string, string>
        {
            {"Pawn", "../Images/PawnB.png"},
            {"Rook", "../Images/RookB.png"},
            {"Knight", "../Images/KnightB.png"},
            {"Bishop", "../Images/BishopB.png"},
            {"Queen", "../Images/QueenB.png"},
            {"King", "/Images/KingB.png"}
        };

        private static Dictionary<string, string> whiteStore = new Dictionary<string, string>
        {
            {"Pawn", "../Images/PawnW.png"},
            {"Rook", "../Images/RookW.png"},
            {"Knight", "../Images/KnightW.png"},
            {"Bishop", "../Images/BishopW.png"},
            {"Queen", "../Images/QueenW.png"},
            {"King", "../Images/KingW.png"}
        };
        public static String getPiece(String pieceType, String color)
        {
            if(color == "white")
            {
                return whiteStore[pieceType];
            }
            else
            {
                return blackStore[pieceType];
            }
        }
    }
}
