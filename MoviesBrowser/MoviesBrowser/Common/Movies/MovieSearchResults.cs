using System.Collections.Generic;

namespace MoviesBrowser.Common.Movies
{
    public class MovieSearchResults
    {
        public List<Movie> Search { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "JSON Object")]
        public string totalResults { get; set; } 
        public string Response { get; set; } 
    }
}