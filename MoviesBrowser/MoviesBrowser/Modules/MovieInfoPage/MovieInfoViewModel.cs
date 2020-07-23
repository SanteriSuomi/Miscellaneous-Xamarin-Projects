using MoviesBrowser.Common.Movies;
using System.Threading.Tasks;

namespace MoviesBrowser.Modules.MovieInfoPage
{
    public class MovieInfoViewModel : BaseViewModel
    {
        private Movie _movie;

        public override Task InitializeAsync(object parameter)
        {
            _movie = parameter as Movie;
            return Task.CompletedTask;
        }
    }
}