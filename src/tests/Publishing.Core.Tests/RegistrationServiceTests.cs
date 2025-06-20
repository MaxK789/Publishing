using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using FluentValidation;
using Publishing.Core.DTOs;
using Publishing.Core.Interfaces;
using Publishing.Core.Services;

namespace Publishing.Core.Tests
{
    [TestClass]
    public class RegistrationServiceTests
    {
        private class StubAuthService : IAuthService
        {
            public RegisterUserDto? Passed;
            public UserDto ReturnUser = new UserDto { Id = "1", Name = "N", Type = "t" };

            public Task<UserDto?> AuthenticateAsync(string email, string password)
                => Task.FromResult<UserDto?>(null);

            public Task<UserDto> RegisterAsync(string firstName, string lastName, string email, string status, string password)
            {
                Passed = new RegisterUserDto { FirstName = firstName, LastName = lastName, Email = email, Status = status, Password = password };
                return Task.FromResult(ReturnUser);
            }
        }

        private class PassValidator : AbstractValidator<RegisterUserDto> { }

        [TestMethod]
        public async Task RegisterAsync_ValidDto_ReturnsUser()
        {
            var auth = new StubAuthService();
            var svc = new RegistrationService(auth, new PassValidator());
            var dto = new RegisterUserDto { FirstName = "F", LastName = "L", Email = "e@e.com", Status = "s", Password = "p" };

            var user = await svc.RegisterAsync(dto);

            Assert.IsNotNull(user);
            Assert.AreEqual(auth.ReturnUser, user);
            Assert.IsNotNull(auth.Passed);
        }

        [TestMethod]
        public async Task RegisterAsync_PassesDtoToAuthService()
        {
            var auth = new StubAuthService();
            var svc = new RegistrationService(auth, new PassValidator());
            var dto = new RegisterUserDto { FirstName = "F", LastName = "L", Email = "e@e.com", Status = "s", Password = "p" };

            await svc.RegisterAsync(dto);

            Assert.IsNotNull(auth.Passed);
            Assert.AreEqual(dto.Email, auth.Passed!.Email);
            Assert.AreEqual(dto.Password, auth.Passed.Password);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public async Task RegisterAsync_InvalidDto_Throws()
        {
            var auth = new StubAuthService();
            var validator = new RegisterUserValidator(new EmailValidator());
            var svc = new RegistrationService(auth, validator);

            await svc.RegisterAsync(new RegisterUserDto { Email = "bad" });
        }
    }
}
