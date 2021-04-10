using Bogus;
using FluentAssertions;
using NUnit.Framework;
using ProjectWeather.Api.Models;
using ProjectWeather.TestsIntegration.Utils;
using System.Net.Http;
using System.Threading.Tasks;

namespace ProjectWeather.TestsIntegration.Specs
{
    [TestFixture]
    public class WeatherForecastApiTests : BaseTestApi
    {
        private const string _baseRequestUri = "api/WeatherForecast";
        private Faker _faker;
        private WeatherForecast _weatherForecast;

        [SetUp]
        public void Setup()
        {
            _faker = new Faker("pt_BR");
        }

        [Test]
        public async Task GetWeatherForecast_Successful()
        {
            // Act
            var getResponse = await _httpClient.GetAsync(_baseRequestUri);

            // Assertion            
            getResponse.EnsureSuccessStatusCode();
        }

        [Test, Order(1)]
        public async Task PostWeatherForecast_Successful()
        {
            // Arrange            
            var weatherForecast = new WeatherForecast
            {
                Date = _faker.Date.Past(),
                Summary = _faker.Commerce.ProductName(),
                TemperatureC = _faker.Random.Int()
            };
            var postRequest = HttpRequestMessageUtil.BuilderRequestMessage(HttpMethod.Post, _baseRequestUri, weatherForecast);

            // Act
            var postResponse = await _httpClient.SendAsync(postRequest);
            _weatherForecast = await postResponse.DeserializeObject<WeatherForecast>();

            // Assertion
            postResponse.EnsureSuccessStatusCode();
            VerifyWeatherForecast();
        }

        [Test, Order(2)]
        public async Task GetWeatherForecastById_Successful()
        {
            // Arrange            
            var requestUri = $"{_baseRequestUri}/{_weatherForecast.Id}";

            // Act
            var getResponse = await _httpClient.GetAsync(requestUri);
            _weatherForecast = await getResponse.DeserializeObject<WeatherForecast>();

            // Assertion
            getResponse.EnsureSuccessStatusCode();
            VerifyWeatherForecast();
        }

        private void VerifyWeatherForecast()
        {
            _weatherForecast.Should().NotBeNull();
            _weatherForecast.Should().BeOfType(typeof(WeatherForecast));
        }
    }
}