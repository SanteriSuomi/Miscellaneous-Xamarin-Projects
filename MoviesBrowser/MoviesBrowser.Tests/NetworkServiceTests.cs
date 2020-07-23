using MoviesBrowser.Common.Movies;
using MoviesBrowser.Common.Networking;
using MoviesBrowser.Common.Utilities;
using Xunit;

namespace MoviesBrowser.Tests
{
    public class NetworkServiceTests
    {
        [Fact]
        public void Get_returns_items_correctly()
        {
            var service = new NetworkService();
            var result = service.GetAsync<MovieSearchResults>(MovieUtils.GetUri("s", "avengers")).Result;

            Assert.True(result.Search.Count > 0
                        && result.Search[0].Title != null
                        && result.Search[0].Title.Length > 0);
        }
    }
}