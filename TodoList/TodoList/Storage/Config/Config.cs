using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;

namespace TodoList.Storage.Config
{
    public static class Config
    {
        public static ConfigSettings Settings { get; }

        public class ConfigSettings
        {
            public string DatabaseName { get; set; }
        }

        static Config()
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
                    Settings = JsonConvert.DeserializeObject<ConfigSettings>(result);
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