using LegacyVideos.Domain.Models;
using LegacyVideos.Domain.Requests;
using LegacyVideos.WebApp.Models;
using LegacyVideos.WebApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LegacyVideos.WebApp.Controllers
{
    public class MoviesController : Controller
    {
        private readonly ILogger<MoviesController> _logger;
        private readonly IWebAppClient _webAppClient;

        public MoviesController(ILogger<MoviesController> logger, IWebAppClient webAppClient)
        {
            _logger = logger;
            _webAppClient = webAppClient;
        }

        public async Task<IActionResult> Index()
        {
            var model = new IndexModel();
            var movies = await _webAppClient.Movies.GetMovies();
            model.Movies = movies;

            return View(model);
        }

        public IActionResult AddMovie()
        {
            var model = new AddMovieModel
            {
                Movie = new Movie()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> AddMovieSubmit(AddMovieModel model)
        {

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

            return View("AddedMoviedSuccess");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}