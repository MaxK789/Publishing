namespace Publishing.Infrastructure
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Dapper;
    using Microsoft.Data.SqlClient;
    using Publishing.Core.Interfaces;

    public class SqlDbContext : IDbContext
    {
        private readonly string _connectionString;

        public SqlDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param = null)
        {
            await using var con = new SqlConnection(_connectionString);
            return await con.QueryAsync<T>(sql, param);
        }

        public async Task<int> ExecuteAsync(string sql, object? param = null)
        {
            await using var con = new SqlConnection(_connectionString);
            return await con.ExecuteAsync(sql, param);
        }
    }
}
