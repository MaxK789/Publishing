using System.Collections.Generic;

namespace Publishing.Core.Interfaces
{
    public interface ILoginRepository
    {
        void OpenConnection();
        string? GetHashedPassword(string email);
        string? GetUserId(string email);
        string? GetUserType(string email);
        string? GetUserName(string email);
        void CloseConnection();
    }
}
