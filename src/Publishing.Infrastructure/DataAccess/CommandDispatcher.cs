using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Publishing.Core.Interfaces;

namespace Publishing.Infrastructure.DataAccess;

public sealed class CommandDispatcher : ICommandDispatcher
{
    private readonly IDbConnectionFactory _factory;
    private readonly ILogger _logger;
    private readonly ActivitySource _activitySource = new("CommandDispatcher");

    public CommandDispatcher(IDbConnectionFactory factory, ILogger logger)
    {
        _factory = factory;
        _logger = logger;
    }

    public async Task<int> ExecuteAsync(SqlCommand command, CancellationToken token = default)
    {
        using var activity = _activitySource.StartActivity(command.Sql);
        using var con = await _factory.CreateOpenConnectionAsync();
        var affected = await con.ExecuteAsync(new CommandDefinition(command.Sql, command.Parameters, cancellationToken: token));
        _logger.LogInformation($"SQL: {command.Sql}; rows: {affected}");
        return affected;
    }
}
