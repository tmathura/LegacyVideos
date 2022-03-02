using LegacyVideos.Domain.Models;
using LegacyVideos.Domain.Requests;
using LegacyVideos.WebApp.Services.Services.Interfaces;
using RestSharp;

namespace LegacyVideos.WebApp.Services.Services.Implementations
{
    public class MoviesService : IMoviesService
    {
        private readonly RestClient _restSharpClient;

        public MoviesService(RestClient restSharpClient)
        {
            _restSharpClient = restSharpClient;
        }

        /// <summary>
        /// Get all movies.
        /// </summary>
        /// <returns>Returns all movies stored in the database</returns>
        public async Task<IList<Movie>?> GetMovies()
        {
            var request = new RestRequest("api/movies");
            var response = await _restSharpClient.ExecuteGetAsync<IList<Movie>?>(request);
            
            return response.Data;
        }

        /// <summary>
        /// Add movie.
        /// </summary>
        /// <param name="addMovieRequest">The <see cref="Movie"/> to create.</param>
        /// <returns></returns>
        public async Task<int> AddMovie(AddMovieRequest addMovieRequest)
        {
            var request = new RestRequest("api/movies");
            request.AddJsonBody(addMovieRequest);
            var response = await _restSharpClient.ExecutePostAsync<int>(request);

            return response.Data;
        }
    }
}
