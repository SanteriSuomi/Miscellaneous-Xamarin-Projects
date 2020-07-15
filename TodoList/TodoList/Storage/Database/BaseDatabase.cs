using Polly;
using SQLite;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TodoList.Storage.ConfigSettings;
using Xamarin.Essentials;

namespace TodoList.Storage.Database
{
    public abstract class BaseDatabase
    {
        private static readonly string path = Path.Combine(FileSystem.AppDataDirectory, Config.ST.DatabaseName);
        private static readonly SQLiteOpenFlags flags = SQLiteOpenFlags.ReadWrite
                                                      | SQLiteOpenFlags.Create
                                                      | SQLiteOpenFlags.SharedCache;

        private static readonly Lazy<SQLiteAsyncConnection> connectionHolder
            = new Lazy<SQLiteAsyncConnection>(() => new SQLiteAsyncConnection(path, flags));
        private static readonly SQLiteAsyncConnection connectionInitializer = connectionHolder.Value;

        /// <summary>
        /// Return a new connection to a database with tables of T created and ready.
        /// </summary>
        protected async ValueTask<SQLiteAsyncConnection> GetConnection<T>()
        {
            if (!connectionInitializer.TableMappings.Any(x => x.MappedType == typeof(T)))
            {
                await connectionInitializer.EnableWriteAheadLoggingAsync().ConfigureAwait(false);
                await connectionInitializer.CreateTablesAsync(CreateFlags.None, typeof(T)).ConfigureAwait(false);
            }

            return connectionInitializer;
        }

        protected async ValueTask<T> AttemptAndRetry<T>(Func<Task<T>> action, int maxNumOfRetries = 5)
        {
            return await Policy.Handle<SQLiteException>().WaitAndRetryAsync(maxNumOfRetries, RetryAttempter).ExecuteAsync(action);
            TimeSpan RetryAttempter(int attemptNumber) => TimeSpan.FromMilliseconds(Math.Pow(2, attemptNumber));
        }
    }
}