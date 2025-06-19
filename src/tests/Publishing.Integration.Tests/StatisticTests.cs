using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
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
            _db = new DapperDbContext(factory);
            _helper = new DbHelper(_db);

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer(cs)
                .Options;
            var efContext = new AppDbContext(options);
            var initializer = new DatabaseInitializer(efContext);
            initializer.InitializeAsync().Wait();

            _db.ExecuteAsync("INSERT INTO Person(FName,LName,emailPerson,typePerson) VALUES('A','B','x@y.com','user');").Wait();
            int id = _db.QueryAsync<int>("SELECT idPerson FROM Person").Result.First();

            _db.ExecuteAsync($"INSERT INTO Orders(idProduct,idPerson,namePrintery,dateOrder,dateStart,dateFinish,statusOrder,tirage,price) VALUES(NULL,{id},'P','2024-01-10','2024-01-10','2024-01-10','done',1,10),(NULL,{id},'P','2024-01-20','2024-01-20','2024-01-20','done',1,10),(NULL,{id},'P','2024-02-05','2024-02-05','2024-02-05','done',1,10)").Wait();
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
