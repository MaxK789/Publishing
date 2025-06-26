using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using FluentValidation;
using Publishing.Core.DTOs;
using Publishing.Core.Interfaces;
using Publishing.Core.Services;
using Publishing.AppLayer.Validators;
using Publishing.Core.Commands;

namespace Publishing.Core.Tests
{
    [TestClass]
    public class OrganizationServiceTests
    {
        private class StubRepo : IOrganizationRepository
        {
            public string? Existing;
            public CreateOrganizationCommand? Inserted;
            public UpdateOrganizationCommand? Updated;

            public Task<string?> GetNameIfExistsAsync(string name)
            {
                return Task.FromResult(name == Existing ? Existing : null);
            }

            public Task InsertAsync(CreateOrganizationCommand cmd)
            {
                Inserted = cmd;
                return Task.CompletedTask;
            }

            public Task UpdateAsync(UpdateOrganizationCommand cmd)
            {
                Updated = cmd;
                return Task.CompletedTask;
            }

            public Task<OrganizationDto?> GetByPersonIdAsync(string personId)
                => Task.FromResult<OrganizationDto?>(null);

            public Task<IEnumerable<OrganizationDto>> GetAllAsync()
                => Task.FromResult<IEnumerable<OrganizationDto>>(Array.Empty<OrganizationDto>());

            public Task DeleteAsync(string id) => Task.CompletedTask;
        }

        private class PassValidator : AbstractValidator<UpdateOrganizationDto> { }

        [TestMethod]
        public async Task UpdateAsync_NameExists_UpdatesRecord()
        {
            var repo = new StubRepo { Existing = "org" };
            var service = new OrganizationService(repo, new PassValidator());

            await service.UpdateAsync(new UpdateOrganizationDto { Id = "1", Name = "org" });

            Assert.IsNotNull(repo.Updated);
            Assert.IsNull(repo.Inserted);
            Assert.AreEqual("1", repo.Updated!.Id);
        }

        [TestMethod]
        public async Task UpdateAsync_NewName_InsertsRecord()
        {
            var repo = new StubRepo { Existing = "other" };
            var service = new OrganizationService(repo, new PassValidator());

            await service.UpdateAsync(new UpdateOrganizationDto { Id = "2", Name = "org", Email = "e" });

            Assert.IsNotNull(repo.Inserted);
            Assert.IsNull(repo.Updated);
            Assert.AreEqual("org", repo.Inserted!.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public async Task UpdateAsync_InvalidDto_Throws()
        {
            var repo = new StubRepo();
            var validator = new UpdateOrganizationValidator(new EmailValidator(), new PhoneFaxValidator());
            var service = new OrganizationService(repo, validator);

            await service.UpdateAsync(new UpdateOrganizationDto { Id = "1", Email = "bad" });
        }
    }
}
