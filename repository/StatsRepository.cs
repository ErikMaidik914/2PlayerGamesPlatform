using _2PlayerGames.domain.Auxiliary;
using _2PlayerGames.domain.DatabaseObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2PlayerGames.repository
{
    internal class StatsRepository
    {
        public static bool addStats(GameStats gameStats)
        {
            SqlConnection sqlConnection = Configurator.SqlConnection;
            using (SqlCommand command = new SqlCommand("addGameStats", sqlConnection))
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@playerId", gameStats.Player.Id);
                command.Parameters.AddWithValue("@gameId", gameStats.Game.Id);
                command.Parameters.AddWithValue("@eloRating", gameStats.EloRating);
                command.Parameters.AddWithValue("@highestEloRating", gameStats.HighestElo);
                command.Parameters.AddWithValue("@totalPlayTime", gameStats.TotalPlayTime);
                command.Parameters.AddWithValue("@totalGamesPlayed", gameStats.TotalMatches);
                command.Parameters.AddWithValue("@totalWins", gameStats.TotalWins);
                command.Parameters.AddWithValue("@totalDraws", gameStats.TotalDraws);
                command.Parameters.AddWithValue("@numberTurns", gameStats.TotalNumberOfTurn);
                sqlConnection.Open();

                int result = command.ExecuteNonQuery();

                sqlConnection.Close();

                return result == 1;
            }

        }

        public static bool UpdateStats(Player player, Games gameType, int newEloRating) {
            SqlConnection sqlConnection = Configurator.SqlConnection;
            using (SqlCommand command = new SqlCommand("updateGameStatsAuto", sqlConnection))
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@playerId", player.Id);
                command.Parameters.AddWithValue("@gameId", gameType.Id);
                command.Parameters.AddWithValue("@eloRating", newEloRating);
                sqlConnection.Open();

                int result = command.ExecuteNonQuery();

                sqlConnection.Close();

                return result == 1;
            }
        }

        public static GameStats GetGameStats(Player player, Games gameType)
        {
            SqlConnection sqlConnection = Configurator.SqlConnection;
            using (SqlCommand command = new SqlCommand("SELECT * FROM getGameStats(@playerId, @gameId)", sqlConnection))
            {
                command.Parameters.AddWithValue("@playerId", player.Id);
                command.Parameters.AddWithValue("@gameId", gameType.Id);
                sqlConnection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        GameStats newGameStats = new GameStats(player, gameType, reader.GetInt32(2), reader.GetInt32(3), reader.GetInt32(4),
                            reader.GetInt32(5), reader.GetInt32(6), reader.GetInt32(7), reader.GetInt32(8));
                        reader.Close();
                        sqlConnection.Close();
                        return newGameStats;
                    }
                    else
                    {
                        reader.Close();
                        sqlConnection.Close();
                        return new GameStats(player, gameType);
                    }
                }
            }
        }

        public static List<GameHistory> GetGameHistory(Player player)
        {
            List<GameHistory> gameHistories = new List<GameHistory>();
            SqlConnection sqlConnection = Configurator.SqlConnection;
            using (SqlCommand command = new SqlCommand("SELECT * FROM getGameHistory(@playerId)", sqlConnection))
            {
                command.Parameters.AddWithValue("@playerId", player.Id);
                sqlConnection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Player player1 = PlayerRepository.getPlayer((Guid)reader["player1"]);
                        Player player2 = PlayerRepository.getPlayer((Guid)reader["player2"]);
                        gameHistories.Add(new GameHistory(
                            player1,
                            player2,
                            GameStore.getGameById((Guid)reader["gameId"]),
                            (player1.Id == (Guid)reader["winner"]) ? player1 : player2
                            )
                        );
                    }
                    reader.Close();
                }
            }
            sqlConnection.Close();
            return gameHistories;
        }

        public static PlayerStats GetProfileStats(Player player)
        {
            SqlConnection sqlConnection = Configurator.SqlConnection;
            using (SqlCommand command = new SqlCommand("SELECT * FROM getPlayerStatsGood(@playerId)", sqlConnection))
            {
                command.Parameters.AddWithValue("@playerId", player.Id);
                sqlConnection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int trophies = (int)reader["trophies"];
                        int eloRating = (int)reader["averageElo"];
                        Games game = GameStore.getGameById((Guid)reader["mostPlayedGame"]);
                        string rank = RankDeterminer.determine(eloRating);
                        PlayerStats playerStats = new PlayerStats(player, trophies, eloRating, rank, game);
                        reader.Close();
                        sqlConnection.Close();
                        return playerStats;
                    }
                    else
                    {            
                        reader.Close();
                        sqlConnection.Close();
                        return new PlayerStats(player);
                    }
                }
            }
        }


    }
}
