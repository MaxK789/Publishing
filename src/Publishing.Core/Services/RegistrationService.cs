using FluentValidation;
using Publishing.Core.DTOs;
using Publishing.Core.Interfaces;
using System.Threading.Tasks;

namespace Publishing.Core.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly IAuthService _authService;
        private readonly IValidator<RegisterUserDto> _validator;

        public RegistrationService(IAuthService authService, IValidator<RegisterUserDto> validator)
        {
            _authService = authService;
            _validator = validator;
        }

        public Task<AuthResultDto> RegisterAsync(RegisterUserDto dto)
        {
            _validator.ValidateAndThrow(dto);
            var cmd = new Publishing.Core.Commands.RegisterUserCommand
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Status = dto.Status!,
                Password = dto.Password
            };
            return _authService.RegisterAsync(cmd);
        }
    }
}
