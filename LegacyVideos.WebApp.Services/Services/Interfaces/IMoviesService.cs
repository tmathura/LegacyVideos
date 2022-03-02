using LegacyVideos.Domain.Models;
using LegacyVideos.Domain.Requests;

namespace LegacyVideos.WebApp.Services.Services.Interfaces
{
    public interface IMoviesService
    {
        /// <summary>
        /// Add movie.
        /// </summary>
        /// <param name="addMovieRequest">The <see cref="Movie"/> to create.</param>
        /// <returns></returns>
        Task<int> AddMovie(AddMovieRequest addMovieRequest);
        
        /// <summary>
        /// Get all movies.
        /// </summary>
        /// <returns>Returns all <see cref="Movie"/>s stored in the database</returns>
        Task<IList<Movie>?> GetAllMovies();

        /// <summary>
        /// Get movie by id.
        /// </summary>
        /// <param name="id">The <see cref="Movie"/> id.</param>
        /// <returns>Returns a <see cref="Movie"/></returns>
        Task<Movie?> GetMovieById(int id);

        /// <summary>
        /// Update movie.
        /// </summary>
        /// <param name="updateMovieRequest">The <see cref="Movie"/> to update.</param>
        Task UpdateMovie(UpdateMovieRequest updateMovieRequest);

        /// <summary>
        /// Delete movie.
        /// </summary>
        /// <param name="id"><see cref="Movie"/> id to delete.</param>
        /// <returns></returns>
        Task DeleteMovie(int id);
    }
}