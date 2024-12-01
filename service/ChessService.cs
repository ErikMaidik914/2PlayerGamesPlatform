using _2PlayerGames;
using _2PlayerGames.domain.Bot;
using _2PlayerGames.domain.Enums;
using _2PlayerGames.domain.GameObjects;
using _2PlayerGames.exceptions;
using _2PlayerGames.repo;
using _2PlayerGames.service;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
//TODO: PROMOTE + DRAW
namespace _2PlayerGames.service
{
    public class ChessService : IGameService
    {
        private ChessGame chessGame;
        ChessRepository chessRepo;
        private List<Player> players;
        private Guid white;

        public Guid White { get => white; }

        public ChessService(Guid gameStateID, Player player1, Player player2, ChessModes mode, int startPlayer)
        {
            chessRepo = new ChessRepository();
            players = [player1, player2];
            if (gameStateID == Guid.Empty)
            {
                chessGame = new ChessGame(player1, player2, mode, startPlayer);
                if(!(player2.Name == "Bot" || player1.Name == "Bot"))
                {
                    chessRepo.AddGame(chessGame);
                }
            }
            else
            {
                chessGame = (ChessGame)chessRepo.GetGame(gameStateID);
            }
        }

        public IGame Play(int nrParameters, object[] parameters)
        {
            int initialX = Convert.ToInt32(parameters[0]);
            int initialY = Convert.ToInt32(parameters[1]);
            int newX = Convert.ToInt32(parameters[2]);
            int newY = Convert.ToInt32(parameters[3]);

            Player player = GetCurrentPlayer();

            if(!ExecuteMoveIfValid(initialX, initialY, newX, newY, player))
            {
                throw new InvalidMoveException();
            }
            int winner = CheckCurrentState();
            if (winner == 0)
            {
                chessGame.GameState.Winner = player;
            }
            SwitchTurn();
            chessGame.saveGame();
            chessRepo.UpdateGame(chessGame);

            return chessGame;
        }

        //function to execute and validate a piece move .
        //parameters:initial positionx : int, initial positiony : int
        private bool ExecuteMoveIfValid(int initialX, int initialY, int newX, int newY, Player player)
        {
            int kingXCoordinate=-1, kingYCoordinate=-1;
            foreach (ChessPiece pieces in chessGame.Board.Board)
            {
                if (pieces is KingPiece && pieces.Player.Id == GetCurrentPlayer().Id)
                {
                    kingXCoordinate = pieces.XPosition;
                    kingYCoordinate = pieces.YPosition;
                    break;
                }
            }
            if (IsInCheck(kingXCoordinate, kingYCoordinate, GetCurrentPlayer()))
            {
                return false;
            }
            ChessPiece? piece = (ChessPiece)chessGame.Board.GetPiece(initialX, initialY);
            if (piece == null)
                return false;
            if (piece.Player.Id != player.Id)
            {
                return false;
            }
            if (!piece.isValidMove(newX, newY))
                return false;

            ChessPiece pieceAtDestination = (ChessPiece)chessGame.Board.GetPiece(newX, newY);

            // Check if the destination square is occupied
            if (pieceAtDestination != null)
            {
                // Check if the piece at the destination square belongs to the current player
                if (pieceAtDestination.Player.Id == player.Id)
                {
                    // Invalid move if the piece at the destination belongs to the current player
                    return false;
                }
                else
                {
                    // Capture the opponent's piece
                    ((ChessBoard)chessGame.Board).removePiece(newX, newY);
                }
            }

            // Check if the trajectory is clear
            if (!IsTrajectoryClear(piece, initialX, initialY, newX, newY))
            {
                // Invalid move if trajectory is blocked
                return false;
            }



            // If moving a king, check if the destination square would put the king in check
            if (piece is KingPiece)
            {
                if (IsInCheck(newX, newY, player))
                {
                    // Invalid move if moving the king puts it in check
                    return false;
                }
            }

            // Move the piece to the new position
            piece.updatePosition(newX, newY);
            return true;
        }



