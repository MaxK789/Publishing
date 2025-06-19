using System;
using BCrypt.Net;
using Publishing.Core.DTOs;
using Publishing.Core.Interfaces;

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
            string? hash = await _repo.GetHashedPasswordAsync(email).ConfigureAwait(false);
            if (hash != null && BCrypt.Net.BCrypt.Verify(password, hash))
            {
                string? id = await _repo.GetUserIdAsync(email).ConfigureAwait(false);
                string? type = await _repo.GetUserTypeAsync(email).ConfigureAwait(false);
                string? name = await _repo.GetUserNameAsync(email).ConfigureAwait(false);
                if (id != null && type != null && name != null)
                {
                    return new UserDto(id, name, type);
                }
            }
            return null;
        }

        public async Task<UserDto> RegisterAsync(string firstName, string lastName, string email, string status, string password)
        {
            if (await _repo.EmailExistsAsync(email).ConfigureAwait(false))
                throw new InvalidOperationException("Email already used");
            string hashed = BCrypt.Net.BCrypt.HashPassword(password, 11);
            int id = await _repo.InsertPersonAsync(firstName, lastName, email, status).ConfigureAwait(false);
            if (id == 0)
                throw new InvalidOperationException("Failed to insert person");
            await _repo.InsertPasswordAsync(hashed, id).ConfigureAwait(false);
            return new UserDto(id.ToString(), firstName, status);
        }
    }
}
