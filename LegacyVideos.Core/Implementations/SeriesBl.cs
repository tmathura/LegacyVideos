using LegacyVideos.Core.Interfaces;
using LegacyVideos.Domain.Models;
using LegacyVideos.Infrastructure.Interfaces;
using log4net;

namespace LegacyVideos.Core.Implementations
{
    /// <summary>
    /// Series business access logic
    /// </summary>
    /// <seealso cref="ISeriesBl" />
    public class SeriesBl : ISeriesBl
    {
        private readonly ISeriesDal _seriesDal;
        private readonly ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        public SeriesBl(ISeriesDal seriesDal)
        {
            _seriesDal = seriesDal;
        }

        /// <summary>
        /// Add series.
        /// </summary>
        /// <param name="series"><see cref="Series"/> to add.</param>
        public async Task<int> AddSeries(Series series)
        {
            try
            {
                _logger.Debug($"Adding series {series.Title} into database.");

                var id = await _seriesDal.AddSeries(series);

                _logger.Debug($"Series added with id: {id}.");

                return id;
            }
            catch (Exception exception)
            {
                _logger.Error($"{exception.Message} - {exception.StackTrace}");
                throw;
            }
        }

        /// <summary>
        /// Get all series.
        /// </summary>
        /// <returns><see cref="Series"/>s</returns>
        public async Task<List<Series>> GetAllSeries()
        {
            try
            {
                _logger.Debug("Getting all series.");

                var series = await _seriesDal.GetAllSeries();

                _logger.Debug("Completed getting all series.");

                return series;
            }
            catch (Exception exception)
            {
                _logger.Error($"{exception.Message} - {exception.StackTrace}");
                throw;
            }
        }
    }
}
