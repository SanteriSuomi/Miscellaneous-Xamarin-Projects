using Newtonsoft.Json;
using System.IO;
using System.Reflection;

namespace TodoList.Storage.ConfigSettings
{
    public static class Config
    {
        private static readonly string configName = "TodoList.config.json";

        /// <summary>
        /// Returns the settings object containing all local settings saved in config.json.
        /// </summary>
        public static ConfigSettings ST { get; private set; }

        public class ConfigSettings
        {
            public string DatabaseName { get; set; }
            public string DefaultTodoIconPath { get; set; }
            public string[] AllowedTodoImageTypes { get; set; }
            public float SingleSelectRemoveDelay { get; set; }
            public int NotificationBodyLength { get; set; }
            public int NotificationMaxRandom { get; set; }
            public double MultiselectBarTranslationAmount { get; set; }
            public uint MultiselectBarTranslationDuration { get; set; }
            public string MultiselectBarEasingType { get; set; }
        }

        static Config()
        {
            ReadConfigFile();
        }

        /// <summary>
        /// Get the value of a field and attemp to return it as T.
        /// </summary>
        public static T GetFieldValue<T>(string fieldName)
        {
            var value = typeof(T).GetField(fieldName).GetValue(null);
            if (!(value is null))
            {
                return (T)value;
            }

            return default;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Critical Code Smell", "S3966:Objects should not be disposed more than once", Justification = "Objects are only disposed once.")]
        private static void ReadConfigFile()
        {
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(configName))
            using (StreamReader reader = new StreamReader(stream))
            {
                string result = reader.ReadToEnd();
                ST = JsonConvert.DeserializeObject<ConfigSettings>(result);
            }
        }
    }
}