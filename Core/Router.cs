using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _2PlayerGames.Pages;
using _2PlayerGames.service;

namespace _2PlayerGames.Core
{
    internal class Router
    {
        private static ChessGameGUI chessPage = new ChessGameGUI();
        private static Connect4GameGUI connectPage=new Connect4GameGUI();
        private static DartGameGUI dartsPage=new DartGameGUI();
        private static ObstructionGameGUI obstructionPage=new ObstructionGameGUI();
        private static ChessGameModeSelection chessSelectionPage=new ChessGameModeSelection();
        private static ObstructionGameMode obstructionModePage = new ObstructionGameMode();
        private static MenuPage menuPage=new MenuPage();
        private static ProfilePage profilePage=new ProfilePage();
        private static StatsPage statsPage = new StatsPage();
        private static HistoryPage historyPage=new HistoryPage();   
        private static LoadingPage loadingPage=new LoadingPage();
        private static OpponentPage opponentPage=new OpponentPage();
        private static AIDifficultySelection aiSelectionPage=new AIDifficultySelection();
        private static string chessMode;
        private static int obstructionMode;
        private static string aiDifficulty;
        private static string gameType;
        private static bool onlineGame;
        private static Player userPlayer=new Player();
        private static Player opponentPlayer=new Player();
        private static IPlayService playService;
        private static PlayerProfileService profileService=new ();
        private static InGameService inGameService = new();
        public Router()
        {
            
        }

        public static PlayerProfileService ProfileService
        {
            get { return profileService; }
            set { profileService = value; }
        }
        public static string GameType
        {
            get { return gameType; }
            set { gameType = value; }
        }

        public static ChessGameGUI ChessPage
        {
            get { return chessPage; }
            set { chessPage = value; }
        }

        public static Connect4GameGUI ConnectPage
        {
            get { return connectPage; }
            set { connectPage = value; }
        }

        public static DartGameGUI DartsPage
        {
            get { return dartsPage; }
            set { dartsPage = value; }
        }

        public static ObstructionGameGUI ObstructionPage
        {
            get { return obstructionPage; }
            set { obstructionPage = value; }
        }

        public static ChessGameModeSelection ChessSelectionPage
        {
            get { return chessSelectionPage; }
            set { chessSelectionPage = value; }
        }

        public static ObstructionGameMode ObstructionModePage
        {
            get { return obstructionModePage; }
            set { obstructionModePage = value; }
        }

        public static MenuPage MenuPage
        {
            get { return menuPage; }
            set { menuPage = value; }
        }

        public static ProfilePage ProfilePage
        {
            get { return profilePage; }
            set { profilePage = value; }
        }

        public static StatsPage StatsPage
        {
            get { return statsPage; }
            set { statsPage = value; }
        }

        public static HistoryPage HistoryPage
        {
            get { return historyPage; }
            set { historyPage = value; }
        }

        public static LoadingPage LoadingPage
        {
            get { return loadingPage; }
            set { loadingPage = value; }
        }

        public static OpponentPage OpponentPage
        {
            get { return opponentPage; }
            set { opponentPage = value; }
        }

        public static AIDifficultySelection AiSelectionPage
        {
            get { return aiSelectionPage; }
            set { aiSelectionPage = value; }
        }

        public static string ChessMode
        {
            get { return chessMode; }
            set { chessMode = value; }
        }

        public static int ObstructionMode
        {
            get { return obstructionMode; }
            set { obstructionMode = value; }
        }

        public static string AiDifficulty
        {
            get { return aiDifficulty; }
            set { aiDifficulty = value; }
        }

        public static Player UserPlayer
        {
            get { return userPlayer; }
            set { userPlayer = value; }
        }

        public static Player OpponentPlayer
        {
            get { return opponentPlayer; }
            set { opponentPlayer = value; }
        }

        public static IPlayService PlayService
        {
            get { return playService; }
            set { playService = value; }
        }

        public static InGameService InGameService
        {
            get { return inGameService; }
            set { inGameService = value; }
        }

        public static bool OnlineGame
        {
            get { return onlineGame; }
            set { onlineGame = value; }
        }
    
    }
}
