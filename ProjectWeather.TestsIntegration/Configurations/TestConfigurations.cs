namespace ProjectWeather.TestsIntegration.Configurations
{
    public class TestConfigurations
    {
        public string BaseUri { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public bool IsBaseUriConfigured
        {
            get
            {
                return !string.IsNullOrWhiteSpace(BaseUri);
            }
        }
    }
}
