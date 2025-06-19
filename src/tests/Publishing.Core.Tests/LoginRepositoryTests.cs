using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Publishing.Core.Interfaces;
using Publishing.Infrastructure.Repositories;

namespace Publishing.Core.Tests
{
    [TestClass]
    public class LoginRepositoryTests
    {
        private class StubDbClient : IDatabaseClient
        {
            public string? LastQuery;
            public List<SqlParameter>? LastParams;
            public string? LastNonQuery;
            public List<SqlParameter>? LastNonParams;
            public string? ExecuteResult = "res";

            public void OpenConnection() { }
            public void OpenConnection(string l, string p) { }
            public System.Data.DataTable ExecuteQueryToDataTable(string q, List<SqlParameter>? p = null) => new();
            public List<string[]> ExecuteQueryList(string q, List<SqlParameter>? p = null) => new();
            public string ExecuteQuery(string q, List<SqlParameter>? p = null)
            {
                LastQuery = q;
                LastParams = p;
                return ExecuteResult ?? string.Empty;
            }
            public void ExecuteQueryWithoutResponse(string q, List<SqlParameter>? p = null)
            {
                LastNonQuery = q;
                LastNonParams = p;
            }
            public void CloseConnection() { }
        }

        [TestMethod]
        public void GetHashedPassword_UsesDatabaseClient()
        {
            var db = new StubDbClient();
            var repo = new LoginRepository(db);
            repo.GetHashedPassword("a@a.com");
            Assert.IsNotNull(db.LastQuery);
            StringAssert.Contains(db.LastQuery!, "password");
            Assert.IsNotNull(db.LastParams);
            Assert.AreEqual(1, db.LastParams!.Count);
            Assert.AreEqual("@Email", db.LastParams[0].ParameterName);
        }

        [TestMethod]
        public void EmailExists_ReturnsTrue_WhenEmailFound()
        {
            var db = new StubDbClient { ExecuteResult = "x@y.com" };
            var repo = new LoginRepository(db);

            bool exists = repo.EmailExists("x@y.com");

            Assert.IsTrue(exists);
            StringAssert.Contains(db.LastQuery!, "emailPerson = @Email");
            Assert.IsNotNull(db.LastParams);
            Assert.AreEqual(1, db.LastParams!.Count);
            Assert.AreEqual("@Email", db.LastParams[0].ParameterName);
        }

        [TestMethod]
        public void InsertPerson_ReturnsInsertedId()
        {
            var db = new StubDbClient { ExecuteResult = "7" };
            var repo = new LoginRepository(db);

            int id = repo.InsertPerson("F", "L", "e", "s");

            Assert.AreEqual(7, id);
            StringAssert.Contains(db.LastQuery!, "INSERT INTO Person");
            Assert.AreEqual(4, db.LastParams!.Count);
        }

        [TestMethod]
        public void InsertPassword_UsesExecuteQueryWithoutResponse()
        {
            var db = new StubDbClient();
            var repo = new LoginRepository(db);

            repo.InsertPassword("hash", 3);

            Assert.IsNotNull(db.LastNonQuery);
            StringAssert.Contains(db.LastNonQuery!, "INSERT INTO Pass");
            Assert.IsNotNull(db.LastNonParams);
            Assert.AreEqual(2, db.LastNonParams!.Count);
        }
    }
}
