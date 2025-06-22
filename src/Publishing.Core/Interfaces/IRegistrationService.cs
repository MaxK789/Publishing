using System.Threading.Tasks;
using Publishing.Core.DTOs;

namespace Publishing.Core.Interfaces
{
    public interface IRegistrationService
    {
        Task<AuthResultDto> RegisterAsync(RegisterUserDto dto);
    }
}
