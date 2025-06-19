using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Publishing.Infrastructure.Migrations;
using System.Threading.Tasks;
using System.Linq;
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
            var options = new DbContextOptionsBuilder<Publishing.Infrastructure.Migrations.PublishingDbContext>()
                .UseSqlServer(cs)
                .Options;
            using (var ctx = new Publishing.Infrastructure.Migrations.PublishingDbContext(options))
            {
                ctx.Database.Migrate();
            }
            _db = new SqlDbContext(config);
        }

        [TestCleanup]
        public void Cleanup()
        {
            // nothing to dispose
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
            DataTable dt = DbContextExtensions.QueryDataTableAsync(_db, "SELECT * FROM Sample").Result;
            Assert.AreEqual(2, dt.Rows.Count);
            Assert.AreEqual("name", dt.Columns[1].ColumnName);
        }

        [TestMethod]
        public void ExecuteQueryToDataTable_WithParameters_FiltersData()
        {
            _db.ExecuteAsync("CREATE TABLE Filter(id INT, name NVARCHAR(30))").Wait();
            _db.ExecuteAsync("INSERT INTO Filter(id,name) VALUES(1,'x'),(2,'y')").Wait();
            DataTable dt = DbContextExtensions.QueryDataTableAsync(_db, "SELECT name FROM Filter WHERE id = @id", new { id = 2 }).Result;
            Assert.AreEqual(1, dt.Rows.Count);
            Assert.AreEqual("y", dt.Rows[0][0]);
        }

        [TestMethod]
        public void ExecuteQueryList_ReturnsStringArrays()
        {
            _db.ExecuteAsync("CREATE TABLE Lst(id INT, name NVARCHAR(30))").Wait();
            _db.ExecuteAsync("INSERT INTO Lst(id,name) VALUES(1,'one'),(2,'two')").Wait();
            var list = DbContextExtensions.QueryStringListAsync(_db, "SELECT name FROM Lst ORDER BY id").Result;
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
