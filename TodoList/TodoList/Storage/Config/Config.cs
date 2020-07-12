using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;

namespace TodoList.Storage.ConfigSettings
{
    public static class Config
    {
        /// <summary>
        /// Returns the settings object containing local settings.
        /// </summary>
        public static ConfigSettings ST { get; private set; }

        public class ConfigSettings
        {
            public string DatabaseName { get; set; }
        }

        static Config()
        {
            ReadConfigFile();
        }

        private static void ReadConfigFile()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "TodoList.config.json";
            Stream stream = null;
            try
            {
                stream = assembly.GetManifestResourceStream(resourceName);
                using (StreamReader reader = new StreamReader(stream))
                {
                    string result = reader.ReadToEnd();
                    ST = JsonConvert.DeserializeObject<ConfigSettings>(result);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                stream?.Dispose();
            }
        }
    }
}