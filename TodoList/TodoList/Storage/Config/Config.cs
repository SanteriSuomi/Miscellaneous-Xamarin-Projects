using Newtonsoft.Json;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using TodoList.Extensions;

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

            public Color ThemeBarColor { get; set; }
            public Color ThemeBackgroundColor { get; set; }
        }

        static Config()
        {
            ReadConfigFile().SafeFireAndForget(false);
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

        #pragma warning disable S3966 // Objects should not be disposed more than once (Justification: objects are only disposed once)
        /// <summary>
        /// Read the config file and update the config file object.
        /// </summary>
        public static async Task ReadConfigFile()
        {
            using (Stream stream = GetLocalAssembly())
            using (StreamReader reader = new StreamReader(stream))
            {
                string result = await reader.ReadToEndAsync();
                ST = JsonConvert.DeserializeObject<ConfigSettings>(result);
            }
        }

        /// <summary>
        /// Update the config file on the disk.
        /// </summary>
        public static async Task WriteConfigFile()
        {
            using (Stream stream = GetLocalAssembly())
            using (StreamWriter writer = new StreamWriter(stream))
            {
                string objAsJson = JsonConvert.SerializeObject(ST, Formatting.Indented);
                await writer.WriteAsync(objAsJson);
            }
        }

        private static Stream GetLocalAssembly() 
            => Assembly.GetExecutingAssembly().GetManifestResourceStream(configName);
    }
}