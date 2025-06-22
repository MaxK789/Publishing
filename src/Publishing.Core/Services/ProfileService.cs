using System;
using System.Threading.Tasks;
using FluentValidation;
using Publishing.Core.DTOs;
using Publishing.Core.Interfaces;
using Publishing.Core.Commands;

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
            var cmd = new UpdateProfileCommand
            {
                Id = dto.Id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Status = dto.Status,
                Phone = dto.Phone,
                Fax = dto.Fax,
                Address = dto.Address
            };
            await _repo.UpdateAsync(cmd).ConfigureAwait(false);
        }
    }
}
