using SQLite;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TodoList.Storage.ConfigSettings;
using Xamarin.Essentials;

namespace TodoList.Storage.Database
{
    public static class Database
    {
        private static readonly Lazy<SQLiteAsyncConnection> connectionHolder
                = new Lazy<SQLiteAsyncConnection>(() => new SQLiteAsyncConnection(path, flags));
        private static readonly SQLiteAsyncConnection connection = connectionHolder.Value;

        private static readonly string path = Path.Combine(FileSystem.AppDataDirectory, Config.ST.DatabaseName);
        private static readonly SQLiteOpenFlags flags = SQLiteOpenFlags.ReadWrite
                                                       | SQLiteOpenFlags.Create
                                                       | SQLiteOpenFlags.SharedCache;

        /// <summary>
        /// Return a new connection to a database with tables of T created and ready.
        /// </summary>
        public static async Task<SQLiteAsyncConnection> InitializeConnection<T>()
        {
            if (!connection.TableMappings.Any(x => x.MappedType == typeof(T)))
            {
                await connection.CreateTablesAsync(CreateFlags.None, typeof(T));
            }

            return connection;
        }
    }
}