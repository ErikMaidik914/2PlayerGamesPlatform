using _2PlayerGames.Core;
using _2PlayerGames.domain.Auxiliary;
using _2PlayerGames.service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
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

namespace _2PlayerGames.Pages
{
    /// <summary>
    /// Interaction logic for ObstructionGameGUI.xaml
    /// </summary>
    public partial class ObstructionGameGUI : Page
    {
        private IPlayService playService;
        private InGameService inGameService;
       
        public ObstructionGameGUI()
        {
            playService = Router.PlayService;
            inGameService = Router.InGameService;
            InitializeComponent();
            Loaded += ObstructionGameGUI_Loaded;
        }

        private void ObstructionGameGUI_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeBoard();
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
            CurrentPlayerTurn.Text = playService.GetTurn() + "'s turn";
        }

        private void InitializeBoard()
        {
            Grid dynamicGrid = new Grid();


            for (int i = 0; i < Router.ObstructionMode; i++)
            {
                ColumnDefinition col = new ColumnDefinition();
                dynamicGrid.ColumnDefinitions.Add(col);
                RowDefinition row = new RowDefinition();
                dynamicGrid.RowDefinitions.Add(row);
            }
            for (int i = 0; i < Router.ObstructionMode; i++)
            {
                for (int j = 0; j < Router.ObstructionMode; j++)
                {
                    Border border = new Border();
                    border.BorderBrush = Brushes.Black;
                    border.BorderThickness = new Thickness(1);

                    // Create a TextBlock to represent the content of the cell
                    TextBlock txt = new TextBlock();
                    txt.Background = Brushes.DodgerBlue;
                    txt.MouseDown += CellClickHandler;
                    txt.Text = "";

                    // Set the content of the border to be the TextBlock
                    border.Child = txt;

                    // Set the column and row indices of the dynamically created Border
                    Grid.SetColumn(border, j);
                    Grid.SetRow(border, i);

                    // Add the border to the dynamic grid
                    dynamicGrid.Children.Add(border);
                }
            }
            Grid.SetColumn(dynamicGrid,1);
            Grid.SetRow(dynamicGrid, 0);
            parentGrid.Children.Add(dynamicGrid);
            
        }

        private void CellClickHandler(object sender, MouseButtonEventArgs e)
        {
            // Get the Border containing the TextBlock
            Border clickedBorder = sender as Border;

            if (clickedBorder != null && clickedBorder.Child is TextBlock)
            {
                TextBlock txt = clickedBorder.Child as TextBlock;

                // Check the current player's turn
                if (CurrentPlayerTurn.Text == Router.UserPlayer.Name + "'s turn")
                {
                    // Set the content of the TextBlock to "X"
                    txt.Text = "X";
                }
                else
                {
                    // Set the content of the TextBlock to "O"
                    txt.Text = "O";
                }
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
    }
}
