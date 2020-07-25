using MoviesBrowser.Common.Movies;
using MoviesBrowser.Common.Navigation;
using MoviesBrowser.Common.Networking;
using MoviesBrowser.Common.Utilities;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using TodoList.Extensions;
using Xamarin.Forms;

namespace MoviesBrowser.Modules.SearchMoviesPage
{
    public class SearchMoviesPageViewModel : BaseViewModel
    {
        public SearchMoviesPageViewModel()
        {
        }

        public SearchMoviesPageViewModel(INetworkService networkService, INavigationService navigationService)
        {
            _networkService = networkService;
            _navigationService = navigationService;
            SearchCommand = new Command<string>((s) => OnSearch(s).SafeFireAndForget(true));
            ItemClickedCommand = new Command<Movie>((m) => OnItemClicked(m).SafeFireAndForget(true));
        }

        private readonly INetworkService _networkService;
        private readonly INavigationService _navigationService;

        private static readonly ObservableCollection<Movie> _emptyItems = new ObservableCollection<Movie>();
        private ObservableCollection<Movie> _items;
        public ObservableCollection<Movie> Items
        {
            get => _items;
            set
            {
                _items = value;
                OnPropertyChanged();
            }
        }

        private string _searchbarText;
        public string SearchbarText
        {
            get => _searchbarText;
            set
            {
                _searchbarText = value;
                OnSearchbarTextChanged(value);
            }
        }

        public ICommand SearchCommand { get; set; }
        public ICommand ItemClickedCommand { get; set; }

        private async Task OnSearch(string text)
        {
            if (string.IsNullOrEmpty(text)) return;

            var result = await _networkService.GetAsync<MovieSearchResults>(MovieUtils.GetUri("s", $"{text}"));
            Items = new ObservableCollection<Movie>(result.Search);
        }

        private async Task OnItemClicked(Movie movie)
        {
            //await _navigationService.PushAsync<MovieInfoViewModel>(movie);
        }

        private void OnSearchbarTextChanged(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                Items = _emptyItems;
            }
        }
    }
}