using Bogus;
using Newtonsoft.Json;
using NUnit.Framework;
using ProjectWeather.Api;
using ProjectWeather.TestsIntegration.Extensions;
using ProjectWeather.TestsIntegration.Utils;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ProjectWeather.TestsIntegration
{
    [TestFixture]
    public class WeatherForecastApiTests : BaseTestApi
    {
        private const string _baseRequestUri = "api/WeatherForecast";
        private Faker _faker;

        [SetUp]
        public async Task Setup()
        {
            _faker = new Faker("pt_BR");

            await Auth();
            _httpClient.SetToken(UserToken);
        }

        [Test]
        public async Task GetWeatherForecast_Successful()
        {
            // Act
            var getResponse = await _httpClient.GetAsync(_baseRequestUri);

            // Assertion            
            getResponse.EnsureSuccessStatusCode();
        }

        [Test]
        public async Task PostWeatherForecast_Successful()
        {
            // Arrange            
            var weatherForecast = new WeatherForecast
            {
                Date = _faker.Date.Past(),
                Summary = _faker.Commerce.ProductName(),
                TemperatureC = _faker.Random.Int()
            };
            var serialized = JsonConvert.SerializeObject(weatherForecast);

            var content = new StringContent(serialized);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var postRequest = new HttpRequestMessage(HttpMethod.Post, _baseRequestUri)
            {
                Content = content
            };

            // Act
            var postResponse = await _httpClient.SendAsync(postRequest);

            // Assertion
            postResponse.EnsureSuccessStatusCode();
        }
    }
}