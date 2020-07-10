using MovieManager.Database;
using MovieManager.MovieData.Specific;
using System.Collections.ObjectModel;

namespace MovieManager
{
    public class MovieGroup : ObservableCollection<MovieDetails>
    {
        /// <summary>
        /// Grouping key
        /// </summary>
        public string FirstLetter { get; }

        public MovieGroup(string firstLetter)
            : base()
        {
            FirstLetter = firstLetter;
        }

        public void Add(MovieDetails movie, bool saveToDB)
        {
            Add(movie);
            if (saveToDB)
            {
                MovieDatabase.DB.SaveMoviesAsync(movie);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Bug", "S3168:\"async\" methods should not return \"void\"", Justification = "<Fire and forget>")]
        public void TryRemove(MovieDetails movie, bool removeFromDB)
        {
            int index = IndexOf(movie);
            if (index >= 0)
            {
                RemoveAt(index);
                if (removeFromDB)
                {
                    MovieDatabase.DB.DeleteMoviesAsync(movie);
                }
            }
        }
    }
}