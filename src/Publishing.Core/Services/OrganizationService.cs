using System.Threading.Tasks;
using FluentValidation;
using Publishing.Core.DTOs;
using Publishing.Core.Interfaces;

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
                await _repo.UpdateAsync(dto.Id, dto.Name, dto.Email, dto.Phone, dto.Fax, dto.Address).ConfigureAwait(false);
            }
            else
            {
                await _repo.InsertAsync(dto.Name, dto.Email, dto.Phone, dto.Fax, dto.Address, dto.Id).ConfigureAwait(false);
            }
        }
    }
}
