using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Linq;
using Publishing.Core.Interfaces;
using Publishing.Infrastructure;

namespace Publishing.Integration.Tests
{
    [TestClass]
    public class StatisticTests
    {
        private const string Server = @"(localdb)\MSSQLLocalDB";
        private const string DbName = "PublishingStat";

        private static string MasterConnection => $"Data Source={Server};Initial Catalog=master;Integrated Security=true";

        private IDbContext _db = null!;
        private IDbHelper _helper = null!;

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
            var factory = new SqlDbConnectionFactory(config);
            _db = new SqlDbContext(factory);
            _helper = new DbHelper(_db);

            _db.ExecuteAsync("CREATE TABLE Person(idPerson INT IDENTITY(1,1) PRIMARY KEY, FName NVARCHAR(50));").Wait();
            _db.ExecuteAsync("CREATE TABLE Orders(idOrder INT IDENTITY(1,1) PRIMARY KEY, idPerson INT, dateOrder DATETIME);").Wait();

            _db.ExecuteAsync("INSERT INTO Person(FName) VALUES('A');").Wait();
            int id = _db.QueryAsync<int>("SELECT idPerson FROM Person").Result.First();

            _db.ExecuteAsync($"INSERT INTO Orders(idPerson,dateOrder) VALUES({id},'2024-01-10'),({id},'2024-01-20'),({id},'2024-02-05')").Wait();
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
        public void Statistic_GeneratesSeries()
        {
            var list = _helper.QueryStringListAsync(
                "SELECT DATENAME(MONTH, dateOrder) AS M, COUNT(*) AS N FROM Orders GROUP BY DATENAME(MONTH, dateOrder) ORDER BY MIN(dateOrder)").Result;
            Assert.AreEqual(2, list.Count);
            Assert.AreEqual("January", list[0][0]);
            Assert.AreEqual("2", list[0][1]);
            Assert.AreEqual("February", list[1][0]);
            Assert.AreEqual("1", list[1][1]);
        }
    }
}
