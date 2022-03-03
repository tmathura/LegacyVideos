using LegacyVideos.WebApp.Services.Services.Implementations;
using LegacyVideos.WebApp.Services.Services.Interfaces;
using RestSharp;

namespace LegacyVideos.WebApp.Services
{
    public class WebAppClient : IWebAppClient
    {
        /// <summary>
        /// Movies service
        /// </summary>
        public IMoviesService Movies { get; }

        /// <summary>
        /// Movies service
        /// </summary>
        public ISeriesService Series { get; }

        public WebAppClient(RestClient restSharpClient)
        {
            Movies = new MoviesService(restSharpClient);
            Series = new SeriesService(restSharpClient);
        }
    }
}
