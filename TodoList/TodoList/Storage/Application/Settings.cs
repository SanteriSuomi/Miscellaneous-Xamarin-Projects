using System;
using Xamarin.Forms;

namespace TodoList.Storage.App
{
    public enum Setting
    {
        BackgroundColor
    }

    public static class Settings
    {
        /// <summary>
        /// Set application setting.
        /// </summary>
        /// <param name="setting">The setting whose value to set.</param>
        /// <param name="value">The value that will be used to set the setting.</param>
        public static void Set(Setting setting, object value)
        {
            var settingKey = setting.ToString();
            Application.Current.Properties[settingKey] = value;
        }

        /// <summary>
        /// Get application setting as the given type.
        /// </summary>
        /// <typeparam name="T">The type to return.</typeparam>
        /// <param name="setting">The setting whose value to set.</param>
        /// <returns>The value of the setting.</returns>
        public static T Get<T>(Setting setting)
        {
            try
            {
                var settingKey = setting.ToString();
                if (Application.Current.Properties.TryGetValue(settingKey, out object settingObj))
                {
                    return (T)settingObj;
                }

                return default;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return default;
            }
        }
    }
}