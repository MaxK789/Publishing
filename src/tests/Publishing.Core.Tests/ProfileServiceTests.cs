using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using FluentValidation;
using Publishing.Core.DTOs;
using Publishing.Core.Interfaces;
using Publishing.Core.Services;
using Publishing.AppLayer.Validators;
using Publishing.Core.Commands;

namespace Publishing.Core.Tests
{
    [TestClass]
    public class ProfileServiceTests
    {
        private class StubRepo : IProfileRepository
        {
            public bool EmailExists;
            public UpdateProfileCommand? Updated;

            public Task<bool> EmailExistsAsync(string email) => Task.FromResult(EmailExists);

            public Task UpdateAsync(UpdateProfileCommand cmd)
            {
                Updated = cmd;
                return Task.CompletedTask;
            }
        }

        private class PassValidator : AbstractValidator<UpdateProfileDto> { }

        [TestMethod]
        public async Task UpdateAsync_ValidData_CallsRepository()
        {
            var repo = new StubRepo();
            var service = new ProfileService(repo, new PassValidator());
            var dto = new UpdateProfileDto { Id = "1", Email = "e@e.com" };

            await service.UpdateAsync(dto);

            Assert.IsNotNull(repo.Updated);
            Assert.AreEqual("1", repo.Updated!.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task UpdateAsync_EmailExists_Throws()
        {
            var repo = new StubRepo { EmailExists = true };
            var service = new ProfileService(repo, new PassValidator());
            await service.UpdateAsync(new UpdateProfileDto { Id = "1", Email = "a@a.com" });
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public async Task UpdateAsync_InvalidDto_Throws()
        {
            var repo = new StubRepo();
            var validator = new UpdateProfileValidator(new EmailValidator(), new PhoneFaxValidator());
            var service = new ProfileService(repo, validator);
            await service.UpdateAsync(new UpdateProfileDto { Id = "1", Email = "bad" });
        }
    }
}
