﻿using MoviesBrowser.Common.Database;
using SQLite;
using System.Collections.Generic;

namespace MoviesBrowser.Common.Movies
{
    public class ImdbMovie : IDatabaseItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public bool IsFavourite { get; set; }

        public string Title { get; set; }
        public string Year { get; set; }
        public string Rated { get; set; }
        public string Released { get; set; }
        public string Runtime { get; set; }
        public string Genre { get; set; }
        public string Director { get; set; }
        public string Writer { get; set; }
        public string Actors { get; set; }
        public string Plot { get; set; }
        public string Language { get; set; }
        public string Country { get; set; }
        public string Awards { get; set; }
        public string Poster { get; set; }
        [Ignore]
        public List<Rating> Ratings { get; set; }
        public string Metascore { get; set; }
        #pragma warning disable IDE1006 // Naming Styles
        public string imdbRating { get; set; }
        public string imdbVotes { get; set; }
        public string imdbID { get; set; }
        #pragma warning restore IDE1006 // Naming Styles
        public string Type { get; set; }
        public string DVD { get; set; }
        public string BoxOffice { get; set; }
        public string Production { get; set; }
        public string Website { get; set; }
        public string Response { get; set; }
    }
}