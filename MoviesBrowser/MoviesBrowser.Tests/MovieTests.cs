using MoviesBrowser.Common.Movies;
using Xunit;

namespace MoviesBrowser.Tests
{
    public class MovieTests
    {
        private const string moviePosterLinkLowQuality = "www.randomimageplaceholderlink.com/SX300";
        private const string moviePosterLinkHighQuality = "www.randomimageplaceholderlink.com/SX900";

        [Fact]
        public void Poster_setting_poster_property_updates_link_to_higher_quality_automatically()
        {
            var movie = new Movie
            {
                Poster = moviePosterLinkLowQuality
            };

            Assert.True(movie.Poster == moviePosterLinkHighQuality);
        }
    }
}