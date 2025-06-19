namespace Publishing.Infrastructure
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Dapper;
    using Publishing.Core.Interfaces;

    public class SqlDbContext : IDbContext
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public SqlDbContext(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param = null)
        {
            await using var con = await _connectionFactory.CreateOpenConnectionAsync();
            return await con.QueryAsync<T>(sql, param);
        }

        public async Task<int> ExecuteAsync(string sql, object? param = null)
        {
            await using var con = await _connectionFactory.CreateOpenConnectionAsync();
            return await con.ExecuteAsync(sql, param);
        }
    }
}
