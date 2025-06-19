using System.Collections.Generic;
using System.Linq;
using Dapper;
using Publishing.Core.Interfaces;

namespace Publishing.Infrastructure.Repositories
{
    public class LoginRepository : ILoginRepository
    {
        private readonly IDbContext _db;

        public LoginRepository(IDbContext db)
        {
            _db = db;
        }


        public string? GetHashedPassword(string email)
        {
            var res = _db.QueryAsync<string>("SELECT password FROM Person INNER JOIN Pass ON Pass.idPerson = Person.idPerson WHERE emailPerson = @Email", new { Email = email }).Result;
            return res.FirstOrDefault();
        }

        public string? GetUserId(string email)
        {
            var res = _db.QueryAsync<string>("SELECT idPerson FROM Person WHERE emailPerson = @Email", new { Email = email }).Result;
            return res.FirstOrDefault();
        }

        public string? GetUserType(string email)
        {
            var res = _db.QueryAsync<string>("SELECT typePerson FROM Person WHERE emailPerson = @Email", new { Email = email }).Result;
            return res.FirstOrDefault();
        }

        public string? GetUserName(string email)
        {
            var res = _db.QueryAsync<string>("SELECT FName FROM Person WHERE emailPerson = @Email", new { Email = email }).Result;
            return res.FirstOrDefault();
        }

        public bool EmailExists(string email)
        {
            var res = _db.QueryAsync<string>("SELECT emailPerson FROM Person WHERE emailPerson = @Email", new { Email = email }).Result.FirstOrDefault();
            return res == email;
        }

        public int InsertPerson(string fName, string lName, string email, string status)
        {
            const string query = "INSERT INTO Person(FName, LName, emailPerson,typePerson) VALUES(@FName, @LName, @Email, @Status); SELECT CAST(SCOPE_IDENTITY() as int);";
            var res = _db.QueryAsync<int>(query, new { FName = fName, LName = lName, Email = email, Status = status }).Result;
            return res.FirstOrDefault();
        }

        public void InsertPassword(string hashedPassword, int personId)
        {
            const string query = "INSERT INTO Pass(password, idPerson) VALUES(@Password, @Id)";
            _db.ExecuteAsync(query, new { Password = hashedPassword, Id = personId }).Wait();
        }
    }
}
