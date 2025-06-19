using Microsoft.VisualStudio.TestTools.UnitTesting;
using Publishing.Core.Interfaces;
using Publishing.Core.Services;
using Publishing.Core.DTOs;
using BCrypt.Net;

namespace Publishing.Core.Tests
{
    [TestClass]
    public class AuthServiceTests
    {
        private class StubLoginRepository : ILoginRepository
        {
            public string? StoredHash { get; set; }
            public string? Id { get; set; } = "1";
            public string? Type { get; set; } = "user";
            public string? Name { get; set; } = "name";
            public bool EmailExistsReturn { get; set; }

            public void OpenConnection() { }
            public void CloseConnection() { }
            public string? GetHashedPassword(string email) => StoredHash;
            public string? GetUserId(string email) => Id;
            public string? GetUserType(string email) => Type;
            public string? GetUserName(string email) => Name;
            public bool EmailExists(string email) => EmailExistsReturn;
            public int InsertPerson(string f, string l, string e, string s) => 5;
            public void InsertPassword(string h, int id) { }
        }

        [TestMethod]
        public void Authenticate_ReturnsUserDto()
        {
            var repo = new StubLoginRepository
            {
                StoredHash = BCrypt.Net.BCrypt.HashPassword("pwd", 11)
            };
            var service = new AuthService(repo);

            var user = service.Authenticate("e", "pwd");

            Assert.IsNotNull(user);
            Assert.AreEqual(repo.Id, user!.Id);
            Assert.AreEqual(repo.Type, user.Type);
            Assert.AreEqual(repo.Name, user.Name);
        }

        [TestMethod]
        public void Register_ReturnsNewUser()
        {
            var repo = new StubLoginRepository();
            var service = new AuthService(repo);

            var user = service.Register("F", "L", "e@e.com", "type", "pass");

            Assert.AreEqual("5", user.Id);
            Assert.AreEqual("F", user.Name);
            Assert.AreEqual("type", user.Type);
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void Register_EmailExists_Throws()
        {
            var repo = new StubLoginRepository { EmailExistsReturn = true };
            var service = new AuthService(repo);
            service.Register("F", "L", "e@e.com", "t", "p");
        }

        [TestMethod]
        public void Authenticate_WrongPassword_ReturnsNull()
        {
            var repo = new StubLoginRepository
            {
                StoredHash = BCrypt.Net.BCrypt.HashPassword("pwd", 11)
            };
            var svc = new AuthService(repo);

            var user = svc.Authenticate("e", "wrong");

            Assert.IsNull(user);
        }
    }
}
