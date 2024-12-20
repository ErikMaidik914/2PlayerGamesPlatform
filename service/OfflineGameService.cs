﻿using _2PlayerGames.domain.Enums;
using _2PlayerGames.repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Security.Cryptography;
using _2PlayerGames.domain.Bot;
using System.Threading;
using _2PlayerGames.service.Bot;
using _2PlayerGames.exceptions;
using System.Numerics;

namespace _2PlayerGames.service
{
    internal class OfflineGameService : IPlayService
    {

        private IGameService gameService;
        private IPlayService statsService;
        private String gameType;
        private Guid startPlayer;
        private IBot bot;
        private Player player;


        public OfflineGameService(string gameType, Player player, List<object> optional_params)
        {
            this.gameType = gameType;
            this.player = player;
            Random random = new Random();
            //startPlayer = random.Next(0, 2) == 0 ? player.Id : Guid.Empty;
            startPlayer = player.Id;
            String difficulty = (String)optional_params[0];
            switch (gameType)
            {
                case "Darts":
           
                    gameService = new DartsService(Guid.Empty, player, Player.Bot());
                    break;
                case "Obstruction":
                    int boardWidth = (int)optional_params[1];
                    int boardHeight = (int)optional_params[2];
                    gameService = new ObstructionService(Guid.Empty, player, Player.Bot(), boardWidth, boardHeight);
                    break;
                case "Connect4":
                    
                    gameService = new Connect4Service(Guid.Empty, player, Player.Bot());
                    break;
                case "Chess":
                    String mode = (String)optional_params[1];
                    ChessModes modes = ChessModeStore.getMode(mode);
                    gameService = new ChessService(Guid.Empty, Player.Bot(), player, modes, startPlayer == Guid.Empty ? 0 : 1);
                    break;
            }
            bot = BotStore.getBot(gameType, difficulty, gameService.GetGame().GameState.Id, player);

        }

        public void Play(int nrParameters, object[] parameters)
        {
            if (GetTurn() != player.Id)
            {
                throw new NotYourTurnException();
            }
            gameService.Play(nrParameters, parameters);
            if(IsGameOver())
            {
                //statsService.UpdateStats;
            }
        }

        public bool IsGameOver()
        {
            if (gameService.GetGame().GameState.Winner != null)
            {
                //statsService.UpdateStats;
                return true;
            }
            return false;
        }


        public List<IPiece> GetBoard()
        {
            return gameService.GetGame().Board.Board;
        }

        public Guid GetTurn()
        {
            return gameService.GetGame().CurrentPlayer.Id;
        }

        public void PlayOther()
        {
            Thread.Sleep(3000);
            if(gameType == "Connect4")
            {
               int column =  ((Connect4Bot)bot).GetBestMove().Item1;
                object[] list = { column };
                gameService.Play(1, list);
            }
            if (gameType == "Darts")
            {
                //int xTarget = ((DartsBot)bot).GetBestMove();
                //int yTarget = ((DartsBot)bot).GetBestMove();
                //int accuracy = 50;
                //object[] list = { xTarget, yTarget, accuracy };
                //gameService.Play(1, list);
            }

        }

        public Guid? GetWinner()
        {
            return gameService.GetGame().GameState.Winner.Id;
        }

        public Guid StartPlayer()
        {
            return startPlayer;
        }


    }

    
}
