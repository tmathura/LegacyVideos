using LegacyVideos.Domain.Models;
using LegacyVideos.Domain.Requests;
using LegacyVideos.WebApp.Models;
using LegacyVideos.WebApp.Models.Series;
using LegacyVideos.WebApp.Services;
using log4net;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LegacyVideos.WebApp.Controllers
{
    public class SeriesController : Controller
    {
        private readonly IWebAppClient _webAppClient;
        private readonly ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        public SeriesController(IWebAppClient webAppClient)
        {
            _webAppClient = webAppClient;
        }

        public async Task<IActionResult> Index()
        {
            _logger.Debug("Navigate to Index");

            var model = new IndexModel();
            var series = await _webAppClient.Series.GetAllSeries();
            model.Series = series;

            return View(model);
        }

        public IActionResult AddSeries()
        {
            _logger.Debug("Navigate to AddSeries");

            var model = new AddSeriesModel
            {
                Series = new Series()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> AddSeriesSubmit(AddSeriesModel model)
        {
            _logger.Debug("Doing add series submit");

            var addSeriesRequest = new AddSeriesRequest
            {
                Title = model.Series.Title,
                Description = model.Series.Description,
                ReleaseDate = model.Series.ReleaseDate,
                AddedDate = model.Series.AddedDate,
                Ended = model.Series.Owned,
                Owned = model.Series.Owned,
                Anime = model.Series.Owned
            };

            var id = await _webAppClient.Series.AddSeries(addSeriesRequest);

            _logger.Debug("Navigate to AddedSeriesSuccess");

            return View("AddedSeriesSuccess");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}