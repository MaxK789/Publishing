using System.Threading.Tasks;
using Publishing.Core.DTOs;

namespace Publishing.Core.Interfaces
{

    public interface IAuthService
    {
        Task<UserDto?> AuthenticateAsync(string email, string password);
        Task<UserDto> RegisterAsync(string firstName, string lastName, string email, string status, string password);
    }
}
