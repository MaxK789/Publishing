using System;
using BCrypt.Net;
using Publishing.Core.DTOs;
using Publishing.Core.Interfaces;
using System.Threading.Tasks;

namespace Publishing.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly ILoginRepository _repo;

        public AuthService(ILoginRepository repo)
        {
            _repo = repo;
        }


        public async Task<UserDto?> AuthenticateAsync(string email, string password)
        {
            string? hash = await _repo.GetHashedPasswordAsync(email);
            if (hash != null && BCrypt.Net.BCrypt.Verify(password, hash))
            {
                string? id = await _repo.GetUserIdAsync(email);
                string? type = await _repo.GetUserTypeAsync(email);
                string? name = await _repo.GetUserNameAsync(email);
                if (id != null && type != null && name != null)
                {
                    return new UserDto(id, name, type);
                }
            }
            return null;
        }

        public async Task<UserDto> RegisterAsync(string firstName, string lastName, string email, string status, string password)
        {
            if (await _repo.EmailExistsAsync(email))
                throw new InvalidOperationException("Email already used");
            string hashed = BCrypt.Net.BCrypt.HashPassword(password, 11);
            int id = await _repo.InsertPersonAsync(firstName, lastName, email, status);
            if (id == 0)
                throw new InvalidOperationException("Failed to insert person");
            await _repo.InsertPasswordAsync(hashed, id);
            return new UserDto(id.ToString(), firstName, status);
        }
    }
}
