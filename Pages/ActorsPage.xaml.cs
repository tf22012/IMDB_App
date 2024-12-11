using IMDB_App.Data;
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
using IMDB_App.Data;
using IMDB_App.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace IMDB_App.Pages
{
    /// <summary>
    /// Interaction logic for ActorsPage.xaml
    /// </summary>
    public partial class ActorsPage : Page
    {

        ImdbContext _context = new ImdbContext();
        CollectionViewSource actorsViewSource = new CollectionViewSource();
        public ActorsPage()
        {
            InitializeComponent();
            //CreateAlphabetButtons();
            //directors = GetDirectorsFromDatabase(); // Fetch the directors
            //Tie the markup viewsource object to the C# code viewsource object
            actorsViewSource = (CollectionViewSource)FindResource(nameof(actorsViewSource));

            //Use the dbContext to tell EF to load the data we want to use on this page.
            _context.Titles.Take(10).Load();
            _context.Names.Take(10).Load();

            //Set the viewsource data source to use the artists data collection (dbset)
            actorsViewSource.Source = _context.Titles.Local.ToObservableCollection();
            LoadActorData("");    
        }

        private void LoadActorData(string searchText = "")
        {
            var actorsWithTitles = _context.Names.Include(n => n.TitlesNavigation);

            var query =
                (from actor in actorsWithTitles
                 where actor.PrimaryProfession.Contains("actor") &&
                       (string.IsNullOrEmpty(searchText) || actor.PrimaryName.Contains(searchText))
                 group actor by actor.PrimaryName.ToUpper().Substring(0, 1) into actorGroup
                 select new
                 {
                     HeaderText = $"{actorGroup.Key}",
                     actors = actorGroup.Take(5).ToList()
                 })
                .Take(5);


            // Execute the query against the database and assign it as the data source for the list view
            actorsListView.ItemsSource = query.ToList();

            //string query = @"
            //    SELECT n.primaryName
            //    FROM dbo.Directors d
            //    INNER JOIN dbo.Names n
            //        ON d.nameID = n.nameID
            //    WHERE n.primaryProfession LIKE '%director%'
            //    ORDER BY n.primaryName";
        }

        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            // Searchs when the button is clicked
            var searchText = actorsSearchBox.Text.ToLower();

            //var filteredDirectors
            LoadActorData(searchText);
            //string searchQuery = catalogSearchBox.Text;

            //if (string.IsNullOrEmpty(searchQuery))
            //{
            //    MessageBox.Show("Please enter a search query.");
            //    return;
            //}

            //var filteredDirectors = directors.Where(d => d.Contains(searchQuery, StringComparison.OrdinalIgnoreCase)).ToList();

            //if (filteredDirectors.Any())
            //{
            //    DirectorsListBox.ItemsSource = filteredDirectors;
            //}
            //else
            //{
            //    MessageBox.Show($"No directors found for '{searchQuery}'.");
            //}
        }

        //// Create alphabet buttons from A-Z
        //private void CreateAlphabetButtons()
        //{
        //    for (char letter = 'A'; letter <= 'Z'; letter++)
        //    {
        //        Button alphabetButton = new Button
        //        {
        //            Content = letter.ToString(),
        //            Width = 35,
        //            Height = 35,
        //            Margin = new Thickness(5),
        //            Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(159, 173, 189)),
        //            Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 0, 0)),
        //            FontSize = 18
        //        };

        //        alphabetButton.Click += AlphabetButton_Click;

        //        AlphabetPanel.Children.Add(alphabetButton);
        //    }
        //}

        //private void AlphabetButton_Click(object sender, RoutedEventArgs e)
        //{
        //    Button clickedButton = sender as Button;
        //    string selectedLetter = clickedButton.Content.ToString();
        //    ShowDirectorsDropdown(selectedLetter);
        //}

        //// Show a dropdown of directors whose first name starts with the selected letter
        //private void ShowDirectorsDropdown(string letter)
        //{
        //    // Filter directors based on the selected letter
        //    var filteredDirectors = directors.Where(d => d.StartsWith(letter, StringComparison.OrdinalIgnoreCase)).ToList();

        //    if (filteredDirectors.Count > 0)
        //    {
        //        ComboBox directorComboBox = new ComboBox
        //        {
        //            Width = 200,
        //            Height = 30,
        //            Margin = new Thickness(5),
        //            ItemsSource = filteredDirectors
        //        };

        //        AlphabetPanel.Children.Add(directorComboBox);
        //    }
        //    else
        //    {
        //        MessageBox.Show($"No directors found with the first name starting with '{letter}'.");
        //    }
        //}

        //// Get directors from the database
        //private List<string> GetDirectorsFromDatabase()
        //{
        //    List<string> directors = new List<string>();
        //    string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=IMDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False";

        //    string query = @"
        //        SELECT n.primaryName
        //        FROM dbo.Directors d
        //        INNER JOIN dbo.Names n
        //            ON d.nameID = n.nameID
        //        WHERE n.primaryProfession LIKE '%director%'
        //        ORDER BY n.primaryName";

        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        SqlCommand command = new SqlCommand(query, connection);

        //        try
        //        {
        //            connection.Open();
        //            SqlDataReader reader = command.ExecuteReader();
        //            while (reader.Read())
        //            {
        //                directors.Add(reader["primaryName"].ToString());
        //            }
        //            reader.Close();
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show($"Error fetching directors: {ex.Message}");
        //        }
        //    }
        //    return directors;
        //}
    }
}
