using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;

namespace ProjectWeather.TestsIntegration.Utils
{
    public static class HttpRequestMessageUtil
    {
        public static HttpRequestMessage BuilderRequestMessage(HttpMethod httpMethod, string requestUri, object document)
        {
            var serialized = JsonConvert.SerializeObject(document);

            var content = new StringContent(serialized);
            content.Headers.ContentType = new MediaTypeHeaderValue(MediaTypeNames.Application.Json);

            return new HttpRequestMessage(httpMethod, requestUri)
            {
                Content = content
            };
        }
    }
}
