using System;

namespace MoviesBrowser.Common.Utilities
{
    public static class MovieUtils
    {
        private const string apiKey = "1f1c040";

        public static Uri GetUri(string parameter, string search)
        {
            return new Uri($"https://www.omdbapi.com/?{parameter}={search}&apikey={apiKey}");
        }
    }
}