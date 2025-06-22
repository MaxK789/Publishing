using System;
using BCrypt.Net;
using Publishing.Core.DTOs;
using Publishing.Core.Interfaces;
using Publishing.Services;
using System.Threading.Tasks;

namespace Publishing.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly ILoginRepository _repo;
        private readonly IJwtFactory _jwtFactory;
        public AuthService(ILoginRepository repo, IJwtFactory jwtFactory)
        {
            _repo = repo;
            _jwtFactory = jwtFactory;
        }


        public async Task<AuthResultDto?> AuthenticateAsync(string email, string password)
        {
            string? hash = await _repo.GetHashedPasswordAsync(email);
            if (hash != null && BCrypt.Net.BCrypt.Verify(password, hash))
            {
                string? id = await _repo.GetUserIdAsync(email);
                string? type = await _repo.GetUserTypeAsync(email);
                string? name = await _repo.GetUserNameAsync(email);
                if (id != null && type != null && name != null)
                {
                    var user = new UserDto(id, name, type);
                    string token = _jwtFactory.GenerateToken(user);
                    return new AuthResultDto(user, token);
                }
            }
            return null;
        }

        public async Task<AuthResultDto> RegisterAsync(Publishing.Core.Commands.RegisterUserCommand cmd)
        {
            if (await _repo.EmailExistsAsync(cmd.Email))
                throw new InvalidOperationException("Email already used");
            string hashed = BCrypt.Net.BCrypt.HashPassword(cmd.Password, 11);
            int id = await _repo.InsertPersonAsync(cmd.FirstName, cmd.LastName, cmd.Email, cmd.Status);
            if (id == 0)
                throw new InvalidOperationException("Failed to insert person");
            await _repo.InsertPasswordAsync(hashed, id);
            var user = new UserDto(id.ToString(), cmd.FirstName, cmd.Status);
            string token = _jwtFactory.GenerateToken(user);
            return new AuthResultDto(user, token);
        }
    }
}
