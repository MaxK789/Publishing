using Microsoft.VisualStudio.TestTools.UnitTesting;
using Publishing.Core.Interfaces;
using Publishing.Core.Services;
using Publishing.Core.DTOs;
using BCrypt.Net;
using System.Threading.Tasks;
using System.Data;

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

        private class StubFactory : IJwtFactory
        {
            public string GenerateToken(UserDto user) => "token";
        }

        [TestMethod]
        public async Task Authenticate_ReturnsUserDto()
        {
            var repo = new StubLoginRepository
            {
                StoredHash = BCrypt.Net.BCrypt.HashPassword("pwd", 11)
            };
            var service = new AuthService(repo, new StubFactory());

            var result = await service.AuthenticateAsync("e", "pwd");

            Assert.IsNotNull(result);
            Assert.AreEqual(repo.Id, result!.User.Id);
            Assert.AreEqual(repo.Type, result.User.Type);
            Assert.AreEqual(repo.Name, result.User.Name);
            Assert.IsFalse(string.IsNullOrEmpty(result.Token));
        }

        [TestMethod]
        public async Task Register_ReturnsNewUser()
        {
            var repo = new StubLoginRepository();
            var service = new AuthService(repo, new StubFactory());

            var cmd = new Publishing.Core.Commands.RegisterUserCommand
            {
                FirstName = "F",
                LastName = "L",
                Email = "e@e.com",
                Status = "type",
                Password = "pass"
            };
            var result = await service.RegisterAsync(cmd);

            Assert.AreEqual("5", result.User.Id);
            Assert.AreEqual("F", result.User.Name);
            Assert.AreEqual("type", result.User.Type);
            Assert.IsFalse(string.IsNullOrEmpty(result.Token));
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public async Task Register_EmailExists_Throws()
        {
            var repo = new StubLoginRepository { EmailExistsReturn = true };
            var service = new AuthService(repo, new StubFactory());
            var cmd = new Publishing.Core.Commands.RegisterUserCommand
            {
                FirstName = "F",
                LastName = "L",
                Email = "e@e.com",
                Status = "t",
                Password = "p"
            };
            await service.RegisterAsync(cmd);
        }

        [TestMethod]
        public async Task Authenticate_WrongPassword_ReturnsNull()
        {
            var repo = new StubLoginRepository
            {
                StoredHash = BCrypt.Net.BCrypt.HashPassword("pwd", 11)
            };
            var svc = new AuthService(repo, new StubFactory());

            var user = await svc.AuthenticateAsync("e", "wrong");

            Assert.IsNull(user);
        }
    }
}
