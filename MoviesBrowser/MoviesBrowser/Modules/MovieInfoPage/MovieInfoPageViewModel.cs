using MoviesBrowser.Common.Movies;
using MoviesBrowser.Common.Networking;
using MoviesBrowser.Common.Utilities;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MoviesBrowser.Modules.MovieInfoPage
{
    public class MovieInfoPageViewModel : BaseViewModel
    {
        public MovieInfoPageViewModel(INetworkService networkService)
        {
            _networkService = networkService;
            FavouriteClickCommand = new Command(OnFavouriteClick);
        }

        public override async Task InitializeAsync(object parameter)
        {
            Movie movie = parameter as Movie;
            Movie = await _networkService.GetAsync<ImdbMovie>(MovieUtils.GetUri("i", movie.imdbID));
        }

        private ImdbMovie _movie;
        public ImdbMovie Movie
        {
            get => _movie;
            set
            {
                _movie = value;
                OnPropertyChanged();
            }
        }

        private bool _favouriteIconState;
        public bool FavouriteIconState
        {
            get => _favouriteIconState;
            set
            {
                _favouriteIconState = value;
                OnPropertyChanged();
            }
        }

        public ICommand FavouriteClickCommand { get; set; }

        private readonly INetworkService _networkService;

        private void OnFavouriteClick()
        {
            FavouriteIconState = !FavouriteIconState;
        }
    }
}