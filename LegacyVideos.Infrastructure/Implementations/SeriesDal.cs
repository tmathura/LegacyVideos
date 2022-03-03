using LegacyVideos.Domain.Models;
using LegacyVideos.Infrastructure.Extensions;
using LegacyVideos.Infrastructure.Interfaces;
using log4net;
using System.Data;
using System.Data.SqlClient;

namespace LegacyVideos.Infrastructure.Implementations
{
    /// <summary>
    /// Series data access logic
    /// </summary>
    /// <seealso cref="ISeriesDal" />
    public class SeriesDal : ISeriesDal
    {
        private readonly string _connectionString;
        private readonly ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        public SeriesDal(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Add series.
        /// </summary>
        /// <param name="series"><see cref="Series"/> to add.</param>
        public async Task<int> AddSeries(Series series)
        {
            int id;

            await using var sqlConnection = new SqlConnection(_connectionString);
            await sqlConnection.OpenAsync();
            var sqlCommand = sqlConnection.CreateCommand();
            var sqlTransaction = sqlConnection.BeginTransaction();
            sqlCommand.Transaction = sqlTransaction;

            try
            {
                _logger.Debug($"Adding series {series.Title} into database.");

                id = await AddSeries(series, sqlCommand);

                await sqlTransaction.CommitAsync();

                _logger.Debug($"Series added with id: {id}.");
            }
            catch (Exception exception)
            {
                _logger.Error($"{exception.Message} - {exception.StackTrace}");
                await sqlTransaction.RollbackAsync();
                throw;
            }

            return id;
        }

        /// <summary>
        /// Add series.
        /// </summary>
        /// <param name="series"><see cref="Series"/> to add.</param>
        /// <param name="sqlCommand">The SqlCommand to use when interacting with the database.</param>
        public async Task<int> AddSeries(Series series, SqlCommand sqlCommand)
        {
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "series_ins";
            sqlCommand.Parameters.Clear();

            sqlCommand.Parameters.Add("@title", SqlDbType.VarChar, 200).Value = series.Title;
            sqlCommand.Parameters.Add("@description", SqlDbType.VarChar, 2000).Value = series.Description;
            sqlCommand.Parameters.Add("@release_date", SqlDbType.DateTime).Value = series.ReleaseDate;
            sqlCommand.Parameters.Add("@added_date", SqlDbType.DateTime).Value = series.AddedDate;
            sqlCommand.Parameters.Add("@ended", SqlDbType.Bit).Value = series.Ended;
            sqlCommand.Parameters.Add("@owned", SqlDbType.Bit).Value = series.Owned;
            sqlCommand.Parameters.Add("@anime", SqlDbType.Bit).Value = series.Anime;

            return Convert.ToInt32(await sqlCommand.ExecuteScalarAsync());
        }

        /// <summary>
        /// Get all series.
        /// </summary>
        /// <returns><see cref="Series"/>s</returns>
        public async Task<List<Series>> GetAllSeries()
        {
            List<Series> series;

            await using var sqlConnection = new SqlConnection(_connectionString);
            await sqlConnection.OpenAsync();
            var sqlCommand = sqlConnection.CreateCommand();
            var sqlTransaction = sqlConnection.BeginTransaction();
            sqlCommand.Transaction = sqlTransaction;

            try
            {
                _logger.Debug("Getting all series.");

                series = await GetSeries(null, null, null, sqlCommand);

                await sqlTransaction.CommitAsync();

                _logger.Debug("Completed getting all series.");
            }
            catch (Exception exception)
            {
                _logger.Error($"{exception.Message} - {exception.StackTrace}");
                await sqlTransaction.RollbackAsync();
                throw;
            }

            return series;
        }

        /// <summary>
        /// Get series.
        /// </summary>
        /// <param name="id">Id of series to lookup.</param>
        /// <param name="title">Title of series to lookup.</param>
        /// <param name="owned">Indicator to lookup owned series.</param>
        /// <param name="sqlCommand">The SqlCommand to use when interacting with the database.</param>
        /// <returns><see cref="Series"/>s</returns>
        public async Task<List<Series>> GetSeries(int? id, string? title, bool? owned, SqlCommand sqlCommand)
        {
            var seriesList = new List<Series>();

            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "series_get";
            sqlCommand.Parameters.Clear();

            sqlCommand.Parameters.Add("@id", SqlDbType.Int).Value = id.ToSqlNull();
            sqlCommand.Parameters.Add("@title", SqlDbType.VarChar, 200).Value = title.ToSqlNull();
            sqlCommand.Parameters.Add("@owned", SqlDbType.Bit).Value = owned.ToSqlNull();

            await using var sqlDataReader = await sqlCommand.ExecuteReaderAsync();
            while (await sqlDataReader.ReadAsync())
            {
                var series = new Series
                {
                    Id = Convert.ToInt32(sqlDataReader["id"]),
                    Title = Convert.ToString(sqlDataReader["title"]),
                    Description = Convert.ToString(sqlDataReader["description"]),
                    ReleaseDate = Convert.ToDateTime(sqlDataReader["release_date"]),
                    AddedDate = Convert.ToDateTime(sqlDataReader["added_date"]),
                    Ended = Convert.ToBoolean(sqlDataReader["ended"]),
                    Owned = Convert.ToBoolean(sqlDataReader["owned"]),
                    Anime = Convert.ToBoolean(sqlDataReader["anime"])
                };

                seriesList.Add(series);
            }

            return seriesList;
        }
    }
}
