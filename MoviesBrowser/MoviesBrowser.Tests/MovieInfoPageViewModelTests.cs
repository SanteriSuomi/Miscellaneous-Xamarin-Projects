using MoviesBrowser.Common.Database;
using MoviesBrowser.Common.Movies;
using MoviesBrowser.Common.Networking;
using MoviesBrowser.Modules.MovieInfoPage;
using System.Threading.Tasks;
using Xunit;

namespace MoviesBrowser.Tests
{
    public class MovieInfoPageViewModelTests
    {
        private const string placeholderMovieTitle = "Man on Fire";

        [Fact]
        public async Task InitializeAsync_passes_movie_parameter_correctly_and_gets_imdb_movie_data_through_network_service()
        {
            var model = new MovieInfoPageViewModel(new NetworkService(), new Repository<ImdbMovie>());

            await model.InitializeAsync(new Movie()
            {
                imdbID = "tt0328107"
            });

            Assert.True(model.Movie?.Title == placeholderMovieTitle);
        }
    }
}