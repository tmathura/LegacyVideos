using LegacyVideos.Domain.Enums;
using LegacyVideos.Domain.Models;
using LegacyVideos.Infrastructure.Interfaces;
using System.Data;
using System.Data.SqlClient;
using log4net;

namespace LegacyVideos.Infrastructure.Implementations
{
    /// <summary>
    /// Movie data access logic
    /// </summary>
    /// <seealso cref="IMovieDal" />
    public class MovieDal : IMovieDal
    {
        private readonly string _connectionString;
        private readonly ILog _logger = LogManager.GetLogger(typeof(MovieDal));

        public MovieDal(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Add movie.
        /// </summary>
        /// <param name="movie"><see cref="Movie"/> to add.</param>
        public async Task<int> AddMovie(Movie movie)
        {
            int id;

            await using var sqlConnection = new SqlConnection(_connectionString);
            await sqlConnection.OpenAsync();
            var sqlCommand = sqlConnection.CreateCommand();
            var sqlTransaction = sqlConnection.BeginTransaction();
            sqlCommand.Transaction = sqlTransaction;

            try
            {
                _logger.Debug($"Adding movie {movie.Title} into database.");

                id = await AddMovie(movie, sqlCommand);

                await sqlTransaction.CommitAsync();

                _logger.Debug($"Movie added with id: {id}.");
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
        /// Add movie.
        /// </summary>
        /// <param name="movie"><see cref="Movie"/> to add.</param>
        /// <param name="sqlCommand">The SqlCommand to use when interacting with the database.</param>
        public async Task<int> AddMovie(Movie movie, SqlCommand sqlCommand)
        {
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "movies_ins";
            sqlCommand.Parameters.Clear();

            sqlCommand.Parameters.Add("@title", SqlDbType.VarChar, 200).Value = movie.Title;
            sqlCommand.Parameters.Add("@description", SqlDbType.VarChar, 2000).Value = movie.Description;
            sqlCommand.Parameters.Add("@movie_type_id", SqlDbType.Int).Value = movie.MovieType;
            sqlCommand.Parameters.Add("@duration", SqlDbType.Int).Value = movie.Duration;
            sqlCommand.Parameters.Add("@release_date", SqlDbType.DateTime).Value = movie.ReleaseDate;
            sqlCommand.Parameters.Add("@added_date", SqlDbType.DateTime).Value = movie.AddedDate;
            sqlCommand.Parameters.Add("@owned", SqlDbType.Bit).Value = movie.Owned;

            return Convert.ToInt32(await sqlCommand.ExecuteScalarAsync());
        }

        /// <summary>
        /// Get movie by id.
        /// </summary>
        /// <param name="id">Id of movie to lookup.</param>
        /// <returns><see cref="Movie"/></returns>
        public async Task<Movie> GetMovieById(int id)
        {
            Movie movies;

            await using var sqlConnection = new SqlConnection(_connectionString);
            await sqlConnection.OpenAsync();
            var sqlCommand = sqlConnection.CreateCommand();
            var sqlTransaction = sqlConnection.BeginTransaction();
            sqlCommand.Transaction = sqlTransaction;

            try
            {
                _logger.Debug($"Getting movie by movie id: {id}.");

                movies = await GetMovieById(id, sqlCommand);

                await sqlTransaction.CommitAsync();

                _logger.Debug($"Completed getting movie by movie id: {id}.");
            }
            catch (Exception exception)
            {
                _logger.Error($"{exception.Message} - {exception.StackTrace}");
                await sqlTransaction.RollbackAsync();
                throw;
            }

            return movies;
        }

        /// <summary>
        /// Get movie by id.
        /// </summary>
        /// <param name="id">Id of movie to lookup.</param>
        /// <param name="sqlCommand">The SqlCommand to use when interacting with the database.</param>
        /// <returns><see cref="Movie"/></returns>
        public async Task<Movie> GetMovieById(int id, SqlCommand sqlCommand)
        {
            Movie movie = null;

            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "movies_get";
            sqlCommand.Parameters.Clear();

            sqlCommand.Parameters.Add("@id", SqlDbType.Int).Value = id;
            sqlCommand.Parameters.Add("@title", SqlDbType.VarChar, 200).Value = DBNull.Value;
            sqlCommand.Parameters.Add("@owned", SqlDbType.Bit).Value = DBNull.Value;

            await using var sqlDataReader = await sqlCommand.ExecuteReaderAsync();
            while (await sqlDataReader.ReadAsync())
            {
                movie = new Movie
                {
                    Id = Convert.ToInt32(sqlDataReader["id"]),
                    Title = Convert.ToString(sqlDataReader["title"]),
                    Description = Convert.ToString(sqlDataReader["description"]),
                    MovieType = (MovieType)Convert.ToInt32(sqlDataReader["movie_type_id"]),
                    Duration = Convert.ToInt32(sqlDataReader["duration"]),
                    ReleaseDate = Convert.ToDateTime(sqlDataReader["release_date"]),
                    AddedDate = Convert.ToDateTime(sqlDataReader["added_date"]),
                    Owned = Convert.ToBoolean(sqlDataReader["owned"])
                };
            }

            return movie;
        }

        /// <summary>
        /// Get movies by title.
        /// </summary>
        /// <param name="title">Title of movies to lookup.</param>
        /// <returns><see cref="Movie"/>s</returns>
        public async Task<List<Movie>> GetMoviesByTitle(string title)
        {
            List<Movie> movies;

            await using var sqlConnection = new SqlConnection(_connectionString);
            await sqlConnection.OpenAsync();
            var sqlCommand = sqlConnection.CreateCommand();
            var sqlTransaction = sqlConnection.BeginTransaction();
            sqlCommand.Transaction = sqlTransaction;

            try
            {
                _logger.Debug($"Getting movies by title: {title}.");

                movies = await GetMoviesByTitle(title, sqlCommand);

                await sqlTransaction.CommitAsync();

                _logger.Debug($"Completed getting movies by title: {title}.");
            }
            catch (Exception exception)
            {
                _logger.Error($"{exception.Message} - {exception.StackTrace}");
                await sqlTransaction.RollbackAsync();
                throw;
            }

            return movies;
        }

        /// <summary>
        /// Get movies by title.
        /// </summary>
        /// <param name="title">Title of movies to lookup.</param>
        /// <param name="sqlCommand">The SqlCommand to use when interacting with the database.</param>
        /// <returns><see cref="Movie"/>s</returns>
        public async Task<List<Movie>> GetMoviesByTitle(string title, SqlCommand sqlCommand)
        {
            var movies = new List<Movie>();

            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "movies_get";
            sqlCommand.Parameters.Clear();

            sqlCommand.Parameters.Add("@id", SqlDbType.Int).Value = DBNull.Value;
            sqlCommand.Parameters.Add("@title", SqlDbType.VarChar, 200).Value = title;
            sqlCommand.Parameters.Add("@owned", SqlDbType.Bit).Value = DBNull.Value;

            await using var sqlDataReader = await sqlCommand.ExecuteReaderAsync();
            while (await sqlDataReader.ReadAsync())
            {
                var movie = new Movie
                {
                    Id = Convert.ToInt32(sqlDataReader["id"]),
                    Title = Convert.ToString(sqlDataReader["title"]),
                    Description = Convert.ToString(sqlDataReader["description"]),
                    MovieType = (MovieType)Convert.ToInt32(sqlDataReader["movie_type_id"]),
                    Duration = Convert.ToInt32(sqlDataReader["duration"]),
                    ReleaseDate = Convert.ToDateTime(sqlDataReader["release_date"]),
                    AddedDate = Convert.ToDateTime(sqlDataReader["added_date"]),
                    Owned = Convert.ToBoolean(sqlDataReader["owned"])
                };

                movies.Add(movie);
            }

            return movies;
        }

        /// <summary>
        /// Get movies that is owned or not.
        /// </summary>
        /// <param name="owned">Indicator to lookup owned movies.</param>
        /// <returns><see cref="Movie"/>s</returns>
        public async Task<List<Movie>> GetMoviesByOwned(bool owned)
        {
            List<Movie> movies;

            await using var sqlConnection = new SqlConnection(_connectionString);
            await sqlConnection.OpenAsync();
            var sqlCommand = sqlConnection.CreateCommand();
            var sqlTransaction = sqlConnection.BeginTransaction();
            sqlCommand.Transaction = sqlTransaction;

            try
            {
                _logger.Debug($"Getting movies by owned: {owned}.");

                movies = await GetMoviesByOwned(owned, sqlCommand);

                await sqlTransaction.CommitAsync();

                _logger.Debug($"Completed getting movies by owned: {owned}.");
            }
            catch (Exception exception)
            {
                _logger.Error($"{exception.Message} - {exception.StackTrace}");
                await sqlTransaction.RollbackAsync();
                throw;
            }

            return movies;
        }

        /// <summary>
        /// Get movies that is owned or not.
        /// </summary>
        /// <param name="owned">Indicator to lookup owned movies.</param>
        /// <param name="sqlCommand">The SqlCommand to use when interacting with the database.</param>
        /// <returns><see cref="Movie"/>s</returns>
        public async Task<List<Movie>> GetMoviesByOwned(bool owned, SqlCommand sqlCommand)
        {
            var movies = new List<Movie>();

            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "movies_get";
            sqlCommand.Parameters.Clear();

            sqlCommand.Parameters.Add("@id", SqlDbType.Int).Value = DBNull.Value;
            sqlCommand.Parameters.Add("@title", SqlDbType.VarChar, 200).Value = DBNull.Value;
            sqlCommand.Parameters.Add("@owned", SqlDbType.Bit).Value = owned;

            await using var sqlDataReader = await sqlCommand.ExecuteReaderAsync();
            while (await sqlDataReader.ReadAsync())
            {
                var movie = new Movie
                {
                    Id = Convert.ToInt32(sqlDataReader["id"]),
                    Title = Convert.ToString(sqlDataReader["title"]),
                    Description = Convert.ToString(sqlDataReader["description"]),
                    MovieType = (MovieType)Convert.ToInt32(sqlDataReader["movie_type_id"]),
                    Duration = Convert.ToInt32(sqlDataReader["duration"]),
                    ReleaseDate = Convert.ToDateTime(sqlDataReader["release_date"]),
                    AddedDate = Convert.ToDateTime(sqlDataReader["added_date"]),
                    Owned = Convert.ToBoolean(sqlDataReader["owned"])
                };

                movies.Add(movie);
            }

            return movies;
        }

        /// <summary>
        /// Get movies by release date.
        /// </summary>
        /// <param name="fromDate">The from date to lookup movies from.</param>
        /// <param name="toDate">The to date to lookup movies to.</param>
        /// <returns><see cref="Movie"/>s</returns>
        public async Task<List<Movie>> GetMovieByReleaseDate(DateTime fromDate, DateTime toDate)
        {
            List<Movie> movies;

            await using var sqlConnection = new SqlConnection(_connectionString);
            await sqlConnection.OpenAsync();
            var sqlCommand = sqlConnection.CreateCommand();
            var sqlTransaction = sqlConnection.BeginTransaction();
            sqlCommand.Transaction = sqlTransaction;

            try
            {
                _logger.Debug($"Getting movies by from date: {fromDate} and to date {toDate}.");

                movies = await GetMovieByReleaseDate(fromDate, toDate, sqlCommand);

                await sqlTransaction.CommitAsync();

                _logger.Debug($"Completed getting movies by from date: {fromDate} and to date {toDate}.");
            }
            catch (Exception exception)
            {
                _logger.Error($"{exception.Message} - {exception.StackTrace}");
                await sqlTransaction.RollbackAsync();
                throw;
            }

            return movies;
        }

        /// <summary>
        /// Get movies by release date.
        /// </summary>
        /// <param name="fromDate">The from date to lookup movies from.</param>
        /// <param name="toDate">The to date to lookup movies to.</param>
        /// <param name="sqlCommand">The SqlCommand to use when interacting with the database.</param>
        /// <returns><see cref="Movie"/>s</returns>
        public async Task<List<Movie>> GetMovieByReleaseDate(DateTime fromDate, DateTime toDate, SqlCommand sqlCommand)
        {
            var movies = new List<Movie>();

            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "movies_by_released_date_get";
            sqlCommand.Parameters.Clear();

            sqlCommand.Parameters.Add("@from_date", SqlDbType.Date).Value = fromDate;
            sqlCommand.Parameters.Add("@to_date", SqlDbType.Date).Value = toDate;

            await using var sqlDataReader = await sqlCommand.ExecuteReaderAsync();
            while (await sqlDataReader.ReadAsync())
            {
                var movie = new Movie
                {
                    Id = Convert.ToInt32(sqlDataReader["id"]),
                    Title = Convert.ToString(sqlDataReader["title"]),
                    Description = Convert.ToString(sqlDataReader["description"]),
                    MovieType = (MovieType)Convert.ToInt32(sqlDataReader["movie_type_id"]),
                    Duration = Convert.ToInt32(sqlDataReader["duration"]),
                    ReleaseDate = Convert.ToDateTime(sqlDataReader["release_date"]),
                    AddedDate = Convert.ToDateTime(sqlDataReader["added_date"]),
                    Owned = Convert.ToBoolean(sqlDataReader["owned"])
                };

                movies.Add(movie);
            }

            return movies;
        }

        /// <summary>
        /// Update movie.
        /// </summary>
        /// <param name="movie"><see cref="Movie"/> to update.</param>
        public async Task UpdateMovie(Movie movie)
        {
            await using var sqlConnection = new SqlConnection(_connectionString);
            await sqlConnection.OpenAsync();
            var sqlCommand = sqlConnection.CreateCommand();
            var sqlTransaction = sqlConnection.BeginTransaction();
            sqlCommand.Transaction = sqlTransaction;

            try
            {
                _logger.Debug($"Start updating movie with id: {movie.Id}.");

                await UpdateMovie(movie, sqlCommand);

                await sqlTransaction.CommitAsync();

                _logger.Debug($"Completed updating movie with id {movie.Id}.");
            }
            catch (Exception exception)
            {
                _logger.Error($"{exception.Message} - {exception.StackTrace}");
                await sqlTransaction.RollbackAsync();
                throw;
            }
        }

        /// <summary>
        /// Update movie.
        /// </summary>
        /// <param name="movie"><see cref="Movie"/> to update.</param>
        /// <param name="sqlCommand">The SqlCommand to use when interacting with the database.</param>
        public async Task UpdateMovie(Movie movie, SqlCommand sqlCommand)
        {
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "movies_upd";
            sqlCommand.Parameters.Clear();

            sqlCommand.Parameters.Add("@id", SqlDbType.Int).Value = movie.Id;
            sqlCommand.Parameters.Add("@title", SqlDbType.VarChar, 200).Value = movie.Title;
            sqlCommand.Parameters.Add("@description", SqlDbType.VarChar, 2000).Value = movie.Description;
            sqlCommand.Parameters.Add("@movie_type_id", SqlDbType.Int).Value = movie.MovieType;
            sqlCommand.Parameters.Add("@duration", SqlDbType.Int).Value = movie.Duration;
            sqlCommand.Parameters.Add("@release_date", SqlDbType.DateTime).Value = movie.ReleaseDate;
            sqlCommand.Parameters.Add("@added_date", SqlDbType.DateTime).Value = movie.AddedDate;
            sqlCommand.Parameters.Add("@owned", SqlDbType.Bit).Value = movie.Owned;

            await sqlCommand.ExecuteNonQueryAsync();
        }

        /// <summary>
        /// Delete movie.
        /// </summary>
        /// <param name="id"><see cref="Movie"/> id to delete.</param>
        public async Task DeleteMovie(int id)
        {
            await using var sqlConnection = new SqlConnection(_connectionString);
            await sqlConnection.OpenAsync();
            var sqlCommand = sqlConnection.CreateCommand();
            var sqlTransaction = sqlConnection.BeginTransaction();
            sqlCommand.Transaction = sqlTransaction;

            try
            {
                _logger.Debug($"Start deleting movie with id: {id}.");

                await DeleteMovie(id, sqlCommand);

                await sqlTransaction.CommitAsync();

                _logger.Debug($"Completed deleting movie with id {id}.");
            }
            catch (Exception exception)
            {
                _logger.Error($"{exception.Message} - {exception.StackTrace}");
                await sqlTransaction.RollbackAsync();
                throw;
            }
        }

        /// <summary>
        /// Delete movie.
        /// </summary>
        /// <param name="id"><see cref="Movie"/> id to delete.</param>
        /// <param name="sqlCommand">The SqlCommand to use when interacting with the database.</param>
        public async Task DeleteMovie(int id, SqlCommand sqlCommand)
        {
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "movies_del";
            sqlCommand.Parameters.Clear();

            sqlCommand.Parameters.Add("@id", SqlDbType.Int).Value = id;

            await sqlCommand.ExecuteNonQueryAsync();
        }
    }
}
