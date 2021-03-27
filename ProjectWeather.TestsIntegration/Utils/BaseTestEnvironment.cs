using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using ProjectWeather.Api;
using ProjectWeather.TestsIntegration.Configurations;
using System.IO;

namespace ProjectWeather.TestsIntegration.Utils
{
    [SetUpFixture]
    public class BaseTestEnvironment
    {
        public static WebApplicationFactory<Startup> Factory { get; private set; }
        public static TestConfigurations TestConfigurations { get; private set; }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            if (TestConfigurations is null)
            {
                IConfiguration configuration = BuildConfiguration();

                TestConfigurations = configuration.GetSection(nameof(TestConfigurations)).Get<TestConfigurations>();
            }

            if (Factory is null)
            {
                Factory = new WebApplicationFactory<Startup>()
                    .WithWebHostBuilder(builder =>
                    {
                        builder.UseEnvironment("Test");
                    });
            }
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            Factory.Dispose();
        }

        private static IConfiguration BuildConfiguration()
         => new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
    }
}