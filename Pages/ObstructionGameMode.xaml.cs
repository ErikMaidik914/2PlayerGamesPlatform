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
    /// Interaction logic for ObstructionGameMode.xaml
    /// </summary>
    public partial class ObstructionGameMode : Page
    {
        private string _gameMode;
        public ObstructionGameMode()
        {
            InitializeComponent();
        }

        private void smallButton_Click(object sender, RoutedEventArgs e)
        {
            Router.ObstructionMode = 5;
            NavigationService.Navigate(Router.OpponentPage);
        }

        private void mediumButton_Click(object sender, RoutedEventArgs e)
        {
            Router.ObstructionMode = 8;
            NavigationService.Navigate(Router.OpponentPage);
        }

        private void largeButton_Click(object sender, RoutedEventArgs e)
        {
            Router.ObstructionMode = 12;
            NavigationService.Navigate(Router.OpponentPage);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}
