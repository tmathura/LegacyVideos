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
    public class SeriesController : ControllerBase
    {
        private readonly ISeriesBl _seriesBl;
        private readonly ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        public SeriesController(ISeriesBl seriesBl)
        {
            _seriesBl = seriesBl;
        }

        /// <summary>
        /// Add series.
        /// </summary>
        /// <param name="addSeriesRequest">The <see cref="Series"/> to create.</param>
        /// <returns>series Id</returns>
        /// <exception cref="HttpResponseException"></exception>
        [HttpPost]
        [Route("")]
        public async Task<int> AddSeries(AddSeriesRequest addSeriesRequest)
        {
            try
            {
                _logger.Debug($"Start adding series {addSeriesRequest.Title}.");

                var series = new Series
                {
                    Title = addSeriesRequest.Title,
                    Description = addSeriesRequest.Description,
                    ReleaseDate = addSeriesRequest.ReleaseDate,
                    AddedDate = addSeriesRequest.AddedDate,
                    Ended = addSeriesRequest.Ended,
                    Owned = addSeriesRequest.Owned,
                    Anime = addSeriesRequest.Anime
                };

                var id = await _seriesBl.AddSeries(series);

                _logger.Info($"Completed adding series for series {addSeriesRequest.Title} with series id: {id}.");

                return id;
            }
            catch (Exception exception)
            {
                throw new HttpResponseException((int)HttpStatusCode.InternalServerError, exception.Message);
            }
        }

        /// <summary>
        /// Get all series.
        /// </summary>
        /// <returns>List of <see cref="Series"/>s</returns>
        /// <exception cref="HttpResponseException"></exception>
        [HttpGet]
        [Route("")]
        public async Task<List<GetAllSeriesResponse>> GetAllSeries()
        {
            try
            {
                _logger.Debug("Start getting all series.");

                List<GetAllSeriesResponse>? getAllSeriesResponse = null;

                var series = await _seriesBl.GetAllSeries();

                if (series != null && series.Count > 0)
                {
                    getAllSeriesResponse = series.Select(series => new GetAllSeriesResponse
                    {
                        Id = series.Id,
                        Title = series.Title,
                        Description = series.Description,
                        ReleaseDate = series.ReleaseDate,
                        AddedDate = series.AddedDate,
                        Ended = series.Ended,
                        Owned = series.Owned,
                        Anime = series.Anime
                    }).ToList();
                }

                _logger.Info("Completed getting all series.");

                return getAllSeriesResponse;
            }
            catch (Exception exception)
            {
                throw new HttpResponseException((int)HttpStatusCode.InternalServerError, exception.Message);
            }
        }
    }
}