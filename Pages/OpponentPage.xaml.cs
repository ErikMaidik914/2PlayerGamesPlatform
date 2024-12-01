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
    /// Interaction logic for OpponentPage.xaml
    /// </summary>
    public partial class OpponentPage : Page
    {
       
        public OpponentPage()
        {
            InitializeComponent();
        }

        private void humanButton_Click(object sender, RoutedEventArgs e)
        {
            Router.OnlineGame = true;
            this.NavigationService.Navigate(Router.LoadingPage);
        }

        private void robotButton_Click(object sender, RoutedEventArgs e)
        {
            Router.OnlineGame = false;
            this.NavigationService.Navigate(Router.AiSelectionPage);

        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}
