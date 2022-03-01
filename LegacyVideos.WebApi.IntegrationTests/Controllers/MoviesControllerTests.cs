using LegacyVideos.WebApi.IntegrationTests.Common.Helpers;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
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
        /// Add a new movie and validate that the movie was added correctly.
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

            var addMovieRequest = new
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
            var response = await _commonHelper.CallEndPoint("api/movies", null, Method.Post, addMovieRequest);
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
        /// Add a new movie and validate that the response is an internal server error.
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

            var addMovieRequest = new
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
            var response = await _commonHelper.CallEndPoint("api/movies", null, Method.Post, addMovieRequest);

            // Assert
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        /// <summary>
        /// Add a new movie, then get it by id and validate that the movie was retrieved correctly.
        /// </summary>
        [Fact]
        public async Task GetMovieById()
        {
            // Arrange
            const string title = "The Matrix";
            const string description = "Set in the 22nd century, The Matrix tells the story of a computer hacker who joins a group of underground insurgents fighting the vast and powerful computers who now rule the earth.";
            const int movieType = 2;
            const int duration = 136;
            const string releaseDate = "1999-03-30";
            const string addedDate = "2022-03-01";
            const bool owned = true;

            var addMovieRequest = new
            {
                title = title,
                description = description,
                movietype = movieType,
                duration = duration,
                releasedate = releaseDate,
                addeddate = addedDate,
                owned = owned
            };

            var response = await _commonHelper.CallEndPoint("api/movies", null, Method.Post, addMovieRequest);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            dynamic movieId = JsonConvert.DeserializeObject(response.Content);
            Assert.NotNull(movieId);

            // Act
            response = await _commonHelper.CallEndPoint($"api/movies/{movieId}", null, Method.Get, null);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            dynamic getMovieByIdResponse = JsonConvert.DeserializeObject(response.Content);

            // Assert
            var sql = $@"SELECT TOP 1 id, title, [description], movie_type_id, duration, release_date, added_date, owned
                         FROM dbo.movies 
                         WHERE id = '{movieId}'";
            var dbRow = _commonHelper.Database.RunSql(sql);

            Assert.NotNull(getMovieByIdResponse);
            Assert.Equal(getMovieByIdResponse.id.Value, Convert.ToInt32(dbRow[0]["id"]));
            Assert.Equal(getMovieByIdResponse.title.Value, Convert.ToString(dbRow[0]["title"]));
            Assert.Equal(getMovieByIdResponse.description.Value, Convert.ToString(dbRow[0]["description"]));
            Assert.Equal(getMovieByIdResponse.movieType.Value, Convert.ToInt32(dbRow[0]["movie_type_id"]));
            Assert.Equal(getMovieByIdResponse.duration.Value, Convert.ToInt32(dbRow[0]["duration"]));
            Assert.Equal(getMovieByIdResponse.releaseDate.Value, Convert.ToDateTime(dbRow[0]["release_date"]));
            Assert.Equal(getMovieByIdResponse.addedDate.Value, Convert.ToDateTime(dbRow[0]["added_date"]));
            Assert.Equal(getMovieByIdResponse.owned.Value, Convert.ToBoolean(dbRow[0]["owned"]));
        }

        /// <summary>
        /// Get a movie by invalid id and validate that the response is no content.
        /// </summary>
        [Fact]
        public async Task GetMovieById_Invalid_Id()
        {
            // Arrange
            const int movieId = 55;

            // Act
            var response = await _commonHelper.CallEndPoint($"api/movies/{movieId}", null, Method.Get, null);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        /// <summary>
        /// Add a new movie, then get it by title and validate that the movies were retrieved correctly.
        /// </summary>
        [Fact]
        public async Task GetMoviesByTitle()
        {
            // Arrange
            const string title = "The Matrix";
            const string description = "Set in the 22nd century, The Matrix tells the story of a computer hacker who joins a group of underground insurgents fighting the vast and powerful computers who now rule the earth.";
            const int movieType = 2;
            const int duration = 136;
            const string releaseDate = "1999-03-30";
            const string addedDate = "2022-03-01";
            const bool owned = true;

            var addMovieRequest = new
            {
                title = title,
                description = description,
                movietype = movieType,
                duration = duration,
                releasedate = releaseDate,
                addeddate = addedDate,
                owned = owned
            };

            var response = await _commonHelper.CallEndPoint("api/movies", null, Method.Post, addMovieRequest);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            dynamic movieId = JsonConvert.DeserializeObject(response.Content);
            Assert.NotNull(movieId);

            // Act
            var endPointParams = new Dictionary<string, string>
            {
                {"title", title}
            };

            response = await _commonHelper.CallEndPoint("api/movies/getmoviesbytitle", endPointParams, Method.Get, null);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            dynamic getMoviesByTitleResponse = JsonConvert.DeserializeObject(response.Content);

            // Assert
            Assert.True(getMoviesByTitleResponse.Count > 0);
        }

        /// <summary>
        /// Get a movie by a title that does not exist and validate that the response is no content.
        /// </summary>
        [Fact]
        public async Task GetMoviesByTitle_Invalid_Title()
        {
            // Arrange
            const string title = "Super Fake Title";

            // Act
            var endPointParams = new Dictionary<string, string>
            {
                {"title", title}
            };

            var response = await _commonHelper.CallEndPoint("api/movies/getmoviesbytitle", endPointParams, Method.Get, null);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        /// <summary>
        /// Add a new movie, then get it by owned and validate that the movies were retrieved correctly.
        /// </summary>
        [Fact]
        public async Task GetMoviesByOwned()
        {
            // Arrange
            const string title = "The Matrix";
            const string description = "Set in the 22nd century, The Matrix tells the story of a computer hacker who joins a group of underground insurgents fighting the vast and powerful computers who now rule the earth.";
            const int movieType = 2;
            const int duration = 136;
            const string releaseDate = "1999-03-30";
            const string addedDate = "2022-03-01";
            const bool owned = true;

            var addMovieRequest = new
            {
                title = title,
                description = description,
                movietype = movieType,
                duration = duration,
                releasedate = releaseDate,
                addeddate = addedDate,
                owned = owned
            };

            var response = await _commonHelper.CallEndPoint("api/movies", null, Method.Post, addMovieRequest);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            dynamic movieId = JsonConvert.DeserializeObject(response.Content);
            Assert.NotNull(movieId);

            // Act
            var endPointParams = new Dictionary<string, string>
            {
                {"owned", owned.ToString()}
            };

            response = await _commonHelper.CallEndPoint("api/movies/getmoviesbyowned", endPointParams, Method.Get, null);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            dynamic getMoviesByOwnedResponse = JsonConvert.DeserializeObject(response.Content);

            // Assert
            Assert.True(getMoviesByOwnedResponse.Count > 0);
        }

        /// <summary>
        /// Add a new movie, then get it by release date and validate that the movies were retrieved correctly.
        /// </summary>
        [Fact]
        public async Task GetMoviesByReleaseDate()
        {
            // Arrange
            const string title = "The Matrix";
            const string description = "Set in the 22nd century, The Matrix tells the story of a computer hacker who joins a group of underground insurgents fighting the vast and powerful computers who now rule the earth.";
            const int movieType = 2;
            const int duration = 136;
            const string releaseDate = "1999-03-30";
            const string addedDate = "2022-03-01";
            const bool owned = true;

            var addMovieRequest = new
            {
                title = title,
                description = description,
                movietype = movieType,
                duration = duration,
                releasedate = releaseDate,
                addeddate = addedDate,
                owned = owned
            };

            var response = await _commonHelper.CallEndPoint("api/movies", null, Method.Post, addMovieRequest);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            dynamic movieId = JsonConvert.DeserializeObject(response.Content);
            Assert.NotNull(movieId);

            // Act
            var endPointParams = new Dictionary<string, string>
            {
                {"fromDate", "1999-01-01T00:00:00.0000000"},
                {"toDate", "2005-01-01T00:00:00.0000000"}

            };

            response = await _commonHelper.CallEndPoint("api/movies/getmoviesbyreleasedate", endPointParams, Method.Get, null);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            dynamic getMoviesByReleaseDateResponse = JsonConvert.DeserializeObject(response.Content);

            // Assert
            Assert.True(getMoviesByReleaseDateResponse.Count > 0);
        }

        /// <summary>
        /// Get a movie by a release date that does not exist and validate that the response is no content.
        /// </summary>
        [Fact]
        public async Task GetMoviesByReleaseDate_Invalid_ReleaseDate()
        {
            // Act
            var endPointParams = new Dictionary<string, string>
            {
                {"fromDate", "2000-01-01T00:00:00.0000000"},
                {"toDate", "2005-01-01T00:00:00.0000000"}

            };

            var response = await _commonHelper.CallEndPoint("api/movies/getmoviesbyreleasedate", endPointParams, Method.Get, null);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        /// <summary>
        /// Update a movie and validate that the movie was updated correctly.
        /// </summary>
        [Fact]
        public async Task UpdateMovie()
        {
            // Arrange
            const string title = "The Matrix";
            const string updatedTitle = "The Updated Matrix";
            const string description = "Set in the 22nd century, The Matrix tells the story of a computer hacker who joins a group of underground insurgents fighting the vast and powerful computers who now rule the earth.";
            const int movieType = 2;
            const int duration = 136;
            const string releaseDate = "1999-03-30";
            const string addedDate = "2022-03-01";
            const bool owned = true;

            var addMovieRequest = new
            {
                title = title,
                description = description,
                movietype = movieType,
                duration = duration,
                releasedate = releaseDate,
                addeddate = addedDate,
                owned = owned
            };

            var response = await _commonHelper.CallEndPoint("api/movies", null, Method.Post, addMovieRequest);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            dynamic movieId = JsonConvert.DeserializeObject(response.Content);
            Assert.NotNull(movieId);

            // Act

            var updateMovieRequest = new
            {
                id = movieId,
                title = updatedTitle,
                description = description,
                movietype = movieType,
                duration = duration,
                releasedate = releaseDate,
                addeddate = addedDate,
                owned = owned
            };

            response = await _commonHelper.CallEndPoint("api/movies", null, Method.Put, updateMovieRequest);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            // Assert

            var sql = $@"SELECT TOP 1 id, title, [description], movie_type_id, duration, release_date, added_date, owned
                         FROM dbo.movies 
                         WHERE id = '{movieId}'";
            var dbRow = _commonHelper.Database.RunSql(sql);

            Assert.Equal(Convert.ToInt32(movieId), Convert.ToInt32(dbRow[0]["id"]));
            Assert.Equal(updatedTitle, Convert.ToString(dbRow[0]["title"]));
            Assert.Equal(description, Convert.ToString(dbRow[0]["description"]));
            Assert.Equal(movieType, Convert.ToInt32(dbRow[0]["movie_type_id"]));
            Assert.Equal(duration, Convert.ToInt32(dbRow[0]["duration"]));
            Assert.Equal(Convert.ToDateTime(releaseDate), Convert.ToDateTime(dbRow[0]["release_date"]));
            Assert.Equal(Convert.ToDateTime(addedDate), Convert.ToDateTime(dbRow[0]["added_date"]));
            Assert.Equal(owned, Convert.ToBoolean(dbRow[0]["owned"]));
        }

        /// <summary>
        /// Delete a movie and validate that the movie was deleted.
        /// </summary>
        [Fact]
        public async Task DeleteMovie()
        {
            // Arrange
            const string title = "The Matrix";
            const string description = "Set in the 22nd century, The Matrix tells the story of a computer hacker who joins a group of underground insurgents fighting the vast and powerful computers who now rule the earth.";
            const int movieType = 2;
            const int duration = 136;
            const string releaseDate = "1999-03-30";
            const string addedDate = "2022-03-01";
            const bool owned = true;

            var addMovieRequest = new
            {
                title = title,
                description = description,
                movietype = movieType,
                duration = duration,
                releasedate = releaseDate,
                addeddate = addedDate,
                owned = owned
            };

            var response = await _commonHelper.CallEndPoint("api/movies", null, Method.Post, addMovieRequest);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            dynamic movieId = JsonConvert.DeserializeObject(response.Content);
            Assert.NotNull(movieId);

            // Act
            var endPointParams = new Dictionary<string, string>
            {
                {"id", movieId.ToString()}

            };

            response = await _commonHelper.CallEndPoint("api/movies", endPointParams, Method.Delete, null);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            // Assert

            var sql = $@"SELECT TOP 1 id, title, [description], movie_type_id, duration, release_date, added_date, owned
                         FROM dbo.movies 
                         WHERE id = '{movieId}'";
            var dbRow = _commonHelper.Database.RunSql(sql);
        }
    }
}