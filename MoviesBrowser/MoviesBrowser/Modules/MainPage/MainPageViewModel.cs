using MoviesBrowser.Common.Movies;
using MoviesBrowser.Common.Navigation;
using MoviesBrowser.Common.Networking;
using MoviesBrowser.Common.Utilities;
using MoviesBrowser.Modules.MovieInfoPage;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using TodoList.Extensions;
using Xamarin.Forms;

namespace MoviesBrowser.Modules.MainPage
{
    public class MainPageViewModel : BaseViewModel
    {
        public MainPageViewModel(INetworkService networkService, INavigationService navigationService)
        {
            _networkService = networkService;
            _navigationService = navigationService;
            SearchCommand = new Command<string>((s) => OnSearch(s).SafeFireAndForget(true));
            ItemClickedCommand = new Command<Movie>((m) => OnItemClicked(m).SafeFireAndForget(true));
        }

        private readonly INetworkService _networkService;
        private readonly INavigationService _navigationService;

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
            Console.WriteLine("SSSS");
            await _navigationService.PushAsync<MovieInfoViewModel>(movie);
        }
    }
}