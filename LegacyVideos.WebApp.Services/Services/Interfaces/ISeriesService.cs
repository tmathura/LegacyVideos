using LegacyVideos.Domain.Models;
using LegacyVideos.Domain.Requests;

namespace LegacyVideos.WebApp.Services.Services.Interfaces
{
    public interface ISeriesService
    {
        /// <summary>
        /// Add series.
        /// </summary>
        /// <param name="addSeriesRequest">The <see cref="Series"/> to create.</param>
        /// <returns></returns>
        Task<int> AddSeries(AddSeriesRequest addSeriesRequest);
        
        /// <summary>
        /// Get all series.
        /// </summary>
        /// <returns>Returns all <see cref="Series"/>s stored in the database</returns>
        Task<IList<Series>?> GetAllSeries();
    }
}