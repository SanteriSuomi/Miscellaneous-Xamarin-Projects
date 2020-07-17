using System;

namespace TodoList.Utilities
{
    public static class RandomUtilities
    {
        private static readonly Random random = new Random();

        public static int GetRandomInt(int min, int max) => random.Next(min, max);
    }
}