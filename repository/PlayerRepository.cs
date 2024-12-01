using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2PlayerGames.repository
{
    internal class PlayerRepository
    {

        public static Dictionary<Guid, Player> Players { get; } = PlayerRepository.getAll();
        public static Player DrawPlayer { get; } = PlayerRepository.getPlayer(Guid.Empty);
        public static Player AIPlayer { get; } = PlayerRepository.getPlayer(Guid.Parse("00000000-0000-0000-0000-000000000001"));

        public static Player getPlayer(Guid id) {
            //SqlConnection sqlConnection = Configurator.SqlConnection;

            //using (SqlCommand command = new SqlCommand("SELECT * FROM getPlayer(@playerId)", sqlConnection))
            //{
            //    command.Parameters.AddWithValue("@playerId", id);
            //    using (SqlDataReader reader = command.ExecuteReader())
            //    {
            //        if (reader.Read())
            //        {
            //            Player newPlayer = new Player(reader.GetGuid(0), reader.GetString(1), reader.GetString(2), reader.GetInt32(3));
            //            sqlConnection.Close();
            //            reader.Close();
            //            return newPlayer;
            //        }
            //        else
            //        {
            //            sqlConnection.Close();
            //            reader.Close();
            //            return null;
            //        }
            //    }   
            //} 

            return Players.ContainsKey(id) ? Players[id] : null;
        }

        public static Dictionary<Guid, Player> getAll()
        {
            SqlConnection connection = Configurator.SqlConnection;
            connection.Open();
            Dictionary<Guid, Player> players = new();
            using (SqlCommand command = new SqlCommand("SELECT * FROM Player", connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        players.Add(reader.GetGuid(0), new Player(reader.GetGuid(0), reader.GetString(1),
                            reader.IsDBNull(2) ? null : reader.GetString(2),
                            reader.IsDBNull(3) ? null : reader.GetInt32(3)));
                    }
                    connection.Close();
                    reader.Close();
                }
            }
            return players;
        }


        public static bool addPlayer(Player player) {
            SqlConnection sqlConnection = Configurator.SqlConnection;
            sqlConnection.Open();
            using (SqlCommand command = new SqlCommand("addPlayer", sqlConnection))
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@playerId", player.Id);
                command.Parameters.AddWithValue("@playerName", player.Name);
                command.Parameters.AddWithValue("@playerPcIp", player.Ip);
                command.Parameters.AddWithValue("@playerPcPort", player.Port);

                int result = command.ExecuteNonQuery();

                sqlConnection.Close();

                return result == 1;
            }
        }

        public static bool removeAddress(Guid id) { 
            SqlConnection sqlConnection = Configurator.SqlConnection;
            using (SqlCommand command = new SqlCommand("removePlayerAddress", sqlConnection)) {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@playerId", id);
                sqlConnection.Open();

                int result = command.ExecuteNonQuery();

                sqlConnection.Close();

                return result == 1;
            }
        }

        public static bool updatePlayer(Player player)
        {
            SqlConnection sqlConnection = Configurator.SqlConnection;
            using (SqlCommand command = new SqlCommand("updatePlayer", sqlConnection))
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@playerId", player.Id);
                command.Parameters.AddWithValue("@playerName", player.Name);
                command.Parameters.AddWithValue("@playerPcIp", player.Ip);
                command.Parameters.AddWithValue("@playerPcPort", player.Port);
                sqlConnection.Open();

                int result = command.ExecuteNonQuery();

                sqlConnection.Close();

                return result == 1;
            }
        }
    }
}
