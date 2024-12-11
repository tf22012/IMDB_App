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
                     actors = actorGroup.Take(10).ToList()
                 })
                .Take(10);


            // Execute the query against the database and assign it as the data source for the list view
            actorsListView.ItemsSource = query.ToList();
        }

        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            // Searchs when the button is clicked
            var searchText = actorsSearchBox.Text.ToLower();

            LoadActorData(searchText);
        }

    }
}
