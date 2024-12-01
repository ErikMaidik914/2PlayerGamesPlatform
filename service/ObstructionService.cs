using System;
using System.Collections.Generic;
using _2PlayerGames;

using _2PlayerGames.domain.GameObjects;
using _2PlayerGames.exceptions;
using _2PlayerGames.repo;

namespace _2PlayerGames.service
{
    internal class ObstructionService : IGameService
    {
        private ObstructionGame obstructionGame;
        private List<Player> players;

        public ObstructionService(Guid gameStateID, Player player1, Player player2, int width, int height)
        {
            ObstructionRepository obstructionRepo = new ObstructionRepository();
            players = [player1, player2];
            obstructionGame = (ObstructionGame?)obstructionRepo.GetGame(gameStateID);
            if (obstructionGame == null)
            {
                obstructionGame = new ObstructionGame(player1, player2, width, height);
                obstructionRepo.AddGame(obstructionGame);
            }
            else
            {
                Random random = new Random();
                int turn = random.Next(0, 2);
                obstructionGame.GameState.Turn = turn;
            }
        }

        public IGame Play(int nrParameters, object[] parameters)
        {
            int x = Convert.ToInt32(parameters[0]);
            int y = Convert.ToInt32(parameters[1]);
            Player player = GetCurrentPlayer();

            PlaceSymbol(x, y);

            BlockSurroundingSquares(x, y);

            if(CheckCurrentState())
                obstructionGame.GameState.Winner = (Player)player;

            SwitchTurn();
            obstructionGame.saveGame();


            return obstructionGame;
        }

        private void PlaceSymbol(int x, int y)
        {
            if (obstructionGame.Board.GetPiece(x, y) != null)
                throw new InvalidMoveException();

            IPiece piece = new ObstructionPiece(x, y, GetCurrentPlayer()); //TODO sa verific simpbolu bun pt current player
            obstructionGame.Board.addPiece(piece);

        }

        private void BlockSurroundingSquares(int x, int y)
        {
            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    int newX = x + dx;
                    int newY = y + dy;
                    if (newX >= 0 && newX < obstructionGame.Board.Width && newY >= 0 && newY < obstructionGame.Board.Height)
                    {
                        IPiece piece = new ObstructionPiece(newX, newY, null);
                        obstructionGame.Board.addPiece(piece);
                    }
                }
            }
        }

        private void SwitchTurn()
        {
            obstructionGame.GameState.Turn = obstructionGame.GameState.Turn == 0 ? 1 : 0;
        }

        private Player GetCurrentPlayer()
        {
            return players[obstructionGame.GameState.Turn];
        }

        private bool CheckCurrentState()
        {
            if (((ObstructionBoard)obstructionGame.Board).isFull() || !HasValidMovesLeft())
            {
                return false;
            }
            return true;
        }

        private bool HasValidMovesLeft()
        {
            for (int x = 0; x < obstructionGame.Board.Width; x++)
            {
                for (int y = 0; y < obstructionGame.Board.Height; y++)
                {
                    if (obstructionGame.Board.GetPiece(x, y) == null)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public IGame GetGame()
        {
            return obstructionGame;
        }

        public void SetGame(IGame game)
        {
            obstructionGame = (ObstructionGame)game;
        }


    }
}

