using LegacyVideos.Domain.Models;
using System;
using System.Collections.Generic;

namespace LegacyVideos.Infrastructure.IntegrationTests.Factories
{
    public static class SeriesFactory
    {
        public static List<Series> GetSeries(int total)
        {
            var seriesList = new List<Series>();
            var random = new Random();

            for (var i = 0; i < total; i++)
            {
                seriesList.Add(new Series
                {
                    Id = i,
                    Title = $"Series title {i}",
                    Description = "This is the description of the series.",
                    ReleaseDate = new DateTime(DateTime.Now.Year, random.Next(1, 12), random.Next(1, 28)),
                    AddedDate = new DateTime(DateTime.Now.Year, random.Next(1, 12), random.Next(1, 28)),
                    Ended = true,
                    Owned = true,
                    Anime = false
                });
            }

            return seriesList;
        }
    }
}