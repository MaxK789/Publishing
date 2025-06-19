using System.Threading.Tasks;
namespace Publishing.Core.Interfaces
{
    using Publishing.Core.DTOs;

    public interface IAuthService
    {
        Task<UserDto?> AuthenticateAsync(string email, string password);
        Task<UserDto> RegisterAsync(string firstName, string lastName, string email, string status, string password);
    }
}
