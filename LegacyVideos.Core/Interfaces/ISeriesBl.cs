using LegacyVideos.Domain.Models;

namespace LegacyVideos.Core.Interfaces
{
    public interface ISeriesBl
    {
        /// <summary>
        /// Add series.
        /// </summary>
        /// <param name="series"><see cref="Series"/> to add.</param>
        Task<int> AddSeries(Series series);

        /// <summary>
        /// Get all series.
        /// </summary>
        /// <returns><see cref="Series"/>s</returns>
        Task<List<Series>> GetAllSeries();
    }
}