using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;

namespace ProjectWeather.TestsIntegration.Extensions
{
    public static class TestsExtensions
    {
        public static void SetBearerToken(this HttpClient client, string token)
        {
            client.SetJsonMediaType();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public static void SetJsonMediaType(this HttpClient client)
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
        }
    }
}
