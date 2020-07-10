using MovieManager.Data;
using MovieManager.Database;
using MovieManager.Extensions;
using MovieManager.MovieData.Specific;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MovieManager
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public static MainPage This { get; private set; }

        private ObservableCollection<MovieGroup> moviesGroup;

        public MainPage()
        {
            InitializeComponent();
            This = this;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (moviesGroup is null)
            {
                InitializeNewMovieList().SafeFireAndForget(false);
            }
        }

        /// <summary>
        /// Create a new 
        /// </summary>
        /// <returns></returns>
        public async Task InitializeNewMovieList()
        {
            var movies = await MovieDatabase.DB.GetAllMoviesAsync().ConfigureAwait(true);
            var listOfMovieLists = new List<MovieGroup>();
            SortMovies(movies, listOfMovieLists);

            var observableListOfMovieLists = new ObservableCollection<MovieGroup>();
            for (int i = 0; i < listOfMovieLists.Count; i++)
            {
                observableListOfMovieLists.Add(listOfMovieLists[i]);
            }

            mainListView.ItemsSource = moviesGroup = observableListOfMovieLists;
            BindingContext = this;
        }

        private void SortMovies(List<MovieDetails> movies, List<MovieGroup> listOfMovieLists)
        {
            for (int i = 0; i < movies.Count; i++)
            {
                var index = listOfMovieLists.FindIndex(m => m.FirstLetter == movies[i].FirstLetter);
                if (index >= 0)
                {
                    listOfMovieLists[index].Add(movies[i], false);
                }
                else
                {
                    var newMovieList = new MovieGroup(movies[i].FirstLetter)
                    {
                        movies[i]
                    };

                    listOfMovieLists.Add(newMovieList);
                }
            }
        }

        private void OnSavedSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(savedItemsSearchBar.Text))
            {
                mainListView.IsGroupingEnabled = true;
                mainListView.ItemsSource = moviesGroup;
                return;
            }

            mainListView.IsGroupingEnabled = false;
            mainListView.ItemsSource = GetFilteredMovies();
        }

        private ObservableCollection<MovieDetails> GetFilteredMovies()
        {
            ObservableCollection<MovieDetails> filteredMovies = new ObservableCollection<MovieDetails>();
            foreach (var group in moviesGroup)
            {
                foreach (var movie in group)
                {
                    if (movie.Title.IndexOf(savedItemsSearchBar.Text, StringComparison.InvariantCultureIgnoreCase) >= 0)
                    {
                        filteredMovies.Add(movie);
                    }
                }
            }

            return filteredMovies;
        }

        private async void OnListItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is null) return;

            var movie = e.SelectedItem as MovieDetails;
            mainListView.SelectedItem = null;

            var action = await DisplayActionSheet($"{movie.Title}", "Cancel", "Delete", "More about the movie").ConfigureAwait(true);
            switch (action)
            {
                case "Delete":
                    var alert = await DisplayAlert("Delete", $"Are you sure you want to delete {movie.Title}?", "Yes", "No").ConfigureAwait(true);
                    if (alert)
                    {
                        for (int i = 0; i < moviesGroup.Count; i++)
                        {
                            moviesGroup[i].TryRemove(movie, true);
                        }
                    }

                    break;
                case "More about the movie":
                    var titleData = await MovieApi.Get<MovieDetails>($"i={movie.imdbID}").ConfigureAwait(true);
                    await Navigation.PushAsync(new MovieInfoPage(titleData, moviesGroup, saveButtonActivated: false), animated: true).ConfigureAwait(true);
                    break;
            }
        }

        private async void OnFindMoreClicked(object sender, EventArgs e)
            => await Navigation.PushAsync(new FindMoreResultsPage(moviesGroup), animated: true).ConfigureAwait(true);
    }
}