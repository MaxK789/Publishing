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


        public UserDto? Authenticate(string email, string password)
        {
            string? hash = _repo.GetHashedPassword(email);
            if (hash != null && BCrypt.Net.BCrypt.Verify(password, hash))
            {
                string? id = _repo.GetUserId(email);
                string? type = _repo.GetUserType(email);
                string? name = _repo.GetUserName(email);
                if (id != null && type != null && name != null)
                {
                    return new UserDto(id, name, type);
                }
            }
            return null;
        }

        public UserDto Register(string firstName, string lastName, string email, string status, string password)
        {
            if (_repo.EmailExists(email))
                throw new InvalidOperationException("Email already used");
            string hashed = BCrypt.Net.BCrypt.HashPassword(password, 11);
            int id = _repo.InsertPerson(firstName, lastName, email, status);
            if (id == 0)
                throw new InvalidOperationException("Failed to insert person");
            _repo.InsertPassword(hashed, id);
            return new UserDto(id.ToString(), firstName, status);
        }
    }
}
