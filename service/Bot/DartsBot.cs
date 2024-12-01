using _2PlayerGames.service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2PlayerGames.domain.Bot
{
    internal class DartsBot
    {

        IGameService gameService;
        Player player;

        public DartsBot(string difficulty, Guid gameStateId, Player player)
        {
            gameService = new DartsService(gameStateId, player, new Player(Guid.NewGuid(), "DartsBot", "Nacho", 89));
            this.player = player;
        }


        //public (int nrParameters, object[] parameters) Play(IGame game)
        //{
        //    //DartsGame dartsGame = (DartsGame)game;
        //    //DartsBoard board = (DartsBoard)dartsGame.Board;

        //    //// Get the current player's turn
        //    //Player currentPlayer = GetCurrentPlayer();

        //    // Generate random coordinates within the board's bounds
        //    //Random random = new Random();
        //    //int xTarget = random.Next(board.LeftBound, board.RightBound + 1);
        //    //int yTarget = random.Next(board.TopBound, board.BottomBound + 1);

        //    //int accuracy = random.Next(1, 101);

        //    //int score = CalculateThrowScore(xTarget, yTarget, accuracy);

        //    //// Perform the throw by calling the Play method of the game service
        //    //gameService.Play(3, new object[] { xTarget, yTarget, accuracy });

        //    // Return the same parameters for consistency
        //    return ;
        //}

        private int CalculateThrowScore(int xTarget, int yTarget, int accuracy)
        {
            DartsBoard board = (DartsBoard)gameService.GetGame().Board;
            int xDistance = Math.Abs(xTarget - board.CenterX);
            int yDistance = Math.Abs(yTarget - board.CenterY);
            int distanceFromCenter = (int)Math.Sqrt(xDistance * xDistance + yDistance * yDistance);
            int angle = (int)Math.Atan2(yDistance, xDistance);
            int dartScore = DartsBoard.DartScores[(int)(angle * 180 / Math.PI)];

            if (distanceFromCenter <= board.BullseyeRadius)
            {
                return 50;
            }
            else if (distanceFromCenter <= board.BullseyeRadius * 2)
            {
                return 25;
            }
            else if (distanceFromCenter > board.BoardRadius)
            {
                return 0;
            }
            else
            {
                int multiplier = 1;
                if (distanceFromCenter > board.BoardRadius / 2 && distanceFromCenter < board.BoardRadius / 2 + board.BullseyeRadius)
                {
                    multiplier = 3;
                }
                else if (distanceFromCenter > board.BoardRadius - board.BullseyeRadius && distanceFromCenter < board.BoardRadius)
                {
                    multiplier = 2;
                }
                return dartScore * multiplier;
            }
        }
    }
}
