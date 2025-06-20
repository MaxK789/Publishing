using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Publishing.Core.Interfaces;
using Publishing.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

namespace Publishing.Integration.Tests
{
    [TestClass]
    public class StatisticTests
    {
        private string _dbPath = null!;
        private string ConnectionString => $"Data Source={_dbPath}";

        private IDbContext _db = null!;
        private IDbHelper _helper = null!;
        private ServiceProvider _serviceProvider = null!;

        [TestInitialize]
        public void Setup()
        {
            _dbPath = Path.Combine(Path.GetTempPath(), $"PublishingStat_{Guid.NewGuid()}.db");
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
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    ["ConnectionStrings:DefaultConnection"] = cs
                })
                .Build();
            var services = new ServiceCollection();
            services.AddSingleton<IConfiguration>(config);
            services.AddTransient<ILogger, LoggerService>();
            services.AddTransient<IDbConnectionFactory, SqliteDbConnectionFactory>();
            services.AddTransient<IDbContext, DapperDbContext>();
            services.AddTransient<IDbHelper, DbHelper>();
            services.AddDbContext<AppDbContext>(o => o.UseSqlite(cs));
            services.AddTransient<IDatabaseInitializer, DatabaseInitializer>();

            _serviceProvider = services.BuildServiceProvider();

            using var scope = _serviceProvider.CreateScope();
            scope.ServiceProvider.GetRequiredService<IDatabaseInitializer>()
                .InitializeAsync().Wait();
            _db = scope.ServiceProvider.GetRequiredService<IDbContext>();
            _helper = scope.ServiceProvider.GetRequiredService<IDbHelper>();

            _db.ExecuteAsync("INSERT INTO Person(FName,LName,emailPerson,typePerson) VALUES('A','B','x@y.com','user');").Wait();
            int id = _db.QueryAsync<int>("SELECT idPerson FROM Person").Result.First();

            _db.ExecuteAsync($"INSERT INTO Orders(idProduct,idPerson,namePrintery,dateOrder,dateStart,dateFinish,statusOrder,tirage,price) VALUES(NULL,{id},'P','2024-01-10','2024-01-10','2024-01-10','done',1,10),(NULL,{id},'P','2024-01-20','2024-01-20','2024-01-20','done',1,10),(NULL,{id},'P','2024-02-05','2024-02-05','2024-02-05','done',1,10)").Wait();
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
        public void Statistic_GeneratesSeries()
        {
            var list = _helper.QueryStringListAsync(
                "SELECT CASE strftime('%m', dateOrder) WHEN '01' THEN 'January' WHEN '02' THEN 'February' WHEN '03' THEN 'March' WHEN '04' THEN 'April' WHEN '05' THEN 'May' WHEN '06' THEN 'June' WHEN '07' THEN 'July' WHEN '08' THEN 'August' WHEN '09' THEN 'September' WHEN '10' THEN 'October' WHEN '11' THEN 'November' ELSE 'December' END AS M, COUNT(*) AS N FROM Orders GROUP BY strftime('%m', dateOrder) ORDER BY MIN(dateOrder)").Result;
            Assert.AreEqual(2, list.Count);
            Assert.AreEqual("January", list[0][0]);
            Assert.AreEqual("2", list[0][1]);
            Assert.AreEqual("February", list[1][0]);
            Assert.AreEqual("1", list[1][1]);
        }
    }
}
