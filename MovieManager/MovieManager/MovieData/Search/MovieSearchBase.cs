using System.Collections.Generic;

namespace MovieManager.MovieData.Search
{
    public class MovieSearchBase
    {
        public List<MovieSearch> Search { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Lowercase for json support")]
        public string totalResults { get; set; }
        public string Response { get; set; }
    }
}