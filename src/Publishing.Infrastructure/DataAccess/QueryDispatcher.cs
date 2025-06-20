using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Publishing.Core.Interfaces;

namespace Publishing.Infrastructure.DataAccess;

public sealed class QueryDispatcher : IQueryDispatcher
{
    private readonly IDbConnectionFactory _factory;
    private readonly ILogger _logger;
    private readonly ActivitySource _activitySource = new("QueryDispatcher");

    public QueryDispatcher(IDbConnectionFactory factory, ILogger logger)
    {
        _factory = factory;
        _logger = logger;
    }

    public async Task<List<T>> QueryAsync<T>(SqlQuery<T> query, CancellationToken token = default)
    {
        using var activity = _activitySource.StartActivity(query.Sql);
        using var con = await _factory.CreateOpenConnectionAsync();
        var list = new List<T>();
        using var reader = await con.ExecuteReaderAsync(new CommandDefinition(query.Sql, query.Parameters, cancellationToken: token));
        while (reader.Read())
        {
            list.Add(query.Map(reader));
        }
        _logger.LogInformation($"SQL: {query.Sql}; rows: {list.Count}");
        return list;
    }

    public async Task<T?> QuerySingleAsync<T>(SqlQuery<T> query, CancellationToken token = default)
    {
        var list = await QueryAsync(query, token);
        return list.Count > 0 ? list[0] : default;
    }
}
