using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Reflection;

namespace Publishing.Integration.Tests
{
    [TestClass]
    public class StatisticTests
    {
        private const string Server = @"(localdb)\MSSQLLocalDB";
        private const string DbName = "PublishingStat";

        private static string MasterConnection => $"Data Source={Server};Initial Catalog=master;Integrated Security=true";

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

            SetInternalFields(Server, DbName);
            Publishing.DataBase.OpenConnection();

            Publishing.DataBase.ExecuteQueryWithoutResponse(
                "CREATE TABLE Person(idPerson INT IDENTITY(1,1) PRIMARY KEY, FName NVARCHAR(50));");
            Publishing.DataBase.ExecuteQueryWithoutResponse(
                "CREATE TABLE Orders(idOrder INT IDENTITY(1,1) PRIMARY KEY, idPerson INT, dateOrder DATETIME);");

            Publishing.DataBase.ExecuteQueryWithoutResponse("INSERT INTO Person(FName) VALUES('A');");
            string id = Publishing.DataBase.ExecuteQuery("SELECT idPerson FROM Person");

            Publishing.DataBase.ExecuteQueryWithoutResponse($"INSERT INTO Orders(idPerson,dateOrder) VALUES({id},'2024-01-10'),({id},'2024-01-20'),({id},'2024-02-05')");
        }

        [TestCleanup]
        public void Cleanup()
        {
            Publishing.DataBase.CloseConnection();
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

        private static void SetInternalFields(string server, string db)
        {
            var t = typeof(Publishing.DataBase);
            var serverField = t.GetField("SQLServerName", BindingFlags.NonPublic | BindingFlags.Static);
            var dbField = t.GetField("dataBaseName", BindingFlags.NonPublic | BindingFlags.Static);
            serverField?.SetValue(null, server);
            dbField?.SetValue(null, db);
        }

        [TestMethod]
        public void Statistic_GeneratesSeries()
        {
            var list = Publishing.DataBase.ExecuteQueryList("SELECT DATENAME(MONTH, dateOrder) AS M, COUNT(*) AS N FROM Orders GROUP BY DATENAME(MONTH, dateOrder) ORDER BY MIN(dateOrder)");
            Assert.AreEqual(2, list.Count);
            Assert.AreEqual("January", list[0][0]);
            Assert.AreEqual("2", list[0][1]);
            Assert.AreEqual("February", list[1][0]);
            Assert.AreEqual("1", list[1][1]);
        }
    }
}
