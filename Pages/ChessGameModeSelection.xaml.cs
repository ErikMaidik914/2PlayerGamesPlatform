using System;
using System.Collections.Generic;
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
using _2PlayerGames.Core;

namespace _2PlayerGames.Pages
{
    /// <summary>
    /// Interaction logic for ChessGameModeSelection.xaml
    /// </summary>
    public partial class ChessGameModeSelection : Page
    {
        
        public ChessGameModeSelection()
        {
            InitializeComponent();
        }

      

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void BulletButton_Click(object sender, RoutedEventArgs e)
        {
            Router.ChessMode = "BULLET";
            this.NavigationService.Navigate(Router.OpponentPage);
        }

        private void BlitzButton_Click(object sender, RoutedEventArgs e)
        {
            Router.ChessMode = "BLITZ";
            this.NavigationService.Navigate(Router.OpponentPage);
        }

        private void RapidButton_Click(object sender, RoutedEventArgs e)
        {
            Router.ChessMode = "RAPID";
            this.NavigationService.Navigate(Router.OpponentPage);
        }
    }
    
}