        //funciton check if a piece blocks the trajectory of another piece
        private bool IsTrajectoryClear(ChessPiece piece, int initialX, int initialY, int newX, int newY)
        {
            // Check if the piece is a Pawn
            if (piece is PawnPiece)
            {
                // Pawn's trajectory is always clear, no need to check
                return true;
            }

            // Check if the piece is a Rook
            else if (piece is RookPiece)
            {
                // Rooks can move horizontally or vertically
                if (newY == initialY)
                {
                    // Determine the direction of movement
                    int stepX = newX > initialX ? 1 : -1;

                    // Check each square along the horizontal path to see if it's empty
                    for (int x = initialX + stepX; x != newX; x += stepX)
                    {
                        // If any square along the horizontal path is occupied, trajectory is blocked
                        if (chessGame.Board.GetPiece(x, newY) != null)
                        {
                            return false;
                        }
                    }

                    // If all squares along the horizontal path are empty, trajectory is clear
                    return true;
                }
                // Check if the movement is vertical
                else if (newX == initialX)
                {
                    // Determine the direction of movement
                    int stepY = newY > initialY ? 1 : -1;

                    // Check each square along the vertical path to see if it's empty
                    for (int y = initialY + stepY; y != newY; y += stepY)
                    {
                        // If any square along the vertical path is occupied, trajectory is blocked
                        if (chessGame.Board.GetPiece(newX, y) != null)
                        {
                            return false;
                        }
                    }

                    // If all squares along the vertical path are empty, trajectory is clear
                    return true;
                }
                else
                {
                    // Rook can only move horizontally or vertically, so if the movement is neither horizontal nor vertical, trajectory is blocked
                    return false;
                }
            }

            // Check if the piece is a Bishop
            else if (piece is BishopPiece)
            {
                // Calculate the change in position in both x and y directions
                int dx = newX - initialX;
                int dy = newY - initialY;

                // Check if the movement is diagonal
                if (Math.Abs(dx) != Math.Abs(dy))
                {
                    // Bishop can only move diagonally, so if the movement is not diagonal, trajectory is blocked
                    return false;
                }

                // Determine the direction of movement
                int stepX = dx > 0 ? 1 : -1;
                int stepY = dy > 0 ? 1 : -1;

                // Check each square along the diagonal path to see if it's empty
                for (int i = 1; i < Math.Abs(dx); i++)
                {
                    int x = initialX + i * stepX;
                    int y = initialY + i * stepY;

                    // If any square along the diagonal path is occupied, trajectory is blocked
                    if (chessGame.Board.GetPiece(x, y) != null)
                    {
                        return false;
                    }
                }

                // If all squares along the diagonal path are empty, trajectory is clear
                return true;
            }

            // Check if the piece is a Queen
            else if (piece is QueenPiece)
            {
                // Queens can move horizontally, vertically, or diagonally
                int xDistance = Math.Abs(newX - initialX);
                int yDistance = Math.Abs(newY - initialY);

                // Horizontal movement
                if (initialY == newY)
                {
                    int step = (newX > initialX) ? 1 : -1;
                    for (int x = initialX + step; x != newX; x += step)
                    {
                        if (chessGame.Board.GetPiece(x, newY) != null)
                            return false;
                    }
                    return true;
                }
                // Vertical movement
                else if (initialX == newX)
                {
                    int step = (newY > initialY) ? 1 : -1;
                    for (int y = initialY + step; y != newY; y += step)
                    {
                        if (chessGame.Board.GetPiece(newX, y) != null)
                            return false;
                    }
                    return true;
                }
                // Diagonal movement
                else if (xDistance == yDistance)
                {
                    int stepX = (newX > initialX) ? 1 : -1;
                    int stepY = (newY > initialY) ? 1 : -1;
                    for (int i = 1; i < xDistance; i++)
                    {
                        int x = initialX + i * stepX;
                        int y = initialY + i * stepY;
                        if (chessGame.Board.GetPiece(x, y) != null)
                            return false;
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }

            // For other types of pieces (Knight and King), return true assuming the trajectory is always clear
            return true;
        }


        //KING SCENARIOSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSS
        private bool IsInCheck(int kingX, int kingY, Player player)
        {
            // Iterate through all enemy pieces and check if any can reach the king's position
            foreach(ChessPiece piece in this.chessGame.Board.Board) {
                    if (piece != null && piece.Player.Id == player.Id)
                    {
                        // Check if the piece can reach the king's position
                        if (IsTrajectoryClear(piece, piece.XPosition, piece.YPosition, kingX, kingY) && piece.isValidMove(kingX, kingY))
                        {
                            return true;
                        }
                    }
                }
            return false;
        }


        // Function to check if the king has any valid moves left
        private bool HasValidMovesLeft(int kingX, int kingY, Player kingColor)
        {
            // Iterate through all adjacent squares around the king's position
            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    // Calculate the new position
                    int newX = kingX + dx;
                    int newY = kingY + dy;

                    // Check if the new position is within the board boundaries and if the move is valid for the king
                    if (newX >= 0 && newX < chessGame.Board.Width && newY >= 0 && newY < chessGame.Board.Height && (newX != kingX || newY != kingY)
                        && chessGame.Board.GetPiece(newX, newY) == null
                        && !IsInCheck(newX, newY, kingColor))
                    {
                        return true;
                    }
                }
            }
            return false;
        }


        private bool HasOtherPieces(Player player)
        {
            // Count the number of pieces of the specified player on the board, excluding the king
            int pieceCount = chessGame.Board.Board.Count(piece => piece.Player == player && !(piece is KingPiece));

            // Return true if there are other pieces besides the king, otherwise return false
            return pieceCount > 0;
        }







        //game state


        private void SwitchTurn()
        {
            chessGame.CurrentPlayerIndex = (chessGame.CurrentPlayerIndex + 1) % 2;
        }

        private Player GetCurrentPlayer()
        {
            return players[chessGame.GameState.Turn];
        }

        private int CheckCurrentState()
        {
            Player currentPlayer = GetCurrentPlayer();

            // Get the position of the king for the current player
            int kingXCoordinate = -1;
            int kingYCoordinate = -1;

            foreach (ChessPiece piece in chessGame.Board.Board)
            {
                if (piece is KingPiece && piece.Player.Id != currentPlayer.Id)
                {
                    kingXCoordinate = piece.XPosition;
                    kingYCoordinate = piece.YPosition;
                    break;
                }
            }

            // Check if the king is in check and has no valid moves left
            if (IsInCheck(kingXCoordinate, kingYCoordinate, currentPlayer) &&
                !HasValidMovesLeft(kingXCoordinate, kingYCoordinate, currentPlayer))
            {
                return 0; // Checkmate
            }
            else if (!IsInCheck(kingXCoordinate, kingYCoordinate, currentPlayer) &&
                !HasValidMovesLeft(kingXCoordinate, kingYCoordinate, currentPlayer) && !HasOtherPieces(currentPlayer))
            {
                return 1; // Draw
            }

            return -1; // No winner yet
        }


        public IGame GetGame()
        {
            return chessGame;
        }

        public void SetGame(IGame game)
        {
            chessGame = (ChessGame)game;
        }

    }
}
