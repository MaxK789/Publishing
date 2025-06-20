using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Publishing.Core.Interfaces;
using Publishing.Infrastructure.DataAccess;

namespace Publishing.Infrastructure
{
    public class SqlDbConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connectionString;
        private readonly ILogger _logger;

        public SqlDbConnectionFactory(IConfiguration configuration, ILogger logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
            _logger = logger;
        }

        public async Task<IDbConnection> CreateOpenConnectionAsync()
        {
            var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            return new LoggingDbConnection(connection, _logger);
        }
    }
}
