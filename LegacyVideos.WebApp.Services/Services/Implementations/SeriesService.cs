using LegacyVideos.Domain.Models;
using LegacyVideos.Domain.Requests;
using LegacyVideos.WebApp.Services.Services.Interfaces;
using log4net;
using RestSharp;

namespace LegacyVideos.WebApp.Services.Services.Implementations
{
    public class SeriesService : ISeriesService
    {
        private readonly RestClient _restSharpClient;
        private readonly ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        public SeriesService(RestClient restSharpClient)
        {
            _restSharpClient = restSharpClient;
        }

        /// <summary>
        /// Add series.
        /// </summary>
        /// <param name="addSeriesRequest">The <see cref="Series"/> to create.</param>
        /// <returns>Return the series id</returns>
        public async Task<int> AddSeries(AddSeriesRequest addSeriesRequest)
        {
            _logger.Debug($"Call api to add series {addSeriesRequest.Title}.");

            var request = new RestRequest("api/series");
            request.AddJsonBody(addSeriesRequest);
            var response = await _restSharpClient.ExecutePostAsync<int>(request);

            _logger.Debug($"Completed calling api to add series {addSeriesRequest.Title}.");

            return response.Data;
        }

        /// <summary>
        /// Get all series.
        /// </summary>
        /// <returns>Returns all <see cref="series"/>s stored in the database</returns>
        public async Task<IList<Series>?> GetAllSeries()
        {
            _logger.Debug("Call api to get all series.");

            var request = new RestRequest("api/series");
            var response = await _restSharpClient.ExecuteGetAsync<IList<Series>?>(request);

            _logger.Debug("Completed call api to get all series.");

            return response.Data;
        }
    }
}
