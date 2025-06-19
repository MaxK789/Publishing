namespace Publishing.Infrastructure
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Dapper;
    using Microsoft.Data.SqlClient;
    using Microsoft.Extensions.Configuration;
    using Publishing.Core.Interfaces;

    public class SqlDbContext : IDbContext
    {
        private readonly string _connectionString;

        public SqlDbContext(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection")!;
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param = null)
        {
            await using var con = new SqlConnection(_connectionString);
            await con.OpenAsync();
            return await con.QueryAsync<T>(sql, param);
        }

        public async Task<int> ExecuteAsync(string sql, object? param = null)
        {
            await using var con = new SqlConnection(_connectionString);
            await con.OpenAsync();
            return await con.ExecuteAsync(sql, param);
        }
    }
}
