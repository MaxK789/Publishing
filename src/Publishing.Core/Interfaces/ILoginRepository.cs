using System.Collections.Generic;

namespace Publishing.Core.Interfaces
{
    public interface ILoginRepository
    {
        string? GetHashedPassword(string email);
        string? GetUserId(string email);
        string? GetUserType(string email);
        string? GetUserName(string email);
        bool EmailExists(string email);
        int InsertPerson(string fName, string lName, string email, string status);
        void InsertPassword(string hashedPassword, int personId);
    }
}
