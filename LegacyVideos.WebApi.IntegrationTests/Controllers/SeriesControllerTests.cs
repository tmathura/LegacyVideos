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
    public class SeriesControllerTests
    {
        private readonly CommonHelper _commonHelper;

        public SeriesControllerTests(ITestOutputHelper outputHelper, CommonHelper commonHelper)
        {
            commonHelper.OutputHelper = outputHelper;
            _commonHelper = commonHelper;
        }

        /// <summary>
        /// Add a new series and validate that the series was added correctly.
        /// </summary>
        [Fact]
        public async Task AddSeries()
        {
            // Arrange
            const string title = "Supernatural";
            const string description = "Two brothers follow their father''s footsteps as hunters, fighting evil supernatural beings of many kinds, including monsters, demons and gods that roam the earth.";
            const string releaseDate = "2005-09-13";
            const string addedDate = "2022-03-03";
            const bool ended = true;
            const bool owned = true;
            const bool anime = false;

            var addSeriesRequest = new
            {
                title = title,
                description = description,
                releasedate = releaseDate,
                addeddate = addedDate,
                ended = ended,
                owned = owned,
                anime = anime
            };

            // Act
            var response = await _commonHelper.CallEndPoint("api/series", null, Method.Post, addSeriesRequest);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            dynamic seriesId = JsonConvert.DeserializeObject(response.Content);

            // Assert

            var sql = $@"SELECT TOP 1 id, title, [description], release_date, added_date, ended, owned, anime
                         FROM dbo.series 
                         WHERE id = '{seriesId}'";
            var dbRow = _commonHelper.Database.RunSql(sql);

            Assert.NotNull(seriesId);
            Assert.Equal(Convert.ToInt32(seriesId), Convert.ToInt32(dbRow[0]["id"]));
            Assert.Equal(title, Convert.ToString(dbRow[0]["title"]));
            Assert.Equal(description, Convert.ToString(dbRow[0]["description"]));
            Assert.Equal(Convert.ToDateTime(releaseDate), Convert.ToDateTime(dbRow[0]["release_date"]));
            Assert.Equal(Convert.ToDateTime(addedDate), Convert.ToDateTime(dbRow[0]["added_date"]));
            Assert.Equal(ended, Convert.ToBoolean(dbRow[0]["ended"]));
            Assert.Equal(owned, Convert.ToBoolean(dbRow[0]["owned"]));
            Assert.Equal(anime, Convert.ToBoolean(dbRow[0]["anime"]));
        }
        
        /// <summary>
        /// Add a 2 new series, then get all and validate that the series were retrieved correctly.
        /// </summary>
        [Fact]
        public async Task GetAllSeries()
        {
            // Arrange
            const string title = "Supernatural";
            const string description = "Two brothers follow their father''s footsteps as hunters, fighting evil supernatural beings of many kinds, including monsters, demons and gods that roam the earth.";
            const string releaseDate = "2005-09-13";
            const string addedDate = "2022-03-03";
            const bool ended = true;
            const bool owned = true;
            const bool anime = false;

            var addSeriesRequest = new
            {
                title = title,
                description = description,
                releasedate = releaseDate,
                addeddate = addedDate,
                ended = ended,
                owned = owned,
                anime = anime
            };

            var response = await _commonHelper.CallEndPoint("api/series", null, Method.Post, addSeriesRequest);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            dynamic seriesId = JsonConvert.DeserializeObject(response.Content);
            Assert.NotNull(seriesId);

            response = await _commonHelper.CallEndPoint("api/series", null, Method.Post, addSeriesRequest);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            seriesId = JsonConvert.DeserializeObject(response.Content);
            Assert.NotNull(seriesId);

            // Act
            response = await _commonHelper.CallEndPoint("api/series", null, Method.Get, null);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            dynamic getSeriesByOwnedResponse = JsonConvert.DeserializeObject(response.Content);

            // Assert
            Assert.True(getSeriesByOwnedResponse.Count > 1);
        }
    }
}