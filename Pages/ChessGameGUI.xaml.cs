using _2PlayerGames.Core;
using _2PlayerGames.domain.Auxiliary;
using _2PlayerGames.exceptions;
using _2PlayerGames.repository;
using _2PlayerGames.service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace _2PlayerGames.Pages
{
    /// <summary>
    /// Interaction logic for ChessGameGUI.xaml
    /// </summary>
    public partial class ChessGameGUI : Page
    {

        private IPlayService playService;
        private InGameService inGameService;
        private Grid chessBoard;
        private int initial_row=-1;
        private int initial_column=-1;
        private int new_row;
        private int new_column;
        private DispatcherTimer timer;
        private DateTime startTime;
        private TimeSpan duration;
        private BackgroundWorker worker = new BackgroundWorker();
        public ChessGameGUI()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            Player1Timer.Text = Router.ChessMode;
            Loaded += ChessGameGUI_Loaded;
            worker.DoWork += Worker_DoWork;

        }

        private void Worker_DoWork(object? sender, DoWorkEventArgs e)
        {
            while (true)
            {
                if (((OnlineGameService)playService).HasData())
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        chessBoard.IsEnabled = false;
                    });
                    playService.PlayOther();
                    setCurrentTurn();
                    ((OnlineGameService)playService).SetFirstTurn();
                    this.Dispatcher.Invoke(() => { updateBoard(); });
                    this.Dispatcher.Invoke(() =>
                    {
                        chessBoard.IsEnabled = true;
                    });
                    if (playService.IsGameOver())
                    {
                        Guid? Winner = playService.GetWinner();
                        if (Winner == Guid.Empty)
                        {
                            this.Dispatcher.Invoke(() =>
                            {
                                MessageBox.Show("It's a draw!");

                            });

                        }
                        else
                        {
                            if (playService.GetWinner() == Router.UserPlayer.Id)
                            {
                                this.Dispatcher.Invoke(() =>
                                {
                                    MessageBox.Show("You Win!");

                                });
                            }
                            else
                            {
                                this.Dispatcher.Invoke(() =>
                                {
                                    MessageBox.Show("You Lost!");

                                });
                            }
                        }
                        this.Dispatcher.Invoke(() =>
                        {
                            this.NavigationService.Navigate(Router.MenuPage);
                        });
                        return;
                    }
                    break;
                }
            }
            return;
        }

        private void ChessGameGUI_Loaded(object sender, RoutedEventArgs e)
        {
            playService = Router.PlayService;
            inGameService = Router.InGameService;
            InitializeBoard();
            updateBoard();
            if (playService.GetTurn() != Router.UserPlayer.Id && Router.OnlineGame)
            {
                worker.RunWorkerAsync();
            }
            setCurrentTurn();
            //populatePlayersData();
            //StartTimer();
        }

  

        private void NewGameButton_Click(object sender, RoutedEventArgs e)
        {
            
                var confirmationDialog = new NewGameDialog();
                if (confirmationDialog.ShowDialog() == true)
                {

                    if (confirmationDialog.DialogResult == true)
                    {
                        MessageBox.Show("Starting a new game...");
                        this.NavigationService.Navigate(Router.MenuPage);
                    }
                    else
                    {
                        MessageBox.Show("User declined to start a new game.");
                        confirmationDialog.Close();
                    }
                }
            
        }

   


        private void populatePlayersData()
        {
            PlayerStats playerStats = inGameService.GetStats(Router.UserPlayer);
            Player1Name.Text = playerStats.Player.Name;
            Player1Rank.Text = playerStats.Rank;
            Player1Trophies.Text = playerStats.Trophies.ToString();

            playerStats = inGameService.GetStats(Router.OpponentPlayer);
            Player2Name.Text = playerStats.Player.Name;
            Player2Rank.Text = playerStats.Rank;
            Player2Trophies.Text = playerStats.Trophies.ToString();

        }

        private void setCurrentTurn()
        {
            if (playService.GetTurn() == Router.UserPlayer.Id)
            {
                this.Dispatcher.Invoke(() => { CurrentPlayerTurn.Text = Router.UserPlayer.Name + "'s turn"; });
            }
            else
            {
                this.Dispatcher.Invoke(() => { CurrentPlayerTurn.Text = Router.OpponentPlayer.Name + "'s turn"; });
            }
        }


            private void InitializeBoard()
            {
                chessBoard = new Grid();



                for (int i = 0; i < 8; i++)
                {
                    ColumnDefinition col = new ColumnDefinition();
                    chessBoard.ColumnDefinitions.Add(col);
                    RowDefinition row = new RowDefinition();
                    chessBoard.RowDefinitions.Add(row);
                }
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if ((j +i)%2 == 0)

                        {

                            Color color = (Color)ColorConverter.ConvertFromString("#eae9d2");
                            Rectangle rect = new Rectangle();
                            rect.Fill = new SolidColorBrush(color);
                            rect.Stroke = new SolidColorBrush();
                            Grid.SetRow(rect, i);
                            Grid.SetColumn(rect, j);
                            rect.AddHandler(MouseDownEvent, new MouseButtonEventHandler(MouseDownHandler), true);
                            chessBoard.Children.Add(rect);
                        }
                        else
                        {
                            Color color = (Color)ColorConverter.ConvertFromString("#4b7399");
                            Rectangle rect = new Rectangle();
                            rect.Fill = new SolidColorBrush(color);
                            rect.Stroke = new SolidColorBrush();
                            Grid.SetRow(rect, i);
                            Grid.SetColumn(rect, j);
                            rect.AddHandler(MouseDownEvent, new MouseButtonEventHandler(MouseDownHandler), true);
                            chessBoard.Children.Add(rect);
                        }
                    
                    }
                }
                Grid.SetColumn(chessBoard, 1);
                Grid.SetRow(chessBoard, 0);
                parentGrid.Children.Add(chessBoard);

            }

        

        //private void MouseMoveHandler(object sender, MouseEventArgs e)
        //{
        //    // Get the x and y coordinates of the mouse pointer.
        //    System.Windows.Point position = e.GetPosition(this);

        //    // Get the column number
        //    column = (int)(position.X / 420);

        //    row = (int)(position.Y / chessBoard.Height);
        //    column = (int)(position.X / chessBoard.Width);

        //    if(currentPiece != null)
            


        //}

        private void MouseDownHandler(object sender, MouseButtonEventArgs e)
        {
            if (initial_column != -1 && initial_row != -1)
            {
                Point position = e.GetPosition(this);
                new_row = (int)((position.Y - 100) / 43.5);
                new_column = (int)((position.X - 235) / 56.125); 
                object[] arg = [initial_column, initial_row, new_column, new_row];
                initial_column = -1;
                initial_row = -1;
                try
                {
                    playService.Play(1, arg);
                    updateBoard();
                    if (playService.IsGameOver())
                    {
                        Guid? Winner = playService.GetWinner();
                        if (Winner == null)
                        {
                            MessageBox.Show("It's a draw!");
                            ;
                        }
                        else
                        {
                            if (playService.GetWinner() == Router.UserPlayer.Id)
                            {
                                MessageBox.Show("You won!");
                            }
                            else
                            {
                                MessageBox.Show("You lost!");
                            }

                        }
                        this.NavigationService.Navigate(Router.MenuPage);
                        return;
                    }
                    setCurrentTurn();
                    if(Router.OnlineGame)
                    {
                        worker.RunWorkerAsync();
                    }
                }
                catch (InvalidMoveException)
                {
                    MessageBox.Show("Invalid move.");
                }
                catch (NotYourTurnException)
                {
                    MessageBox.Show("It is not your turn.");
                }
            }
            else
            {
                Point position = e.GetPosition(this);
                initial_row = (int)((position.Y - 100) / 43.5);
                initial_column = (int)((position.X - 235) / 56.125);

           }

        }

        private void updateBoard()
        {
            chessBoard.Children.Clear();
            InitializeBoard();
            List<IPiece> pieces = playService.GetBoard();
            foreach(IPiece piece in pieces)
            { 
                String color = piece.Player.Id == playService.StartPlayer() ? "white" : "black";
                AddPiece(piece.YPosition, piece.XPosition, ((IChessPiece)piece).GetPieceType(), color);
            }
        }

        private void AddPiece(int row, int column, String pieceType, String color)
        {
            Image pieceImage = new Image();
            string imagePath = ChessPieceStore.getPiece(pieceType, color);
            pieceImage.Source = new BitmapImage(new Uri(imagePath, UriKind.Relative));
            pieceImage.AddHandler(MouseDownEvent, new MouseButtonEventHandler(MouseDownHandler), true);
            Grid.SetRow(pieceImage, row);
            Grid.SetColumn(pieceImage, column);

            chessBoard.Children.Add(pieceImage);
        }



        private void StartTimer()
        {
            // Parse the initial value from the TextBox
            if (TimeSpan.TryParse(Player1Timer.Text, out duration))
            {
                // Start the timer
                startTime = DateTime.Now;
                timer.Start();
            }
            else
            {
                MessageBox.Show("Invalid time format. Please enter time in HH:mm:ss format.");
            }
        }

        private void StopTimer()
        {
            // Stop the timer
            timer.Stop();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Calculate the remaining time
            TimeSpan elapsedTime = DateTime.Now - startTime;
            TimeSpan remainingTime = duration - elapsedTime;

            // Check if the timer has reached zero
            if (remainingTime <= TimeSpan.Zero)
            {
               StopTimer();
               Player1Timer.Text = "00:00:00";
               MessageBox.Show("Timer has reached zero.");
            }
            else
            {
                // Update the TextBlock with the remaining time
                Player1Timer.Text = $"{remainingTime.Minutes:D2}:{remainingTime.Seconds:D2}";
            }
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            if (SettingsPopup.IsOpen == false)
            {
                SettingsPopup.IsOpen = true;
            }
            else
            {
                SettingsPopup.IsOpen = false;
            }
        }
        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            if (InformationPopup.IsOpen == false)
            {
                InformationPopup.IsOpen = true;
            }
            else
            {
                InformationPopup.IsOpen = false;
            }
        }
        private void newGameButton_Click(object sender, RoutedEventArgs e)
        {
            var confirmationDialog = new NewGameDialog();
            if (confirmationDialog.ShowDialog() == true)
            {

                if (confirmationDialog.DialogResult == true)
                {
                    MessageBox.Show("Starting a new game...");
                    this.NavigationService.Navigate(Router.MenuPage);
                }
                else
                {
                    MessageBox.Show("User declined to start a new game.");
                    confirmationDialog.Close();
                }
            }
        }
    }
    
}
