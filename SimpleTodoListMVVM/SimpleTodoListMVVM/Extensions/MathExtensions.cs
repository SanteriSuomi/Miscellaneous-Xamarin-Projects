using System;

namespace SimpleTodoListMVVM.Extensions
{
    public static class MathExtensions
    {
        public static T Clamp<T>(this T value, T min, T max) where T: IComparable<T>
        {
            if (value.CompareTo(min) < 0)
            {
                return min;
            }
            else if (value.CompareTo(max) > 0)
            {
                return max;
            }

            return value;
        }
    }
}