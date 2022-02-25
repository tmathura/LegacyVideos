using LegacyVideos.Domain.Models;
using LegacyVideos.Infrastructure.Implementations;
using LegacyVideos.Infrastructure.IntegrationTests.Common.Helpers;
using LegacyVideos.Infrastructure.IntegrationTests.Factories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace LegacyVideos.Infrastructure.IntegrationTests.Implementations
{
    [Collection(nameof(CommonHelper))]
    public class MovieDalTests
    {
        private readonly ITestOutputHelper _outputHelper;
        private readonly CommonHelper _commonHelper;
        private readonly MovieDal _movieDal;

        public MovieDalTests(ITestOutputHelper outputHelper, CommonHelper commonHelper)
        {
            _outputHelper = outputHelper;
            _commonHelper = commonHelper;
            _movieDal = new MovieDal(commonHelper.Settings.Database.ConnectionString);
        }

        /// <summary>
        /// Add a new <see cref="Movie"/> and make sure it is added to the database.
        /// </summary>
        [Fact]
        public async Task AddMovie()
        {
            // Arrange
            var movie = MovieFactory.GetMovies(1).FirstOrDefault();

            int id;

            await using var sqlConnection = new SqlConnection(_commonHelper.Settings.Database.ConnectionString);
            await sqlConnection.OpenAsync();
            var sqlCommand = sqlConnection.CreateCommand();
            var sqlTransaction = sqlConnection.BeginTransaction();
            sqlCommand.Transaction = sqlTransaction;

            await AddMovieTypeToDatabase(sqlCommand);

            // Act
            try
            {
                id = await _movieDal.AddMovie(movie, sqlCommand);
            }
            finally
            {
                await sqlTransaction.RollbackAsync();
            }

            // Assert
            Assert.True(id > 0);

            _outputHelper.WriteLine($"Movie was added with id: {id}.");
        }

        /// <summary>
        /// Get a <see cref="Movie"/> by id.
        /// </summary>
        [Fact]
        public async Task GetMovieById()
        {
            // Arrange
            Movie movie;
            const int numberOfMovies = 5;

            await using var sqlConnection = new SqlConnection(_commonHelper.Settings.Database.ConnectionString);
            await sqlConnection.OpenAsync();
            var sqlCommand = sqlConnection.CreateCommand();
            var sqlTransaction = sqlConnection.BeginTransaction();
            sqlCommand.Transaction = sqlTransaction;

            var randomMovie = await GetRandomMovieFromDatabase(sqlCommand, numberOfMovies);

            // Act
            try
            {
                movie = await _movieDal.GetMovieById(randomMovie.Id, sqlCommand);
            }
            finally
            {
                await sqlTransaction.RollbackAsync();
            }

            // Assert
            Assert.Equal(randomMovie.Description, movie.Description);
        }

        /// <summary>
        /// Get all <see cref="Movie"/>s by title.
        /// </summary>
        [Fact]
        public async Task GetMovieByTitle()
        {
            // Arrange
            List<Movie> movies;
            const string title = "Title 1";

            await using var sqlConnection = new SqlConnection(_commonHelper.Settings.Database.ConnectionString);
            await sqlConnection.OpenAsync();
            var sqlCommand = sqlConnection.CreateCommand();
            var sqlTransaction = sqlConnection.BeginTransaction();
            sqlCommand.Transaction = sqlTransaction;

            await AddMovieToDatabase(sqlCommand, 20);

            // Act
            try
            {
                movies = await _movieDal.GetMovieByTitle(title, sqlCommand);
            }
            finally
            {
                await sqlTransaction.RollbackAsync();
            }

            // Assert
            Assert.Equal(11, movies.Count);
        }

        /// <summary>
        /// Get all <see cref="Movie"/>s that are owned.
        /// </summary>
        [Fact]
        public async Task GetMovieByOwned()
        {
            // Arrange
            List<Movie> movies;
            const bool owned = true;
            const int numberOfMovies = 5;

            await using var sqlConnection = new SqlConnection(_commonHelper.Settings.Database.ConnectionString);
            await sqlConnection.OpenAsync();
            var sqlCommand = sqlConnection.CreateCommand();
            var sqlTransaction = sqlConnection.BeginTransaction();
            sqlCommand.Transaction = sqlTransaction;

            await AddMovieToDatabase(sqlCommand, numberOfMovies);

            // Act
            try
            {
                movies = await _movieDal.GetMovieByOwned(owned, sqlCommand);
            }
            finally
            {
                await sqlTransaction.RollbackAsync();
            }

            // Assert
            Assert.Equal(numberOfMovies, movies.Count);
        }

        /// <summary>
        /// Get <see cref="Movie"/>s by release date and see that a range of <see cref="Movie"/>s are returned.
        /// </summary>
        [Fact]
        public async Task GetMovieByReleaseDate()
        {
            // Arrange
            List<Movie> movies;
            var fromDate = new DateTime(DateTime.Now.Year, 1, 1);
            var toDate = new DateTime(DateTime.Now.Year + 1, 1, 1);
            const int numberOfMovies = 7;

            await using var sqlConnection = new SqlConnection(_commonHelper.Settings.Database.ConnectionString);
            await sqlConnection.OpenAsync();
            var sqlCommand = sqlConnection.CreateCommand();
            var sqlTransaction = sqlConnection.BeginTransaction();
            sqlCommand.Transaction = sqlTransaction;

            await AddMovieToDatabase(sqlCommand, numberOfMovies);

            // Act
            try
            {
                movies = await _movieDal.GetMovieByReleaseDate(fromDate, toDate, sqlCommand);
            }
            finally
            {
                await sqlTransaction.RollbackAsync();
            }

            // Assert
            Assert.Equal(numberOfMovies, movies.Count);
        }

        /// <summary>
        /// Update a <see cref="Movie"/> and make sure it is updated correctly.
        /// </summary>
        [Fact]
        public async Task UpdateMovie()
        {
            // Arrange
            const int numberOfMovies = 10;

            await using var sqlConnection = new SqlConnection(_commonHelper.Settings.Database.ConnectionString);
            await sqlConnection.OpenAsync();
            var sqlCommand = sqlConnection.CreateCommand();
            var sqlTransaction = sqlConnection.BeginTransaction();
            sqlCommand.Transaction = sqlTransaction;

            var movie = await GetRandomMovieFromDatabase(sqlCommand, numberOfMovies);
            const string title = "The updated title.";
            movie.Title = title;

            Movie updatedMovie;

            // Act
            try
            {
                await _movieDal.UpdateMovie(movie, sqlCommand);
                updatedMovie = await _movieDal.GetMovieById(movie.Id, sqlCommand);
            }
            finally
            {
                await sqlTransaction.RollbackAsync();
            }

            // Assert
            Assert.Equal(title, updatedMovie.Title);
        }

        private static async Task AddMovieTypeToDatabase(SqlCommand sqlCommand)
        {
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.CommandText = "" +
                                     "SET IDENTITY_INSERT dbo.movie_type ON; " +
                                     "IF NOT EXISTS(SELECT 1 FROM [dbo].[movie_type] WHERE id = 1) " +
                                     "BEGIN " +
                                     "INSERT INTO dbo.movie_type (id,[type]) " +
                                     "SELECT 1, 'Dvd'; " +
                                     "END; " +
                                     "IF NOT EXISTS(SELECT 1 FROM [dbo].[movie_type] WHERE id = 2) " +
                                     "BEGIN " +
                                     "INSERT INTO dbo.movie_type (id,[type]) " +
                                     "SELECT 2, 'BluRay'; " +
                                     "END; " +
                                     "SET IDENTITY_INSERT dbo.movie_type OFF; " +
                                     "";
            await sqlCommand.ExecuteNonQueryAsync();
        }

        private async Task AddMovieToDatabase(SqlCommand sqlCommand, int numberOfMovies)
        {
            var movies = MovieFactory.GetMovies(numberOfMovies);

            await AddMovieTypeToDatabase(sqlCommand);

            foreach (var movie in movies)
            {
                await _movieDal.AddMovie(movie, sqlCommand);
            }
        }

        private async Task<Movie> GetRandomMovieFromDatabase(SqlCommand sqlCommand, int numberOfMovies)
        {
            await AddMovieToDatabase(sqlCommand, numberOfMovies);

            var movies = await _movieDal.GetMovieByOwned(true, sqlCommand);
            var random = new Random();
            return movies[random.Next(movies.Count)];
        }
    }
}