using LegacyVideos.Domain.Models;
using LegacyVideos.Domain.Requests;
using LegacyVideos.WebApp.Services.Services.Interfaces;
using log4net;
using RestSharp;

namespace LegacyVideos.WebApp.Services.Services.Implementations
{
    public class MoviesService : IMoviesService
    {
        private readonly RestClient _restSharpClient;
        private readonly ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        public MoviesService(RestClient restSharpClient)
        {
            _restSharpClient = restSharpClient;
        }

        /// <summary>
        /// Add movie.
        /// </summary>
        /// <param name="addMovieRequest">The <see cref="Movie"/> to create.</param>
        /// <returns>Return the movie id</returns>
        public async Task<int> AddMovie(AddMovieRequest addMovieRequest)
        {
            _logger.Debug($"Call api to add movie {addMovieRequest.Title}.");

            var request = new RestRequest("api/movies");
            request.AddJsonBody(addMovieRequest);
            var response = await _restSharpClient.ExecutePostAsync<int>(request);

            _logger.Debug($"Completed calling api to add movie {addMovieRequest.Title}.");

            return response.Data;
        }

        /// <summary>
        /// Get all movies.
        /// </summary>
        /// <returns>Returns all <see cref="Movie"/>s stored in the database</returns>
        public async Task<IList<Movie>?> GetAllMovies()
        {
            _logger.Debug("Call api to get all movies.");

            var request = new RestRequest("api/movies");
            var response = await _restSharpClient.ExecuteGetAsync<IList<Movie>?>(request);

            _logger.Debug("Completed call api to get all movies.");

            return response.Data;
        }

        /// <summary>
        /// Get movie by id.
        /// </summary>
        /// <param name="id">The <see cref="Movie"/> id.</param>
        /// <returns>Returns a <see cref="Movie"/></returns>
        public async Task<Movie?> GetMovieById(int id)
        {
            _logger.Debug($"Call api to get movie by id: {id}.");

            var request = new RestRequest($"api/movies/{id}");
            var response = await _restSharpClient.ExecuteGetAsync<Movie?>(request);

            _logger.Debug($"Completed call api to get movie by id: {id}.");

            return response.Data;
        }

        /// <summary>
        /// Update movie.
        /// </summary>
        /// <param name="updateMovieRequest">The <see cref="Movie"/> to update.</param>
        public async Task UpdateMovie(UpdateMovieRequest updateMovieRequest)
        {
            _logger.Debug($"Call api to update movie {updateMovieRequest.Title}.");

            var request = new RestRequest("api/movies");
            request.AddJsonBody(updateMovieRequest);
            await _restSharpClient.ExecutePutAsync(request);

            _logger.Debug($"Completed call api to update movie {updateMovieRequest.Title}.");
        }

        /// <summary>
        /// Delete movie.
        /// </summary>
        /// <param name="id"><see cref="Movie"/> id to delete.</param>
        public async Task DeleteMovie(int id)
        {
            _logger.Debug($"Call api to delete movie {id}.");

            var request = new RestRequest($"api/movies?id={id}");
            await _restSharpClient.DeleteAsync(request);

            _logger.Debug($"Completed call api to delete movie {id}.");
        }
    }
}
