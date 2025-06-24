using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.Sqlite;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Publishing.Core.Interfaces;
using Publishing.Infrastructure;
using BCrypt.Net;
using Publishing.Core.Services;
using Publishing.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Publishing.Services;

class StubJwt : IJwtFactory
{
    public string GenerateToken(Publishing.Core.DTOs.UserDto user) => "tkn";
}

namespace Publishing.Integration.Tests
{
    [TestClass]
    public class CrudFlowTests
    {
        private string _dbPath = null!;
        private string ConnectionString => $"Data Source={_dbPath}";

        private IDbContext _db = null!;
        private ServiceProvider _serviceProvider = null!;

        [TestInitialize]
        public async Task Setup()
        {
            _dbPath = Path.Combine(Path.GetTempPath(), $"PublishingCrud_{Guid.NewGuid()}.db");
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
            services.AddDbContext<AppDbContext>(o => o.UseSqlite(cs));
            services.AddTransient<IDatabaseInitializer, DatabaseInitializer>();
            services.AddTransient<ILoginRepository, LoginRepository>();

            _serviceProvider = services.BuildServiceProvider();
            using var scope = _serviceProvider.CreateScope();
            await scope.ServiceProvider.GetRequiredService<IDatabaseInitializer>()
                .InitializeAsync();
            _db = scope.ServiceProvider.GetRequiredService<IDbContext>();
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
        public void Registration_InsertsPerson()
        {
            _db.ExecuteAsync("INSERT INTO Person(FName,LName,emailPerson,typePerson) VALUES('A','B','a@b.com','user')").Wait();
            var result = _db.QueryAsync<int>("SELECT COUNT(*) FROM Person").Result.First();
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public async Task Login_SetsCurrentUser()
        {
            string hash = BCrypt.Net.BCrypt.HashPassword("pass", 11);
            _db.ExecuteAsync("INSERT INTO Person(FName,LName,emailPerson,typePerson) VALUES('A','B','c@d.com','user');").Wait();
            string id = _db.QueryAsync<int>("SELECT idPerson FROM Person WHERE emailPerson='c@d.com'").Result.First().ToString();
            _db.ExecuteAsync($"INSERT INTO Pass(password,idPerson) VALUES('{hash}', {id})").Wait();

            var service = new AuthService(new LoginRepository(_db), new StubJwt());
            var result = await service.AuthenticateAsync("c@d.com", "pass");

            Assert.IsNotNull(result);
            Assert.AreEqual(id, result!.User.Id);
        }

        [TestMethod]
        public void AddOrder_InsertsOrder()
        {
            _db.ExecuteAsync("INSERT INTO Person(FName,LName,emailPerson,typePerson) VALUES('A','B','e@f.com','user');").Wait();
            int id = _db.QueryAsync<int>("SELECT idPerson FROM Person WHERE emailPerson='e@f.com'").Result.First();

            _db.ExecuteAsync($"INSERT INTO Product(idPerson,typeProduct,nameProduct,pagesNum) VALUES({id},'book','Title',10)").Wait();
            int idProd = _db.QueryAsync<int>("SELECT idProduct FROM Product WHERE nameProduct='Title'").Result.First();

            _db.ExecuteAsync($"INSERT INTO Orders(idProduct,idPerson,namePrintery,dateOrder,dateStart,dateFinish,statusOrder,tirage,price) VALUES({idProd},{id},'P','2024-01-01','2024-01-01','2024-01-01','в роботі',5,50)").Wait();
            int count = _db.QueryAsync<int>("SELECT COUNT(*) FROM Orders").Result.First();
            Assert.AreEqual(1, count);
        }

        [TestMethod]
        public void DeleteOrder_RemovesRow()
        {
            _db.ExecuteAsync("INSERT INTO Person(FName,LName,emailPerson,typePerson) VALUES('A','B','g@h.com','user');").Wait();
            int id = _db.QueryAsync<int>("SELECT idPerson FROM Person WHERE emailPerson='g@h.com'").Result.First();
            _db.ExecuteAsync($"INSERT INTO Product(idPerson,typeProduct,nameProduct,pagesNum) VALUES({id},'book','Del',10)").Wait();
            int idProd = _db.QueryAsync<int>("SELECT idProduct FROM Product WHERE nameProduct='Del'").Result.First();
            _db.ExecuteAsync($"INSERT INTO Orders(idProduct,idPerson,namePrintery,dateOrder,dateStart,dateFinish,statusOrder,tirage,price) VALUES({idProd},{id},'P','2024-01-01','2024-01-01','2024-01-01','в роботі',5,50)").Wait();
            int idOrder = _db.QueryAsync<int>("SELECT idOrder FROM Orders").Result.First();

            _db.ExecuteAsync("DELETE FROM Orders WHERE idOrder=@id", new { id = idOrder }).Wait();

            int count2 = _db.QueryAsync<int>("SELECT COUNT(*) FROM Orders").Result.First();
            Assert.AreEqual(0, count2);
        }
    }
}
