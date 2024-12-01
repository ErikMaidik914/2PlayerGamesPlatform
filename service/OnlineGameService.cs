using _2PlayerGames.domain.DatabaseObjects;
using _2PlayerGames.domain.Enums;
using _2PlayerGames.domain.GameObjects;
using _2PlayerGames.exceptions;
using _2PlayerGames.repo;
using _2PlayerGames.repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace _2PlayerGames.service
{
    internal class OnlineGameService : IPlayService
    {
        private IGameService gameService;
        private IPlayService statsService;
        private BaseGameRepository gameRepo;
        private Guid startPlayer;

        TcpListener? listener = null;
        TcpClient? client = null;
        Socket? socket = null;

        private bool host = false;
        private bool firstTurn = true;

        private int elo;
        private int boardWidth;
        private int boardHeight;
        private PlayerQueue playerQueue;
        Player opponentPlayer;

        private Player player;

        String gameType;

        ChessModes modes;

        public void SetFirstTurn()
        {
            firstTurn = false;
        }
        public OnlineGameService(string gameType, Player player, params object[] optional_params)
        {
            this.player = player;
            this.gameType = gameType;
            Games game = GameStore.Games[gameType];
            modes = ChessModes.RAPID;
            //TODO: get elo from statsService
            elo = 500;
            boardWidth = 0;
            boardHeight = 0;
            PlayerQueue playerQueue;
            Player opponentPlayer;
            if (optional_params.Length == 1)
            {
                String mode = (String)optional_params[0];
                modes = ChessModeStore.getMode(mode);
                playerQueue = new PlayerQueue(player, game, elo, modes, null, null);
            }
            else if (optional_params.Length == 2)
            {
                boardWidth = (int)optional_params[0];
                boardHeight = (int)optional_params[1];
                playerQueue = new PlayerQueue(player, game, elo, null, boardWidth, boardHeight);
            }
            else
            {
                playerQueue = new PlayerQueue(player, game, elo, null, null, null);
            }
            PlayerRepository.getPlayer(Guid.Empty);
            List<PlayerQueue> availablePlayers;
            availablePlayers = PlayerQueueRepository.GetPlayers();

            if (availablePlayers.Count == 0)
            {
                PlayerQueueRepository.AddPlayer(playerQueue);
                int minPort = 1024;
                int maxPort = 65535;

                //Random random = new Random();
                //int randomPort = random.Next(minPort, maxPort + 1);
                //while (!IsPortAvailable(randomPort))
                //{
                //    randomPort = random.Next(minPort, maxPort + 1);
                //}
                listener = new TcpListener(System.Net.IPAddress.Any, 69);
                host = true;
                startPlayer = player.Id;
                listener.Start();
                socket = listener.AcceptSocket();
                opponentPlayer = receivePlayer();
            }
            else
            {
                Random random = new();
                int random_idx = random.Next(0, availablePlayers.Count);
                PlayerQueue opponent = availablePlayers[random_idx];
                opponentPlayer = opponent.Player;
                startPlayer = opponentPlayer.Id;
                PlayerQueueRepository.removePlayer(opponent);
                client = new TcpClient("localhost", 69);
                socket = client.Client;
                sendPlayer(player);
            }
            if (host)
            {
                switch (gameType)
                {
                    case "Obstruction":

                        gameService = new ObstructionService(Guid.Empty, player, opponentPlayer, boardWidth, boardHeight);
                        break;
                    case "Connect4":
                        gameService = new Connect4Service(Guid.Empty, player, opponentPlayer);
                        break;
                    case "Chess":
                        gameService = new ChessService(Guid.Empty, player, opponentPlayer, modes, 0);
                        break;
                    case "Darts":
                        gameService = new DartsService(Guid.Empty, player, opponentPlayer);
                        break;
                }
                sendGame(gameService.GetGame());
                switch (gameType)
                {
                    case "Obstruction":
                        gameRepo = new ObstructionRepository();
                        break;
                    case "Connect4":
                        gameRepo = new Connect4Repository();
                        break;
                    case "Chess":
                        gameRepo = new ChessRepository();
                        break;
                    case "Darts":
                        gameRepo = new DartsRepository();
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
            else
            {
                switch (gameType)
                {
                    case "Obstruction":
                        gameRepo = new ObstructionRepository();
                        break;
                    case "Connect4":
                        gameRepo = new Connect4Repository();
                        break;
                    case "Chess":
                        gameRepo = new ChessRepository();
                        break;
                    case "Darts":
                        gameRepo = new DartsRepository();
                        break;
                    default:
                        throw new NotImplementedException();
                }
                IGame game1 = receiveGame();
                switch (gameType)
                {
                    case "Darts":
                        gameService = new DartsService(game1.GameState.Id, player, opponentPlayer);
                        break;
                    case "Obstruction":
                        gameService = new ObstructionService(game1.GameState.Id, player, opponentPlayer, boardWidth, boardHeight);
                        break;
                    case "Connect4":
                        gameService = new Connect4Service(game1.GameState.Id, player, opponentPlayer);
                        break;
                    case "Chess":
                        gameService = new ChessService(game1.GameState.Id, player, opponentPlayer, modes, 1);
                        break;
                }
            }
            //switch (gameType)
            //{
            //    case "Darts":
            //        availablePlayers = playersRepo.GetAvailableDartsPlayers(playerQueue);
            //        break;
            //    case "Obstruction":
            //        availablePlayers = playersRepo.GetAvailableObstructionPlayers(playerQueue);
            //        break;
            //    case "Connect4":
            //        availablePlayers = playersRepo.GetAvailableConnect4Players(playerQueue);
            //        break;
            //    case "Chess":
            //        availablePlayers = playersRepo.GetAvailableChessPlayers(playerQueue);
            //        break;
            //    default:

            //        throw new NotImplementedException();
            //}

        }

        static bool IsPortAvailable(int port)
        {
            try
            {
                var listener = new TcpListener(IPAddress.Any, port);
                listener.Start();
                listener.Stop();
                return true;
            }
            catch (SocketException)
            {
                return false;
            }
        }

        public void Play(int nrParameters, object[] parameters)
        {
            if (firstTurn && !host)
            {
                throw new NotYourTurnException();
            }
            if(GetTurn() != player.Id)
            {
                throw new NotYourTurnException();
            }

            IGame newGame = gameService.Play(nrParameters, parameters);
            sendGame(newGame);
        }

        void sendPlayer(Player player)
        {
            //            MemoryStream stream = new MemoryStream();
            //#pragma warning disable SYSLIB0011 // Type or member is obsolete
            //            BinaryFormatter formatter = new();
            //#pragma warning restore SYSLIB0011 // Type or member is obsolete
            //            formatter.Serialize(stream, player);
            //            long length = stream.Length;
            //            socket.Send(BitConverter.GetBytes(length));
            //            byte[] buffer = stream.ToArray();
            //            socket.Send(buffer);

            String playerId = player.Id.ToString();
            byte[] idBuffer = Encoding.ASCII.GetBytes(playerId);
            socket.Send(idBuffer);
        }

        void sendGame(IGame game)
        {
            String gameStateId = game.GameState.Id.ToString();
            byte[] idBuffer = Encoding.ASCII.GetBytes(gameStateId);
            try
            {
                socket.Send(idBuffer);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        IGame receiveGame()
        {
            byte[] buffer = new byte[1024];
            try
            {
                int bytesRead = socket.Receive(buffer);
                String gameStateId = System.Text.Encoding.ASCII.GetString(buffer, 0, bytesRead);
                IGame? game = gameRepo.GetGame(Guid.Parse(gameStateId));
                if (game == null)
                    game = gameRepo.GetGameFromDatabase(Guid.Parse(gameStateId));
                return game;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
            //switch (gameType)
            //{
            //    case "Darts":
            //        game = new Connect4Game(Player.Null(), Player.Null());
            //        break;
            //    case "Obstruction":
            //        game = new Connect4Game(Player.Null(), Player.Null());
            //        break;
            //    case "Connect4":
            //        game = new Connect4Game(Player.Null(), Player.Null());
            //        break;
            //    case "Chess":
            //        game = new Connect4Game(Player.Null(), Player.Null());
            //        break;
            //    default:
            //        game = new Connect4Game(Player.Null(), Player.Null());
            //        break;

            //}
        }

        Player receivePlayer()
        {
            //long size;
            //byte[] buffer = new byte[8];
            //socket.Receive(buffer);
            //size = BitConverter.ToInt32(buffer, 0);
            //buffer = new byte[size];
            //int bytesRead = socket.Receive(buffer);

            //MemoryStream stream = new MemoryStream(buffer);
            //#pragma warning disable SYSLIB0011 // Type or member is obsolete
            //BinaryFormatter formatter = new();
            //#pragma warning restore SYSLIB0011 // Type or member is obsolete
            //return (Player)formatter.Deserialize(stream);

            byte[] buffer = new byte[1024];
            int bytesRead = socket.Receive(buffer);
            String playerId = System.Text.Encoding.ASCII.GetString(buffer, 0, bytesRead);
            return PlayerRepository.getPlayer(Guid.Parse(playerId));

        }

        public bool IsGameOver()
        {
            if (gameService.GetGame().GameState.Winner != null)
            {
                //statsService.UpdateStats(gameService.GetGame().GameState.Winner);
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

            gameService.SetGame(receiveGame());
        }

        public Guid? GetWinner()
        {
            return gameService.GetGame().GameState.Winner.Id;
        }

        public bool HasData()
        {
            return socket.Available > 0;
        }

        public Guid StartPlayer()
        {
            return startPlayer;
        }
    }
}

