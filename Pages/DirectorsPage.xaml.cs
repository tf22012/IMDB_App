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

namespace IMDB_App.Pages
{
    public partial class DirectorsPage : Page
    {
        //private List<string> directors;
        
        ImdbContext _context = new ImdbContext();
        CollectionViewSource directorsViewSource = new CollectionViewSource();

        public DirectorsPage()
        {
            InitializeComponent();

            //Tie the markup viewsource object to the C# code viewsource object
            directorsViewSource = (CollectionViewSource)FindResource(nameof(directorsViewSource));

            //Use the dbContext to tell EF to load the data we want to use on this page.
            _context.Titles.Take(10).Load();
            _context.Names.Take(10).Load();

            //Set the viewsource data source to use the artists data collection (dbset)
            directorsViewSource.Source = _context.Titles.Local.ToObservableCollection();

            LoadDirectorData("");
        }

        private void LoadDirectorData(string searchText = "")
        {
            var directorsWithTitles = _context.Names.Include(n => n.Titles);

            var query =
                (from director in directorsWithTitles
                 where director.PrimaryProfession.Contains("director") &&
                       (string.IsNullOrEmpty(searchText) || director.PrimaryName.Contains(searchText))
                 group director by director.PrimaryName.ToUpper().Substring(0, 1) into directorGroup
                 select new
                 {
                     HeaderText = $"{directorGroup.Key}",
                     directors = directorGroup.Take(10).ToList()
                 })
                .Take(10);

            // Execute the query against the database and assign it as the data source for the list view
            directorsListView.ItemsSource = query.ToList();

        }

        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            // Searchs when the button is clicked
            var searchText = directorsSearchBox.Text.ToLower();

            LoadDirectorData(searchText);
        }

    }
}
