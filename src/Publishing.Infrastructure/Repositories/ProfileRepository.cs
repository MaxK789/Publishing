using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Dapper;
using Publishing.Core.Interfaces;

namespace Publishing.Infrastructure.Repositories
{
    [Obsolete("Replaced by query objects")]
    public class ProfileRepository : IProfileRepository
    {
        private readonly IDbContext _db;

        public ProfileRepository(IDbContext db)
        {
            _db = db;
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            var res = await _db.QueryAsync<string>("SELECT emailPerson FROM Person WHERE emailPerson = @Email", new { Email = email });
            return res.FirstOrDefault() == email;
        }

        public Task UpdateAsync(string id, string? fName, string? lName, string? email, string? status, string? phone, string? fax, string? address)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@id", id);
            var sb = new StringBuilder("UPDATE Person SET");
            void Add(string field, string? value, string param)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    sb.Append($" {field} = @{param},");
                    parameters.Add($"@{param}", value);
                }
            }
            Add("FName", fName, "FName");
            Add("LName", lName, "LName");
            Add("emailPerson", email, "Email");
            if (!string.IsNullOrEmpty(status))
            {
                sb.Append(" typePerson = @Status,");
                parameters.Add("@Status", status);
            }
            Add("phonePerson", phone, "phone");
            Add("faxPerson", fax, "fax");
            Add("addressPerson", address, "address");
            if (sb[sb.Length - 1] == ',')
                sb.Remove(sb.Length - 1, 1);
            sb.Append(" WHERE idPerson = @id");
            return _db.ExecuteAsync(sb.ToString(), parameters);
        }
    }
}
