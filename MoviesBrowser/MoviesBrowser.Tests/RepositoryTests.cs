using MoviesBrowser.Common.Database;
using MoviesBrowser.Common.Movies;
using System.Threading.Tasks;
using Xunit;
using System;
using SQLite;
using System.IO;

namespace MoviesBrowser.Tests
{
    public class RepositoryTests
    {
        //private ImdbMovie _movie;

        //[Fact]
        //public async Task Repository_initializes_tables_correctly_and_get_post_work()
        //{
        //    IRepository<ImdbMovie> repository = new Repository<ImdbMovie>(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "database.db3"),
        //                                                                  SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
        //    _movie = new ImdbMovie();

        //    await repository.SaveAsync(_movie);
        //    var repMovie = await repository.GetByObject(_movie);

        //    Assert.IsType<ImdbMovie>(repMovie);
        //}
    }
}
