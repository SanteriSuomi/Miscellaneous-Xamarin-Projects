using Autofac;
using MoviesBrowser.Common.Movies;
using MoviesBrowser.Common.Navigation;
using MoviesBrowser.Common.Networking;
using MoviesBrowser.Modules.SearchMoviesPage;
using MoviesBrowser.Modules.SavedMoviesPage;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Mocks;
using Xunit;

namespace MoviesBrowser.Tests
{
    public class MainPageViewModelTests
    {
        private SearchMoviesPageViewModel _model;
        private Lazy<INavigation> _lazyNavigation;
        private NavigationPage _navigationPage;

        [Fact]
        public async Task SearchCommand_is_executed_correctly_and_items_get_populated()
        {
            _navigationPage = new NavigationPage();
            _lazyNavigation = new Lazy<INavigation>(_navigationPage.Navigation);
            _model = new SearchMoviesPageViewModel(new NetworkService(), new NavigationService(_lazyNavigation));

            _model.SearchCommand.Execute("mortdecai");

            await Task.Delay(TimeSpan.FromSeconds(0.5));

            Assert.True(_model.Items.Count > 0);
        }

        [Fact]
        public async Task MainPage_navigation_works()
        {
            MockForms.Init();

            _navigationPage = new NavigationPage();
            _lazyNavigation = new Lazy<INavigation>(() => _navigationPage.Navigation);
            var app = Application.Current = new App();
            app.MainPage = _navigationPage;

            var mainPage = App.Container.Resolve<SearchMoviesPageView>();
            await _navigationPage.PushAsync(mainPage);
            var movieInfoPage = App.Container.Resolve<SavedMoviesPageView>();
            await _navigationPage.Navigation.PushAsync(movieInfoPage);

            Assert.Equal(2, _navigationPage.Navigation.NavigationStack.Count);
        }

        [Fact]
        public void SearchbarText_when_empty_or_null_resets_list()
        {
            _model = new SearchMoviesPageViewModel
            {
                SearchbarText = "placeholder",
                Items = new ObservableCollection<Movie>()
                {
                    new Movie(),
                    new Movie()
                }
            };

            _model.SearchbarText = string.Empty;

            Assert.True(_model.Items.Count == 0);
        }
    }
}