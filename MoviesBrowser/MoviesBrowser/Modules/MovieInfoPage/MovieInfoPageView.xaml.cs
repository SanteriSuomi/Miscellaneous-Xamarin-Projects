using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System;

namespace MoviesBrowser.Modules.MovieInfoPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MovieInfoPageView : ContentPage
    {
        private const double favouriteIconAnimationScale = 1.35;
        private const uint favouriteIconAnimationDuration = 200;

        public MovieInfoPageView(MovieInfoPageViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        private async void OnFavouriteIconClicked(object sender, EventArgs e)
        {
            var imgButton = sender as ImageButton;
            await imgButton.ScaleTo(favouriteIconAnimationScale, favouriteIconAnimationDuration, Easing.CubicOut);
            await imgButton.ScaleTo(1, favouriteIconAnimationDuration, Easing.CubicOut);
        }
    }
}