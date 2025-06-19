using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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


        public async Task<string?> GetHashedPasswordAsync(string email)
        {
            var res = await _db.QueryAsync<string>(
                "SELECT password FROM Person INNER JOIN Pass ON Pass.idPerson = Person.idPerson WHERE emailPerson = @Email",
                new { Email = email }).ConfigureAwait(false);
            return res.FirstOrDefault();
        }

        public async Task<string?> GetUserIdAsync(string email)
        {
            var res = await _db.QueryAsync<string>("SELECT idPerson FROM Person WHERE emailPerson = @Email",
                new { Email = email }).ConfigureAwait(false);
            return res.FirstOrDefault();
        }

        public async Task<string?> GetUserTypeAsync(string email)
        {
            var res = await _db.QueryAsync<string>("SELECT typePerson FROM Person WHERE emailPerson = @Email",
                new { Email = email }).ConfigureAwait(false);
            return res.FirstOrDefault();
        }

        public async Task<string?> GetUserNameAsync(string email)
        {
            var res = await _db.QueryAsync<string>("SELECT FName FROM Person WHERE emailPerson = @Email",
                new { Email = email }).ConfigureAwait(false);
            return res.FirstOrDefault();
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            var res = await _db.QueryAsync<string>("SELECT emailPerson FROM Person WHERE emailPerson = @Email",
                new { Email = email }).ConfigureAwait(false);
            return res.FirstOrDefault() == email;
        }

        public async Task<int> InsertPersonAsync(string fName, string lName, string email, string status)
        {
            const string query = "INSERT INTO Person(FName, LName, emailPerson,typePerson) VALUES(@FName, @LName, @Email, @Status); SELECT CAST(SCOPE_IDENTITY() as int);";
            var res = await _db.QueryAsync<int>(query,
                new { FName = fName, LName = lName, Email = email, Status = status }).ConfigureAwait(false);
            return res.FirstOrDefault();
        }

        public async Task InsertPasswordAsync(string hashedPassword, int personId)
        {
            const string query = "INSERT INTO Pass(password, idPerson) VALUES(@Password, @Id)";
            await _db.ExecuteAsync(query, new { Password = hashedPassword, Id = personId }).ConfigureAwait(false);
        }
    }
}
