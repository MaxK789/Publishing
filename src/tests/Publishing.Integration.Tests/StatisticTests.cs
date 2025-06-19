using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Publishing.Infrastructure.Migrations;
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
                ctx.Persons.Add(new Publishing.Infrastructure.Migrations.PersonEntity { FName = "A" });
                ctx.SaveChanges();
                int id = ctx.Persons.First().Id;
                ctx.Orders.AddRange(
                    new Publishing.Infrastructure.Migrations.OrderEntity { PersonId = id, ProductId = 1, DateOrder = new DateTime(2024, 1, 10) },
                    new Publishing.Infrastructure.Migrations.OrderEntity { PersonId = id, ProductId = 1, DateOrder = new DateTime(2024, 1, 20) },
                    new Publishing.Infrastructure.Migrations.OrderEntity { PersonId = id, ProductId = 1, DateOrder = new DateTime(2024, 2, 5) }
                );
                ctx.SaveChanges();
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
        public void Statistic_GeneratesSeries()
        {
            var list = DbContextExtensions.QueryStringListAsync(_db,
                "SELECT DATENAME(MONTH, dateOrder) AS M, COUNT(*) AS N FROM Orders GROUP BY DATENAME(MONTH, dateOrder) ORDER BY MIN(dateOrder)").Result;
            Assert.AreEqual(2, list.Count);
            Assert.AreEqual("January", list[0][0]);
            Assert.AreEqual("2", list[0][1]);
            Assert.AreEqual("February", list[1][0]);
            Assert.AreEqual("1", list[1][1]);
        }
    }
}
