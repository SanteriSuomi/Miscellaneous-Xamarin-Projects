namespace MoviesBrowser.Common.Movies
{
    public class Movie
    {
        public string Title { get; set; } 
        public string Year { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "JSON Object")]
        public string imdbID { get; set; } 
        public string Type { get; set; } 
        public string Poster { get; set; } 
    }
}