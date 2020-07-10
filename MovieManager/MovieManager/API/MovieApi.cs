using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace MovieManager.Data
{
    public static class MovieApi
    {
        private static readonly HttpClient client = new HttpClient();
        private const string apiKey = "1f1c040";

        /// <summary>
        /// Get data from the OMD api.
        /// </summary>
        /// <typeparam name="T">The object to which the data will be converted to.</typeparam>
        /// <param name="request">String query. Authentication handled automatically.</param>
        /// <returns>Specified data converted to a specific object.</returns>
        public static async Task<T> Get<T>(string request)
        {
            var address = $"https://www.omdbapi.com/?apikey={apiKey}&{request}";
            var httpData = await client.GetAsync(address).ConfigureAwait(false);
            if (httpData.StatusCode == HttpStatusCode.OK)
            {
                var messageData = await httpData.Content.ReadAsStringAsync().ConfigureAwait(false);
                return JsonConvert.DeserializeObject<T>(messageData);
            }
            
            return default;
        }
    }
}