using MovieManager.Database;
using MovieManager.MovieData.Specific;
using System.Collections.ObjectModel;

namespace MovieManager.Extensions
{
    public static class MovieGroupExtensions
    {
        public static void AddAndOrganize(this ObservableCollection<MovieGroup> list, MovieDetails movie)
        {
            int indexOfLetter = GetLetterIndex(list, movie);
            SortList(list, movie, indexOfLetter);
            MovieDatabase.DB.SaveMoviesAsync(movie);
        }

        private static int GetLetterIndex(ObservableCollection<MovieGroup> list, MovieDetails movie)
        {
            int indexOfLetter = -1;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].FirstLetter == movie.FirstLetter)
                {
                    indexOfLetter = i;
                    break;
                }
            }

            return indexOfLetter;
        }

        private static void SortList(ObservableCollection<MovieGroup> list, MovieDetails movie, int indexOfLetter)
        {
            if (indexOfLetter >= 0)
            {
                list[indexOfLetter].Add(movie, true);
            }
            else
            {
                var newMovieList = new MovieGroup(movie.FirstLetter)
                {
                    movie
                };

                list.Add(newMovieList);
            }
        }
    }
}