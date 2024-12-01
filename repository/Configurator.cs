using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace _2PlayerGames.repository
{
    internal class Configurator
    {

        public static SqlConnection SqlConnection { get; } = new SqlConnection(@"Data Source=ROGER;Initial Catalog=TwoPlayerDB;Integrated Security=True");

        public static String SoundStorePath { get; } = "C:\\soundEffects\\"; 

        //private static SqlConnection getConnection()
        //{
        //    DotEnv.Load();
        //    var envVars = DotEnv.Read();
        //    string connectionString = envVars["CONNECTION_STRING"];
        //    return new SqlConnection(connectionString);
        //}
        
        //private static String  getSoundStorePath()
        //{
        //    DotEnv.Load();
        //    var envVars = DotEnv.Read();
        //    return envVars["SOUND_STORE_PATH"];
        //}

    }
}
