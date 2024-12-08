using IMDB_App.Data;
using Microsoft.EntityFrameworkCore;
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
    /// Interaction logic for TitlesPage.xaml
    /// </summary>
    public partial class TitlesPage : Page
    {
        ImdbContext _context = new ImdbContext();
        CollectionViewSource titlesViewSource = new CollectionViewSource();

        public TitlesPage()
        {
            InitializeComponent();

            //Tie the markup viewsource object to the C# code viewsource object
            titlesViewSource = (CollectionViewSource)FindResource(nameof(titlesViewSource));

            //Use the dbContext to tell EF to load the data we want to use on this page.
           _context.Titles.Load();

            //Set the viewsource data source to use the artists data collection (dbset)
            titlesViewSource.Source = _context.Titles.Local.ToObservableCollection();

            LoadTitleData("");
        }

        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            LoadTitleData(titlesSearchBox.Text);
        }

        private void LoadTitleData(string searchText="")
        {
            var query =
                from title in _context.Titles
                where string.IsNullOrEmpty(searchText) || title.PrimaryTitle.Contains(searchText)
                orderby title.PrimaryTitle
                select new
                {
                    PTitle = title.PrimaryTitle,
                    Genres = title.Genres.Select(genre => new
                    {
                        GenreName = genre.Name
                    }).ToList()
                };

            titlesListView.ItemsSource = query.ToList();
        }
    }
}
