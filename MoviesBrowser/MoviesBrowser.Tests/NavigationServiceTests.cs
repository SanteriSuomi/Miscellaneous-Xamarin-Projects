using Moq;
using MoviesBrowser.Common.Navigation;
using MoviesBrowser.Modules.MainPage;
using System;
using Xamarin.Forms;
using Xunit;

namespace MoviesBrowser.Tests
{
    public class NavigationServiceTests
    {
        [Fact]
        public void Constructor_registers_views_to_viewmodels_from_assembly_correctly()
        {
            var navigationMock = new Mock<INavigation>();
            var lazyNavigation = new Lazy<INavigation>(() => navigationMock.Object);
            _ = new NavigationService(lazyNavigation);

            var pageMap = NavigationService.PageMap;
            Assert.True(pageMap.ContainsKey(typeof(MainPageViewModel)) 
                        && pageMap[typeof(MainPageViewModel)].Equals(typeof(MainPageView)));
        }
    }
}