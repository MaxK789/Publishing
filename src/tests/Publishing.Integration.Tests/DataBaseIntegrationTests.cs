using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Publishing.Services;
using Publishing.Core.Interfaces;
using Publishing.Infrastructure;
using System.IO;

namespace Publishing.Integration.Tests
{
    [TestClass]
    public class DataBaseIntegrationTests
    {
        private string _dbPath = null!;
        private string ConnectionString => $"Data Source={_dbPath}";

        private IDbContext _db = null!;
        private IDbHelper _helper = null!;
        private ServiceProvider _serviceProvider = null!;

        [TestInitialize]
        public async Task Setup()
        {
            _dbPath = Path.Combine(Path.GetTempPath(), $"PublishingTest_{Guid.NewGuid()}.db");
            if (File.Exists(_dbPath))
            {
                File.Delete(_dbPath);
            }
            var builder = new SqliteConnectionStringBuilder(ConnectionString)
            {
                Pooling = false
            };
            var cs = builder.ToString();
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["ConnectionStrings:DefaultConnection"] = cs
                })
                .Build();
            var services = new ServiceCollection();
            services.AddSingleton<IConfiguration>(config);
            services.AddSingleton<IUiNotifier, SilentUiNotifier>();
            services.AddTransient<ILogger, LoggerService>();
            services.AddTransient<IDbConnectionFactory, SqliteDbConnectionFactory>();
            services.AddTransient<IDbContext, DapperDbContext>();
            services.AddTransient<IDbHelper, DbHelper>();
            services.AddDbContext<AppDbContext>(o => o.UseSqlite(cs));
            services.AddTransient<IDatabaseInitializer, DatabaseInitializer>();

            _serviceProvider = services.BuildServiceProvider();

            using var scope = _serviceProvider.CreateScope();
            await scope.ServiceProvider.GetRequiredService<IDatabaseInitializer>()
                .InitializeAsync();

            _db = scope.ServiceProvider.GetRequiredService<IDbContext>();
            _helper = scope.ServiceProvider.GetRequiredService<IDbHelper>();
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (_serviceProvider is not null)
            {
                _serviceProvider.Dispose();
            }
            if (File.Exists(_dbPath))
            {
                File.Delete(_dbPath);
            }
        }


        [TestMethod]
        public void OpenConnection_StateIsOpen()
        {
            var result = _db.QueryAsync<int>("SELECT 1").Result;
            Assert.AreEqual(1, result.First());
        }

        [TestMethod]
        public void ExecuteQuery_InsertAndSelect_ReturnsValue()
        {
            _db.ExecuteAsync("CREATE TABLE Settings(id INTEGER PRIMARY KEY AUTOINCREMENT, value INT)").Wait();
            _db.ExecuteAsync("INSERT INTO Settings(value) VALUES(42)").Wait();
            var result = _db.QueryAsync<int>("SELECT value FROM Settings WHERE id = 1").Result.First();
            Assert.AreEqual(42, result);
        }

        [TestMethod]
        public void ExecuteQueryToDataTable_ReturnsRows()
        {
            _db.ExecuteAsync("CREATE TABLE Sample(id INT, name NVARCHAR(30))").Wait();
            _db.ExecuteAsync("INSERT INTO Sample(id,name) VALUES(1,'a'),(2,'b')").Wait();
            DataTable dt = _helper.QueryDataTableAsync("SELECT * FROM Sample").Result;
            Assert.AreEqual(2, dt.Rows.Count);
            Assert.AreEqual("name", dt.Columns[1].ColumnName);
        }

        [TestMethod]
        public void ExecuteQueryToDataTable_WithParameters_FiltersData()
        {
            _db.ExecuteAsync("CREATE TABLE Filter(id INT, name NVARCHAR(30))").Wait();
            _db.ExecuteAsync("INSERT INTO Filter(id,name) VALUES(1,'x'),(2,'y')").Wait();
            DataTable dt = _helper.QueryDataTableAsync("SELECT name FROM Filter WHERE id = @id", new { id = 2 }).Result;
            Assert.AreEqual(1, dt.Rows.Count);
            Assert.AreEqual("y", dt.Rows[0][0]);
        }

        [TestMethod]
        public void ExecuteQueryList_ReturnsStringArrays()
        {
            _db.ExecuteAsync("CREATE TABLE Lst(id INT, name NVARCHAR(30))").Wait();
            _db.ExecuteAsync("INSERT INTO Lst(id,name) VALUES(1,'one'),(2,'two')").Wait();
            var list = _helper.QueryStringListAsync("SELECT name FROM Lst ORDER BY id").Result;
            Assert.AreEqual(2, list.Count);
            CollectionAssert.AreEqual(new[] { "one", "two" }, new[] { list[0][0], list[1][0] });
        }

        [TestMethod]
        public void ExecuteQueryWithoutResponse_UpdatesRow()
        {
            _db.ExecuteAsync("CREATE TABLE Upd(id INT, name NVARCHAR(30))").Wait();
            _db.ExecuteAsync("INSERT INTO Upd(id,name) VALUES(1,'old')").Wait();
            _db.ExecuteAsync("UPDATE Upd SET name='new' WHERE id=1").Wait();
            var val = _db.QueryAsync<string>("SELECT name FROM Upd WHERE id=1").Result.First();
            Assert.AreEqual("new", val);
        }
    }
}
