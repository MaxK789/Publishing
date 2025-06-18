using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Publishing.Core.Interfaces;

namespace Publishing.Infrastructure.Repositories
{
    public class LoginRepository : ILoginRepository
    {
        public void OpenConnection() => DataBase.OpenConnection();

        public void CloseConnection() => DataBase.CloseConnection();

        public string? GetHashedPassword(string email)
        {
            var parameters = new List<SqlParameter> { new("@Email", email) };
            return DataBase.ExecuteQuery("SELECT password FROM Person INNER JOIN Pass ON Pass.idPerson = Person.idPerson WHERE emailPerson = @Email", parameters);
        }

        public string? GetUserId(string email)
        {
            var parameters = new List<SqlParameter> { new("@Email", email) };
            return DataBase.ExecuteQuery("SELECT idPerson FROM Person WHERE emailPerson = @Email", parameters);
        }

        public string? GetUserType(string email)
        {
            var parameters = new List<SqlParameter> { new("@Email", email) };
            return DataBase.ExecuteQuery("SELECT typePerson FROM Person WHERE emailPerson = @Email", parameters);
        }

        public string? GetUserName(string email)
        {
            var parameters = new List<SqlParameter> { new("@Email", email) };
            return DataBase.ExecuteQuery("SELECT FName FROM Person WHERE emailPerson = @Email", parameters);
        }
    }
}
