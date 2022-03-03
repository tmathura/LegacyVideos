using LegacyVideos.Domain.Enums;
using LegacyVideos.Domain.Models;
using System;
using System.Collections.Generic;

namespace LegacyVideos.Infrastructure.IntegrationTests.Factories
{
    public static class MovieFactory
    {
        public static List<Movie> GetMovies(int total)
        {
            var movies = new List<Movie>();

            var movieTypes = Enum.GetValues(typeof(MovieType));
            var random = new Random();
            var randomMovieType = (MovieType)movieTypes.GetValue(random.Next(movieTypes.Length));

            for (var i = 0; i < total; i++)
            {
                movies.Add(new Movie
                {
                    Id = i,
                    Title = $"Movie title {i}",
                    Description = "This is the description of the movie.",
                    MovieType = randomMovieType,
                    Duration = random.Next(100),
                    ReleaseDate = new DateTime(DateTime.Now.Year, random.Next(1, 12), random.Next(1, 28)),
                    AddedDate = new DateTime(DateTime.Now.Year, random.Next(1, 12), random.Next(1, 28)),
                    Owned = true
                });
            }

            return movies;
        }
    }
}