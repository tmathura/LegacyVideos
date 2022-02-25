using LegacyVideos.Core.Interfaces;
using LegacyVideos.Domain.Models;
using LegacyVideos.Infrastructure.Interfaces;
using log4net;

namespace LegacyVideos.Core.Implementations
{
    /// <summary>
    /// Movie business access logic
    /// </summary>
    /// <seealso cref="IMovieBl" />
    public class MovieBl : IMovieBl
    {
        private readonly IMovieDal _movieDal;
        private readonly ILog _logger = LogManager.GetLogger(typeof(MovieBl));

        public MovieBl(IMovieDal movieDal)
        {
            _movieDal = movieDal;
        }

        /// <summary>
        /// Add movie.
        /// </summary>
        /// <param name="movie"><see cref="Movie"/> to add.</param>
        public async Task<int> AddMovie(Movie movie)
        {
            try
            {
                _logger.Debug($"Adding movie {movie.Title} into database.");

                var id = await _movieDal.AddMovie(movie);

                _logger.Debug($"Movie added with id: {id}.");

                return id;
            }
            catch (Exception exception)
            {
                _logger.Error($"{exception.Message} - {exception.StackTrace}");
                throw;
            }
        }

        /// <summary>
        /// Get movie by id.
        /// </summary>
        /// <param name="id">Id of movie to lookup.</param>
        /// <returns><see cref="Movie"/></returns>
        public async Task<Movie> GetMovieById(int id)
        {
            try
            {
                _logger.Debug($"Getting movie by movie id: {id}.");

                var movie = await _movieDal.GetMovieById(id);

                _logger.Debug($"Completed getting movie by movie id: {id}.");

                return movie;
            }
            catch (Exception exception)
            {
                _logger.Error($"{exception.Message} - {exception.StackTrace}");
                throw;
            }
        }


        /// <summary>
        /// Get movies by title.
        /// </summary>
        /// <param name="title">Title of movies to lookup.</param>
        /// <returns><see cref="Movie"/>s</returns>
        public async Task<List<Movie>> GetMovieByTitle(string title)
        {
            try
            {
                _logger.Debug($"Getting movies by title: {title}.");

                var movies = await _movieDal.GetMovieByTitle(title);

                _logger.Debug($"Completed getting movies by title: {title}.");

                return movies;
            }
            catch (Exception exception)
            {
                _logger.Error($"{exception.Message} - {exception.StackTrace}");
                throw;
            }
        }

        /// <summary>
        /// Get movies that is owned or not.
        /// </summary>
        /// <param name="owned">Indicator to lookup owned movies.</param>
        /// <returns><see cref="Movie"/>s</returns>
        public async Task<List<Movie>> GetMovieByOwned(bool owned)
        {
            try
            {
                _logger.Debug($"Getting movies by owned: {owned}.");

                var movies = await _movieDal.GetMovieByOwned(owned);

                _logger.Debug($"Completed getting movies by owned: {owned}.");

                return movies;
            }
            catch (Exception exception)
            {
                _logger.Error($"{exception.Message} - {exception.StackTrace}");
                throw;
            }
        }

        /// <summary>
        /// Get movies by release date.
        /// </summary>
        /// <param name="fromDate">The from date to lookup movies from.</param>
        /// <param name="toDate">The to date to lookup movies to.</param>
        /// <returns><see cref="Movie"/>s</returns>
        public async Task<List<Movie>> GetMovieByReleaseDate(DateTime fromDate, DateTime toDate)
        {
            try
            {
                _logger.Debug($"Getting movies by from date: {fromDate} and to date {toDate}.");

                var movies = await _movieDal.GetMovieByReleaseDate(fromDate, toDate);

                _logger.Debug($"Completed getting movies by from date: {fromDate} and to date {toDate}.");

                return movies;
            }
            catch (Exception exception)
            {
                _logger.Error($"{exception.Message} - {exception.StackTrace}");
                throw;
            }
        }

        /// <summary>
        /// Update movie.
        /// </summary>
        /// <param name="movie"><see cref="Movie"/> to update.</param>
        public async Task UpdateMovie(Movie movie)
        {
            try
            {
                _logger.Debug($"Start updating movie with id: {movie.Id}.");

                await _movieDal.UpdateMovie(movie);

                _logger.Debug($"Completed updating movie with id {movie.Id}.");
            }
            catch (Exception exception)
            {
                _logger.Error($"{exception.Message} - {exception.StackTrace}");
                throw;
            }
        }

        /// <summary>
        /// Delete movie.
        /// </summary>
        /// <param name="id"><see cref="Movie"/> id to delete.</param>
        public async Task DeleteMovie(int id)
        {
            try
            {
                _logger.Debug($"Start deleting movie with id: {id}.");

                await _movieDal.DeleteMovie(id);

                _logger.Debug($"Completed deleting movie with id {id}.");
            }
            catch (Exception exception)
            {
                _logger.Error($"{exception.Message} - {exception.StackTrace}");
                throw;
            }
        }
    }
}
