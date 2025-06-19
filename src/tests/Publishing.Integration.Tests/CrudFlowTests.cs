using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Publishing.Core.Interfaces;
using Publishing.Infrastructure;
using BCrypt.Net;
using Publishing.Core.Services;
using Publishing.Infrastructure.Repositories;

namespace Publishing.Integration.Tests
{
    [TestClass]
    public class CrudFlowTests
    {
        private const string Server = @"(localdb)\MSSQLLocalDB";
        private const string DbName = "PublishingCrud";

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
            var factory = new SqlDbConnectionFactory(config);
            _db = new DapperDbContext(factory);

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer(cs)
                .Options;
            var efContext = new AppDbContext(options);
            var initializer = new DatabaseInitializer(efContext);
            initializer.InitializeAsync().Wait();
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
        public void Registration_InsertsPerson()
        {
            _db.ExecuteAsync("INSERT INTO Person(FName,LName,emailPerson,typePerson) VALUES('A','B','a@b.com','user')").Wait();
            var result = _db.QueryAsync<int>("SELECT COUNT(*) FROM Person").Result.First();
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void Login_SetsCurrentUser()
        {
            string hash = BCrypt.Net.BCrypt.HashPassword("pass", 11);
            _db.ExecuteAsync("INSERT INTO Person(FName,LName,emailPerson,typePerson) VALUES('A','B','c@d.com','user');").Wait();
            string id = _db.QueryAsync<int>("SELECT idPerson FROM Person WHERE emailPerson='c@d.com'").Result.First().ToString();
            _db.ExecuteAsync($"INSERT INTO Pass(password,idPerson) VALUES('{hash}', {id})").Wait();

            var service = new AuthService(new LoginRepository(_db));
            var user = service.Authenticate("c@d.com", "pass");

            Assert.IsNotNull(user);
            Assert.AreEqual(id, user!.Id);
        }

        [TestMethod]
        public void AddOrder_InsertsOrder()
        {
            _db.ExecuteAsync("INSERT INTO Person(FName,LName,emailPerson,typePerson) VALUES('A','B','e@f.com','user');").Wait();
            int id = _db.QueryAsync<int>("SELECT idPerson FROM Person WHERE emailPerson='e@f.com'").Result.First();

            _db.ExecuteAsync($"INSERT INTO Product(idPerson,typeProduct,nameProduct,pagesNum) VALUES({id},'book','Title',10)").Wait();
            int idProd = _db.QueryAsync<int>("SELECT idProduct FROM Product WHERE nameProduct='Title'").Result.First();

            _db.ExecuteAsync($"INSERT INTO Orders(idProduct,idPerson,namePrintery,dateOrder,dateStart,dateFinish,statusOrder,tirage,price) VALUES({idProd},{id},'P',GETDATE(),GETDATE(),GETDATE(),'в роботі',5,50)").Wait();
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
            _db.ExecuteAsync($"INSERT INTO Orders(idProduct,idPerson,namePrintery,dateOrder,dateStart,dateFinish,statusOrder,tirage,price) VALUES({idProd},{id},'P',GETDATE(),GETDATE(),GETDATE(),'в роботі',5,50)").Wait();
            int idOrder = _db.QueryAsync<int>("SELECT idOrder FROM Orders").Result.First();

            _db.ExecuteAsync("DELETE FROM Orders WHERE idOrder=@id", new { id = idOrder }).Wait();

            int count2 = _db.QueryAsync<int>("SELECT COUNT(*) FROM Orders").Result.First();
            Assert.AreEqual(0, count2);
        }
    }
}
