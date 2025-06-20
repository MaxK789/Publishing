using System.Data.Common;
using System.Data;
using Publishing.Core.Interfaces;
using System.Diagnostics;

namespace Publishing.Infrastructure.DataAccess;

public class LoggingDbConnection : DbConnection
{
    private readonly DbConnection _inner;
    private readonly ILogger _logger;

    public LoggingDbConnection(DbConnection inner, ILogger logger)
    {
        _inner = inner;
        _logger = logger;
    }

    protected override DbTransaction BeginDbTransaction(System.Data.IsolationLevel isolationLevel) => _inner.BeginTransaction(isolationLevel);
    public override void Close() => _inner.Close();
    public override void ChangeDatabase(string databaseName) => _inner.ChangeDatabase(databaseName);
    public override string ConnectionString { get => _inner.ConnectionString; set => _inner.ConnectionString = value; }
    public override string Database => _inner.Database;
    public override string DataSource => _inner.DataSource;
    public override string ServerVersion => _inner.ServerVersion;
    public override System.Data.ConnectionState State => _inner.State;
    public override void Open() => _inner.Open();

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _inner.Dispose();
        }
        base.Dispose(disposing);
    }

    protected override DbCommand CreateDbCommand() => new LoggingDbCommand(_inner.CreateCommand(), _logger);
}

internal class LoggingDbCommand : DbCommand
{
    private readonly DbCommand _inner;
    private readonly ILogger _logger;

    public LoggingDbCommand(DbCommand inner, ILogger logger)
    {
        _inner = inner;
        _logger = logger;
    }

    public override string CommandText { get => _inner.CommandText; set => _inner.CommandText = value; }
    public override int CommandTimeout { get => _inner.CommandTimeout; set => _inner.CommandTimeout = value; }
    public override System.Data.CommandType CommandType { get => _inner.CommandType; set => _inner.CommandType = value; }
    public override bool DesignTimeVisible { get => _inner.DesignTimeVisible; set => _inner.DesignTimeVisible = value; }
    public override UpdateRowSource UpdatedRowSource { get => _inner.UpdatedRowSource; set => _inner.UpdatedRowSource = value; }
    protected override DbConnection DbConnection { get => _inner.Connection!; set => _inner.Connection = value; }
    protected override DbParameterCollection DbParameterCollection => _inner.Parameters;
    protected override DbTransaction DbTransaction { get => _inner.Transaction!; set => _inner.Transaction = value; }
    public override void Cancel() => _inner.Cancel();

    protected override DbParameter CreateDbParameter() => _inner.CreateParameter();

    protected override DbDataReader ExecuteDbDataReader(System.Data.CommandBehavior behavior)
    {
        _logger.LogInformation($"SQL: {CommandText}");
        var sw = Stopwatch.StartNew();
        var reader = _inner.ExecuteReader(behavior);
        sw.Stop();
        _logger.LogInformation($"Elapsed {sw.ElapsedMilliseconds} ms");
        return reader;
    }

    public override int ExecuteNonQuery()
    {
        _logger.LogInformation($"SQL: {CommandText}");
        var sw = Stopwatch.StartNew();
        var result = _inner.ExecuteNonQuery();
        sw.Stop();
        _logger.LogInformation($"Affected {result} rows in {sw.ElapsedMilliseconds} ms");
        return result;
    }

    public override object ExecuteScalar()
    {
        _logger.LogInformation($"SQL: {CommandText}");
        var sw = Stopwatch.StartNew();
        var result = _inner.ExecuteScalar();
        sw.Stop();
        _logger.LogInformation($"Elapsed {sw.ElapsedMilliseconds} ms");
        return result!;
    }

    public override void Prepare() => _inner.Prepare();
}
