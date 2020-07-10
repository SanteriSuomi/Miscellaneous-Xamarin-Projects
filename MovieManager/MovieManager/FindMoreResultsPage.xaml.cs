using MovieManager.Data;
using MovieManager.MovieData.Search;
using MovieManager.MovieData.Specific;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MovieManager
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FindMoreResultsPage : ContentPage
    {
        private List<MovieSearch> resultsList;
        private readonly ObservableCollection<MovieGroup> savedMoviesList;

        public FindMoreResultsPage(ObservableCollection<MovieGroup> savedMoviesList)
        {
            InitializeComponent();
            this.savedMoviesList = savedMoviesList;
        }

        private async void OnSearchButtonPressed(object sender, EventArgs e)
        {
            try
            {
                var data = await MovieApi.Get<MovieSearchBase>($"s={itemSearchBar.Text}").ConfigureAwait(true);
                resultsList = data.Search.Where(m => m.Title.IndexOf(itemSearchBar.Text, StringComparison.InvariantCultureIgnoreCase) >= 0).ToList();
                if (resultsList?.Count > 0)
                {
                    resultsListView.ItemsSource = resultsList;
                }
            }
            catch (ArgumentNullException)
            {
                resultsListView.ItemsSource = null;
            }
        }

        private async void OnResultsListItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is null) return;

            var search = e.SelectedItem as MovieSearch;
            resultsListView.SelectedItem = null;
            var action = await DisplayActionSheet($"{search.Title}", "Cancel", null, "More about the movie").ConfigureAwait(true);
            switch (action)
            {
                case "More about the movie":
                    var titleData = await MovieApi.Get<MovieDetails>($"i={search.imdbID}").ConfigureAwait(true);
                    await Navigation.PushAsync(new MovieInfoPage(titleData, savedMoviesList, saveButtonActivated: true), animated: true).ConfigureAwait(true);
                    break;
            }
        }
    }
}