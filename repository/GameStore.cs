using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2PlayerGames.repository
{
    public class GameStore
    {
        public static Dictionary<string, Games> Games { get; } = initializeGames();

        private static Dictionary<string, Games> initializeGames()
        {
            Dictionary<string, Games> dictionaryOfGames = new Dictionary<string, Games>();
            //dictionaryOfGames.Add("Connect4", new Games("Connect4", "2 Player Game"));
            //dictionaryOfGames.Add("Chess", new Games("Chess", "2 Player Game"));
            //dictionaryOfGames.Add("Obstruction", new Games("Obstruction", "2 Player Game"));
            //dictionaryOfGames.Add("Darts", new Games("Darts", "2 Player Game"));
            // this is temp, we need to get these from the database

            SqlConnection sqlConnection = Configurator.SqlConnection;
            using (SqlCommand command = new SqlCommand("SELECT * FROM Game", sqlConnection))
            {
                sqlConnection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dictionaryOfGames.Add(reader.GetString(1), new Games(reader.GetGuid(0), reader.GetString(1), reader.GetString(2)));
                    }
                    reader.Close();
                }
            }
            sqlConnection.Close();
            return dictionaryOfGames;
        }

        public static Games getGameById(Guid id) => GameStore.Games.Values.FirstOrDefault(x => x.Id.Equals(id));
    }
}
