using _2PlayerGames.domain.Enums;
using _2PlayerGames.exceptions;
using _2PlayerGames.service.Bot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2PlayerGames.repository
{
    internal class BotStore
    {
        public static IBot getBot(String gameType, String difficulty, Guid gameStateId, Player player)
        {
            if(difficulty != "easy" && difficulty != "medium" && difficulty != "hard")
            {
                throw new InvalidDifficultyException();
            }

            switch (gameType)
            {
                case "Connect4":
                    return new Connect4Bot(difficulty, gameStateId, player);
                case "Chess":
                    return new Connect4Bot(difficulty, gameStateId, player);
                case "Obstruction":
                    return new Connect4Bot(difficulty, gameStateId, player);
                case "Darts":
                    return new Connect4Bot(difficulty, gameStateId, player);
                default:
                    throw new InvalidGameException();
            }
        }
    }
}
