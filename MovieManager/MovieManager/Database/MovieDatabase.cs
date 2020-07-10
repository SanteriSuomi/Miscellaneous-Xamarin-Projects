using MovieManager.Extensions;
using MovieManager.MovieData.Specific;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

namespace MovieManager.Database
{
    public class MovieDatabase
    {
        private static MovieDatabase movieDatabase;

        [SuppressMessage("Style", "IDE0074:Use compound assignment", Justification = "<Compound assignment not available in this version of C#>")]
        public static MovieDatabase DB
            => movieDatabase ?? (movieDatabase = new MovieDatabase());

        private static readonly Lazy<SQLiteAsyncConnection> lazyDatabaseInitializer = new Lazy<SQLiteAsyncConnection>(()
            => new SQLiteAsyncConnection(DatabaseConstants.Path, DatabaseConstants.Flags));

        private static SQLiteAsyncConnection DatabaseConnection => lazyDatabaseInitializer.Value;
        private static bool isInitialized;

        public MovieDatabase()
        {
            InitializeAsync().SafeFireAndForget(false);
        }

        private static async Task InitializeAsync()
        {
            if (!isInitialized
                && !DatabaseConnection.TableMappings.Any(m => m.MappedType.Name == typeof(MovieDetails).Name))
            {
                await DatabaseConnection.CreateTablesAsync(CreateFlags.None, typeof(MovieDetails)).ConfigureAwait(false);
                isInitialized = true;
            }
        }

        public Task<List<MovieDetails>> GetAllMoviesAsync()
            => DatabaseConnection.Table<MovieDetails>().ToListAsync();

        public Task<MovieDetails> GetMovieAsync(System.Linq.Expressions.Expression<Func<MovieDetails, bool>> expression)
            => DatabaseConnection.Table<MovieDetails>().Where(expression).FirstOrDefaultAsync();

        public AsyncTableQuery<MovieDetails> GetTable()
            => DatabaseConnection.Table<MovieDetails>();

        public Task<int> SaveMoviesAsync(params MovieDetails[] movies)
            => DatabaseConnection.InsertAllAsync(movies);

        [SuppressMessage("Naming", "RCS1047:Non-asynchronous method name should not end with 'Async'.", Justification = "<Returns an array of tasks>")]
        public Task<int>[] DeleteMoviesAsync(params MovieDetails[] movies)
        {
            Task<int>[] tasks = new Task<int>[movies.Length + 1];
            for (int i = 0; i < movies.Length; i++)
            {
                tasks[i] = DatabaseConnection.DeleteAsync(movies[i]);
            }

            return tasks;
        }

        public Task<int> DeleteAllMoviesAsync() => DatabaseConnection.DeleteAllAsync<MovieDetails>();
    }
}