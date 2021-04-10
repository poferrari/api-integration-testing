using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;
using ProjectWeather.Api.Models;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ProjectWeather.TestsIntegration.Utils
{
    [TestFixture]
    public class BaseTestApi : BaseTestEnvironment
    {
        private const string _requestUriLogin = "api/login";
        protected HttpClient _httpClient;
        protected string UserToken { get; set; }

        [SetUp]
        public void SetUpHttpClient()
        {
            _httpClient = TestConfigurations.IsBaseUriConfigured ?
                new HttpClient
                {
                    BaseAddress = new Uri(TestConfigurations.BaseUri)
                } :
                Factory.CreateClient(new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = true,
                    HandleCookies = true,
                    MaxAutomaticRedirections = 7
                });
        }

        [TearDown]
        public void TearDown()
        {
            _httpClient.Dispose();
        }

        protected async Task Auth()
        {
            var login = TestConfigurations.Login;
            var password = TestConfigurations.Password;

            var tokenDto = await AuthLoginApi(login, password);
            if (tokenDto.Authenticated)
            {
                UserToken = tokenDto.AccessToken;
            }
        }

        private async Task<TokenDto> AuthLoginApi(string login, string password)
        {
            var userData = new LoginViewModel
            {
                Login = login,
                Password = password
            };

            var response = await _httpClient.PostAsJsonAsync(_requestUriLogin, userData);
            response.EnsureSuccessStatusCode();

            return await response.DeserializeObject<TokenDto>();
        }


    }
}
