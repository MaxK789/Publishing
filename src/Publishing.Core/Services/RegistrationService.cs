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

        public Task<UserDto> RegisterAsync(RegisterUserDto dto)
        {
            _validator.ValidateAndThrow(dto);
            return _authService.RegisterAsync(dto.FirstName, dto.LastName, dto.Email, dto.Status!, dto.Password);
        }
    }
}
