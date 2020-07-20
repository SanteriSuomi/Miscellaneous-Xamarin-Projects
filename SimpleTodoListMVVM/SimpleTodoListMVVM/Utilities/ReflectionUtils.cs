using System.Reflection;

namespace SimpleTodoListMVVM.Utilities
{
    public static class ReflectionUtils
    {
        public static T GetObjectFieldValue<T>(string fieldName, BindingFlags flags = BindingFlags.Default)
        {
            var value = typeof(T).GetField(fieldName, flags).GetValue(null);
            if (!(value is null))
            {
                return (T)value;
            }

            return default;
        }
    }
}