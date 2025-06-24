using Microsoft.VisualStudio.TestTools.UnitTesting;
using Publishing.Infrastructure.DataAccess;
using Publishing.Infrastructure;
using Publishing.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Publishing.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;
using System.Data;
using Dapper;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using System;
using System.IO;

namespace Publishing.Integration.Tests;

[TestClass]
public class SqlQueryTests
{
    private string _dbPath = null!;
    private ServiceProvider _sp = null!;
    private string _cs = null!;

    [TestInitialize]
    public void Init()
    {
        _dbPath = Path.Combine(Path.GetTempPath(), $"SqlQueryTest_{Guid.NewGuid()}.db");
        if (File.Exists(_dbPath))
        {
            File.Delete(_dbPath);
        }
        _cs = $"Data Source={_dbPath}";

        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:DefaultConnection"] = _cs
            })
            .Build();
        var services = new ServiceCollection();
        services.AddSingleton<IConfiguration>(config);
        services.AddTransient<ILogger, LoggerService>();
        services.AddSingleton<IUiNotifier, SilentUiNotifier>();
        services.AddSingleton<IDbConnectionFactory>(sp =>
            new SqliteDbConnectionFactory(sp.GetRequiredService<IConfiguration>(), sp.GetRequiredService<ILogger>()));
        services.AddTransient<IDbContext, DapperDbContext>();
        services.AddMemoryCache();
        services.AddSingleton<QueryDispatcher>();
        services.AddSingleton<IQueryDispatcher>(sp =>
            new MemoryCacheQueryDispatcher(
                sp.GetRequiredService<QueryDispatcher>(),
                sp.GetRequiredService<IMemoryCache>(),
                TimeSpan.FromMinutes(10)));
        services.AddSingleton<ICommandDispatcher, CommandDispatcher>();
        _sp = services.BuildServiceProvider();
    }

    [TestCleanup]
    public void Cleanup()
    {
        if (_sp != null) _sp.Dispose();
        if (File.Exists(_dbPath))
        {
            File.Delete(_dbPath);
        }
    }

    [TestMethod]
    public async Task Execute_InsertAndSelect_Works()
    {
        using var scope = _sp.CreateScope();
        var factory = scope.ServiceProvider.GetRequiredService<IDbConnectionFactory>();
        using var con = await factory.CreateOpenConnectionAsync();
        await con.ExecuteAsync("CREATE TABLE Sample(id INT PRIMARY KEY, val INT)");
        var cmdDispatcher = scope.ServiceProvider.GetRequiredService<ICommandDispatcher>();
        await cmdDispatcher.ExecuteAsync(new RawSqlCommand("INSERT INTO Sample(id,val) VALUES(1,42)"));
        var queryDispatcher = scope.ServiceProvider.GetRequiredService<IQueryDispatcher>();
        var result = await queryDispatcher.QuerySingleAsync(new RawScalarQuery<int>("SELECT val FROM Sample WHERE id=1"));
        Assert.AreEqual(42, result);
    }

    [TestMethod]
    public async Task Execute_UpdateAndSelect_Works()
    {
        using var scope = _sp.CreateScope();
        var factory = scope.ServiceProvider.GetRequiredService<IDbConnectionFactory>();
        using var con = await factory.CreateOpenConnectionAsync();
        await con.ExecuteAsync("CREATE TABLE Sample(id INT PRIMARY KEY, val INT)");
        var cmdDispatcher = scope.ServiceProvider.GetRequiredService<ICommandDispatcher>();
        await cmdDispatcher.ExecuteAsync(new RawSqlCommand("INSERT INTO Sample(id,val) VALUES(1,42)"));
        await cmdDispatcher.ExecuteAsync(new RawSqlCommand("UPDATE Sample SET val=99 WHERE id=1"));
        var queryDispatcher = scope.ServiceProvider.GetRequiredService<IQueryDispatcher>();
        var result = await queryDispatcher.QuerySingleAsync(new RawScalarQuery<int>("SELECT val FROM Sample WHERE id=1"));
        Assert.AreEqual(99, result);
    }

    [TestMethod]
    public async Task Execute_DeleteAndCount_Works()
    {
        using var scope = _sp.CreateScope();
        var factory = scope.ServiceProvider.GetRequiredService<IDbConnectionFactory>();
        using var con = await factory.CreateOpenConnectionAsync();
        await con.ExecuteAsync("CREATE TABLE Sample(id INT PRIMARY KEY, val INT)");
        var cmdDispatcher = scope.ServiceProvider.GetRequiredService<ICommandDispatcher>();
        await cmdDispatcher.ExecuteAsync(new RawSqlCommand("INSERT INTO Sample(id,val) VALUES(1,42)"));
        await cmdDispatcher.ExecuteAsync(new RawSqlCommand("DELETE FROM Sample WHERE id=1"));
        var queryDispatcher = scope.ServiceProvider.GetRequiredService<IQueryDispatcher>();
        var count = await queryDispatcher.QuerySingleAsync(new RawScalarQuery<int>("SELECT COUNT(*) FROM Sample"));
        Assert.AreEqual(0, count);
    }

    private class RawSqlCommand : Publishing.Infrastructure.DataAccess.SqlCommand
    {
        public RawSqlCommand(string sqlText) { SqlText = sqlText; }
        private string SqlText { get; }
        public override string Sql => SqlText;
    }

    private class RawScalarQuery<T> : Publishing.Infrastructure.DataAccess.SqlQuery<T>
    {
        public RawScalarQuery(string sqlText) { SqlText = sqlText; }
        private string SqlText { get; }
        public override string Sql => SqlText;
        public override T Map(IDataReader reader) => (T)Convert.ChangeType(reader.GetValue(0), typeof(T));
    }

}
