using LegacyVideos.Domain.Models;
using LegacyVideos.Domain.Requests;

namespace LegacyVideos.WebApp.Services.Services.Interfaces
{
    public interface IMoviesService
    {
        /// <summary>
        /// Get all movies.
        /// </summary>
        /// <returns>Returns all movies stored in the database</returns>
        Task<IList<Movie>?> GetMovies();

        /// <summary>
        /// Add movie.
        /// </summary>
        /// <param name="addMovieRequest">The <see cref="Movie"/> to create.</param>
        /// <returns></returns>
        Task<int> AddMovie(AddMovieRequest addMovieRequest);
    }
}