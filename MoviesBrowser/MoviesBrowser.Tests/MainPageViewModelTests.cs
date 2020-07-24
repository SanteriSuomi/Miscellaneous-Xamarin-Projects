using MoviesBrowser.Common.Movies;
using MoviesBrowser.Common.Navigation;
using MoviesBrowser.Common.Networking;
using MoviesBrowser.Modules.MainPage;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xunit;

namespace MoviesBrowser.Tests
{
    public class MainPageViewModelTests
    {
        [Fact]
        public async Task SearchCommand_is_executed_correctly_and_items_get_populated()
        {
            var tempNavigation = new MainPageView(null);
            var lazyNavigation = new Lazy<INavigation>(tempNavigation.Navigation);
            var model = new MainPageViewModel(new NetworkService(), new NavigationService(lazyNavigation));

            model.SearchCommand.Execute(new SearchBar(){ Text = "avengers" });

            await Task.Delay(TimeSpan.FromSeconds(0.5));

            Assert.True(model.Items.Count > 0);
        }

        [Fact]
        public async Task ItemClickedCommand_is_executed_correctly_and_navigation_works()
        {
            var tempNavigation = new MainPageView(null);
            var lazyNavigation = new Lazy<INavigation>(tempNavigation.Navigation);
            var model = new MainPageViewModel(new NetworkService(), new NavigationService(lazyNavigation));
            var view = new MainPageView(model);

            var countBefore = view.Navigation.NavigationStack.Count;
            model.ItemClickedCommand.Execute(new Movie());

            await Task.Delay(TimeSpan.FromSeconds(0.5));

            var countAfter = view.Navigation.NavigationStack.Count;

            Assert.True(countAfter == countBefore + 1);
        }
    }
}