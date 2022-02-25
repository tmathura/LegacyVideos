using LegacyVideos.Infrastructure.IntegrationTests.Common.AppSettings;
using Microsoft.Extensions.Configuration;

namespace LegacyVideos.Infrastructure.IntegrationTests.Common.Helpers
{
    public class CommonHelper
    {
        public readonly Settings Settings;

        public CommonHelper()
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            Settings = new Settings();
            configuration.Bind(Settings);
        }
    }
}
