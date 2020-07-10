using MovieManager.Extensions;
using MovieManager.MovieData.Specific;
using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MovieManager
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MovieInfoPage : ContentPage
    {
        private readonly ObservableCollection<MovieGroup> savedMoviesList;
        private readonly ToolbarItem toolbar;
        private readonly MovieDetails localMovieInfo;

        public MovieInfoPage(MovieDetails localInfo, ObservableCollection<MovieGroup> savedMoviesList, bool saveButtonActivated)
        {
            InitializeComponent();

            movieImage.Source = localInfo.Poster;
            movieTitle.Text = localInfo.Title;
            Title = localInfo.Title;
            movieRating.Text = $"{localInfo.imdbRating} out of 10";
            movieDescription.Text = localInfo.Plot;

            if (saveButtonActivated)
            {
                toolbar = new ToolbarItem()
                {
                    Text = "Save",
                };

                toolbar.Clicked += OnToolbarSaveClicked;
                ToolbarItems.Add(toolbar);
            }

            localMovieInfo = localInfo;
            this.savedMoviesList = savedMoviesList;
        }

        private async void OnToolbarSaveClicked(object sender, EventArgs e)
        {
            await DisplayAlert(localMovieInfo.Title, $"{localMovieInfo.Title} has been saved successfully!", "OK").ConfigureAwait(true);
            savedMoviesList.AddAndOrganize(localMovieInfo);
        }

        ~MovieInfoPage()
        {
            if (!(toolbar is null))
            {
                toolbar.Clicked -= OnToolbarSaveClicked;
            }
        }
    }
}