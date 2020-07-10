using SQLite;
using System;

namespace MovieManager.Database
{
    public static class DatabaseConstants
    {
        public const string Filename = "movies.db";

        public const SQLiteOpenFlags Flags =
                     SQLiteOpenFlags.Create
                     | SQLiteOpenFlags.ReadWrite
                     | SQLiteOpenFlags.SharedCache;

        public static string Path
        {
            get
            {
                var localFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                return System.IO.Path.Combine(localFolder, Filename);
            }
        }
    }
}