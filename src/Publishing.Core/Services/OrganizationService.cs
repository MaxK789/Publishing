using System.Threading.Tasks;
using FluentValidation;
using Publishing.Core.DTOs;
using Publishing.Core.Interfaces;
using Publishing.Core.Commands;

namespace Publishing.Core.Services
{
    public class OrganizationService : IOrganizationService
    {
        private readonly IOrganizationRepository _repo;
        private readonly IValidator<UpdateOrganizationDto> _validator;

        public OrganizationService(IOrganizationRepository repo, IValidator<UpdateOrganizationDto> validator)
        {
            _repo = repo;
            _validator = validator;
        }

        public async Task UpdateAsync(UpdateOrganizationDto dto)
        {
            _validator.ValidateAndThrow(dto);
            string? existing = await _repo.GetNameIfExistsAsync(dto.Name).ConfigureAwait(false);
            if (existing == dto.Name)
            {
                var cmd = new UpdateOrganizationCommand
                {
                    Id = dto.Id,
                    Name = dto.Name,
                    Email = dto.Email,
                    Phone = dto.Phone,
                    Fax = dto.Fax,
                    Address = dto.Address
                };
                await _repo.UpdateAsync(cmd).ConfigureAwait(false);
            }
            else
            {
                var cmd = new CreateOrganizationCommand
                {
                    Name = dto.Name,
                    Email = dto.Email,
                    Phone = dto.Phone,
                    Fax = dto.Fax,
                    Address = dto.Address,
                    PersonId = dto.Id
                };
                await _repo.InsertAsync(cmd).ConfigureAwait(false);
            }
        }
    }
}
