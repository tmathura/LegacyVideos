using LegacyVideos.WebApp.Services.Services.Interfaces;

namespace LegacyVideos.WebApp.Services;

public interface IWebAppClient
{
    /// <summary>
    /// Movies service
    /// </summary>
    IMoviesService Movies { get; }
}