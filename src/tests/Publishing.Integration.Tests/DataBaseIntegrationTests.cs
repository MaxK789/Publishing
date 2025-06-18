using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Reflection;

namespace Publishing.Integration.Tests
{
    [TestClass]
    public class DataBaseIntegrationTests
    {
        private const string Server = @"(localdb)\MSSQLLocalDB";
        private const string DbName = "PublishingTest";

        private static string MasterConnection => $"Data Source={Server};Initial Catalog=master;Integrated Security=true";

        [TestInitialize]
        public void Setup()
        {
            using (var con = new SqlConnection(MasterConnection))
            {
                con.Open();
                using var cmd = con.CreateCommand();
                cmd.CommandText = $"IF DB_ID('{DbName}') IS NOT NULL DROP DATABASE [{DbName}]; CREATE DATABASE [{DbName}];";
                cmd.ExecuteNonQuery();
            }

            SetInternalFields(Server, DbName);
            Publishing.DataBase.OpenConnection();
        }

        [TestCleanup]
        public void Cleanup()
        {
            Publishing.DataBase.CloseConnection();
            using (var con = new SqlConnection(MasterConnection))
            {
                con.Open();
                using var cmd = con.CreateCommand();
                cmd.CommandText = $"IF DB_ID('{DbName}') IS NOT NULL DROP DATABASE [{DbName}];";
                cmd.ExecuteNonQuery();
            }
        }

        private static void SetInternalFields(string server, string db)
        {
            var t = typeof(Publishing.DataBase);
            var serverField = t.GetField("SQLServerName", BindingFlags.NonPublic | BindingFlags.Static);
            var dbField = t.GetField("dataBaseName", BindingFlags.NonPublic | BindingFlags.Static);
            serverField?.SetValue(null, server);
            dbField?.SetValue(null, db);
        }

        [TestMethod]
        public void OpenConnection_StateIsOpen()
        {
            Publishing.DataBase.CloseConnection();
            Publishing.DataBase.OpenConnection();
            var conField = typeof(Publishing.DataBase).GetField("sqlConnection", BindingFlags.NonPublic | BindingFlags.Static);
            var con = (SqlConnection?)conField?.GetValue(null);
            Assert.IsNotNull(con);
            Assert.AreEqual(ConnectionState.Open, con!.State);
        }

        [TestMethod]
        public void ExecuteQuery_InsertAndSelect_ReturnsValue()
        {
            Publishing.DataBase.ExecuteQueryWithoutResponse("CREATE TABLE Settings(id INT IDENTITY(1,1), value INT)");
            Publishing.DataBase.ExecuteQueryWithoutResponse("INSERT INTO Settings(value) VALUES(42)");
            string result = Publishing.DataBase.ExecuteQuery("SELECT value FROM Settings WHERE id = 1");
            Assert.AreEqual("42", result);
        }

        [TestMethod]
        public void ExecuteQueryToDataTable_ReturnsRows()
        {
            Publishing.DataBase.ExecuteQueryWithoutResponse("CREATE TABLE Sample(id INT, name NVARCHAR(30))");
            Publishing.DataBase.ExecuteQueryWithoutResponse("INSERT INTO Sample(id,name) VALUES(1,'a'),(2,'b')");
            DataTable dt = Publishing.DataBase.ExecuteQueryToDataTable("SELECT * FROM Sample");
            Assert.AreEqual(2, dt.Rows.Count);
            Assert.AreEqual("name", dt.Columns[1].ColumnName);
        }

        [TestMethod]
        public void ExecuteQueryToDataTable_WithParameters_FiltersData()
        {
            Publishing.DataBase.ExecuteQueryWithoutResponse("CREATE TABLE Filter(id INT, name NVARCHAR(30))");
            Publishing.DataBase.ExecuteQueryWithoutResponse("INSERT INTO Filter(id,name) VALUES(1,'x'),(2,'y')");
            var parameters = new List<SqlParameter> { new SqlParameter("@id", 2) };
            DataTable dt = Publishing.DataBase.ExecuteQueryToDataTable("SELECT name FROM Filter WHERE id = @id", parameters);
            Assert.AreEqual(1, dt.Rows.Count);
            Assert.AreEqual("y", dt.Rows[0][0]);
        }

        [TestMethod]
        public void ExecuteQueryList_ReturnsStringArrays()
        {
            Publishing.DataBase.ExecuteQueryWithoutResponse("CREATE TABLE Lst(id INT, name NVARCHAR(30))");
            Publishing.DataBase.ExecuteQueryWithoutResponse("INSERT INTO Lst(id,name) VALUES(1,'one'),(2,'two')");
            var list = Publishing.DataBase.ExecuteQueryList("SELECT name FROM Lst ORDER BY id");
            Assert.AreEqual(2, list.Count);
            CollectionAssert.AreEqual(new[] { "one", "two" }, new[] { list[0][0], list[1][0] });
        }

        [TestMethod]
        public void ExecuteQueryWithoutResponse_UpdatesRow()
        {
            Publishing.DataBase.ExecuteQueryWithoutResponse("CREATE TABLE Upd(id INT, name NVARCHAR(30))");
            Publishing.DataBase.ExecuteQueryWithoutResponse("INSERT INTO Upd(id,name) VALUES(1,'old')");
            Publishing.DataBase.ExecuteQueryWithoutResponse("UPDATE Upd SET name='new' WHERE id=1");
            string val = Publishing.DataBase.ExecuteQuery("SELECT name FROM Upd WHERE id=1");
            Assert.AreEqual("new", val);
        }
    }
}
