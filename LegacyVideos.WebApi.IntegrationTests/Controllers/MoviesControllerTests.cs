using LegacyVideos.WebApi.IntegrationTests.Common.Helpers;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace LegacyVideos.WebApi.IntegrationTests.Controllers
{
    [Collection(nameof(CommonHelper))]
    public class MoviesControllerTests
    {
        private readonly CommonHelper _commonHelper;

        public MoviesControllerTests(ITestOutputHelper outputHelper, CommonHelper commonHelper)
        {
            commonHelper.OutputHelper = outputHelper;
            _commonHelper = commonHelper;
        }

        /// <summary>
        /// Add a new move and validate that the movie was added correctly.
        /// </summary>
        [Fact]
        public async Task AddMovie()
        {
            // Arrange
            const string title = "The Matrix";
            const string description = "Set in the 22nd century, The Matrix tells the story of a computer hacker who joins a group of underground insurgents fighting the vast and powerful computers who now rule the earth.";
            const int movieType = 2;
            const int duration = 136;
            const string releaseDate = "1999-03-30";
            const string addedDate = "2022-03-01";
            const bool owned = true;

            var movieRequest = new
            {
                title = title,
                description = description,
                movietype = movieType,
                duration = duration,
                releasedate = releaseDate,
                addeddate = addedDate,
                owned = owned
            };

            // Act
            var response = await _commonHelper.CallEndPoint("api/movies", Method.Post, movieRequest);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            dynamic movieId = JsonConvert.DeserializeObject(response.Content);

            // Assert

            var sql = $@"SELECT TOP 1 id, title, [description], movie_type_id, duration, release_date, added_date, owned
                         FROM dbo.movies 
                         WHERE id = '{movieId}'";
            var dbRow = _commonHelper.Database.RunSql(sql);

            Assert.NotNull(movieId);
            Assert.Equal(Convert.ToInt32(movieId), Convert.ToInt32(dbRow[0]["id"]));
            Assert.Equal(title, Convert.ToString(dbRow[0]["title"]));
            Assert.Equal(description, Convert.ToString(dbRow[0]["description"]));
            Assert.Equal(movieType, Convert.ToInt32(dbRow[0]["movie_type_id"]));
            Assert.Equal(duration, Convert.ToInt32(dbRow[0]["duration"]));
            Assert.Equal(Convert.ToDateTime(releaseDate), Convert.ToDateTime(dbRow[0]["release_date"]));
            Assert.Equal(Convert.ToDateTime(addedDate), Convert.ToDateTime(dbRow[0]["added_date"]));
            Assert.Equal(owned, Convert.ToBoolean(dbRow[0]["owned"]));
        }

        /// <summary>
        /// Add a new move and validate that the response to an internal server error.
        /// </summary>
        [Fact]
        public async Task AddMovie_Invalid_MovieType()
        {
            // Arrange
            const string title = "The Matrix";
            const string description = "Set in the 22nd century, The Matrix tells the story of a computer hacker who joins a group of underground insurgents fighting the vast and powerful computers who now rule the earth.";
            const int movieType = 3;
            const int duration = 136;
            const string releaseDate = "1999-03-30";
            const string addedDate = "2022-03-01";
            const bool owned = true;

            var movieRequest = new
            {
                title = title,
                description = description,
                movietype = movieType,
                duration = duration,
                releasedate = releaseDate,
                addeddate = addedDate,
                owned = owned
            };

            // Act
            var response = await _commonHelper.CallEndPoint("api/movies", Method.Post, movieRequest);

            // Assert
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }
    }
}