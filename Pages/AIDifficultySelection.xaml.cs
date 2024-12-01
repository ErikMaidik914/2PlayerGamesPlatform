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
    /// Interaction logic for AIDifficultySelection.xaml
    /// </summary>
    public partial class AIDifficultySelection : Page
    {
        public AIDifficultySelection()
        {
            InitializeComponent();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(Router.OpponentPage);
        }

        private void easyMode_Click(object sender, RoutedEventArgs e)
        {
            Router.AiDifficulty = "easy";
            NavigationService.Navigate(Router.LoadingPage);
        }

        private void mediumMode_Click(object sender, RoutedEventArgs e)
        {
            Router.AiDifficulty = "medium";
            NavigationService.Navigate(Router.LoadingPage);
        }

        private void hardMode_Click(object sender, RoutedEventArgs e)
        {
            Router.AiDifficulty = "hard";
            NavigationService.Navigate(Router.LoadingPage);
        }
    }
}
