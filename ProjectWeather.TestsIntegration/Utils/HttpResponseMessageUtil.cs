using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace ProjectWeather.TestsIntegration.Utils
{
    public static class HttpResponseMessageUtil
    {
        public static async Task<T> DeserializeObject<T>(this HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
