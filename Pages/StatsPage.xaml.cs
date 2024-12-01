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
using _2PlayerGames.service;

namespace _2PlayerGames.Pages
{
    /// <summary>
    /// Interaction logic for StatsPage.xaml
    /// </summary>
    public partial class StatsPage : Page
    {

 
        public StatsPage()
        {
            InitializeComponent();
        
            Loaded += StatsPage_Loaded;
        }

        private void StatsPage_Loaded(object sender, RoutedEventArgs e)
        {
            PopulateTextBlocks();
        }

        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(Router.ProfilePage);
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(Router.MenuPage);
        }
      
        private void PopulateTextBlocks()
        {
            // Example: Retrieving data from a data source
            List<String> textData = Router.ProfileService.GetGameStats(Router.UserPlayer, Router.GameType);

            // Assuming you have text blocks named textBlock1, textBlock2, textBlock3, etc.
            // and you want to populate them dynamically
            for (int i = 0; i < textData.Count; i++)
            {
                // Example: Populate each text block with corresponding data
                switch (i)
                {
                    case 0:
                        eloTextInfo.Text = textData[i];
                        break;
                    case 1:
                        gamesTextInfo.Text = textData[i];
                        break;
                    // Add cases for additional text blocks if needed
                    case 2:
                        hoursTextInfo.Text = textData[i];
                        break;
                    case 3:
                        rankingTextInfo.Text = textData[i];
                        break;
                    // Add cases for additional text blocks if needed
                    case 4:
                        winLossInfo.Text = textData[i];
                        break;
                    case 5:
                        drawTextInfo.Text = textData[i];
                        break;
                    // Add cases for additional text blocks if needed
                }
            }
        }
    }
}
