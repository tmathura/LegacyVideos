using LegacyVideos.Domain.Models;
using LegacyVideos.Domain.Requests;
using LegacyVideos.WebApp.Models;
using LegacyVideos.WebApp.Models.Movies;
using LegacyVideos.WebApp.Services;
using log4net;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LegacyVideos.WebApp.Controllers
{
    public class MoviesController : Controller
    {
        private readonly IWebAppClient _webAppClient;
        private readonly ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        public MoviesController(IWebAppClient webAppClient)
        {
            _webAppClient = webAppClient;
        }

        public async Task<IActionResult> Index()
        {
            _logger.Debug("Navigate to Index");

            var model = new IndexModel();
            var movies = await _webAppClient.Movies.GetAllMovies();
            model.Movies = movies;

            return View(model);
        }

        public IActionResult AddMovie()
        {
            _logger.Debug("Navigate to AddMovie");

            var model = new AddMovieModel
            {
                Movie = new Movie()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> AddMovieSubmit(AddMovieModel model)
        {
            _logger.Debug("Doing add movie submit");

            var addMovieRequest = new AddMovieRequest
            {
                Title = model.Movie.Title,
                Description = model.Movie.Description,
                MovieType = model.Movie.MovieType,
                Duration = model.Movie.Duration,
                ReleaseDate = model.Movie.ReleaseDate,
                AddedDate = model.Movie.AddedDate,
                Owned = model.Movie.Owned
            };

            var id = await _webAppClient.Movies.AddMovie(addMovieRequest);

            _logger.Debug("Navigate to AddedMovieSuccess");

            return View("AddedMovieSuccess");
        }

        public async Task<IActionResult> UpdateMovie(int id)
        {
            _logger.Debug("Navigate to UpdateMovie");

            var model = new UpdateMovieModel();

            var movie = await _webAppClient.Movies.GetMovieById(id);
            model.Movie = movie;

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateMovieSubmit(AddMovieModel model)
        {
            _logger.Debug("Doing update movie submit");

            var updateMovieRequest = new UpdateMovieRequest
            {
                Id = model.Movie.Id,
                Title = model.Movie.Title,
                Description = model.Movie.Description,
                MovieType = model.Movie.MovieType,
                Duration = model.Movie.Duration,
                ReleaseDate = model.Movie.ReleaseDate,
                AddedDate = model.Movie.AddedDate,
                Owned = model.Movie.Owned
            };

            await _webAppClient.Movies.UpdateMovie(updateMovieRequest);

            _logger.Debug("Navigate to UpdatedMovieSuccess");

            return View("UpdatedMovieSuccess");
        }

        public async Task<IActionResult> DeleteMovie(int id)
        {
            _logger.Debug("Doing delete movie");

            await _webAppClient.Movies.DeleteMovie(id);

            _logger.Debug("Navigate to DeleteMovieSuccess");

            return View("DeleteMovieSuccess");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}