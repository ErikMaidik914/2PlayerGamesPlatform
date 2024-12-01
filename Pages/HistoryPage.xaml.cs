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
    /// Interaction logic for HistoryPage.xaml
    /// </summary>
    public partial class HistoryPage : Page
    {

        private List<string> dataList;
        private int currentPageIndex = 0;
        private int itemsPerPage = 6;
        public HistoryPage()
        {
            InitializeComponent();
            InitializeData();
            DisplayPage();
        }

        private void InitializeData()
        {
            // Sample data (replace this with your own list)
            dataList = new List<string> { "abcd", "adcasd" };
        }

        private void ProfileButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new ProfilePage());
        }

        private void MenuButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new MenuPage());
        }


        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            currentPageIndex--;
            if (currentPageIndex < 0)
                currentPageIndex = dataList.Count / itemsPerPage;
            DisplayPage();
        }

        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            currentPageIndex++;
            if (currentPageIndex * itemsPerPage >= dataList.Count)
                currentPageIndex = 0;
            DisplayPage();
        }
        private void DisplayPage()
        {
            TextPagination.Children.Clear();

            // Add dynamically generated TextBlocks
            var currentPageData = dataList.Skip(currentPageIndex * itemsPerPage).Take(itemsPerPage);

            foreach (var item in currentPageData)
            {
                var border = new Border
                {
                    Margin = new Thickness(0, 20, 0, 10),
                    CornerRadius = new CornerRadius(10),
                    Width = 493,
                    Height = 61,
                    BorderBrush = Brushes.Aqua,
                    BorderThickness = new Thickness(2)
                };

                var textBlockInsideBorder = new TextBlock
                {
                    Text = item,
                    Foreground = Brushes.White,
                    Width = 200,
                    Height = 50,
                    FontSize = 24,
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    FontFamily = new FontFamily("Times New Roman"),
                    TextAlignment = TextAlignment.Center,
                    Margin = new Thickness(0, 16, 0, 0)
                };  
                border.Child = textBlockInsideBorder;
                TextPagination.Children.Add(border);
            }
        }




    }
}

