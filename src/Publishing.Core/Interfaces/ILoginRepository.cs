using System.Collections.Generic;

namespace Publishing.Core.Interfaces
{
    public interface ILoginRepository
    {
        Task<string?> GetHashedPasswordAsync(string email);
        Task<string?> GetUserIdAsync(string email);
        Task<string?> GetUserTypeAsync(string email);
        Task<string?> GetUserNameAsync(string email);
        Task<bool> EmailExistsAsync(string email);
        Task<int> InsertPersonAsync(string fName, string lName, string email, string status);
        Task InsertPasswordAsync(string hashedPassword, int personId);
    }
}
