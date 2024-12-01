using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2PlayerGames.repository
{
    internal class SoundStore
    {
        public static Dictionary<string, Sounds> Sounds { get; } = initializeSounds();

        private static Dictionary<string, Sounds> initializeSounds()
        {
            SqlConnection sqlConnection = Configurator.SqlConnection;
            Dictionary<string, Sounds> dictionaryOfSounds = new Dictionary<string, Sounds>();
            using (SqlCommand command = new SqlCommand("SELECT * FROM SoundEffects", sqlConnection))
            {
                sqlConnection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        dictionaryOfSounds.Add(reader.GetString(1), new Sounds(reader.GetGuid(0), reader.GetString(1), reader.GetString(2), Configurator.SoundStorePath + reader.GetString(3)));
                    }
                }
            }
            return dictionaryOfSounds;
        }
    }
}
