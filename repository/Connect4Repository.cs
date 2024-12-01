using _2PlayerGames.domain.GameObjects;
using _2PlayerGames.exceptions;
using _2PlayerGames.repository;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using System.Data.SqlClient;

namespace _2PlayerGames.repo
{
    internal class Connect4Repository : BaseGameRepository
    {
        public  Connect4Repository() : base()
        {

        }

        public override Dictionary<Guid, IGame> GetGames()
        {
            SqlConnection sqlConnection = Configurator.SqlConnection;
            Dictionary<Guid, IGame> games = new Dictionary<Guid, IGame>();
            using (SqlCommand command = new SqlCommand("SELECT * FROM getAllConnect4Games", sqlConnection))
            {
                sqlConnection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Guid GameStateId = reader.GetGuid(0);
                        Player player1 = PlayerRepository.getPlayer(reader.GetGuid(1));
                        Player player2 = PlayerRepository.getPlayer(reader.GetGuid(2));
                        Games gameType = GameStore.getGameById(reader.GetGuid(3));
                        int turn = reader.GetInt32(4);
                        int timePlayed = reader.GetInt32(5);
                        Player? winner = reader[6] == System.DBNull.Value? null : (player1.Id == reader.GetGuid(6)? player1: player2);
                        string jsonString = reader.GetString(7);
                        GameState gameState = new GameState(GameStateId,player1, player2, gameType, turn, timePlayed, winner, jsonString);                        
                        IGame connect4Game = LoadGame(gameState);
                        games.Add(GameStateId, connect4Game);
                    }
                    reader.Close();
                }
            }
            sqlConnection.Close();
            return games;
        }
    }
}
