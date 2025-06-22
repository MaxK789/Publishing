namespace Publishing.Core.Interfaces
{
    using Publishing.Core.DTOs;
    using System.Threading.Tasks;

    public interface IAuthService
    {
        Task<AuthResultDto?> AuthenticateAsync(string email, string password);
        Task<AuthResultDto> RegisterAsync(Publishing.Core.Commands.RegisterUserCommand cmd);
    }
}
