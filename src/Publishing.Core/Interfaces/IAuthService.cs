namespace Publishing.Core.Interfaces
{
    using Publishing.Core.DTOs;
    using System.Threading.Tasks;

    public interface IAuthService
    {
        Task<UserDto?> AuthenticateAsync(string email, string password);
        Task<UserDto> RegisterAsync(string firstName, string lastName, string email, string status, string password);
    }
}
