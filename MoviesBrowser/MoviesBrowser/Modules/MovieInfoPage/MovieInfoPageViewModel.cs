using Autofac;
using MoviesBrowser.Common.Database;
using MoviesBrowser.Common.Movies;
using MoviesBrowser.Common.Networking;
using MoviesBrowser.Common.Utilities;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using TodoList.Extensions;
using Xamarin.Forms;

namespace MoviesBrowser.Modules.MovieInfoPage
{
    public class MovieInfoPageViewModel : BaseViewModel
    {
        public MovieInfoPageViewModel(INetworkService networkService, IRepository<ImdbMovie> repository)
        {
            _networkService = networkService;
            _repository = repository;
            _repository = App.Container.Resolve<IRepository<ImdbMovie>>();
            FavouriteClickCommand = new Command<ImdbMovie>((m) =>
            {
                OnFavouriteClick(m).SafeFireAndForget(false);
            });
        }

        public override async Task InitializeAsync(object parameter)
        {
            var movie = parameter as Movie;
            var allMovies = await _repository.GetAllAsync();
            if (allMovies != null && allMovies.Count > 0)
            {
                var imdbMovie = allMovies.FirstOrDefault(m => m.imdbID == movie.imdbID);
                if (imdbMovie != null)
                {
                    Movie = imdbMovie;
                    FavouriteIconState = imdbMovie.IsFavourite;
                    return;
                }
            }

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
        private readonly IRepository<ImdbMovie> _repository;

        private async Task OnFavouriteClick(ImdbMovie movie)
        {
            FavouriteIconState = !FavouriteIconState;
            movie.IsFavourite = FavouriteIconState;
            if (FavouriteIconState)
            {
                await _repository.SaveAsync(movie);
            }
            else
            {
                await _repository.DeleteAsync(movie);
            }
        }
    }
}