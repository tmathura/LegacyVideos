using LegacyVideos.Core.Interfaces;
using LegacyVideos.Domain.Models;
using LegacyVideos.Domain.Requests;
using LegacyVideos.Domain.Responses;
using LegacyVideos.WebApi.Filters;
using log4net;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace LegacyVideos.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieBl _movieBl;
        private readonly ILog _logger = LogManager.GetLogger(typeof(MoviesController));

        public MoviesController(IMovieBl movieBl)
        {
            _movieBl = movieBl;
        }

        /// <summary>
        /// Add movie.
        /// </summary>
        /// <param name="movieRequest">The <see cref="Movie"/> to create.</param>
        /// <returns>Movie Id</returns>
        /// <exception cref="HttpResponseException"></exception>
        [HttpPost]
        [Route("")]
        public async Task<int> AddMovie(AddMovieRequest movieRequest)
        {
            try
            {
                _logger.Debug($"Start adding movie {movieRequest.Title}.");

                var movie = new Movie
                {
                    Title = movieRequest.Title,
                    Description = movieRequest.Description,
                    MovieType = movieRequest.MovieType,
                    Duration = movieRequest.Duration,
                    ReleaseDate = movieRequest.ReleaseDate,
                    AddedDate = movieRequest.AddedDate,
                    Owned = movieRequest.Owned
                };

                var id = await _movieBl.AddMovie(movie);

                _logger.Info($"Completed adding movie for movie {movieRequest.Title} with movie id: {id}.");

                return id;
            }
            catch (Exception exception)
            {
                throw new HttpResponseException((int)HttpStatusCode.InternalServerError, exception.Message);
            }
        }

        /// <summary>
        /// Get movie by id.
        /// </summary>
        /// <param name="id">The <see cref="Movie"/> id.</param>
        /// <returns><see cref="Movie"/></returns>
        /// <exception cref="HttpResponseException"></exception>
        [HttpGet]
        [Route("{id}")]
        public async Task<GetMovieByIdResponse> GetMovieById(int id)
        {
            try
            {
                _logger.Debug($"Start getting movie by id: {id}.");

                GetMovieByIdResponse? getMovieByIdResponse = null;

                var movie = await _movieBl.GetMovieById(id);

                if (movie != null)
                {
                    getMovieByIdResponse = new GetMovieByIdResponse
                    {
                        Id = movie.Id,
                        Title = movie.Title,
                        Description = movie.Description,
                        MovieType = movie.MovieType,
                        Duration = movie.Duration,
                        ReleaseDate = movie.ReleaseDate,
                        AddedDate = movie.AddedDate,
                        Owned = movie.Owned
                    };
                }

                _logger.Info($"Completed getting movie by id: {id}.");

                return getMovieByIdResponse;
            }
            catch (Exception exception)
            {
                throw new HttpResponseException((int)HttpStatusCode.InternalServerError, exception.Message);
            }
        }

        /// <summary>
        /// Get movies by title.
        /// </summary>
        /// <param name="title">The title to search by.</param>
        /// <returns>List of <see cref="Movie"/>s</returns>
        /// <exception cref="HttpResponseException"></exception>
        [HttpGet]
        [Route("getmoviesbytitle")]
        public async Task<List<GetMovieByTitleResponse>> GetMoviesByTitle(string title)
        {
            try
            {
                _logger.Debug($"Start getting movies by title {title}.");

                List<GetMovieByTitleResponse>? getMovieByTitleResponse = null;

                var movies = await _movieBl.GetMoviesByTitle(title);

                if (movies != null && movies.Count > 0)
                {
                    getMovieByTitleResponse = movies.Select(movie => new GetMovieByTitleResponse
                    {
                        Id = movie.Id,
                        Title = movie.Title,
                        Description = movie.Description,
                        MovieType = movie.MovieType,
                        Duration = movie.Duration,
                        ReleaseDate = movie.ReleaseDate,
                        AddedDate = movie.AddedDate,
                        Owned = movie.Owned
                    }).ToList();
                }

                _logger.Info($"Completed getting movies by title {title}.");

                return getMovieByTitleResponse;
            }
            catch (Exception exception)
            {
                throw new HttpResponseException((int)HttpStatusCode.InternalServerError, exception.Message);
            }
        }

        /// <summary>
        /// Get movies by owned.
        /// </summary>
        /// <param name="owned">The owned bool to search by.</param>
        /// <returns>List of <see cref="Movie"/>s</returns>
        /// <exception cref="HttpResponseException"></exception>
        [HttpGet]
        [Route("getmoviesbyowned")]
        public async Task<List<GetMovieByOwnedResponse>> GetMoviesByOwned(bool owned)
        {
            try
            {
                _logger.Debug($"Start getting movies by owned {owned}.");

                List<GetMovieByOwnedResponse>? getMovieByOwnedResponse = null;

                var movies = await _movieBl.GetMoviesByOwned(owned);

                if (movies != null && movies.Count > 0)
                {
                    getMovieByOwnedResponse = movies.Select(movie => new GetMovieByOwnedResponse
                    {
                        Id = movie.Id,
                        Title = movie.Title,
                        Description = movie.Description,
                        MovieType = movie.MovieType,
                        Duration = movie.Duration,
                        ReleaseDate = movie.ReleaseDate,
                        AddedDate = movie.AddedDate,
                        Owned = movie.Owned
                    }).ToList();
                }

                _logger.Info($"Completed getting movies by owned {owned}.");

                return getMovieByOwnedResponse;
            }
            catch (Exception exception)
            {
                throw new HttpResponseException((int)HttpStatusCode.InternalServerError, exception.Message);
            }
        }

        /// <summary>
        /// Get movies by release date.
        /// </summary>
        /// <param name="fromDate">The from date to search by.</param>
        /// <param name="toDate">The to date to search by.</param>
        /// <returns>List of <see cref="Movie"/>s</returns>
        /// <exception cref="HttpResponseException"></exception>
        [HttpGet]
        [Route("getmoviesbyreleasedate")]
        public async Task<List<GetMoviesByReleaseDateResponse>> GetMoviesByReleaseDate(DateTime fromDate, DateTime toDate)
        {
            try
            {
                _logger.Debug($"Start getting movies by from date: {fromDate} and to date {toDate}.");

                List<GetMoviesByReleaseDateResponse>? getMoviesByReleaseDateResponse = null;

                var movies = await _movieBl.GetMoviesByReleaseDate(fromDate, toDate);

                if (movies != null && movies.Count > 0)
                {
                    getMoviesByReleaseDateResponse = movies.Select(movie => new GetMoviesByReleaseDateResponse
                    {
                        Id = movie.Id,
                        Title = movie.Title,
                        Description = movie.Description,
                        MovieType = movie.MovieType,
                        Duration = movie.Duration,
                        ReleaseDate = movie.ReleaseDate,
                        AddedDate = movie.AddedDate,
                        Owned = movie.Owned
                    }).ToList();
                }

                _logger.Info($"Completed getting movies by from date: {fromDate} and to date {toDate}.");

                return getMoviesByReleaseDateResponse;
            }
            catch (Exception exception)
            {
                throw new HttpResponseException((int)HttpStatusCode.InternalServerError, exception.Message);
            }
        }
    }
}