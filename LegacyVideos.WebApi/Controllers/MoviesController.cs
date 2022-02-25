using LegacyVideos.Core.Interfaces;
using LegacyVideos.Domain.Models;
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

        [HttpPost]
        [Route("")]
        public async Task<int> AddMovie(MovieRequest movieRequest)
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
    }
}