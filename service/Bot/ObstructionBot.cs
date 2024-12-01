using _2PlayerGames.domain.GameObjects;
using _2PlayerGames.exceptions;
using _2PlayerGames.service;
using System;

namespace _2PlayerGames.domain.Bot
{
    internal class ObstructionBot : Player
    {
        private readonly string difficulty;
        private readonly IGameService gameService;

        public ObstructionBot(string difficulty, IGameService gameService)
        {
            this.difficulty = difficulty;
            this.gameService = gameService;
        }

        private void CalculateMove(ObstructionGame game)
        {
            int width = game.Board.Width;
            int height = game.Board.Height;

            int bestX = 0;
            int bestY = 0;
            int maxPriority = int.MinValue;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (IsMoveValid(game, x, y))
                    {
                        int priority = CalculatePriority(game, x, y);
                        if (priority > maxPriority)
                        {
                            maxPriority = priority;
                            bestX = x;
                            bestY = y;
                        }
                    }
                }
            }

            gameService.Play(2, new object[] { bestX, bestY });
        }

        private int CalculatePriority(ObstructionGame game, int x, int y)
        {

            int priority = Math.Min(Math.Min(x, game.Board.Width - x - 1), Math.Min(y, game.Board.Height - y - 1));

            return priority;
        }

        private bool IsMoveValid(ObstructionGame game, int x, int y)
        {
            return x >= 0 && x < game.Board.Width && y >= 0 && y < game.Board.Height && game.Board.GetPiece(x, y) == null;
        }

        private void RandomMove(ObstructionGame game)
        {
            int width = game.Board.Width;
            int height = game.Board.Height;

            Random random = new Random();
            int x = random.Next(0, width);
            int y = random.Next(0, height);

            while (!IsMoveValid(game, x, y))
            {
                x = random.Next(0, width);
                y = random.Next(0, height);
            }

            gameService.Play(2, new object[] { x, y });
        }
    }
}
