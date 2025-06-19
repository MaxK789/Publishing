using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Publishing.Core.Interfaces;
using Publishing.Infrastructure.Repositories;

namespace Publishing.Core.Tests
{
    [TestClass]
    public class LoginRepositoryTests
    {
        private class StubDbClient : IDbContext
        {
            public string? LastQuery;
            public IDictionary<string, object>? LastParams;
            public string? LastNonQuery;
            public IDictionary<string, object>? LastNonParams;
            public object? ExecuteResult = "res";

            public Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param = null)
            {
                LastQuery = sql;
                LastParams = param == null ? null :
                    param.GetType().GetProperties().ToDictionary(p => p.Name, p => p.GetValue(param) ?? new object());
                IEnumerable<T> result = new List<T> { (T)Convert.ChangeType(ExecuteResult!, typeof(T)) };
                return Task.FromResult(result);
            }

            public Task<int> ExecuteAsync(string sql, object? param = null)
            {
                LastNonQuery = sql;
                LastNonParams = param == null ? null :
                    param.GetType().GetProperties().ToDictionary(p => p.Name, p => p.GetValue(param) ?? new object());
                return Task.FromResult(0);
            }
        }

        [TestMethod]
        public async Task GetHashedPassword_UsesDatabaseClient()
        {
            var db = new StubDbClient();
            var repo = new LoginRepository(db);
            await repo.GetHashedPasswordAsync("a@a.com");
            Assert.IsNotNull(db.LastQuery);
            StringAssert.Contains(db.LastQuery!, "password");
            Assert.IsNotNull(db.LastParams);
            Assert.AreEqual(1, db.LastParams!.Count);
            Assert.IsTrue(db.LastParams.ContainsKey("Email"));
            Assert.AreEqual("a@a.com", db.LastParams["Email"]);
        }

        [TestMethod]
        public async Task EmailExists_ReturnsTrue_WhenEmailFound()
        {
            var db = new StubDbClient { ExecuteResult = "x@y.com" };
            var repo = new LoginRepository(db);

            bool exists = await repo.EmailExistsAsync("x@y.com");

            Assert.IsTrue(exists);
            StringAssert.Contains(db.LastQuery!, "emailPerson = @Email");
            Assert.IsNotNull(db.LastParams);
            Assert.AreEqual(1, db.LastParams!.Count);
            Assert.IsTrue(db.LastParams.ContainsKey("Email"));
            Assert.AreEqual("x@y.com", db.LastParams["Email"]);
        }

        [TestMethod]
        public async Task InsertPerson_ReturnsInsertedId()
        {
            var db = new StubDbClient { ExecuteResult = 7 };
            var repo = new LoginRepository(db);

            int id = await repo.InsertPersonAsync("F", "L", "e", "s");

            Assert.AreEqual(7, id);
            StringAssert.Contains(db.LastQuery!, "INSERT INTO Person");
            Assert.AreEqual(4, db.LastParams!.Count);
            CollectionAssert.AreEquivalent(
                new[] { "FName", "LName", "Email", "Status" },
                db.LastParams!.Keys.ToArray());
        }

        [TestMethod]
        public async Task InsertPassword_UsesExecuteQueryWithoutResponse()
        {
            var db = new StubDbClient();
            var repo = new LoginRepository(db);

            await repo.InsertPasswordAsync("hash", 3);

            Assert.IsNotNull(db.LastNonQuery);
            StringAssert.Contains(db.LastNonQuery!, "INSERT INTO Pass");
            Assert.IsNotNull(db.LastNonParams);
            Assert.AreEqual(2, db.LastNonParams!.Count);
            CollectionAssert.AreEquivalent(
                new[] { "Password", "Id" },
                db.LastNonParams!.Keys.ToArray());
        }
    }
}
