using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2PlayerGames.domain.Auxiliary
{
    internal class RankDeterminer
    {
        public static string determine(int elo) {
            if (elo < 500)
                return "Bronze";
            else if (elo < 1000)
                return "Silver";
            else if (elo < 1500)
                return "Gold";
            else
                return "Diamonds";
        }
    }
}
