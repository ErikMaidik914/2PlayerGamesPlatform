using _2PlayerGames.domain.DatabaseObjects;
using _2PlayerGames.domain.Enums;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace _2PlayerGames.repository
{
    internal class PlayerQueueRepository
    {

        private static PlayerQueueRepository instance;

        private PlayerQueueRepository()
        {
        }

        public static PlayerQueueRepository GetInstance()
        {
            if (instance == null)
            {
                instance = new PlayerQueueRepository();
            }
            return instance;
        }


        public static bool AddPlayer(PlayerQueue player)
        {
            SqlConnection sqlConnection = Configurator.SqlConnection;
            using (SqlCommand command = new SqlCommand("addToQueue", sqlConnection))
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@playerId", player.Player.Id);
                command.Parameters.AddWithValue("@gameId", player.GameType.Id);
                command.Parameters.AddWithValue("@eloRating", player.EloRating);
                command.Parameters.AddWithValue("@chessGameMode", player.ChessMode is null ? DBNull.Value : (int)player.ChessMode);
                command.Parameters.AddWithValue("@obstructionWidth", player.ObstractionWidth is null ? DBNull.Value : player.ObstractionWidth);
                command.Parameters.AddWithValue("@obstructionHeight", player.ObstractionHeight is null ? DBNull.Value : player.ObstractionHeight);
                sqlConnection.Open();

                bool result = command.ExecuteNonQuery() == 1;
                sqlConnection.Close();
                return result;
            }
        }

        public static PlayerQueue GetPlayer(Guid playerId)
        {
            SqlConnection sqlConnection = Configurator.SqlConnection;
            using (SqlCommand command = new SqlCommand("SELECT * FROM getPlayerQueue(@playerId)", sqlConnection))
            {
                command.Parameters.AddWithValue("@playerId", playerId);
                sqlConnection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    
                    if (reader.Read())
                    {
                        PlayerQueue queue = new PlayerQueue(
                            player: PlayerRepository.getPlayer(reader.GetGuid(0)),
                            gameType: GameStore.getGameById(reader.GetGuid(1)),
                            elo: reader.GetInt32(2),
                            chessMode: reader[3] == System.DBNull.Value ? null : (ChessModes)reader.GetInt32(5),
                            obstrustionWidth: reader[4] == System.DBNull.Value ? null : reader.GetInt32(4),
                            obstructionHeigth: reader[5] == System.DBNull.Value ? null : reader.GetInt32(5)
                            );
                        reader.Close();
                        sqlConnection.Close();

                        return queue;

                    }
                    reader.Close();
                    sqlConnection.Close();
                    return null;
                }
            }

        }

        public static List<PlayerQueue> GetPlayers()
        {
            List<PlayerQueue> playerQueues = new List<PlayerQueue>();
            SqlConnection sqlConnection = Configurator.SqlConnection;
            using (SqlCommand command = new SqlCommand("SELECT * FROM PlayerQueue", sqlConnection))
            {
                sqlConnection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        playerQueues.Add(new PlayerQueue(
                           player: PlayerRepository.getPlayer(reader.GetGuid(0)),
                           gameType: GameStore.getGameById(reader.GetGuid(1)),
                           elo: reader.GetInt32(2),
                           chessMode: reader[3] == System.DBNull.Value ? null : (ChessModes)reader.GetInt32(3),
                           obstrustionWidth: reader[4] == System.DBNull.Value ? null : reader.GetInt32(4),
                           obstructionHeigth: reader[5] == System.DBNull.Value ? null : reader.GetInt32(5)
                           ));
                    }
                    reader.Close();
                }
            }
            sqlConnection.Close();
            return playerQueues;
        }

        public static void removePlayer(PlayerQueue player)
        {
            SqlConnection sqlConnection = Configurator.SqlConnection;
            using (SqlCommand command = new SqlCommand("removeFromQueue", sqlConnection))
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@playerId", player.Player.Id);
                sqlConnection.Open();

                command.ExecuteNonQuery();
                sqlConnection.Close();
            }
        }

        public List<PlayerQueue> GetAvailableChessPlayers(PlayerQueue player)
        {
            List<PlayerQueue> playerQueues = new List<PlayerQueue>();
            SqlConnection sqlConnection = Configurator.SqlConnection;
            using (SqlCommand command = new SqlCommand("SELECT * FORM getAvailableChessGames(@playerId, @eloRating, @chessGameMode))", sqlConnection))
            {
                command.Parameters.AddWithValue("@playerId", player.Player.Id);
                command.Parameters.AddWithValue("@eloRating", player.EloRating);
                command.Parameters.AddWithValue("@chessGameMode", (int)player.ChessMode);
                sqlConnection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        playerQueues.Add(new PlayerQueue(
                           player: PlayerRepository.getPlayer((Guid)reader["PlayerId"]),
                           gameType: GameStore.getGameById((Guid)reader["GameId"]),
                           elo: (int)reader["Elo"],
                           chessMode: reader[3] == System.DBNull.Value ? null : (ChessModes)reader.GetInt32(3),
                           obstrustionWidth: null,
                           obstructionHeigth: null
                           ));
                    }
                    reader.Close();
                }
            }
            sqlConnection.Close();
            return playerQueues;
        }

        public List<PlayerQueue> GetAvailableDartsPlayers(PlayerQueue player)
        {
            List<PlayerQueue> playerQueues = new List<PlayerQueue>();
            SqlConnection sqlConnection = Configurator.SqlConnection;
            using (SqlCommand command = new SqlCommand("SELECT * FORM getAvailableDartsGames(@playerId, @eloRating))", sqlConnection))
            {
                command.Parameters.AddWithValue("@playerId", player.Player.Id);
                command.Parameters.AddWithValue("@eloRating", player.EloRating);
                sqlConnection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        playerQueues.Add(new PlayerQueue(
                           player: PlayerRepository.getPlayer((Guid)reader["PlayerId"]),
                           gameType: GameStore.Games["Darts"],
                           elo: (int)reader["Elo"],
                           chessMode: null,
                           obstrustionWidth: null,
                           obstructionHeigth: null
                           ));
                    }
                    reader.Close();
                }
            }
            sqlConnection.Close();
            return playerQueues;
        }

        public List<PlayerQueue> GetAvailableConnect4Players(PlayerQueue player)
        {
            List<PlayerQueue> playerQueues = new List<PlayerQueue>();
            SqlConnection sqlConnection = Configurator.SqlConnection;
            using (SqlCommand command = new SqlCommand("SELECT * FORM getAvailableConnect4Games(@playerId, @eloRating))", sqlConnection))
            {
                command.Parameters.AddWithValue("@playerId", player.Player.Id);
                command.Parameters.AddWithValue("@eloRating", player.EloRating);
                sqlConnection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        playerQueues.Add(new PlayerQueue(
                           player: PlayerRepository.getPlayer((Guid)reader["PlayerId"]),
                           gameType: GameStore.Games["Connect4"],
                           elo: (int)reader["Elo"],
                           chessMode: null,
                           obstrustionWidth: null,
                           obstructionHeigth: null
                           ));
                    }
                    reader.Close();
                }
            }
            sqlConnection.Close();
            return playerQueues;
        }

        public List<PlayerQueue> GetAvailableObstructionPlayers(PlayerQueue player)
        {
            List<PlayerQueue> playerQueues = new List<PlayerQueue>();
            SqlConnection sqlConnection = Configurator.SqlConnection;
            using (SqlCommand command = new SqlCommand("SELECT * FORM getAvailableObstructionGames(@playerId, @eloRating, @obstructionWidth, @obstructionHeight))",
                sqlConnection))
            {
                command.Parameters.AddWithValue("@playerId", player.Player.Id);
                command.Parameters.AddWithValue("@eloRating", player.EloRating);
                command.Parameters.AddWithValue("@obstructionWidth", player.ObstractionWidth);
                command.Parameters.AddWithValue("@obstructionHeight", player.ObstractionHeight);
                sqlConnection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        playerQueues.Add(new PlayerQueue(
                           player: PlayerRepository.getPlayer((Guid)reader["PlayerId"]),
                           gameType: GameStore.Games["Obstruction"],
                           elo: (int)reader["Elo"],
                           chessMode: null,
                           obstrustionWidth: (int)reader["ObsWidth"],
                           obstructionHeigth: (int)reader["ObsHeight"]
                           ));
                    }
                    reader.Close();
                }
            }
            sqlConnection.Close();
            return playerQueues;
        }

    }

}
