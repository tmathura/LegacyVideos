using LegacyVideos.WebApi.IntegrationTests.Common.AppSettings;
using Microsoft.Extensions.Configuration;
using RestSharp;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace LegacyVideos.WebApi.IntegrationTests.Common.Helpers
{
    public class CommonHelper
    {
        public ITestOutputHelper OutputHelper { get; set; }

        public readonly Settings Settings;
        private readonly RestClient _restClient;
        public DatabaseDriver Database { get; }

        public CommonHelper()
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            Settings = new Settings();
            configuration.Bind(Settings);

            _restClient = new RestClient(Settings.WebApi.ApiBaseUrl);
            Database = new DatabaseDriver(Settings.Database.ConnectionString);
        }

        public async Task<RestResponse> CallEndPoint(string endPoint, Method method, object requestBody)
        {
            var request = new RestRequest(endPoint, method);

            if (requestBody != null)
            {
                request.AddJsonBody(requestBody);
            }

            var response = await _restClient.ExecuteAsync(request);

            if (response.Content != null)
            {
                OutputHelper.WriteLine(response.Content);
            }

            return response;
        }
    }
}
