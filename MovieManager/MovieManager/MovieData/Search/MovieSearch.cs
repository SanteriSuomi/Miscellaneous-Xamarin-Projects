namespace MovieManager.MovieData.Search
{
    public class MovieSearch
    {
        public string Title { get; set; }
        public string Year { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Lowercase for json support")]
        public string imdbID { get; set; }
        public string Type { get; set; }
        public string Poster { get; set; }
    }
}