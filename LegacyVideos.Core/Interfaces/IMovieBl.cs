using LegacyVideos.Domain.Models;

namespace LegacyVideos.Core.Interfaces
{
    public interface IMovieBl
    {
        /// <summary>
        /// Add movie.
        /// </summary>
        /// <param name="movie"><see cref="Movie"/> to add.</param>
        Task<int> AddMovie(Movie movie);

        /// <summary>
        /// Get all movies.
        /// </summary>
        /// <returns><see cref="Movie"/>s</returns>
        Task<List<Movie>> GetAllMovies();

        /// <summary>
        /// Get movie by id.
        /// </summary>
        /// <param name="id">Id of movie to lookup.</param>
        /// <returns><see cref="Movie"/></returns>
        Task<Movie> GetMovieById(int id);

        /// <summary>
        /// Get movies by title.
        /// </summary>
        /// <param name="title">Title of movies to lookup.</param>
        /// <returns><see cref="Movie"/></returns>
        Task<List<Movie>> GetMoviesByTitle(string title);

        /// <summary>
        /// Get movies that is owned or not.
        /// </summary>
        /// <param name="owned">Indicator to lookup owned movies.</param>
        /// <returns><see cref="Movie"/></returns>
        Task<List<Movie>> GetMoviesByOwned(bool owned);

        /// <summary>
        /// Get movies by release date.
        /// </summary>
        /// <param name="fromDate">The from date to lookup movies from.</param>
        /// <param name="toDate">The to date to lookup movies to.</param>
        /// <returns><see cref="Movie"/></returns>
        Task<List<Movie>> GetMoviesByReleaseDate(DateTime fromDate, DateTime toDate);

        /// <summary>
        /// Update movie.
        /// </summary>
        /// <param name="movie"><see cref="Movie"/> to update.</param>
        Task UpdateMovie(Movie movie);

        /// <summary>
        /// Delete movie.
        /// </summary>
        /// <param name="id"><see cref="Movie"/> id to delete.</param>
        Task DeleteMovie(int id);
    }
}