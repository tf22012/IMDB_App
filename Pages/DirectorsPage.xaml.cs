using Microsoft.Data.SqlClient;
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
using IMDB_App.Models;

namespace IMDB_App.Pages
{
    public partial class DirectorsPage : Page
    {
        private List<string> directors;

        public DirectorsPage()
        {
            InitializeComponent();
            CreateAlphabetButtons();
            directors = GetDirectorsFromDatabase(); // Fetch the directors
        }

        // Create alphabet buttons from A-Z
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

                // Attach the click event for each button
                alphabetButton.Click += AlphabetButton_Click;

                // Add the button to the StackPanel
                AlphabetPanel.Children.Add(alphabetButton);
            }
        }

        // When an alphabet button is clicked, show a dropdown of directors starting with that letter
        private void AlphabetButton_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;
            string selectedLetter = clickedButton.Content.ToString();
            ShowDirectorsDropdown(selectedLetter);
        }

        // Show a dropdown of directors whose first name starts with the selected letter
        private void ShowDirectorsDropdown(string letter)
        {
            // Filter directors based on the selected letter
            var filteredDirectors = directors.Where(d => d.StartsWith(letter, StringComparison.OrdinalIgnoreCase)).ToList();

            if (filteredDirectors.Count > 0)
            {
                ComboBox directorComboBox = new ComboBox
                {
                    Width = 200,
                    Height = 30,
                    Margin = new Thickness(5),
                    ItemsSource = filteredDirectors
                };

                // Add the ComboBox to the UI
                AlphabetPanel.Children.Add(directorComboBox);
            }
            else
            {
                MessageBox.Show($"No directors found with the first name starting with '{letter}'.");
            }
        }

        // Get directors from the database
        private List<string> GetDirectorsFromDatabase()
        {
            List<string> directors = new List<string>();
            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=IMDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False";

            string query = @"
                SELECT n.primaryName
                FROM dbo.Directors d
                INNER JOIN dbo.Names n
                    ON d.nameID = n.nameID
                WHERE n.primaryProfession LIKE '%director%'
                ORDER BY n.primaryName";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        directors.Add(reader["primaryName"].ToString());
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error fetching directors: {ex.Message}");
                }
            }
            return directors;
        }

        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            string searchQuery = catalogSearchBox.Text;

            if (string.IsNullOrEmpty(searchQuery))
            {
                MessageBox.Show("Please enter a search query.");
                return;
            }

            var filteredDirectors = directors.Where(d => d.Contains(searchQuery, StringComparison.OrdinalIgnoreCase)).ToList();

            if (filteredDirectors.Any())
            {
                DirectorsListBox.ItemsSource = filteredDirectors;
            }
            else
            {
                MessageBox.Show($"No directors found for '{searchQuery}'.");
            }
        }
    }
}
