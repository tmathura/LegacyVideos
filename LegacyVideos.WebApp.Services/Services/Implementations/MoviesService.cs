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
        /// Add movie.
        /// </summary>
        /// <param name="addMovieRequest">The <see cref="Movie"/> to create.</param>
        /// <returns>Return the movie id</returns>
        public async Task<int> AddMovie(AddMovieRequest addMovieRequest)
        {
            var request = new RestRequest("api/movies");
            request.AddJsonBody(addMovieRequest);
            var response = await _restSharpClient.ExecutePostAsync<int>(request);

            return response.Data;
        }

        /// <summary>
        /// Get all movies.
        /// </summary>
        /// <returns>Returns all <see cref="Movie"/>s stored in the database</returns>
        public async Task<IList<Movie>?> GetAllMovies()
        {
            var request = new RestRequest("api/movies");
            var response = await _restSharpClient.ExecuteGetAsync<IList<Movie>?>(request);

            return response.Data;
        }

        /// <summary>
        /// Get movie by id.
        /// </summary>
        /// <param name="id">The <see cref="Movie"/> id.</param>
        /// <returns>Returns a <see cref="Movie"/></returns>
        public async Task<Movie?> GetMovieById(int id)
        {
            var request = new RestRequest($"api/movies/{id}");
            var response = await _restSharpClient.ExecuteGetAsync<Movie?>(request);

            return response.Data;
        }

        /// <summary>
        /// Update movie.
        /// </summary>
        /// <param name="updateMovieRequest">The <see cref="Movie"/> to update.</param>
        public async Task UpdateMovie(UpdateMovieRequest updateMovieRequest)
        {
            var request = new RestRequest("api/movies");
            request.AddJsonBody(updateMovieRequest);
            var response = await _restSharpClient.ExecutePutAsync<int>(request);
        }

        /// <summary>
        /// Delete movie.
        /// </summary>
        /// <param name="id"><see cref="Movie"/> id to delete.</param>
        public async Task DeleteMovie(int id)
        {
            var request = new RestRequest($"api/movies?id={id}");
            var response = await _restSharpClient.DeleteAsync(request);
        }
    }
}
