namespace MoviesBrowser.Common.Movies
{
    public class Movie
    {
        public string Title { get; set; }
        public string Year { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "JSON Object")]
        public string imdbID { get; set; }
        public string Type { get; set; }
        public string PlaceholderPoster { get; } = "noimageplaceholder.jpg";

        private string _poster;
        public string Poster
        {
            get => _poster;
            set
            {
                // Replace the lower quality image link with a higher quality one.
                if (value.Contains("SX300"))
                {
                    _poster = value.Replace("SX300", "SX900");
                }
                else
                {
                    _poster = value;
                }
            }
        }
    }
}