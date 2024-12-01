using _2PlayerGames.domain.Auxiliary;
using _2PlayerGames.domain.DatabaseObjects;
using _2PlayerGames.repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2PlayerGames.service
{
    internal class PlayerProfileService
    {

        public PlayerProfileService()
        {
        }

        public List<String> GetProfileStats(Player player)
        {
            //PlayerStats profileStats = statsRepository.GetProfileStats(player);
            //List<String> stats =
            //[
            //    profileStats.Rank.ToString(),
            //    profileStats.Trophies.ToString(),
            //    profileStats.FavoriteGame.ToString(),
            //];
            List<String> profileStats = new List<String> { "diamond", "512", "Mata"};
            return profileStats;
        }

        public List<String> GetGameStats(Player player, string gameType)
        {
            GameStats gameStats = StatsRepository.GetGameStats(player, GameStore.Games[gameType]);
            List<String> stats =
            [
                gameStats.EloRating.ToString(),
                gameStats.HighestElo.ToString(),
                gameStats.TotalMatches.ToString(),
                gameStats.TotalWins.ToString(),
                gameStats.TotalDraws.ToString(),
                gameStats.TotalPlayTime.ToString(),
                gameStats.TotalNumberOfTurn.ToString(),
            ];
            return stats;
        }

        public List<String> GetGameHistory(Player player)
        {
            //list<gamestate> gamehistory = statsrepository.getgamehistory(player);
            //list<string> history = new list<string>();
            //foreach (gamehistory game in gamehistory)
            //{
            //    history.add(game.gametype + " " + game. + " " + game.date);
            //  }
            return new List<String>();
         }


                public void updateStats(Player player, Games gameType, int newElo)
                {
                    StatsRepository.UpdateStats(player, gameType, newElo);
                }

    }
}
