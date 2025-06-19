using Microsoft.VisualStudio.TestTools.UnitTesting;
using Publishing.Core.Interfaces;
using Publishing.Core.Services;
using Publishing.Core.DTOs;
using BCrypt.Net;
using System.Threading.Tasks;

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

            public Task<string?> GetHashedPasswordAsync(string email) => Task.FromResult(StoredHash);
            public Task<string?> GetUserIdAsync(string email) => Task.FromResult(Id);
            public Task<string?> GetUserTypeAsync(string email) => Task.FromResult(Type);
            public Task<string?> GetUserNameAsync(string email) => Task.FromResult(Name);
            public Task<bool> EmailExistsAsync(string email) => Task.FromResult(EmailExistsReturn);
            public Task<int> InsertPersonAsync(string f, string l, string e, string s) => Task.FromResult(5);
            public Task InsertPasswordAsync(string h, int id) => Task.CompletedTask;
        }

        [TestMethod]
        public async Task Authenticate_ReturnsUserDto()
        {
            var repo = new StubLoginRepository
            {
                StoredHash = BCrypt.Net.BCrypt.HashPassword("pwd", 11)
            };
            var service = new AuthService(repo);

            var user = await service.AuthenticateAsync("e", "pwd");

            Assert.IsNotNull(user);
            Assert.AreEqual(repo.Id, user!.Id);
            Assert.AreEqual(repo.Type, user.Type);
            Assert.AreEqual(repo.Name, user.Name);
        }

        [TestMethod]
        public async Task Register_ReturnsNewUser()
        {
            var repo = new StubLoginRepository();
            var service = new AuthService(repo);

            var user = await service.RegisterAsync("F", "L", "e@e.com", "type", "pass");

            Assert.AreEqual("5", user.Id);
            Assert.AreEqual("F", user.Name);
            Assert.AreEqual("type", user.Type);
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public async Task Register_EmailExists_Throws()
        {
            var repo = new StubLoginRepository { EmailExistsReturn = true };
            var service = new AuthService(repo);
            await service.RegisterAsync("F", "L", "e@e.com", "t", "p");
        }

        [TestMethod]
        public async Task Authenticate_WrongPassword_ReturnsNull()
        {
            var repo = new StubLoginRepository
            {
                StoredHash = BCrypt.Net.BCrypt.HashPassword("pwd", 11)
            };
            var svc = new AuthService(repo);

            var user = await svc.AuthenticateAsync("e", "wrong");

            Assert.IsNull(user);
        }
    }
}
