using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using FluentValidation;
using Publishing.Core.DTOs;
using Publishing.Core.Interfaces;
using Publishing.Core.Services;
using Publishing.AppLayer.Validators;

namespace Publishing.Core.Tests
{
    [TestClass]
    public class RegistrationServiceTests
    {
        private class StubAuthService : IAuthService
        {
            public Publishing.Core.Commands.RegisterUserCommand? Passed;
            public AuthResultDto ReturnUser = new(new UserDto("1", "N", "t"), "tkn");

            public Task<AuthResultDto?> AuthenticateAsync(string email, string password)
                => Task.FromResult<AuthResultDto?>(null);

            public Task<AuthResultDto> RegisterAsync(Publishing.Core.Commands.RegisterUserCommand cmd)
            {
                Passed = cmd;
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

            var result = await svc.RegisterAsync(dto);

            Assert.IsNotNull(result);
            Assert.AreEqual(auth.ReturnUser, result);
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
