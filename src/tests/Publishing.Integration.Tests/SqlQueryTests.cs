using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Publishing.Infrastructure.DataAccess;
using Publishing.Infrastructure;
using Publishing.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;
using System.Data;
using Dapper;
using System.Linq;
using System.Threading;
using System.Collections.Generic;

namespace Publishing.Integration.Tests;

[TestClass]
public class SqlQueryTests
{
    private TestcontainerDatabase _dbContainer = null!;
    private ServiceProvider _sp = null!;

    [TestInitialize]
    public async Task Init()
    {
        _dbContainer = new TestcontainersBuilder<MsSqlTestcontainer>()
            .WithDatabase(new MsSqlTestcontainerConfiguration())
            .Build();
        await _dbContainer.StartAsync();

        var services = new ServiceCollection();
        services.AddTransient<ILogger, LoggerService>();
        services.AddSingleton<IDbConnectionFactory>(sp =>
            new SqlDbConnectionFactory(new TestConfiguration(_dbContainer.ConnectionString), sp.GetRequiredService<ILogger>()));
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
    public async Task Cleanup()
    {
        if (_sp != null) _sp.Dispose();
        if (_dbContainer != null) await _dbContainer.StopAsync();
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

    private record RawSqlCommand(string SqlText) : SqlCommand
    {
        public override string Sql => SqlText;
    }

    private record RawScalarQuery<T>(string SqlText) : SqlQuery<T>
    {
        public override string Sql => SqlText;
        public override T Map(IDataReader reader) => (T)reader.GetValue(0);
    }

    private class TestConfiguration : Microsoft.Extensions.Configuration.IConfiguration
    {
        private readonly string _cs;
        public TestConfiguration(string cs) { _cs = cs; }
        public string? this[string key] { get => key == "ConnectionStrings:DefaultConnection" ? _cs : null; set { } }
        public IEnumerable<IConfigurationSection> GetChildren() => Enumerable.Empty<IConfigurationSection>();
        public IChangeToken GetReloadToken() => new Microsoft.Extensions.Primitives.CancellationChangeToken(new CancellationTokenSource().Token);
        public IConfigurationSection GetSection(string key) => new ConfigurationSection(this, key);
        private class ConfigurationSection : IConfigurationSection
        {
            private readonly IConfiguration _cfg; private readonly string _key;
            public ConfigurationSection(IConfiguration cfg, string key){_cfg=cfg;_key=key;}
            public string Key => _key; public string Path => _key; public string? Value {get=>_cfg[_key];set{}}
            public string this[string key] { get => _cfg[$"{_key}:{key}"]!; set { } }
            public IEnumerable<IConfigurationSection> GetChildren() => Enumerable.Empty<IConfigurationSection>();
            public IChangeToken GetReloadToken() => _cfg.GetReloadToken();
            public IConfigurationSection GetSection(string key) => new ConfigurationSection(_cfg,$"{_key}:{key}");
        }
    }
}
