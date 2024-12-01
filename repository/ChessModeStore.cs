using _2PlayerGames.domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2PlayerGames.repository
{
    internal class ChessModeStore
    {
        public static ChessModes getMode(String mode)
        {
            switch(mode)
            {
                case "RAPID":
                    return ChessModes.RAPID;
                case "BLITZ":
                    return ChessModes.BLITZ;
                case "BULLET":
                    return ChessModes.BULLET;
                default:
                    throw new Exception("Invalid mode");
            }
        }
    }
}
