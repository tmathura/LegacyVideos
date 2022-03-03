using LegacyVideos.WebApp.Services.Services.Interfaces;

namespace LegacyVideos.WebApp.Services;

public interface IWebAppClient
{
    /// <summary>
    /// Movies service
    /// </summary>
    IMoviesService Movies { get; }

    /// <summary>
    /// Series service
    /// </summary>
    ISeriesService Series { get; }
}