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

namespace IMDB_App.Pages
{
    /// <summary>
    /// Interaction logic for DirectorsPage.xaml
    /// </summary>
    public partial class DirectorsPage : Page
    {
        public DirectorsPage()
        {
            InitializeComponent();
            CreateAlphabetButtons();
        }

        private void CreateAlphabetButtons()
        {
            for (char letter = 'A'; letter <= 'Z'; letter++)
            {
                Button alphabetButton = new Button
                {
                    Content = letter.ToString(),
                    Width = 35,
                    Height = 35,
                    Margin = new Thickness(5),
                    Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(159, 173, 189)),
                    Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 0, 0)),
                    FontSize = 18
                };

                // Attach the click event
                alphabetButton.Click += AlphabetButton_Click;

                // Add the button to the StackPanel
                AlphabetPanel.Children.Add(alphabetButton);
            }
        }

        private void AlphabetButton_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;
            string selectedLetter = clickedButton.Content.ToString();
        }

        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            string searchQuery = catalogSearchBox.Text;
            MessageBox.Show($"Searching for {searchQuery}");
        }
    }
}
