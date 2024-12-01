using System;
using System.Text;
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
using _2PlayerGames.repository;

namespace _2PlayerGames
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;

        }
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Router.UserPlayer = new Player(Guid.NewGuid(), "David", "0.0.0.0", 69);
            PlayerRepository.addPlayer(Router.UserPlayer);
            MainFrame.NavigationService.Navigate(Router.MenuPage);
        }

    }
}