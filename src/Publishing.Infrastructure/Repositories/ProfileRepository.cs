using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Dapper;
using Publishing.Core.Interfaces;
using Publishing.Core.Commands;

namespace Publishing.Infrastructure.Repositories
{
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

        public Task UpdateAsync(UpdateProfileCommand cmd)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@id", cmd.Id);
            var sb = new StringBuilder("UPDATE Person SET");
            void Add(string field, string? value, string param)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    sb.Append($" {field} = @{param},");
                    parameters.Add($"@{param}", value);
                }
            }
            Add("FName", cmd.FirstName, "FName");
            Add("LName", cmd.LastName, "LName");
            Add("emailPerson", cmd.Email, "Email");
            if (!string.IsNullOrEmpty(cmd.Status))
            {
                sb.Append(" typePerson = @Status,");
                parameters.Add("@Status", cmd.Status);
            }
            Add("phonePerson", cmd.Phone, "phone");
            Add("faxPerson", cmd.Fax, "fax");
            Add("addressPerson", cmd.Address, "address");
            if (sb[sb.Length - 1] == ',')
                sb.Remove(sb.Length - 1, 1);
            sb.Append(" WHERE idPerson = @id");
            return _db.ExecuteAsync(sb.ToString(), parameters);
        }
    }
}
