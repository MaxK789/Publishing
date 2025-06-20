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
    public class OrganizationServiceTests
    {
        private class StubRepo : IOrganizationRepository
        {
            public string? Existing;
            public UpdateOrganizationDto? Inserted;
            public UpdateOrganizationDto? Updated;

            public Task<string?> GetNameIfExistsAsync(string name)
            {
                return Task.FromResult(name == Existing ? Existing : null);
            }

            public Task InsertAsync(string name, string email, string phone, string fax, string address, string personId)
            {
                Inserted = new UpdateOrganizationDto
                {
                    Id = personId,
                    Name = name,
                    Email = email,
                    Phone = phone,
                    Fax = fax,
                    Address = address
                };
                return Task.CompletedTask;
            }

            public Task UpdateAsync(string id, string? name, string? email, string? phone, string? fax, string? address)
            {
                Updated = new UpdateOrganizationDto
                {
                    Id = id,
                    Name = name ?? string.Empty,
                    Email = email ?? string.Empty,
                    Phone = phone ?? string.Empty,
                    Fax = fax ?? string.Empty,
                    Address = address ?? string.Empty
                };
                return Task.CompletedTask;
            }
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
