using System.Data;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Publishing.Core.Interfaces;
using Publishing.Infrastructure.DataAccess;
using Publishing.Infrastructure;

namespace Publishing.Integration.Tests;

public class SqliteDbConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;
    private readonly ILogger _logger;

    public SqliteDbConnectionFactory(IConfiguration configuration, ILogger logger)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        _logger = logger;
    }

    public async Task<IDbConnection> CreateOpenConnectionAsync()
    {
        var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();
        return new LoggingDbConnection(connection, _logger);
    }
}
