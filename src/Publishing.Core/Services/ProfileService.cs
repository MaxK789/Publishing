using System;
using System.Threading.Tasks;
using FluentValidation;
using Publishing.Core.DTOs;
using Publishing.Core.Interfaces;

namespace Publishing.Core.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IProfileRepository _repo;
        private readonly IValidator<UpdateProfileDto> _validator;

        public ProfileService(IProfileRepository repo, IValidator<UpdateProfileDto> validator)
        {
            _repo = repo;
            _validator = validator;
        }

        public async Task UpdateAsync(UpdateProfileDto dto)
        {
            _validator.ValidateAndThrow(dto);
            if (!string.IsNullOrEmpty(dto.Email) && await _repo.EmailExistsAsync(dto.Email).ConfigureAwait(false))
            {
                throw new InvalidOperationException("Email вже використовується");
            }
            await _repo.UpdateAsync(dto.Id, dto.FirstName, dto.LastName, dto.Email, dto.Status, dto.Phone, dto.Fax, dto.Address).ConfigureAwait(false);
        }
    }
}
