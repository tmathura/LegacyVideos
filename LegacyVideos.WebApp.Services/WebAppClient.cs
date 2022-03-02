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

        public WebAppClient(RestClient restSharpClient)
        {
            Movies = new MoviesService(restSharpClient);
        }
    }
}
