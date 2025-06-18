using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Reflection;
using BCrypt.Net;

namespace Publishing.Integration.Tests
{
    [TestClass]
    public class CrudFlowTests
    {
        private const string Server = @"(localdb)\MSSQLLocalDB";
        private const string DbName = "PublishingCrud";

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

            // create minimal schema
            Publishing.DataBase.ExecuteQueryWithoutResponse(
                "CREATE TABLE Person(idPerson INT IDENTITY(1,1) PRIMARY KEY, FName NVARCHAR(50), LName NVARCHAR(50), emailPerson NVARCHAR(50), typePerson NVARCHAR(20));");
            Publishing.DataBase.ExecuteQueryWithoutResponse(
                "CREATE TABLE Pass(password NVARCHAR(255), idPerson INT);");
            Publishing.DataBase.ExecuteQueryWithoutResponse(
                "CREATE TABLE Product(idProduct INT IDENTITY(1,1) PRIMARY KEY, idPerson INT, typeProduct NVARCHAR(50), nameProduct NVARCHAR(50), pagesNum INT);");
            Publishing.DataBase.ExecuteQueryWithoutResponse(
                "CREATE TABLE Orders(idOrder INT IDENTITY(1,1) PRIMARY KEY, idProduct INT, idPerson INT, namePrintery NVARCHAR(50), dateOrder DATETIME, dateStart DATETIME, dateFinish DATETIME, statusOrder NVARCHAR(50), tirage INT, price INT);");
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
        public void Registration_InsertsPerson()
        {
            Publishing.DataBase.ExecuteQueryWithoutResponse(
                "INSERT INTO Person(FName,LName,emailPerson,typePerson) VALUES('A','B','a@b.com','user')");
            string result = Publishing.DataBase.ExecuteQuery("SELECT COUNT(*) FROM Person");
            Assert.AreEqual("1", result);
        }

        [TestMethod]
        public void Login_SetsCurrentUser()
        {
            string hash = BCrypt.Net.BCrypt.HashPassword("pass", 11);
            Publishing.DataBase.ExecuteQueryWithoutResponse(
                "INSERT INTO Person(FName,LName,emailPerson,typePerson) VALUES('A','B','c@d.com','user');");
            string id = Publishing.DataBase.ExecuteQuery("SELECT idPerson FROM Person WHERE emailPerson='c@d.com'");
            Publishing.DataBase.ExecuteQueryWithoutResponse($"INSERT INTO Pass(password,idPerson) VALUES('{hash}', {id})");

            List<SqlParameter> parameters = new List<SqlParameter> { new SqlParameter("@Email", "c@d.com") };
            string storedHash = Publishing.DataBase.ExecuteQuery("SELECT password FROM Pass WHERE idPerson = (SELECT idPerson FROM Person WHERE emailPerson=@Email)", parameters);
            bool verified = BCrypt.Net.BCrypt.Verify("pass", storedHash);
            Assert.IsTrue(verified);
        }

        [TestMethod]
        public void AddOrder_InsertsOrder()
        {
            Publishing.DataBase.ExecuteQueryWithoutResponse(
                "INSERT INTO Person(FName,LName,emailPerson,typePerson) VALUES('A','B','e@f.com','user');");
            string id = Publishing.DataBase.ExecuteQuery("SELECT idPerson FROM Person WHERE emailPerson='e@f.com'");

            Publishing.DataBase.ExecuteQueryWithoutResponse(
                $"INSERT INTO Product(idPerson,typeProduct,nameProduct,pagesNum) VALUES({id},'book','Title',10)");
            string idProd = Publishing.DataBase.ExecuteQuery("SELECT idProduct FROM Product WHERE nameProduct='Title'");

            Publishing.DataBase.ExecuteQueryWithoutResponse(
                $"INSERT INTO Orders(idProduct,idPerson,namePrintery,dateOrder,dateStart,dateFinish,statusOrder,tirage,price) VALUES({idProd},{id},'P',GETDATE(),GETDATE(),GETDATE(),'в роботі',5,50)");
            string count = Publishing.DataBase.ExecuteQuery("SELECT COUNT(*) FROM Orders");
            Assert.AreEqual("1", count);
        }

        [TestMethod]
        public void DeleteOrder_RemovesRow()
        {
            Publishing.DataBase.ExecuteQueryWithoutResponse(
                "INSERT INTO Person(FName,LName,emailPerson,typePerson) VALUES('A','B','g@h.com','user');");
            string id = Publishing.DataBase.ExecuteQuery("SELECT idPerson FROM Person WHERE emailPerson='g@h.com'");
            Publishing.DataBase.ExecuteQueryWithoutResponse(
                $"INSERT INTO Product(idPerson,typeProduct,nameProduct,pagesNum) VALUES({id},'book','Del',10)");
            string idProd = Publishing.DataBase.ExecuteQuery("SELECT idProduct FROM Product WHERE nameProduct='Del'");
            Publishing.DataBase.ExecuteQueryWithoutResponse(
                $"INSERT INTO Orders(idProduct,idPerson,namePrintery,dateOrder,dateStart,dateFinish,statusOrder,tirage,price) VALUES({idProd},{id},'P',GETDATE(),GETDATE(),GETDATE(),'в роботі',5,50)");
            string idOrder = Publishing.DataBase.ExecuteQuery("SELECT idOrder FROM Orders");

            List<SqlParameter> parameters = new List<SqlParameter> { new SqlParameter("@id", idOrder) };
            Publishing.DataBase.ExecuteQueryWithoutResponse("DELETE FROM Orders WHERE idOrder=@id", parameters);

            string count = Publishing.DataBase.ExecuteQuery("SELECT COUNT(*) FROM Orders");
            Assert.AreEqual("0", count);
        }
    }
}
