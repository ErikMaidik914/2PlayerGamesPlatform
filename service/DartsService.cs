using _2PlayerGames.domain.Bot;
using _2PlayerGames.domain.GameObjects;
using _2PlayerGames.repo;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2PlayerGames.service
{
    internal class DartsService : IGameService
    {
        private DartsGame dartsGame;
        private readonly List<Player> playersId;


        public DartsService(Guid gameStateId, Player player1, Player player2)
        {
            DartsRepository dartsRepo = new DartsRepository();
            playersId = [player1, player2];
            if (gameStateId == Guid.Empty)
            {
                dartsGame = new DartsGame(player1, player2);
                if(player2.Name != "Bot")
                {
                    dartsRepo.AddGame(dartsGame);
                }
                Random random = new();
                int turn = random.Next(0, 2);
                dartsGame.GameState.Turn = turn;
            }
            else
            {
                dartsGame = (DartsGame?)dartsRepo.GetGame(gameStateId);
            }
            
        }

        private int CalculateScore(DartsBolt bolt)
        {
            DartsBoard board = (DartsBoard)dartsGame.Board;
            int xDistance = Math.Abs(bolt.XPosition - board.CenterX);
            int yDistance = Math.Abs(bolt.YPosition - board.CenterY);
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

        private int GetRadius(int x)
        {
            return (-5/2)*x+250;
        }

        private (int, int) GetRandomPoint(int xCentre, int yCentre, int radius)
        {
            Random random = new();
            int angle = (int)(random.Next() * 2 * Math.PI);
            int hypothenuse = (int)(Math.Sqrt(random.Next()) * radius);
            int adjecent = (int)(Math.Cos(angle) * hypothenuse);
            int opposite = (int)(Math.Sin(angle) * hypothenuse);
            return (xCentre + adjecent, yCentre + opposite);
        }

        private (int, int) ThrowDart(int xTarget, int yTarget, int accuracy)
        {
            int radius = GetRadius(accuracy);
            return GetRandomPoint(xTarget, yTarget, radius);
        }

        private int CalculateThrowScore(int xTarget, int yTarget, Player player)
        {
            DartsBolt dartsBolt = new DartsBolt(xTarget, yTarget, player);
            dartsGame.Board.addPiece(dartsBolt);
            return CalculateScore(dartsBolt);
        }

        private bool CheckCurrentState()
        {
            DartsBoard board = (DartsBoard)dartsGame.Board;
            return board.Scores[dartsGame.GameState.Turn] == 0;   
        }


        private void SwitchTurn()
        {
            ((DartsBoard)dartsGame.Board).Count++;
            if(((DartsBoard)dartsGame.Board).Count == 3)
            {
                dartsGame.GameState.Turn = dartsGame.GameState.Turn == 0 ? 1 : 0;
            }
        }

        private Player GetCurrentPlayer()
        {
            return playersId[dartsGame.GameState.Turn];
        }

        public IGame Play(int nrParameters, object[] parameters)
        {
            Player player = GetCurrentPlayer();
           
            int xTarget = Convert.ToInt32(parameters[0]);
            int yTarget = Convert.ToInt32(parameters[1]);
            int accuracy = Convert.ToInt32(parameters[2]);

            (int xToss, int yToss) = ThrowDart(xTarget, yTarget, accuracy);
            int score = CalculateThrowScore(xToss, yToss, player);

            int turn = dartsGame.GameState.Turn;
            DartsBoard board = (DartsBoard)dartsGame.Board;
            int oldScore = board.Scores[turn];
            int newScore = board.Scores[turn] - score;
            board.Scores[turn] = newScore < 0 ? oldScore : newScore ;

            if (CheckCurrentState())
            {
                dartsGame.GameState.Winner = (Player)player;
            }

            SwitchTurn();

            dartsGame.saveGame();

            return dartsGame;
        }

        public IGame GetGame()
        {
            return dartsGame;
        }

        public void SetGame(IGame game)
        {
            dartsGame = (DartsGame)game;
        }
    }
}
