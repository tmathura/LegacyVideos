using LegacyVideos.Domain.Models;
using LegacyVideos.Infrastructure.Implementations;
using LegacyVideos.Infrastructure.IntegrationTests.Common.Helpers;
using LegacyVideos.Infrastructure.IntegrationTests.Factories;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace LegacyVideos.Infrastructure.IntegrationTests.Implementations
{
    [Collection(nameof(CommonHelper))]
    public class SeriesDalTests
    {
        private readonly ITestOutputHelper _outputHelper;
        private readonly CommonHelper _commonHelper;
        private readonly SeriesDal _seriesDal;

        public SeriesDalTests(ITestOutputHelper outputHelper, CommonHelper commonHelper)
        {
            _outputHelper = outputHelper;
            _commonHelper = commonHelper;
            _seriesDal = new SeriesDal(commonHelper.Settings.Database.ConnectionString);
        }

        /// <summary>
        /// Add a new <see cref="Series"/> and make sure it is added to the database.
        /// </summary>
        [Fact]
        public async Task AddSeries()
        {
            // Arrange
            var series = SeriesFactory.GetSeries(1).FirstOrDefault();

            int id;

            await using var sqlConnection = new SqlConnection(_commonHelper.Settings.Database.ConnectionString);
            await sqlConnection.OpenAsync();
            var sqlCommand = sqlConnection.CreateCommand();
            var sqlTransaction = sqlConnection.BeginTransaction();
            sqlCommand.Transaction = sqlTransaction;
            
            // Act
            try
            {
                id = await _seriesDal.AddSeries(series, sqlCommand);
            }
            finally
            {
                await sqlTransaction.RollbackAsync();
            }

            // Assert
            Assert.True(id > 0);

            _outputHelper.WriteLine($"Series was added with id: {id}.");
        }

        /// <summary>
        /// Get all <see cref="Series"/>s.
        /// </summary>
        [Fact]
        public async Task GetAllSeries()
        {
            // Arrange
            List<Series> series;
            const int numberOfSeries = 17;

            await using var sqlConnection = new SqlConnection(_commonHelper.Settings.Database.ConnectionString);
            await sqlConnection.OpenAsync();
            var sqlCommand = sqlConnection.CreateCommand();
            var sqlTransaction = sqlConnection.BeginTransaction();
            sqlCommand.Transaction = sqlTransaction;

            await AddSeriesToDatabase(sqlCommand, numberOfSeries);

            // Act
            try
            {
                series = await _seriesDal.GetSeries(null, null, null, sqlCommand);
            }
            finally
            {
                await sqlTransaction.RollbackAsync();
            }

            // Assert
            Assert.Equal(numberOfSeries, series.Count);
        }
        
        private async Task AddSeriesToDatabase(SqlCommand sqlCommand, int numberOfSeries)
        {
            var seriesList = SeriesFactory.GetSeries(numberOfSeries);
            
            foreach (var series in seriesList)
            {
                await _seriesDal.AddSeries(series, sqlCommand);
            }
        }
    }
}