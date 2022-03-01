using System.Collections.Generic;
using System.Data.SqlClient;

namespace LegacyVideos.WebApi.IntegrationTests.Common.Helpers
{
    public class DatabaseDriver
    {
        private readonly string _connectionString;

        public DatabaseDriver(string connectionString)
        {
            _connectionString = connectionString;
        }

        private static List<Dictionary<string, string>> RunSql(string connectionString, string sql)
        {
            var rows = new List<Dictionary<string, string>>();

            using var connection = new SqlConnection(connectionString);
            connection.Open();
            using (var command = new SqlCommand(sql, connection))
            {
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var row = new Dictionary<string, string>();

                    for (var i = 0; i < reader.FieldCount; i++)
                    {
                        var key = reader.GetName(i);
                        var value = reader.GetValue(i);
                        row.Add(key, value?.ToString());
                    }

                    rows.Add(row);
                }
            }
            connection.Close();

            return rows;
        }

        public List<Dictionary<string, string>> RunSql(string sql)
        {
            return RunSql(_connectionString, sql);
        }
    }
}