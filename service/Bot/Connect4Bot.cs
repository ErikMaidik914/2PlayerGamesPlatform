using _2PlayerGames.domain.DatabaseObjects;
using _2PlayerGames.repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2PlayerGames.service.Bot
{
    internal class Connect4Bot : IBot
    {

        Connect4Service gameService;
        Player player;
        private int _searchDepth;

        public Connect4Bot(string difficulty, Guid gameStateId, Player player)
        {
            gameService = new Connect4Service(gameStateId, player, Player.Null());
            this.player = player;
            _searchDepth = difficulty.ToLower() switch
            {
                "easy" => 1,
                "medium" => 3,
                "hard" => 5,
                _ => 1,
            };
        }

        public (int nrParameters, object[] parameters) Play(IGame game)
        {
            throw new NotImplementedException();
        }

        private int Score_Connect_4_Block(List<IPiece> connect4Block, Player currentPlayer, Player currentOpponent)
        {
            int score = 0;
            int playerPieces = 0;
            int opponentPieces = 0;
            foreach (IPiece piece in connect4Block)
            {
                if (piece.Player == currentPlayer)
                    playerPieces++;
                else if (piece.Player != currentOpponent)
                    opponentPieces++;
            }
            if (opponentPieces == 4)
                score += 100;
            else if (opponentPieces == 3 && playerPieces == 1)
                score += 5;
            else if (opponentPieces == 2 && playerPieces == 2)
                score += 2;
            if (playerPieces == 3 && opponentPieces == 1)
                score -= 4;
            return score;
        }

        private List<IPiece> getCentreColumn()
        {
            List<IPiece> centreColumn = new();
            foreach(IPiece piece in gameService.GetGame().Board.Board)
            {
                if (piece.XPosition == 3)
                    centreColumn.Add(piece);
            }
            return centreColumn;
        }

        private int ScoreCenterColumn(List<IPiece> centreColumn)
        {
            int score = 0;
            foreach(IPiece piece in centreColumn)
            {
                if (piece.Player != player)
                    score += 3;
            }
            return score;
        }

        private int ScoreBoard(Player currentPlayer, Player currentOpponent)
        {
            int score = 0;
            List<IPiece> centre_column = getCentreColumn();
            score += ScoreCenterColumn(centre_column);


            for (int i = 0; i < Constants.BOARD_WIDTH - 4; i++)
            {
                for (int j = 0; j < Constants.BOARD_LENGTH; j++)
                {
                    List<IPiece> connect4Block = new(4);
                    for (int k = 0; k < 4; k++)
                    {
                        connect4Block[k] = (IPiece)gameService.GetGame().Board.GetPiece(i + k, j);
                    }
                    score += Score_Connect_4_Block(connect4Block, currentPlayer, currentOpponent);
                }
            }

            for (int i = 0; i < Constants.BOARD_WIDTH; i++)
            {
                for (int j = 0; j < Constants.BOARD_LENGTH - 4; j++)
                {

                    List<IPiece> connect4Block = new(4);
                    for (int k = 0; k < 4; k++)
                    {
                        connect4Block[k] = (IPiece)gameService.GetGame().Board.GetPiece(i, j + k);
                    }
                    score += Score_Connect_4_Block(connect4Block, currentPlayer, currentOpponent);
                }
          
            }

            for (int i = 0; i < Constants.BOARD_WIDTH - 4; i++)
            {
                for (int j = 0; j < Constants.BOARD_LENGTH - 4; j++)
                {
                    List<IPiece> connect4Block = new(4);
                    for (int k = 0; k < 4; k++)
                    {
                        connect4Block[k] = (IPiece)gameService.GetGame().Board.GetPiece(i + k, j + k);
                    }
                    score += Score_Connect_4_Block(connect4Block, currentPlayer, currentOpponent);
                }
            }

            for (int i = 0; i < Constants.BOARD_WIDTH - 4; i++)
            {
                for (int j = 0; j < Constants.BOARD_LENGTH - 4; j++)
                {
                    List<IPiece> connect4Block = new(4);
                    for (int k = 0; k < 4; k++)
                    {
                        connect4Block[k] = (IPiece)gameService.GetGame().Board.GetPiece(i + 3 - k, j + k);
                    }
                    score += Score_Connect_4_Block(connect4Block, currentPlayer, currentOpponent);
                }
            }

            return score;
        }
        private bool IsTerminalNode() => gameService.CheckCurrentState() >= 0;

        public (int, int) GetBestMove(int? searchDepth = null, bool isMaximizingPlayer = true)
        {
            if (searchDepth is null) {
                searchDepth = this._searchDepth;
            }
            if (searchDepth == 0 || this.IsTerminalNode()) {
                if (searchDepth == 0) {
                    Player currentPlayer = null;
                    Player currentOpponent = null;
                    if (isMaximizingPlayer)
                    {
                        currentPlayer = Player.Null();
                        currentOpponent = this.player;
                    }
                    else
                    {
                        currentOpponent = Player.Null();
                        currentPlayer = this.player;
                    }
                    return (-1, ScoreBoard(currentPlayer, currentOpponent));
                }
                else
                {
                    if (gameService.GetGame().GameState.Winner == Player.Null())
                    {
                        return (-1, Int32.MaxValue);
                    }
                    else if (gameService.GetGame().GameState.Winner == player)
                    {
                        return (-1, Int32.MinValue);
                    }
                    else
                    {
                        return (-1, 0);
                    }
                }
            }
            else
            {
                Random random = new();
                if (isMaximizingPlayer)
                {
                    int optimalScore = Int32.MinValue;
                    int optimalColumn = -1;
                    int check = 0;
                    HashSet<int> ints = new();
                    while (check < 7)
                    {
                        int randInt = random.Next(0, 7);
                        if (ints.Contains(randInt))
                        {
                            if (gameService.CheckValidity(randInt))
                            {
                                IPiece piece = gameService.DropPiece(randInt, Player.Null());
                                int score = GetBestMove(searchDepth - 1, false).Item2;
                                ((Connect4Board)gameService.GetGame().Board).removePiece(piece.XPosition, piece.YPosition);
                                if (score > optimalScore)
                                {
                                    optimalScore = score;
                                    optimalColumn = randInt;
                                }
                            }
                            check++;
                        }
                        ints.Add(randInt);
                    }
                    return (optimalColumn, optimalScore);
                }
                else
                {
                    int optimalScore = Int32.MaxValue;
                    int optimalColumn = -1;
                    int check = 0;
                    HashSet<int> ints = new();
                    while (check < 7)
                    {
                        int randInt = random.Next(0, 7);
                        if (ints.Contains(randInt))
                        {
                            if (gameService.CheckValidity(randInt))
                            {
                                IPiece piece = gameService.DropPiece(randInt, player);
                                int score = GetBestMove(searchDepth - 1, true).Item2;
                                ((Connect4Board)gameService.GetGame().Board).removePiece(piece.XPosition, piece.YPosition);
                                if (score < optimalScore)
                                {
                                    optimalScore = score;
                                    optimalColumn = randInt;
                                }
                            }
                            check++;
                        }
                        ints.Add(randInt);
                    }
                    return (optimalColumn, optimalScore);
                }
            }
            //int bestMove = -1;
            //int bestScore = isMaximizingPlayer ? int.MinValue : int.MaxValue;
            //if (searchDepth == 0 || IsTerminalNode())
            //{
            //    return ScoreBoard();
            //}
            //for (int i = 0; i < Constants.BOARD_WIDTH; i++)
            //{
            //    if (gameService.GetGame().Board.GetPiece(i, 0) == null)
            //    {
            //        gameService.Play(1, new object[] { i });
            //        int score = GetBestMove(searchDepth - 1, !isMaximizingPlayer);
            //        //gameService.GetGame().Board.removePiece(i, 0);
            //        //TODO: UNDO MOVE
            //        if (isMaximizingPlayer)
            //        {
            //            if (score > bestScore)
            //            {
            //                bestScore = score;
            //                bestMove = i;
            //            }
            //        }
            //        else
            //        {
            //            if (score < bestScore)
            //            {
            //                bestScore = score;
            //                bestMove = i;
            //            }
            //        }
            //    }
            //}
        }   
    }

    
}
