using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Publishing.Core.Interfaces;

namespace Publishing.Infrastructure.Repositories
{
    public class LoginRepository : ILoginRepository
    {
        private readonly IDatabaseClient _db;

        public LoginRepository(IDatabaseClient db)
        {
            _db = db;
        }

        public void OpenConnection() => _db.OpenConnection();

        public void CloseConnection() => _db.CloseConnection();

        public string? GetHashedPassword(string email)
        {
            var parameters = new List<SqlParameter> { new("@Email", email) };
            return _db.ExecuteQuery("SELECT password FROM Person INNER JOIN Pass ON Pass.idPerson = Person.idPerson WHERE emailPerson = @Email", parameters);
        }

        public string? GetUserId(string email)
        {
            var parameters = new List<SqlParameter> { new("@Email", email) };
            return _db.ExecuteQuery("SELECT idPerson FROM Person WHERE emailPerson = @Email", parameters);
        }

        public string? GetUserType(string email)
        {
            var parameters = new List<SqlParameter> { new("@Email", email) };
            return _db.ExecuteQuery("SELECT typePerson FROM Person WHERE emailPerson = @Email", parameters);
        }

        public string? GetUserName(string email)
        {
            var parameters = new List<SqlParameter> { new("@Email", email) };
            return _db.ExecuteQuery("SELECT FName FROM Person WHERE emailPerson = @Email", parameters);
        }

        public bool EmailExists(string email)
        {
            var parameters = new List<SqlParameter> { new("@Email", email) };
            string? result = _db.ExecuteQuery("SELECT emailPerson FROM Person WHERE emailPerson = @Email", parameters);
            return result == email;
        }

        public int InsertPerson(string fName, string lName, string email, string status)
        {
            const string query = "INSERT INTO Person(FName, LName, emailPerson, typePerson) VALUES(@FName, @LName, @Email, @Status); SELECT SCOPE_IDENTITY();";
            var parameters = new List<SqlParameter>
            {
                new("@FName", fName),
                new("@LName", lName),
                new("@Email", email),
                new("@Status", status)
            };
            string idStr = _db.ExecuteQuery(query, parameters);
            return int.TryParse(idStr, out int id) ? id : 0;
        }

        public void InsertPassword(string hashedPassword, int personId)
        {
            const string query = "INSERT INTO Pass(password, idPerson) VALUES(@Password, @Id)";
            var parameters = new List<SqlParameter>
            {
                new("@Password", hashedPassword),
                new("@Id", personId)
            };
            _db.ExecuteQueryWithoutResponse(query, parameters);
        }
    }
}
