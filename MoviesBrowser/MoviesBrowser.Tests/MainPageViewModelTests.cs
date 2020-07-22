using MoviesBrowser.Modules.MainPage;
using Xunit;

namespace MoviesBrowser.Tests
{
    public class MainPageViewModelTests
    {
        [Fact]
        public void Constructor_sets_title_correctly()
        {
            var viewModel = new MainPageViewModel();
            Assert.True(viewModel.Title != null && viewModel.Title.Length > 0);
        }
    }
}