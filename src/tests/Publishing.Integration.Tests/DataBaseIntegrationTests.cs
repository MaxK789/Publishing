using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Publishing.Core.Interfaces;
using Publishing.Infrastructure;

namespace Publishing.Integration.Tests
{
    [TestClass]
    public class DataBaseIntegrationTests
    {
        private const string Server = @"(localdb)\MSSQLLocalDB";
        private const string DbName = "PublishingTest";

        private static string MasterConnection => $"Data Source={Server};Initial Catalog=master;Integrated Security=true";

        private IDbContext _db = null!;
        private IDbHelper _helper = null!;
        private ServiceProvider _serviceProvider = null!;

        [TestInitialize]
        public void Setup()
        {
            using (var con = new SqlConnection(MasterConnection))
            {
                con.Open();
                using var cmd = con.CreateCommand();
                cmd.CommandText = $@"
IF DB_ID('{DbName}') IS NOT NULL
BEGIN
    ALTER DATABASE [{DbName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE [{DbName}];
END
CREATE DATABASE [{DbName}];";
                cmd.ExecuteNonQuery();
            }

            var cs = $"Data Source={Server};Initial Catalog={DbName};Integrated Security=true";
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    ["ConnectionStrings:DefaultConnection"] = cs
                })
                .Build();
            var services = new ServiceCollection();
            services.AddSingleton<IConfiguration>(config);
            services.AddTransient<IDbConnectionFactory, SqlDbConnectionFactory>();
            services.AddTransient<IDbContext, DapperDbContext>();
            services.AddTransient<IDbHelper, DbHelper>();
            services.AddDbContext<AppDbContext>(o => o.UseSqlServer(cs));
            services.AddTransient<IDatabaseInitializer, DatabaseInitializer>();

            _serviceProvider = services.BuildServiceProvider();

            using var scope = _serviceProvider.CreateScope();
            scope.ServiceProvider.GetRequiredService<IDatabaseInitializer>()
                .InitializeAsync().Wait();

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
            using (var con = new SqlConnection(MasterConnection))
            {
                con.Open();
                using var cmd = con.CreateCommand();
                cmd.CommandText = $@"
IF DB_ID('{DbName}') IS NOT NULL
BEGIN
    ALTER DATABASE [{DbName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE [{DbName}];
END";
                cmd.ExecuteNonQuery();
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
            _db.ExecuteAsync("CREATE TABLE Settings(id INT IDENTITY(1,1), value INT)").Wait();
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
